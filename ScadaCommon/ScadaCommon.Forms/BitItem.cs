// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Forms
{
    /// <summary>
    /// Represents a bitmask item.
    /// <para>Представляет элемент битовой маски.</para>
    /// </summary>
    public class BitItem
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BitItem(int bit, string descr)
        {
            Bit = bit;
            Descr = descr;
        }


        /// <summary>
        /// Gets a bit number.
        /// </summary>
        public int Bit { get; }

        /// <summary>
        /// Gets a bit description.
        /// </summary>
        public string Descr { get; }


        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString() => $"{Bit}: {Descr}";
    }
}
