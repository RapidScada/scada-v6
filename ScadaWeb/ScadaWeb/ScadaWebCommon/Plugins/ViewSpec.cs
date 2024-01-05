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
 * Module   : ScadaWebCommon
 * Summary  : Represents a view specification
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2023
 */

namespace Scada.Web.Plugins
{
    /// <summary>
    /// Represents a view specification.
    /// <para>Представляет спецификацию представления.</para>
    /// </summary>
    public abstract class ViewSpec
    {
        /// <summary>
        /// Gets the view type code.
        /// </summary>
        public abstract string TypeCode { get; }

        /// <summary>
        /// Gets the extension of view files.
        /// </summary>
        public abstract string FileExtension { get; }

        /// <summary>
        /// Gets the view icon URL.
        /// </summary>
        public abstract string IconUrl { get; }

        /// <summary>
        /// Gets the view type.
        /// </summary>
        public abstract Type ViewType { get; }


        /// <summary>
        /// Gets the view frame URL.
        /// </summary>
        public abstract string GetFrameUrl(int viewID);
    }
}
