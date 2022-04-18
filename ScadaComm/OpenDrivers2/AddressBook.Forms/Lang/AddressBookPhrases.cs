// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.AB.Forms.Lang
{
    /// <summary>
    /// The phrases used by the address book.
    /// <para>Фразы, используемые адресной книгой.</para>
    /// </summary>
    internal class AddressBookPhrases
    {
        // Scada.AB.Forms.FrmAddressBook
        public static string AddressBookNode { get; private set; }
        public static string NewContactGroup { get; private set; }
        public static string NewContact { get; private set; }
        public static string NewPhoneNumber { get; private set; }
        public static string NewEmail { get; private set; }
        public static string ContactGroupExists { get; private set; }
        public static string ContactExists { get; private set; }
        public static string PhoneNumberExists { get; private set; }
        public static string EmailExists { get; private set; }
        public static string IncorrectEmail { get; private set; }
        public static string EmptyValueNotAllowed { get; private set; }
        public static string SaveAddressBookConfirm { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.AB.Forms.FrmAddressBook");
            AddressBookNode = dict[nameof(AddressBookNode)];
            NewContactGroup = dict[nameof(NewContactGroup)];
            NewContact = dict[nameof(NewContact)];
            NewPhoneNumber = dict[nameof(NewPhoneNumber)];
            NewEmail = dict[nameof(NewEmail)];
            ContactGroupExists = dict[nameof(ContactGroupExists)];
            ContactExists = dict[nameof(ContactExists)];
            PhoneNumberExists = dict[nameof(PhoneNumberExists)];
            EmailExists = dict[nameof(EmailExists)];
            IncorrectEmail = dict[nameof(IncorrectEmail)];
            EmptyValueNotAllowed = dict[nameof(EmptyValueNotAllowed)];
            SaveAddressBookConfirm = dict[nameof(SaveAddressBookConfirm)];
        }
    }
}
