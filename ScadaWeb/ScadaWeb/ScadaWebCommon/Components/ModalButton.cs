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
 * Module   : ScadaWebCommon
 * Summary  : Specifies the modal dialog buttons
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Web.Components
{
    /// <summary>
    /// Specifies the modal dialog buttons.
    /// <para>Задаёт кнопки модального диалога.</para>
    /// </summary>
    public class ModalButton
    {
        public const string Ok = "Ok";
        public const string Yes = "Yes";
        public const string No = "No";
        public const string Exec = "Execute";
        public const string Cancel = "Cancel";
        public const string Close = "Close";
    }
}
