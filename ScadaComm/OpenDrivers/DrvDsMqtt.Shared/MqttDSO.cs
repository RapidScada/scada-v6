// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvMqtt;
using Scada.Config;

namespace Scada.Comm.Drivers.DrvDsMqtt
{
    /// <summary>
    /// Represents data source options.
    /// <para>Представляет параметры источника данных.</para>
    /// </summary>
    internal class MqttDSO
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MqttDSO(OptionList options)
        {
            ConnectionOptions = new MqttConnectionOptions(options);
            PublishOptions = new PublishOptions(options);
        }


        /// <summary>
        /// Gets the connection options.
        /// </summary>
        public MqttConnectionOptions ConnectionOptions { get; }

        /// <summary>
        /// Gets the publish options.
        /// </summary>
        public PublishOptions PublishOptions { get; }


        /// <summary>
        /// Adds the options to the list.
        /// </summary>
        public void AddToOptionList(OptionList options)
        {
            options.Clear();
            ConnectionOptions.AddToOptionList(options, false);
            PublishOptions.AddToOptionList(options, false);
        }
    }
}
