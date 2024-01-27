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
 * Summary  : Specifies the event text formats
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

namespace Scada.Data.Models
{
    /// <summary>
    /// Specifies the event text formats.
    /// <para>Задаёт форматы текста событий.</para>
    /// </summary>
    public enum EventTextFormat
    {
        /// <summary>
        /// Joins automatically generated text and custom text.
        /// </summary>
        Full = 0,

        /// <summary>
        /// Displays only automatically generated text.
        /// </summary>
        AutoText = 1,

        /// <summary>
        /// Displays only custom text.
        /// </summary>
        CustomText = 2,

        /// <summary>
        /// Displays command value and data.
        /// </summary>
        Command = 3
    }
}
