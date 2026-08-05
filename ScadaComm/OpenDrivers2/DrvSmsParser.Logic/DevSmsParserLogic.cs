// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using DrvSmsParser.Shared.Config;
using Esprima.Ast;
using Jint;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvSms.Logic.Messaging;
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

        /// <summary>
        /// Contains the keys for shared line data.
        /// </summary>
        private static class SharedDataKey
        {
            public const string Templates = "SmsParser.Templates";
            public const string MessageBag = "Sms.MessageBag";
        }

        private TimeSpan dataLifetime;            // specifies when tag values should be invalidated
        private bool useDataLifetime;             // indicates that lifetime is used
        private DateTime[] updateTimestamps;      // the update timestamps by device tag
        private DeviceTemplate deviceTemplate;    // the device template
        private Engine jsEngine;                  // executes JavaScript
        private Prepared<Script>? preparedScript; // the precompiled script


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
            jsEngine = null;
            preparedScript = null;

            ConnectionRequired = false;
        }


        /// <summary>
        /// Gets the template dictionary from the communication line shared data, or creates a new one.
        /// </summary>
        private TemplateDictionary GetTemplates()
        {
            if (!LineContext.SharedData.TryGetValueOfType(SharedDataKey.Templates, out TemplateDictionary templates))
            {
                templates = [];
                LineContext.SharedData.Add(SharedDataKey.Templates, templates);
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
        /// Invalidates device tags that have not been updated for longer than the data lifetime.
        /// </summary>
        private void InvalidateOutdatedData()
        {
            DateTime utcNow = DateTime.UtcNow;

            for (int i = 0, len = updateTimestamps.Length; i < len; i++)
            {
                if (utcNow - updateTimestamps[i] > dataLifetime)
                {
                    DeviceData.Invalidate(i);
                    updateTimestamps[i] = utcNow;
                }
            }
        }

        /// <summary>
        /// Gets the messages addressed to the device.
        /// </summary>
        private bool GetMessages(out List<IMessageItem> messageItems)
        {
            if (LineContext.SharedData.TryGetValueOfType(SharedDataKey.MessageBag, out IMessageBag messageBag))
            {
                messageItems = messageBag.GetByAddress(StrAddress).ToList();
                return true;
            }
            else
            {
                Log.WriteLine(Locale.IsRussian ?
                    "{0}Хранилище сообщений не найдено" :
                    "{0}Message bag not found", CommPhrases.ErrorPrefix);
                messageItems = null;
                return false;
            }
        }

        /// <summary>
        /// Processes the received messages.
        /// </summary>
        private void ProcessMessages(List<IMessageItem> messageItems)
        {
            Log.WriteLine(Locale.IsRussian ?
                "Полученных сообщений: {0}" :
                "Messages received: {0}", messageItems.Count);
            DeviceData.Add(TagCode.Msg, messageItems.Count);
            DeviceTag eventTag = DeviceTags[TagCode.Msg];

            foreach (IMessageItem messageItem in messageItems)
            {
                DeviceEvent deviceEvent = EventFactory.CreateDeviceEvent(eventTag, messageItem);
                ExecuteScript(messageItem, deviceEvent);
                DeviceData.EnqueueEvent(deviceEvent);
                messageItem.IsProcessed = true;
            }
        }

        /// <summary>
        /// Executes the script specified in the device template.
        /// </summary>
        private void ExecuteScript(IMessageItem messageItem, DeviceEvent deviceEvent)
        {
            // prepare script
            // braces distinguish variable scopes
            if (string.IsNullOrEmpty(deviceTemplate.Script)) return;
            preparedScript ??= Engine.PrepareScript($"{{ {deviceTemplate.Script} }}");

            // initialize scripting engine
            jsEngine ??= new Engine(options => options.Strict())
                .SetValue("log", new Action<string>(s => Log.WriteLine(s)))
                .SetValue("setTagValue", new Action<int, double>((idx, val) => {
                    DeviceData.Set(idx, val); 
                    updateTimestamps[idx] = LastSessionTime; }));

            // set script methods and variables that depend on current call
            jsEngine
                .SetValue("setEventStatus", new Action<int>(stat => deviceEvent.CnlStat = stat))
                .SetValue("address", messageItem.Address)
                .SetValue("text", messageItem.Text);

            try
            {
                jsEngine.Execute(preparedScript.Value);
            }
            catch (Exception ex)
            {
                Log.WriteLine(CommPhrases.ErrorPrefix + ex.Message);
            }
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
            DeviceTags.AddGroup(CnlPrototypeFactory.GetGeneralGroup().ToTagGroup());
            
            deviceTemplate = GetDeviceTemplate();
            TagGroup customGroup = CnlPrototypeFactory.GetCustomGroup(deviceTemplate).ToTagGroup();

            if (customGroup.DeviceTags.Count > 0)
            {
                DeviceTags.AddGroup(customGroup);
                updateTimestamps = new DateTime[customGroup.DeviceTags.Count];
            }
        }

        /// <summary>
        /// Initializes the device data.
        /// </summary>
        public override void InitDeviceData()
        {
            base.InitDeviceData();
            DeviceData.Set(TagCode.Msg, 0);
        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {
            base.Session();

            if (string.IsNullOrEmpty(StrAddress))
            {
                Log.WriteLine(Locale.IsRussian ?
                    "{0}Строковый адрес не может быть пустым" :
                    "{0}String address cannot be empty", CommPhrases.ErrorPrefix);
                LastRequestOK = false;
            }
            else
            {
                if (GetMessages(out List<IMessageItem> messageItems))
                {
                    if (messageItems.Count > 0)
                    {
                        ProcessMessages(messageItems);
                    }
                    else
                    {
                        Log.WriteLine(Locale.IsRussian ?
                            "Новые сообщения отсутствуют" :
                            "No new messages");
                    }
                }
                else
                {
                    LastRequestOK = false;
                }

                if (useDataLifetime)
                {
                    InvalidateOutdatedData();
                }
            }

            FinishRequest();
            FinishSession();
        }
    }
}
