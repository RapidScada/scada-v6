// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvOpcUa.View
{
    /// <summary>
    /// Represents an object associated with a monitored item configuration.
    /// <para>Представляет объект, связанный с конфигурацией отслеживаемого элемента.</para>
    /// </summary>
    internal class ItemConfigTag
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ItemConfigTag(int tagNum)
        {
            TagNum = tagNum;
        }


        /// <summary>
        /// Gets or sets the tag number.
        /// </summary>
        public int TagNum { get; set; }

        /// <summary>
        /// Gets a string representation of the tag number.
        /// </summary>
        public string TagNumStr => TagNum.ToString();
    }
}
