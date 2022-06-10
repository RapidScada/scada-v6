/*
 * Copyright 2022 Rapid Software LLC
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
 * Module   : ScadaCommCommon
 * Summary  : Provides a base class for custom device configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

using Scada.Comm.Lang;
using Scada.Config;
using System;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Provides a base class for custom device configuration.
    /// <para>Предоставляет базовый класс для пользовательский конфигурации устройства.</para>
    /// </summary>
    [Serializable]
    public abstract class DeviceConfigBase : ConfigBase
    {
        /// <summary>
        /// Builds an error message for the load operation.
        /// </summary>
        protected override string BuildLoadErrorMessage(Exception ex)
        {
            return ex.BuildErrorMessage(CommPhrases.LoadDeviceConfigError);
        }

        /// <summary>
        /// Builds an error message for the save operation.
        /// </summary>
        protected override string BuildSaveErrorMessage(Exception ex)
        {
            return ex.BuildErrorMessage(CommPhrases.SaveDeviceConfigError);
        }

        /// <summary>
        /// Gets the short name of the device configuration file.
        /// </summary>
        public static string GetFileName(string driverCode, int deviceNum)
        {
            return $"{driverCode}_{deviceNum:D3}.xml";
        }
    }
}
