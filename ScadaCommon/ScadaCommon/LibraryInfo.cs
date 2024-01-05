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
 * Summary  : Represents information about a library
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2023
 * Modified : 2023
 */

namespace Scada
{
    /// <summary>
    /// Represents information about a library.
    /// <para>Представляет информацию о библиотеке.</para>
    /// </summary>
    public abstract class LibraryInfo
    {
        /// <summary>
        /// Gets the library code.
        /// </summary>
        public abstract string Code { get; }

        /// <summary>
        /// Gets the library name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the library description.
        /// </summary>
        public abstract string Descr { get; }

        /// <summary>
        /// Gets the library version.
        /// </summary>
        public virtual string Version
        {
            get
            {
                return GetType().Assembly.GetName().Version.ToString();
            }
        }
    }
}
