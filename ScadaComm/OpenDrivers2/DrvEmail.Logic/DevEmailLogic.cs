// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.AB;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Lang;
using Scada.Data.Models;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Scada.Comm.Drivers.DrvEmail.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevEmailLogic : DeviceLogic
    {
        private readonly EmailDeviceConfig config; // the device configuration
        private readonly SmtpClient smtpClient;    // sends emails
        private AddressBook addressBook;           // the address book shared for the communication line
        private bool isReady;                      // indicates that the device is ready to send requests
        private bool loggingFlag;                  // indicates that a ready message should be logged


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevEmailLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            CanSendCommands = true;
            ConnectionRequired = false;

            config = new EmailDeviceConfig();
            smtpClient = new SmtpClient();
            addressBook = null;
            isReady = false;
            loggingFlag = false;
        }


        /// <summary>
        /// Writes the readiness status to the log.
        /// </summary>
        private void WriteReadyStatus()
        {
            if (isReady)
            {
                Log.WriteLine(Locale.IsRussian ?
                    "{0} ожидает команд..." :
                    "{0} is waiting for commands...", Title);
            }
            else
            {
                Log.WriteLine(Locale.IsRussian ?
                    "{0}{1} не может отправлять письма" :
                    "{0}{1} unable to send emails", CommPhrases.ErrorPrefix, Title);
            }
        }

        /// <summary>
        /// Initializes the SNMP client according to the device configuration.
        /// </summary>
        private void InitSnmpClient()
        {
            smtpClient.Host = config.Host;
            smtpClient.Port = config.Port;
            smtpClient.Credentials = string.IsNullOrEmpty(config.Password) 
                ? CredentialCache.DefaultNetworkCredentials 
                : new NetworkCredential(config.Username, config.Password);
            smtpClient.Timeout = PollingOptions.Timeout;
            smtpClient.EnableSsl = config.EnableSsl;
        }

        /// <summary>
        /// Tries to get a mail message based on the specified command.
        /// </summary>
        private bool TryGetMessage(TeleCommand cmd, bool withAttachments, out MailMessage message)
        {
            const char CmdSep = ';';
            const char FileSep = ',';
            string cmdDataStr = cmd.GetCmdDataString();
            int sepIdx1 = cmdDataStr.IndexOf(CmdSep);
            int sepIdx2 = sepIdx1 >= 0 ? cmdDataStr.IndexOf(CmdSep, sepIdx1 + 1) : -1;

            if (sepIdx1 >= 0 && sepIdx2 >= 0)
            {
                // get addresses
                string recipient = cmdDataStr.Substring(0, sepIdx1);
                List<string> addresses = new List<string>();

                if (addressBook == null)
                {
                    // add address from command data
                    addresses.Add(recipient);
                }
                else
                {
                    // search for addresses in address book
                    if (addressBook.FindContactGroup(recipient) is ContactGroup contactGroup)
                    {
                        // add addresses from contact group
                        foreach (Contact contact in contactGroup.Contacts)
                        {
                            addresses.AddRange(contact.Emails);
                        }
                    }
                    else if (addressBook.FindContact(recipient) is Contact contact)
                    {
                        // add address from contact
                        addresses.AddRange(contact.Emails);
                    }
                    else
                    {
                        // add address from command data
                        addresses.Add(recipient);
                    }
                }

                // get subject, text and attachments
                string subject = cmdDataStr.Substring(sepIdx1 + 1, sepIdx2 - sepIdx1 - 1);
                string text = null;
                string[] fileNames = null;

                if (withAttachments)
                {
                    int sepIdx3 = cmdDataStr.LastIndexOf(CmdSep);

                    if (sepIdx2 < sepIdx3)
                    {
                        text = cmdDataStr.Substring(sepIdx2 + 1, sepIdx3 - sepIdx2 - 1);
                        List<string> fileNameList = new List<string>();

                        foreach (string s in cmdDataStr.Substring(sepIdx3 + 1).Split(FileSep))
                        {
                            string fileName = s.Trim();
                            if (File.Exists(fileName))
                            {
                                fileNameList.Add(fileName);
                            }
                        }

                        if (fileNameList.Count > 0)
                            fileNames = fileNameList.ToArray();
                    }
                }

                if (text == null)
                    text = cmdDataStr.Substring(sepIdx2 + 1);

                // create message
                message = CreateMessage(addresses, subject, text, fileNames);
                return message != null;
            }
            else
            {
                message = null;
                return false;
            }
        }

        /// <summary>
        /// Creates a mail message.
        /// </summary>
        private MailMessage CreateMessage(List<string> addresses, string subject, string text, string[] fileNames)
        {
            MailMessage message = new MailMessage();

            try
            {
                message.From = new MailAddress(config.SenderAddress, config.SenderDisplayName);
            }
            catch
            {
                Log.WriteLine(Locale.IsRussian ?
                    "{0}Некорректный адрес отправителя {1}" :
                    "{0}Invalid sender address {1}", CommPhrases.ErrorPrefix, config.SenderAddress);
                return null;
            }

            foreach (string address in addresses)
            {
                try
                {
                    message.To.Add(new MailAddress(address));
                }
                catch
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "{0}Некорректный адрес получателя {1}" :
                        "{0}Invalid recipient address {1}", CommPhrases.ErrorPrefix, address);
                }
            }

            if (message.To.Count > 0)
            {
                message.Subject = subject;
                message.Body = text;

                if (fileNames != null)
                {
                    foreach (string fileName in fileNames)
                    {
                        message.Attachments.Add(new Attachment(fileName));
                    }
                }

                return message;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Sends the mail message.
        /// </summary>
        private bool SendMessage(MailMessage message, string tagCode)
        {
            try
            {
                smtpClient.Send(message);
                DeviceData.Add(TagCode.Mail, 1);

                Log.WriteLine(Locale.IsRussian ?
                    "Письмо отправлено на {0}" :
                    "Mail has been sent to {0}", message.To.ToString());
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLine(Locale.IsRussian ?
                    "Ошибка при отправке письма на {0}: {1}" :
                    "Error sending mail to {0}: {1}", message.To.ToString(), ex.Message);
                return false;
            }
        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            isReady = false;
            loggingFlag = false;

            if (config.Load(Storage, EmailDeviceConfig.GetFileName(DeviceNum), out string errMsg))
            {
                InitSnmpClient();
                addressBook = AddressBookUtils.GetOrLoad(LineContext.SharedData, Storage, Log);
                isReady = true;
                loggingFlag = true;
            }
            else
            {
                Log.WriteLine(errMsg);
            }
        }

        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        public override void InitDeviceTags()
        {
            TagGroup tagGroup = new TagGroup();
            tagGroup.AddTag(TagCode.Mail, TagName.Mail).Format = TagFormat.IntNumber;
            tagGroup.AddTag(TagCode.MailAttach, TagName.MailAttach).Format = TagFormat.IntNumber;
            DeviceTags.AddGroup(tagGroup);
        }

        /// <summary>
        /// Initializes the device data.
        /// </summary>
        public override void InitDeviceData()
        {
            base.InitDeviceData();

            // reset counters
            DeviceData.Set(TagCode.Mail, 0);
            DeviceData.Set(TagCode.MailAttach, 0);

            // set device status
            if (isReady)
            {
                DeviceStatus = DeviceStatus.Normal;
                DeviceData.SetStatusTag(DeviceStatus);
            }
        }

        /// <summary>
        /// Выполнить сеанс опроса КП
        /// </summary>
        public override void Session()
        {
            if (loggingFlag)
            {
                loggingFlag = false;
                Log.WriteLine();
                WriteReadyStatus();
            }

            SleepPollingDelay();
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public override void SendCommand(TeleCommand cmd)
        {
            base.SendCommand(cmd);
            LastRequestOK = false;

            if (isReady)
            {
                string cmdCode = "";
                bool withAttachments = false;

                if (cmd.CmdCode == TagCode.Mail || cmd.CmdNum == 1)
                {
                    cmdCode = TagCode.Mail;
                }
                else if (cmd.CmdCode == TagCode.MailAttach || cmd.CmdNum == 2)
                {
                    cmdCode = TagCode.MailAttach;
                    withAttachments = true;
                }

                if (cmdCode != "" && TryGetMessage(cmd, withAttachments, out MailMessage message))
                {
                    LastRequestOK = SendMessage(message, cmdCode);
                    FinishRequest();
                }
                else
                {
                    Log.WriteLine(CommPhrases.InvalidCommand);
                }

                loggingFlag = true;
            }
            else
            {
                WriteReadyStatus();
            }

            FinishCommand();
        }
    }
}
