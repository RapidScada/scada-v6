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
    internal class PublishOptions : IPublishOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PublishOptions(OptionList options)
        {
            RootTopic = options.GetValueAsString("RootTopic", "Communicator");
            UndefinedValue = options.GetValueAsString("UndefinedValue", "NaN");
            PublishFormat = options.GetValueAsString("PublishFormat");
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
        /// Adds the options to the list.
        /// </summary>
        public void AddToOptionList(OptionList options, bool clearList = true)
        {
            if (clearList)
                options.Clear();

            options["RootTopic"] = RootTopic;
            options["UndefinedValue"] = UndefinedValue;
            options["PublishFormat"] = PublishFormat;
        }
    }
}
