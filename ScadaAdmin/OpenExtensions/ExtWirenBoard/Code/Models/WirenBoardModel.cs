// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Admin.Extensions.ExtWirenBoard.Code.Models
{
    /// <summary>
    /// Represents a Wiren Board data model.
    /// <para>Представляет модель данных Wiren Board.</para>
    /// </summary>
    internal class WirenBoardModel
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public WirenBoardModel()
        {
            Devices = new List<DeviceModel>();
        }


        /// <summary>
        /// Gets the device models.
        /// </summary>
        public List<DeviceModel> Devices { get; }


        /// <summary>
        /// Parses the topic and adds to the model.
        /// </summary>
        public void AddTopic(string topic, string payload)
        {

        }
    }
}
