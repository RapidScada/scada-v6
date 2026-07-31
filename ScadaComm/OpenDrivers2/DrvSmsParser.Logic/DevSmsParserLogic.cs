// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using DrvSmsParser.Shared.Config;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Lang;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvSmsParser.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevSmsParserLogic : DeviceLogic
    {
        /// <summary>
        /// Represents a template dictionary.
        /// </summary>
        private class TemplateDictionary : Dictionary<string, DeviceTemplate>
        {
            public override string ToString()
            {
                return Locale.IsRussian ?
                    $"Словарь из {Count} шаблонов" :
                    $"Dictionary of {Count} templates";
            }
        }

        private const string TemplateDictonaryKey = "SmsParser.Templates";

        private TimeSpan dataLifetime;         // specifies when tag values should be invalidated
        private bool useDataLifetime;          // indicates that lifetime is used
        private DateTime[] updateTimestamps;   // the update timestamps by device tag
        private DeviceTemplate deviceTemplate; // the device template


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevSmsParserLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            dataLifetime = TimeSpan.Zero;
            useDataLifetime = false;
            updateTimestamps = null;
            deviceTemplate = null;

            ConnectionRequired = false;
        }


        /// <summary>
        /// Gets the template dictionary from the communication line shared data, or creates a new one.
        /// </summary>
        private TemplateDictionary GetTemplates()
        {
            if (!LineContext.SharedData.TryGetValueOfType(TemplateDictonaryKey, out TemplateDictionary templates))
            {
                templates = [];
                LineContext.SharedData.Add(TemplateDictonaryKey, templates);
            }

            return templates;
        }

        /// <summary>
        /// Gets the device template from the shared dictionary.
        /// </summary>
        private DeviceTemplate GetDeviceTemplate()
        {
            DeviceTemplate deviceTemplate = null;
            string fileName = PollingOptions.CmdLine.Trim();

            if (string.IsNullOrEmpty(fileName))
            {
                Log.WriteLine(Locale.IsRussian ?
                    "{0}Не задан шаблон устройства для {1}" :
                    "{0}Device template is undefined for {1}", CommPhrases.ErrorPrefix, Title);
            }
            else
            {
                TemplateDictionary templates = GetTemplates();

                if (templates.TryGetValue(fileName, out DeviceTemplate existingTemplate))
                {
                    deviceTemplate = existingTemplate;
                }
                else
                {
                    Log.WriteLine(Locale.IsRussian ?
                        "Загрузка шаблона устройства из файла {0}" :
                        "Load device template from file {0}", fileName);

                    DeviceTemplate newTemplate = new();
                    templates.Add(fileName, newTemplate);

                    if (newTemplate.Load(Storage, fileName, out string errMsg))
                    {
                        deviceTemplate = newTemplate;
                    }
                    else
                    {
                        Log.WriteLine(errMsg);
                    }
                }
            }

            return deviceTemplate;
        }

        /// <summary>
        /// Creates a device tag from the specified string.
        /// </summary>
        private static DeviceTag ParseDeviceTag(string fullName)
        {
            if (string.IsNullOrEmpty(fullName)) return new DeviceTag();

            int idx1 = fullName.IndexOf('[');
            int idx2 = fullName.IndexOf(']');

            return idx1 >= 0 && idx1 < idx2
                ? new DeviceTag(fullName[(idx1 + 1)..idx2].Trim(), fullName[(idx2 + 1)..].Trim())
                : new DeviceTag(fullName, fullName);
        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            dataLifetime = TimeSpan.FromSeconds(LineContext.LineConfig.CustomOptions.GetValueAsInt("DataLifetime"));
            useDataLifetime = dataLifetime > TimeSpan.Zero;
        }
        
        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        public override void InitDeviceTags()
        {
            deviceTemplate = GetDeviceTemplate();

            if (deviceTemplate != null)
            {
                TagGroup tagGroup = new();

                foreach (string tag in deviceTemplate.Tags)
                {
                    tagGroup.DeviceTags.Add(ParseDeviceTag(tag));
                }

                if (tagGroup.DeviceTags.Count > 0)
                {
                    DeviceTags.AddGroup(tagGroup);
                    DeviceTags.FlattenGroups = true;
                    updateTimestamps = new DateTime[tagGroup.DeviceTags.Count];
                }
            }
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {

        }
    }
}
