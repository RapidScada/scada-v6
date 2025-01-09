// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Queues;
using Scada.Dbms;
using Scada.Lang;
using Scada.Log;
using Scada.Server.Modules.ModDeviceAlarm.Config;
using Scada.Server.Modules.ModDeviceAlarm.Logic.Triggers;
using System.Data.Common;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Scada.Server.Modules.ModDeviceAlarm.Logic
{
    /// <summary>
    /// Exports data to a one target database.
    /// </summary>
    internal sealed class Exporter : IExporterContext
    {
        private readonly SmtpClient smtpClient;    // sends emails
        private readonly int smtpTimeout = 25000;  //smtp timeout
        private readonly EmailDeviceConfig emailConfig;  // the exporter configuration
        private readonly ExportTargetConfig exporterConfig;    // the exporter configuration
        private readonly TriggerOptions triggerOptions;            // the exporter trigger
        private readonly IServerContext serverContext;         // the application context

        private readonly DataTrigger dataTrigger; 

        private readonly string exporterTitle;                 // the title of the exporter
        private readonly ILog exporterLog;                     // the exporter log
        private readonly string filePrefix;                    // the prefix of the exporter files
        private readonly string infoFileName;                  // the information file name
        private readonly Dictionary<int, CnlData> prevCnlData; // the previous channel data

        private Thread exporterThread;         // the working thread of the exporter
        private volatile bool terminated;      // necessary to stop the thread

        private long cnlListID; // 实时数据缓存id
        private bool canSendMail = true; // 是否可发送邮件

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Exporter(EmailDeviceConfig emailDeviceConfig, ExportTargetConfig exporterConfig, TriggerOptions triggerOptions, IServerContext serverContext)
        {
            this.emailConfig = emailDeviceConfig ?? throw new ArgumentNullException(nameof(emailDeviceConfig));
            this.exporterConfig = exporterConfig ?? throw new ArgumentNullException(nameof(exporterConfig));
            this.triggerOptions = triggerOptions ?? throw new ArgumentNullException(nameof(triggerOptions));
            this.serverContext = serverContext ?? throw new ArgumentNullException(nameof(serverContext));

            // get options
            GeneralOptions generalOptions = exporterConfig.GeneralOptions;
            exporterTitle = generalOptions.Title;

            // initialize other fields
            filePrefix = ModuleUtils.ModuleCode + "_" + generalOptions.ID.ToString("D3") + "_" + triggerOptions.Name;
            exporterLog = new LogFile(LogFormat.Simple)
            {
                FileName = Path.Combine(serverContext.AppDirs.LogDir, filePrefix + ".log"),
                CapacityMB = serverContext.AppConfig.GeneralOptions.MaxLogSize
            };
            infoFileName = Path.Combine(serverContext.AppDirs.LogDir, filePrefix + ".txt");
            prevCnlData = new Dictionary<int, CnlData>();

            exporterThread = null;
            terminated = false;

            smtpClient = new SmtpClient();
            dataTrigger = new DataTrigger(this.triggerOptions);
        }

        /// <summary>
        /// Gets the smtp
        /// </summary>
        EmailDeviceConfig IExporterContext.EmailDeviceConfig => emailConfig;

        /// <summary>
        /// Gets the gate configuration.
        /// </summary>
        ExportTargetConfig IExporterContext.ExporterConfig => exporterConfig;

        /// <summary>
        /// Gets the gate log.
        /// </summary>
        ILog IExporterContext.ExporterLog => exporterLog;

        /// <summary>
        /// Gets the prefix of the gate files.
        /// </summary>
        string IExporterContext.FilePrefix => filePrefix;

        /// <summary>
        /// Operating loop running in a separate thread.
        /// </summary>
        private void Execute()
        {
            DateTime utcNow = DateTime.UtcNow;
            DateTime timerDT = LogicUtils.CalcNextTimer(utcNow, dataTrigger.DataPeriod);

            DateTime writeInfoDT = utcNow;
            WriteInfo();
            InitSnmpClient();
            InitCnlNums();
            //延迟10s执行,
            //Thread.Sleep(10 * 1000);
            while (!terminated)
            {
                // export to database
                try
                {
                    utcNow = DateTime.UtcNow;
                    if (timerDT <= utcNow)
                    {
                        timerDT = LogicUtils.CalcNextTimer(utcNow, dataTrigger.DataPeriod);
                        DoTrigger();
                    }
                }
                finally
                {

                }

                // write exporter info
                utcNow = DateTime.UtcNow;
                if (utcNow - writeInfoDT >= ScadaUtils.WriteInfoPeriod)
                {
                    writeInfoDT = utcNow;
                    WriteInfo();
                }

                Thread.Sleep(ScadaUtils.ThreadDelay);
            }

            WriteInfo();
        }

        /// <summary>
        /// 执行触发器
        /// </summary>
        private void DoTrigger()
        {
            var cnlNums = dataTrigger.CnlNumFilter.ToArray();
            var cnlDatas = cnlListID > 0 ?
                serverContext.GetCurrentData(cnlListID) :
                serverContext.GetCurrentData(cnlNums, true, out cnlListID);
            if (cnlDatas == null || cnlDatas.Length == 0) return;
            var isNormal = false;

            if (triggerOptions.TriggerKind == TriggerKind.Status)
            {
                for (int i = 0; i < cnlNums.Length; i++)
                {
                    //有一个为0就判定为正常
                    if (cnlDatas[i].Stat != triggerOptions.StatusCnlNum) { isNormal = true; break; }
                }
            }
            else if (triggerOptions.TriggerKind == TriggerKind.ValueUnchange)
            {
                for (int i = 0; i < cnlNums.Length; i++)
                {
                    // 有一个改变就返回
                    // exporterLog.WriteAction($"Cnl: {cnlNums[i]}, Val: {cnlDatas[i].Val}");
                    if (ChannelDataChanged(cnlNums[i], cnlDatas[i])) { isNormal = true; break; }
                }
                
                if (isNormal)
                {
                    dataTrigger.AbnormalTimes = 0;
                } // 判断异常次数是否达到预设值
                else if (dataTrigger.AbnormalTimes < triggerOptions.DataUnchangedNumber)
                {
                    dataTrigger.AbnormalTimes++;
                    exporterLog.WriteAction($"[第{dataTrigger.AbnormalTimes}次]检测到数据不更新");
                    isNormal = true;
                }
            }

            if (isNormal && dataTrigger.IsAlarmed) //设备正常,且为告警状态，发送恢复邮件
            {
                exporterLog.WriteAction($"[恢复]设备恢复正常，发送邮件");
                dataTrigger.IsAlarmed = false;
                dataTrigger.SendMailTimes = 0;
                var mailMess = CreateMailMessage(true);
                SendEmail(mailMess);
            }
            else if (!isNormal && dataTrigger.SendMailTimes < exporterConfig.GeneralOptions.SendTimes) // 不正常且比预设发送次数小，发送邮件
            {
                dataTrigger.SendMailTimes++;
                exporterLog.WriteAction($"[异常][第{dataTrigger.SendMailTimes}次]发送设备异常告警邮件");
                dataTrigger.IsAlarmed = true;
                var mailMess = CreateMailMessage(false);
                SendEmail(mailMess);
            }
        }

        /// <summary>
        /// Initializes the exporter.
        /// </summary>
        private void InitSnmpClient()
        {
            try
            {
                smtpClient.Host = emailConfig.Host;
                smtpClient.Port = emailConfig.Port;
                smtpClient.Credentials = string.IsNullOrEmpty(emailConfig.Password)
                    ? CredentialCache.DefaultNetworkCredentials
                    : new NetworkCredential(emailConfig.Username, emailConfig.Password);
                smtpClient.Timeout = smtpTimeout;
                smtpClient.EnableSsl = emailConfig.EnableSsl;
            }
            catch (Exception ex)
            {
                exporterLog.WriteError(ex, "Error initializing smtp client");
            }
        }

        /// <summary>
        /// Initializes the channel numbers of the exporter.
        /// </summary>
        private void InitCnlNums()
        {
            dataTrigger.FillCnlNumFilter(serverContext.ConfigDatabase);
            exporterLog.WriteAction($"设备IDs:{string.Join(',', dataTrigger.DeviceNumFilter)}, 设备名称：{dataTrigger.DeviceName}");
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="isRecover">是否恢复状态</param>
        /// <returns></returns>
        private MailMessage CreateMailMessage(bool isRecover)
        {
            var subject = CreateMailSubjectOrContent(exporterConfig.GeneralOptions.EmailSubject, isRecover);
            var body = CreateMailSubjectOrContent(exporterConfig.GeneralOptions.EmailContent, isRecover);
            MailMessage message = new MailMessage();
            try
            {
                message.From = new MailAddress(emailConfig.SenderAddress, emailConfig.SenderDisplayName);
            }
            catch
            {
                exporterLog.WriteLine("Invalid sender address {0}", emailConfig.SenderAddress);
                return null;
            }
            var emailAddrs = exporterConfig.GeneralOptions.EmailAddress.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < emailAddrs.Length; i++)
            {
                try
                {
                    message.To.Add(new MailAddress(emailAddrs[i]));
                }
                catch
                {
                    exporterLog.WriteLine("Invalid recipient address {0}", emailAddrs[i]);
                }
            }
            message.Subject = subject;
            message.Body = body;
            return message;
        }

        /*
         * 模板占位符：
         设备名称：    @Device
         数据中断时常：@DataAbnormalPeriod
         设备状态中文：@AlarmStatusCn 
         设备状态英文：@AlarmStatusEn
         */

        private string CreateMailSubjectOrContent(string template,bool isRecover)
        {

            template = template.Replace("@Device", dataTrigger.DeviceName);
            if (isRecover)
            {
                template = template.Replace("@DataAbnormalPeriod", "0");
                template = template.Replace("@AlarmStatusCn", "恢复").Replace("@AlarmStatusEn", "recover");
            }
            else
            {
                if (triggerOptions.TriggerKind == TriggerKind.Status)
                {
                    var period = (triggerOptions.StatusPeriod * (1 + dataTrigger.SendMailTimes)) / 60;
                    template = template.Replace("@DataAbnormalPeriod", $"{period}");
                    template = template.Replace("@AlarmStatusCn", "中断").Replace("@AlarmStatusEn", "interrupted");
                }
                else if (triggerOptions.TriggerKind == TriggerKind.ValueUnchange)
                {
                    var period = (triggerOptions.DataUnchangedPeriod * (triggerOptions.DataUnchangedNumber + dataTrigger.SendMailTimes)) / 60;
                    template = template.Replace("@DataAbnormalPeriod", $"{period}");
                    template = template.Replace("@AlarmStatusCn", "停止更新").Replace("@AlarmStatusEn", "stopped updating");
                }
            }
            return template;
        }

        private bool SendEmail(MailMessage message)
        {
            try
            {
                smtpClient.Send(message);

                exporterLog.WriteAction("Mail has been sent to {0}", message.To.ToString());
                return true;
            }
            catch (Exception ex)
            {
                exporterLog.WriteError("[Error]Sending mail to {0}: {1}", message.To.ToString(), ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Checks if the channel data has changed and saves the previous data.
        /// </summary>
        private bool ChannelDataChanged(int cnlNum, CnlData cnlData)
        {
            if (prevCnlData == null)
            {
                return false;
            }
            else if ((prevCnlData.TryGetValue(cnlNum, out CnlData prevCnlDataItem) && prevCnlDataItem == cnlData) && cnlData.Stat != 0)
            {
                return false;
            }
            else
            {
                prevCnlData[cnlNum] = cnlData;
                return true;
            }
        }

        /// <summary>
        /// Writes the exporter information to the file.
        /// </summary>
        private void WriteInfo()
        {
            try
            {
                // prepare information
                StringBuilder sbInfo = new();
                string dbName = triggerOptions.Name;

                sbInfo
                        .AppendLine("Exporter State")
                        .AppendLine("--------------")
                        .Append("Name       : ").AppendLine(exporterTitle);

                sbInfo.AppendLine();

                // write to file
                using StreamWriter writer = new(infoFileName, false, Encoding.UTF8);
                writer.Write(sbInfo.ToString());
            }
            catch (Exception ex)
            {
                exporterLog.WriteError(ex, "Error writing exporter information to the file");
            }
        }


        /// <summary>
        /// Starts the exporter.
        /// </summary>
        public void Start()
        {
            try
            {
                if (exporterThread == null)
                {
                    exporterLog.WriteBreak();
                    exporterLog.WriteAction("Exporter \"{0}\" started", exporterTitle);

                    terminated = false;
                    exporterThread = new Thread(Execute);
                    exporterThread.Start();
                }
                else
                {
                    exporterLog.WriteError("Exporter is already running");
                }
            }
            catch (Exception ex)
            {
                exporterLog.WriteError(ex, "Error starting exporter");
            }
        }

        /// <summary>
        /// Stops the exporter.
        /// </summary>
        public void Stop()
        {
            try
            {
                // stop exporter thread
                if (exporterThread != null)
                {
                    terminated = true;
                    exporterThread.Join();
                    exporterThread = null;
                }

                exporterLog.WriteAction("Exporter \"{0}\" is stopped", exporterTitle);
                exporterLog.WriteBreak();
            }
            catch (Exception ex)
            {
                exporterLog.WriteError(ex, "Error stopping exporter");
            }
        }
    }
}
