// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtWirenBoard.Code.Meta;
using Scada.Lang;
using System.Globalization;
using System.Text.Json;

namespace Scada.Admin.Extensions.ExtWirenBoard.Code.Models
{
    /// <summary>
    /// Represents a Wiren Board data model.
    /// <para>Представляет модель данных Wiren Board.</para>
    /// </summary>
    internal class WirenBoardModel
    {
        /// <summary>
        /// Specifies how to parse JSON.
        /// </summary>
        private static readonly JsonSerializerOptions JsonSerializerOptions = 
            new() { PropertyNameCaseInsensitive = true };

        private readonly Dictionary<string, DeviceModel> deviceByCode; // the device models accessed by code


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public WirenBoardModel()
        {
            deviceByCode = new Dictionary<string, DeviceModel>();
            Devices = new List<DeviceModel>();
        }


        /// <summary>
        /// Gets the device models.
        /// </summary>
        public List<DeviceModel> Devices { get; }


        /// <summary>
        /// Parses the device meta.
        /// </summary>
        private void ParseDeviceMeta(string[] topicParts, string payload, string deviceCode)
        {
            if (!string.IsNullOrEmpty(deviceCode))
            {
                DeviceModel deviceModel = GetOrAddDevice(deviceCode);

                if (topicParts.Length == 3 && payload.StartsWith('{'))
                {
                    DeviceMeta deviceMeta = JsonSerializer.Deserialize<DeviceMeta>(payload, JsonSerializerOptions);
                    deviceMeta.Name = deviceMeta.Title.En ?? deviceCode;
                    deviceModel.UpdateMeta(deviceMeta);
                }
                else if (topicParts.Length == 4)
                {
                    DeviceMeta deviceMeta = deviceModel.Meta;
                    string propName = topicParts[3];

                    if (propName == "driver")
                        deviceMeta.Driver = payload;
                    else if (propName == "name")
                        deviceMeta.Name = payload;
                }
            }
        }

        /// <summary>
        /// Parses the control meta.
        /// </summary>
        private void ParseControlMeta(string[] topicParts, string payload, string deviceCode, string controlCode)
        {
            if (!string.IsNullOrEmpty(deviceCode) &&
                !string.IsNullOrEmpty(controlCode))
            {
                DeviceModel deviceModel = GetOrAddDevice(deviceCode);
                ControlModel controlModel = deviceModel.GetOrAddControl(controlCode);

                if (topicParts.Length == 5 && payload.StartsWith('{'))
                {
                    ControlMeta controlMeta = JsonSerializer.Deserialize<ControlMeta>(payload);
                    controlMeta.Name = controlMeta.Title.En ?? controlCode;
                    controlModel.UpdateMeta(controlMeta);
                }
                else if (topicParts.Length == 6)
                {
                    ControlMeta controlMeta = controlModel.Meta;
                    string propName = topicParts[5];

                    if (propName == "type")
                        controlMeta.Type = payload;
                    else if (propName == "min")
                        controlMeta.Min = double.Parse(payload, NumberFormatInfo.InvariantInfo);
                    else if (propName == "max")
                        controlMeta.Max = double.Parse(payload, NumberFormatInfo.InvariantInfo);
                    else if (propName == "order")
                        controlMeta.Order = int.Parse(payload);
                    else if (propName == "readonly")
                        controlMeta.ReadOnly = payload == "1";
                }
            }
        }

        /// <summary>
        /// Gets the existing device model, or adds a new one if not found.
        /// </summary>
        private DeviceModel GetOrAddDevice(string deviceCode)
        {
            if (deviceByCode.TryGetValue(deviceCode, out DeviceModel deviceModel))
            {
                return deviceModel;
            }
            else
            {
                deviceModel = new DeviceModel(deviceCode);
                Devices.Add(deviceModel);
                deviceByCode.Add(deviceCode, deviceModel);
                return deviceModel;
            }
        }


        /// <summary>
        /// Parses the topic and adds to the model.
        /// </summary>
        public bool AddTopic(string topic, string payload, out string errMsg)
        {
            ArgumentNullException.ThrowIfNull(topic, nameof(topic));
            ArgumentNullException.ThrowIfNull(payload, nameof(payload));

            try
            {
                string[] topicParts = topic.Split('/', StringSplitOptions.RemoveEmptyEntries);

                if (topicParts.Length > 2 && topicParts[0] == "devices")
                {
                    string deviceCode = topicParts[1];
                    string topicPart2 = topicParts[2];

                    if (topicPart2 == "meta")
                        ParseDeviceMeta(topicParts, payload, deviceCode);
                    else if (topicParts.Length > 4 && topicPart2 == "controls" && topicParts[4] == "meta")
                        ParseControlMeta(topicParts, payload, deviceCode, topicParts[3]);
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = string.Format(Locale.IsRussian ?
                    "Ошибка при распознавании топика: {0}" :
                    "Error parsing topic: {0}", ex.Message);
                return false;
            }
        }
    }
}
