using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Server.Modules.ModDeviceAlarm.Config;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Server.Modules.ModDeviceAlarm.Logic.Triggers
{
    internal class DataTrigger
    {
        public DataTrigger(TriggerOptions triggerOptions)
        {
            this.Options = triggerOptions;
            CnlNumFilter = new HashSet<int>();
            DeviceNumFilter = new HashSet<int>();
            DeviceName = string.Empty;
            IsAlarmed = false;
            SendMailTimes = 0;
            AbnormalTimes = 0;
        }

        /// <summary>
        /// 触发器选项
        /// </summary>
        public TriggerOptions Options { get; }

        /// <summary>
        /// 通道过滤器
        /// </summary>
        public HashSet<int> CnlNumFilter { get; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public HashSet<int> DeviceNumFilter { get; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 采样频率
        /// </summary>
        public int DataPeriod
        {
            get
            {
                if (Options.TriggerKind == TriggerKind.Status) return Options.StatusPeriod;
                else return Options.DataUnchangedPeriod;
            }
        }

        /// <summary>
        /// 是否告警
        /// </summary>
        public bool IsAlarmed { get; set; }

        /// <summary>
        /// 异常次数
        /// </summary>
        public int AbnormalTimes { get; set; }

        /// <summary>
        /// 告警次数
        /// </summary>
        public int SendMailTimes { get; set; }

        /// <summary>
        /// Fills the combined channel filter.
        /// </summary>
        public void FillCnlNumFilter(ConfigDatabase configDatabase)
        {
            ArgumentNullException.ThrowIfNull(configDatabase, nameof(configDatabase));
            CnlNumFilter.UnionWith(Options.Filter.CnlNums);


            //获取设备名称
            if (Options.Filter.CnlNums.Count > 0)
            {
                var deviceNums = new List<int>();
                foreach (var cnlNum in Options.Filter.CnlNums)
                {
                    var cnl = configDatabase.CnlTable.GetItem(cnlNum);
                    if (cnl == null || !cnl.DeviceNum.HasValue) continue;
                    deviceNums.Add(cnl.DeviceNum.Value);
                }
                DeviceNumFilter.UnionWith(deviceNums.Distinct().ToHashSet<int>());
            }

            // extract channels from device filter
            if (Options.Filter.DeviceNums.Count > 0)
            {
                List<int> cnlNumsByDevice = new();

                foreach (int deviceNum in Options.Filter.DeviceNums)
                {
                    foreach (Cnl cnl in configDatabase.CnlTable
                        .Select(new TableFilter("DeviceNum", deviceNum), true)
                        .Where(c => c.Active))
                    {
                        for (int i = 0, len = cnl.GetDataLength(); i < len; i++)
                        {
                            cnlNumsByDevice.Add(cnl.CnlNum + i);
                        }
                    }
                }

                if (CnlNumFilter.Count > 0)
                    CnlNumFilter.IntersectWith(cnlNumsByDevice);
                else
                    CnlNumFilter.UnionWith(cnlNumsByDevice);
                if(DeviceNumFilter.Count > 0)
                    DeviceNumFilter.IntersectWith(Options.Filter.DeviceNums.ToHashSet<int>());
                else
                    DeviceNumFilter.UnionWith(Options.Filter.DeviceNums.ToHashSet<int>());
            }

            if(DeviceNumFilter.Count > 0)
            {
                var deviceNameArr = new List<string>();
                foreach (int deviceNum in DeviceNumFilter)
                {
                    var device = configDatabase.DeviceTable.GetItem(deviceNum);
                    if (device == null) continue;
                    deviceNameArr.Add(device.Name);
                }
                DeviceName = string.Join(',', deviceNameArr.ToArray());
            }
        }
    }
}
