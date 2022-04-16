// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.AB
{
    /// <summary>
    /// Represents a phone number.
    /// <para>Представляет телефонный номер.</para>
    /// </summary>
    public class PhoneNumber : AddressBookItem
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PhoneNumber()
            : this("")
        {

        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PhoneNumber(string value)
            : base()
        {
            Value = value;
        }

        /// <summary>
        /// Gets the sort order of the item type.
        /// </summary>
        public override int Order => 2;
    }
}
