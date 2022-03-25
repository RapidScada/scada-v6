// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvMqtt;
using Scada.Config;

namespace Scada.Comm.Drivers.DrvDsMqtt
{
    /// <summary>
    /// Represents publishing options.
    /// <para>Представляет параметры публикации.</para>
    /// </summary>
    internal class PublishOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PublishOptions(OptionList options)
        {
            RootTopic = options.GetValueAsString("RootTopic", "Communicator/");
            UndefinedValue = options.GetValueAsString("UndefinedValue", "NaN");
            PublishFormat = options.GetValueAsString("PublishFormat");
            QosLevel = options.GetValueAsInt("QosLevel");
            Retain = options.GetValueAsBool("Retain");
            MaxQueueSize = options.GetValueAsInt("MaxQueueSize", 1000);
            DataLifetime = options.GetValueAsInt("DataLifetime", 60);
            DetailedLog = options.GetValueAsBool("DetailedLog");
            DeviceFilter = new List<int>();
            DeviceFilter.AddRange(ScadaUtils.ParseRange(options.GetValueAsString("DeviceFilter"), true, true));
        }


        /// <summary>
        /// Gets or sets the root topic used as a prefix for all device topics.
        /// </summary>
        public string RootTopic { get; set; }

        /// <summary>
        /// Gets or sets the payload to send if channel value is undefined.
        /// </summary>
        public string UndefinedValue { get; set; }

        /// <summary>
        /// Gets or sets the format of published channel data.
        /// </summary>
        public string PublishFormat { get; set; }
        
        /// <summary>
        /// Gets or sets the quality of service level.
        /// </summary>
        public int QosLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to set the retained flag.
        /// </summary>
        public bool Retain { get; set; }

        /// <summary>
        /// Gets or sets the maximum queue size.
        /// </summary>
        public int MaxQueueSize { get; set; }

        /// <summary>
        /// Gets or sets the data lifetime in the queue, in seconds.
        /// </summary>
        public int DataLifetime { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to write detailed information to the log.
        /// </summary>
        public bool DetailedLog { get; set; }

        /// <summary>
        /// Gets the device IDs that filter data sent to the server.
        /// </summary>
        public List<int> DeviceFilter { get; private set; }


        /// <summary>
        /// Adds the options to the list.
        /// </summary>
        public void AddToOptionList(OptionList options, bool clearList = true)
        {
            if (clearList)
                options.Clear();

            options["RootTopic"] = RootTopic;
            options["UndefinedValue"] = UndefinedValue;
            options["PublishFormat"] = PublishFormat;
            options["QosLevel"] = QosLevel.ToString();
            options["Retain"] = Retain.ToString();
            options["MaxQueueSize"] = MaxQueueSize.ToString();
            options["DataLifetime"] = DataLifetime.ToString();
            options["DetailedLog"] = DetailedLog.ToString();
            options["DeviceFilter"] = DeviceFilter.ToRangeString();
        }
    }
}
