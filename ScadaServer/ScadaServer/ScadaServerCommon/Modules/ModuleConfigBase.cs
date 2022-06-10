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
 * Module   : ScadaServerCommon
 * Summary  : Provides a base class for module configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

using Scada.Config;
using Scada.Server.Lang;
using System;

namespace Scada.Server.Modules
{
    /// <summary>
    /// Provides a base class for module configuration.
    /// <para>Предоставляет базовый класс для конфигурации модуля.</para>
    /// </summary>
    [Serializable]
    public abstract class ModuleConfigBase : ConfigBase
    {
        /// <summary>
        /// Builds an error message for the load operation.
        /// </summary>
        protected override string BuildLoadErrorMessage(Exception ex)
        {
            return ex.BuildErrorMessage(ServerPhrases.LoadModuleConfigError);
        }

        /// <summary>
        /// Builds an error message for the save operation.
        /// </summary>
        protected override string BuildSaveErrorMessage(Exception ex)
        {
            return ex.BuildErrorMessage(ServerPhrases.SaveModuleConfigError);
        }
    }
}
