// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Scada.AB
{
    /// <summary>
    /// Represents a contact.
    /// <para>Представляет контакт.</para>
    /// </summary>
    public class Contact : AddressBookItem, ITreeNode
    {
        private List<string> phoneNumbers;
        private List<string> emails;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Contact()
            : this("")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Contact(string name)
            : base()
        {
            phoneNumbers = null;
            emails = null;

            Name = name ?? "";
            ContactItems = new List<AddressBookItem>();
            Children = ContactItems;
        }


        /// <summary>
        /// Gets the sort order of the item type.
        /// </summary>
        public override int Order => 1;

        /// <summary>
        /// Ges or sets the contact name.
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
        /// Gets the sorted contact items.
        /// </summary>
        public List<AddressBookItem> ContactItems { get; private set; }

        /// <summary>
        /// Gets the phone numbers of the contact.
        /// </summary>
        public List<string> PhoneNumbers
        {
            get
            {
                if (phoneNumbers == null)
                    FillPhoneNumbers();

                return phoneNumbers;
            }
        }

        /// <summary>
        /// Gets the emails of the contact.
        /// </summary>
        public List<string> Emails
        {
            get
            {
                if (emails == null)
                    FillEmails();

                return emails;
            }
        }


        /// <summary>
        /// Fills the list of phone numbers from the contact items.
        /// </summary>
        public void FillPhoneNumbers()
        {
            phoneNumbers = new List<string>();

            foreach (AddressBookItem item in ContactItems)
            {
                if (item is PhoneNumber)
                    phoneNumbers.Add(item.Value);
            }
        }

        /// <summary>
        /// Fills the list of emails from the contact items.
        /// </summary>
        public void FillEmails()
        {
            emails = new List<string>();

            foreach (AddressBookItem item in ContactItems)
            {
                if (item is Email)
                    emails.Add(item.Value);
            }
        }
    }
}
