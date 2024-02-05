/*
 * Copyright 2024 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaCommon
 * Summary  : Represents a wrapper for safely calling methods of storage logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2023
 */

using Scada.Lang;
using System;

namespace Scada.Storages
{
    /// <summary>
    /// Represents a wrapper for safely calling methods of storage logic.
    /// <param>Представляет обёртку для безопасного выполнения методов логики хранилища.</param>
    /// </summary>
    public class StorageWrapper
    {
        private readonly StorageContext storageContext;
        private StorageLogic storageLogic;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public StorageWrapper(StorageContext storageContext)
        {
            this.storageContext = storageContext ?? throw new ArgumentNullException(nameof(storageContext));
            storageLogic = null;
        }


        /// <summary>
        /// Gets the application storage.
        /// </summary>
        public IStorage Storage => storageLogic ?? throw new ScadaException("Storage is not initialized.");


        /// <summary>
        /// Initializes the application storage.
        /// </summary>
        public bool InitStorage()
        {
            if (StorageFactory.GetStorage(storageContext.AppDirs.ExeDir, storageContext.InstanceConfig.ActiveStorage,
                storageContext, out storageLogic, out string message))
            {
                storageContext.Log.WriteAction(message);
            }
            else
            {
                storageContext.Log.WriteError(message);
                return false;
            }

            try
            {
                storageLogic.LoadConfig(storageContext.InstanceConfig.GetActiveStorageXml());
                storageLogic.MakeReady();
                storageLogic.IsReady = true;
                return true;
            }
            catch (Exception ex)
            {
                storageContext.Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при инициализации хранилища приложения" :
                    "Error initializing application storage");
                return false;
            }
        }

        /// <summary>
        /// Closes the application storage.
        /// </summary>
        public void CloseStorage()
        {
            try
            {
                if (storageLogic != null)
                {
                    storageLogic.Close();
                    storageLogic.IsReady = true;
                }
            }
            catch (Exception ex)
            {
                storageContext.Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при закрытии хранилища приложения" :
                    "Error closing application storage");
            }
        }
    }
}
