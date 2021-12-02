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
        public ItemConfigTag(int tagNum, bool isArray, int arrayLen)
        {
            TagNum = tagNum;
            SetLength(isArray, arrayLen);
        }


        /// <summary>
        /// Gets or sets the tag number.
        /// </summary>
        public int TagNum { get; set; }

        /// <summary>
        /// Gets or sets the quantity of tags.
        /// </summary>
        public int Length { get; set; }


        /// <summary>
        /// Normalizes and sets the quantity of tags.
        /// </summary>
        public void SetLength(bool isArray, int arrayLen)
        {
            Length = isArray && arrayLen > 1 ? arrayLen : 1;
        }

        /// <summary>
        /// Gets a range of tag numbers.
        /// </summary>
        public string GetTagNumInfo()
        {
            return Length > 1 ?
                TagNum + " - " + (TagNum + Length - 1) :
                TagNum.ToString();
        }
    }
}
