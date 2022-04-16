// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Scada.AB
{
    /// <summary>
    /// Represents a contact group.
    /// <para>Представляет контактную группу.</para>
    /// </summary>
    public class ContactGroup : AddressBookItem, ITreeNode
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ContactGroup()
            : this("")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ContactGroup(string name)
            : base()
        {
            Name = name ?? "";
            Contacts = new List<Contact>();
            Children = Contacts;
        }


        /// <summary>
        /// Gets the sort order of the item type.
        /// </summary>
        public override int Order => 0;

        /// <summary>
        /// Ges or sets the group name.
        /// </summary>
        public string Name
        {
            get
            {
                return Value;
            }
            set
            {
                Value = value;
            }
        }

        /// <summary>
        /// Gets the contacts sorted by name.
        /// </summary>
        public List<Contact> Contacts { get; private set; }
    }
}
