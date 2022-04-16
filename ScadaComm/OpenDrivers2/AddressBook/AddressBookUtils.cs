// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using Scada.Log;
using Scada.Storages;
using System;
using System.Collections.Generic;

namespace Scada.AB
{
    /// <summary>
    /// The class provides helper methods with an address book.
    /// <para>Класс, предоставляющий вспомогательные методы с адресной книгой.</para>
    /// </summary>
    public static class AddressBookUtils
    {
        /// <summary>
        /// Gets the address book key in the shared data.
        /// </summary>
        private const string AddressBookKey = "AddressBook";

        /// <summary>
        /// Gets an address book from the shared data, or loads from the storage if the book is missing.
        /// </summary>
        public static AddressBook GetOrLoad(IDictionary<string, object> sharedData, IStorage storage, ILog log)
        {
            if (sharedData == null)
                throw new ArgumentNullException(nameof(sharedData));
            if (storage == null)
                throw new ArgumentNullException(nameof(storage));
            if (log == null)
                throw new ArgumentNullException(nameof(log));

            AddressBook addressBook = sharedData.ContainsKey(AddressBookKey) ? 
                sharedData[AddressBookKey] as AddressBook : null;

            if (addressBook == null)
            {
                addressBook = new AddressBook();
                sharedData.Add(AddressBookKey, addressBook);

                // load address book
                if (storage.GetFileInfo(DataCategory.Config, AddressBook.DefaultFileName).Exists)
                {
                    if (!addressBook.Load(storage, AddressBook.DefaultFileName, out string errMsg))
                        log.WriteLine(errMsg);
                }
                else
                {
                    log.WriteLine(Locale.IsRussian ?
                        "Адресная книга отсутствует" :
                        "Address book is missing");
                }
            }

            return addressBook;
        }
    }
}
