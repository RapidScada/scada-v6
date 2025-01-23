/*
 * Copyright 2025 Rapid Software LLC
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
 * Summary  : Represents information about a library registration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2025
 * Modified : 2025
 */

namespace Scada
{
    /// <summary>
    /// Represents information about a library registration.
    /// <para>Представляет информацию о регистрации библиотеки.</para>
    /// </summary>
    public class RegistrationInfo
    {
        /// <summary>
        /// Gets or sets the product code.
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the URL to get a permanent key.
        /// </summary>
        public string PermanentKeyUrl { get; set; }

        /// <summary>
        /// Gets or sets the default URL to get a trial key.
        /// </summary>
        public string TrialKeyUrl { get; set; }
    }
}
