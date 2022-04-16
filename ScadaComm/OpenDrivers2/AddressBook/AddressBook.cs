// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.AB
{
    /// <summary>
    /// Represents an address book.
    /// <para>Представляет адресную книгу.</para>
    /// </summary>
    public class AddressBook : BaseConfig, ITreeNode
    {
        /// <summary>
        /// Gets the default file name of the address book.
        /// </summary>
        public const string DefaultFileName = "AddressBook.xml";

        private List<Contact> allContacts; // the sorted contacts


        /// <summary>
        /// Gets the contact groups sorted by name.
        /// </summary>
        public List<ContactGroup> ContactGroups { get; private set; }

        /// <summary>
        /// Gets the contacts sorted by name.
        /// </summary>
        public List<Contact> AllContacts
        {
            get
            {
                if (allContacts == null)
                    FillAllContacts();

                return allContacts;
            }
        }

        /// <summary>
        /// Gets or sets the parent tree node.
        /// </summary>
        ITreeNode ITreeNode.Parent
        {
            get
            {
                return null;
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the child tree nodes.
        /// </summary>
        IList ITreeNode.Children
        {
            get
            {
                return ContactGroups;
            }
        }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            allContacts = null;
            ContactGroups = new List<ContactGroup>();
        }

        /// <summary>
        /// Loads the address book from the specified reader.
        /// </summary>
        protected override void Load(TextReader reader)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);

            foreach (XmlElement contactGroupElem in xmlDoc.DocumentElement.SelectNodes("ContactGroup"))
            {
                ContactGroup contactGroup = new ContactGroup(contactGroupElem.GetAttribute("name"))
                {
                    Parent = this
                };

                foreach (XmlElement contactElem in contactGroupElem.SelectNodes("Contact"))
                {
                    Contact contact = new Contact(contactElem.GetAttribute("name"))
                    {
                        Parent = contactGroup
                    };

                    foreach (XmlElement phoneNumberElem in contactElem.SelectNodes("PhoneNumber"))
                    {
                        contact.ContactItems.Add(
                            new PhoneNumber(phoneNumberElem.InnerText) { Parent = contact });
                    }

                    foreach (XmlElement emailElem in contactElem.SelectNodes("Email"))
                    {
                        contact.ContactItems.Add(
                            new Email(emailElem.InnerText) { Parent = contact });
                    }

                    contact.ContactItems.Sort();
                    contactGroup.Contacts.Add(contact);
                }

                contactGroup.Contacts.Sort();
                ContactGroups.Add(contactGroup);
            }

            ContactGroups.Sort();
        }

        /// <summary>
        /// Saves the address book to the specified writer.
        /// </summary>
        protected override void Save(TextWriter writer)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("AddressBook");
            xmlDoc.AppendChild(rootElem);

            foreach (ContactGroup contactGroup in ContactGroups)
            {
                XmlElement contactGroupElem = xmlDoc.CreateElement("ContactGroup");
                contactGroupElem.SetAttribute("name", contactGroup.Name);
                rootElem.AppendChild(contactGroupElem);

                foreach (Contact contact in contactGroup.Contacts)
                {
                    XmlElement contactElem = xmlDoc.CreateElement("Contact");
                    contactElem.SetAttribute("name", contact.Name);

                    foreach (AddressBookItem contactItem in contact.ContactItems)
                    {
                        if (contactItem is PhoneNumber)
                            contactElem.AppendElem("PhoneNumber", contactItem);
                        else if (contactItem is Email)
                            contactElem.AppendElem("Email", contactItem);
                    }

                    contactGroupElem.AppendChild(contactElem);
                }
            }

            xmlDoc.Save(writer);
        }


        /// <summary>
        /// Finds a contact group by name.
        /// </summary>
        public ContactGroup FindContactGroup(string name)
        {
            int i = ContactGroups.BinarySearch(new ContactGroup(name));
            return i >= 0 ? ContactGroups[i] : null;
        }

        /// <summary>
        /// Finds a contact by name.
        /// </summary>
        public Contact FindContact(string name)
        {
            int i = AllContacts.BinarySearch(new Contact(name));
            return i >= 0 ? AllContacts[i] : null;
        }

        /// <summary>
        /// Fills the list of contacts from the contact groups.
        /// </summary>
        public void FillAllContacts()
        {
            allContacts = new List<Contact>();

            foreach (ContactGroup contactGroup in ContactGroups)
            {
                allContacts.AddRange(contactGroup.Contacts);
            }

            allContacts.Sort();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return "Address book";
        }
    }
}
