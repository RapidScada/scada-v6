// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.AB
{
    /// <summary>
    /// Represents an email.
    /// <para>Представляет адрес электронной почты.</para>
    /// </summary>
    public class Email : AddressBookItem
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Email()
            : this("")
        {

        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Email(string value)
            : base()
        {
            Value = value;
        }

        /// <summary>
        /// Gets the sort order of the item type.
        /// </summary>
        public override int Order => 3;
    }
}
