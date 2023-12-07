// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Google.Apis.Auth.OAuth2;
using Google.Cloud.BigQuery.V2;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvGoogle.Config;
using Scada.Comm.Drivers.DrvGoogleBigQueue.Config;
using Scada.Comm.Drivers.DrvGoogleBigQueue.Logic.GoogleAuth;
using Scada.Comm.Drivers.DrvGoogleBigQueue.Protocol;
using Scada.Comm.Lang;
using Scada.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scada.Comm.Drivers.DrvGoogleBigQueue.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// </summary>
    public class DevGoogleBigQueueLogic : DeviceLogic
    {
        /// <summary>
        /// Represents a template dictionary.
        /// </summary>
        protected class TemplateDict : Dictionary<string, DeviceTemplate>
        {
            public override string ToString()
            {
                return $"Dictionary of {Count} templates";
            }
        }

        protected DeviceModel deviceModel; // the device model
        private IGoogleCloudChannel googleCloudChannel; //
        private string location;// sql location
        private Dictionary<string, BigQueryClient> bigQueueClients;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevGoogleBigQueueLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            deviceModel = null;
            bigQueueClients = new Dictionary<string, BigQueryClient>();
            ConnectionRequired = false;
        }


        /// <summary>
        /// Gets the shared data key of the template dictionary.
        /// </summary>
        protected virtual string TemplateDictKey => "GoogleBigQueue.Templates";

        /// <summary>
        /// Sets the data of the element group tags.
        /// </summary>
        private void SetTagData(ElemGroup elemGroup)
        {
            for (int elemIdx = 0, tagIdx = elemGroup.StartTagIdx + elemIdx, cnt = elemGroup.Elems.Count; 
                elemIdx < cnt; elemIdx++, tagIdx++)
            {
                DeviceData.Set(tagIdx, elemGroup.GetElemVal(elemIdx));
            }
        }

        /// <summary>
        /// Gets the device tag format depending on the Modbus element type.
        /// </summary>
        private static TagFormat GetTagFormat(ElemConfig elemConfig)
        {
            if (elemConfig.ElemType == ElemType.Bool)
                return TagFormat.OffOn;
            else if (elemConfig.ElemType == ElemType.Float || elemConfig.ElemType == ElemType.Double)
                return TagFormat.FloatNumber;
            else if (elemConfig.IsBitMask)
                return TagFormat.HexNumber;
            else
                return TagFormat.IntNumber;
        }


        /// <summary>
        /// Gets a template dictionary from the shared data of the communication line, or creates a new one.
        /// </summary>
        protected virtual TemplateDict GetTemplateDict()
        {
            TemplateDict templateDict = LineContext.SharedData.ContainsKey(TemplateDictKey) ?
                LineContext.SharedData[TemplateDictKey] as TemplateDict : null;

            if (templateDict == null)
            {
                templateDict = new TemplateDict();
                LineContext.SharedData.Add(TemplateDictKey, templateDict);
            }

            return templateDict;
        }

        /// <summary>
        /// Gets the device template from the shared dictionary.
        /// </summary>
        protected virtual DeviceTemplate GetDeviceTemplate()
        {
            DeviceTemplate deviceTemplate = null;
            string fileName = PollingOptions.CmdLine.Trim();

            if (string.IsNullOrEmpty(fileName))
            {
                Log.WriteLine(string.Format("Error: Device template is undefined for {0}", Title));
            }
            else
            {
                TemplateDict templateDict = GetTemplateDict();

                if (templateDict.TryGetValue(fileName, out DeviceTemplate existingTemplate))
                {
                    deviceTemplate = existingTemplate;
                }
                else
                {
                    Log.WriteLine(string.Format("Load device template from file {0}", fileName));

                    DeviceTemplate newTemplate = CreateDeviceTemplate();
                    templateDict.Add(fileName, newTemplate);

                    if (newTemplate.Load(Storage, fileName, out string errMsg))
                        deviceTemplate = newTemplate;
                    else
                        Log.WriteLine(errMsg);
                }
            }

            return deviceTemplate;
        }

        /// <summary>
        /// Create a new device template.
        /// </summary>
        protected virtual DeviceTemplate CreateDeviceTemplate()
        {
            return new DeviceTemplate();
        }

        /// <summary>
        /// Create a new device model.
        /// </summary>
        protected virtual DeviceModel CreateDeviceModel()
        {
            return new DeviceModel();
        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            location = LineContext.LineConfig.CustomOptions.GetValueAsString("Location", "asia-east1");
            Log.WriteLine($"CycleDelay: {LineContext.LineConfig.LineOptions.CycleDelay}ms");
            Log.WriteLine($"Location: {location}");
            if (LineContext.Channel is IGoogleCloudChannel channel)
            {
                this.googleCloudChannel = channel;
            }
            else
            {
                Log.WriteLine(CommPhrases.DeviceMessage, Title, "Google cloud channel is required");
            }
        }

        /// <summary>
        /// Performs actions after setting the connection.
        /// </summary>
        public override void OnConnectionSet()
        {

        }

        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        public override void InitDeviceTags()
        {
            DeviceTemplate deviceTemplate = GetDeviceTemplate();

            if (deviceTemplate == null)
                return;

            // create device model
            deviceModel = CreateDeviceModel();
            deviceModel.Addr = (byte)NumAddress;

            // add model elements and device tags
            foreach (ElemGroupConfig elemGroupConfig in deviceTemplate.ElemGroups)
            {
                bool groupActive = elemGroupConfig.Active;
                ElemGroup elemGroup = null;
                TagGroup tagGroup = new TagGroup(elemGroupConfig.Name) { Hidden = !groupActive };
                int elemAddrOffset = 0;

                if (groupActive)
                {
                    elemGroup = deviceModel.CreateElemGroup();
                    elemGroup.Name = elemGroupConfig.Name;
                    elemGroup.Code = elemGroupConfig.Code;
                    elemGroup.QuerySql = elemGroupConfig.QuerySql;
                    elemGroup.Address = (ushort)elemGroupConfig.Address;
                    elemGroup.StartTagIdx = DeviceTags.Count;
                }

                foreach (ElemConfig elemConfig in elemGroupConfig.Elems)
                {
                    // add model element
                    if (groupActive)
                    {
                        Elem elem = elemGroup.CreateElem();
                        elem.Name = elemConfig.Name;
                        elem.ElemType = elemConfig.ElemType;
                        elem.ByteOrder = BigQueueUtils.ParseByteOrder(elemConfig.ByteOrder) ??
                            deviceTemplate.Options.GetDefaultByteOrder(BigQueueUtils.GetDataLength(elemConfig.ElemType));
                        elemGroup.Elems.Add(elem);
                    }

                    // add device tag
                    tagGroup.AddTag(elemConfig.TagCode, elemConfig.Name).SetFormat(GetTagFormat(elemConfig));
                    elemAddrOffset += elemConfig.Quantity;
                }

                if (groupActive)
                {
                    DeviceTag deviceTagTime = tagGroup.AddTag(elemGroup.Code,"Time");
                    deviceTagTime.SetDataType(TagDataType.Int64);
                    deviceTagTime.SetFormat(TagFormat.DateTime);

                    elemGroup.LastRecordTime = DateTime.UtcNow.AddMinutes(-10);
                    deviceModel.ElemGroups.Add(elemGroup);

                    // 创建queue clients
                    if (!bigQueueClients.ContainsKey(elemGroupConfig.Code))
                    {
                        Log.WriteLine($"Create big queue client: {elemGroupConfig.Code}");
                        ICredential credential = new CloudScadaCredential(this.googleCloudChannel);
                        BigQueryClientBuilder bigQueryClientBuilder = new BigQueryClientBuilder()
                        {
                            ProjectId = elemGroupConfig.ProjectId,
                            Credential = credential,
                            DefaultLocation = location
                        };
                        BigQueryClient bigQueryClient = bigQueryClientBuilder.Build();
                        bigQueueClients.Add(elemGroupConfig.Code, bigQueryClient);
                    }
                }

                DeviceTags.AddGroup(tagGroup);
            }

            CanSendCommands = false;
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            base.Session();

            if (deviceModel == null)
            {
                Log.WriteLine("Unable to poll the device because device model is undefined");
                SleepPollingDelay();
                LastRequestOK = false;
            }
            else if (deviceModel.ElemGroups.Count > 0)
            {
                // request element groups
                int elemGroupCnt = deviceModel.ElemGroups.Count;
                int elemGroupIdx = 0;

                while (elemGroupIdx < elemGroupCnt && LastRequestOK)
                {
                    ElemGroup elemGroup = deviceModel.ElemGroups[elemGroupIdx];
                    LastRequestOK = false;

                    var bigQueryRows = new List<BigQueryRowDto>();
                    try
                    {
                        Log.WriteLine($"[{elemGroup.Code}]LastRecordTime: {elemGroup.LastRecordTime.ToLocalTime():G}");
                        var client = bigQueueClients[elemGroup.Code];
                        var parameters = new BigQueryParameter[]
                                        {
                                             new BigQueryParameter("lastQueryTime", BigQueryDbType.DateTime, elemGroup.LastRecordTime)
                                        };
                        BigQueryJob job = client.CreateQueryJob(
                                 sql: elemGroup.QuerySql,
                                 parameters: parameters,
                                 options: new QueryOptions { UseQueryCache = false }
                                );
                        // Wait for the job to complete.
                        job = job.PollUntilCompleted().ThrowOnAnyError();
                        // Display the results
                        foreach (BigQueryRow row in client.GetQueryResults(job.Reference))
                        {
                            Log.WriteLine($"{row["timestamp"]}\t{row["timeserie_name"]}\t{row["value"]}");
                            bigQueryRows.Add(new BigQueryRowDto
                            {
                                Timestamp = (DateTime)row["timestamp"],
                                SerieName = row["timeserie_name"].ToString(),
                                Value = (double)row["value"]
                            });
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Log.WriteError($"big queue run sql error :{ex.Message}");
                    }
                    try
                    {
                        if (bigQueryRows.Any())
                        {
                            Log.WriteLine($"big queue has {bigQueryRows.Count} result，lastQueryTime：{elemGroup.LastRecordTime.ToLocalTime():G}");
                            var utcNow = DateTime.UtcNow;
                            var isCurrent = false;
                            //create a historical data slice
                            for (int i = 0; i < bigQueryRows.Count; i++)
                            {
                                var bigQueueRow = bigQueryRows[i];
                                //判断测点记录时间是否存在并且是最新的
                                if (!(elemGroup.DataTimeCache.TryGetValue(bigQueueRow.SerieName, out var dataTime) && dataTime > bigQueueRow.Timestamp))
                                {
                                    isCurrent = true;
                                    DeviceData.Set(bigQueueRow.SerieName, bigQueueRow.Value);
                                    elemGroup.DataTimeCache.AddOrUpdate(bigQueueRow.SerieName, bigQueueRow.Timestamp, (k, v) => bigQueueRow.Timestamp);
                                }
                                //覆盖历史数据
                                DeviceSlice deviceSlice = new DeviceSlice(bigQueueRow.Timestamp,1, 1);
                                deviceSlice.DeviceTags[0] = DeviceTags[bigQueueRow.SerieName];
                                deviceSlice.CnlData[0] = new CnlData(bigQueueRow.Value);
                                deviceSlice.Descr = $"{bigQueueRow.SerieName}: {bigQueueRow.Value}";
                                DeviceData.EnqueueSlice(deviceSlice);
                            }

                            //最近查询最新数据时间
                            elemGroup.LastRecordTime = bigQueryRows.First().Timestamp;
                            //判断实时数据，更新当前时间
                            if (isCurrent)
                            {
                                DeviceData.SetDateTime(elemGroup.Code, utcNow, 1);
                            }
                            LastRequestOK = true;
                        }
                        else
                        {
                            Log.WriteError($"big queue has no result");
                        }
                    }
                    catch (Exception ex2)
                    {
                        Log.WriteError($"big queue set value error :{ex2.Message}");
                    }
                    elemGroupIdx++;
                }
            }
            else
            {
                Log.WriteLine("No elements to query");
                SleepPollingDelay();
            }

            FinishSession();
        }
    }
}
