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
 * Module   : ScadaWebCommon
 * Summary  : Specified the form operating modes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2025
 * Modified : 2025
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Web.Pages
{
    /// <summary>
    /// Specified the form operating modes.
    /// <para>Задаёт режимы работы формы.</para>
    /// </summary>
    public enum FormMode
    {
        Undefined,
        Create,
        Edit,
        View
    }
}
