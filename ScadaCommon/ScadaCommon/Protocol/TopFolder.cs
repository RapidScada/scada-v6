/*
 * Copyright 2021 Rapid Software LLC
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
 * Summary  : Specifies the top level folders
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Protocol
{
    /// <summary>
    /// Specifies the top level folders.
    /// <para>Определяет папки верхнего уровня.</para>
    /// </summary>
    public enum TopFolder : byte
    {
        Undefined = 0,
        Archive = 1,
        ArchiveCopy = 2,
        Base = 3,
        View = 4,
        Server = 5,
        Comm = 6,
        Web = 7
    }
}
