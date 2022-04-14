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
 * Module   : ScadaCommon
 * Summary  : Specifies the category displayed in PropertyGrid which can be changed programmatically
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2022
 */

namespace Scada.ComponentModel
{
    /// <summary>
    /// Specifies the category displayed in PropertyGrid which can be changed programmatically.
    /// <para>Задаёт категорию, отображаемую в PropertyGrid, которая может быть изменена программно.</para>
    /// </summary>
    public class CategoryAttribute : System.ComponentModel.CategoryAttribute
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CategoryAttribute()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CategoryAttribute(string category)
        {
            CategoryName = category;
        }


        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        public string CategoryName { get; set; }


        /// <summary>
        /// Looks up the localized name of the specified category.
        /// </summary>
        protected override string GetLocalizedString(string value)
        {
            return CategoryName;
        }
    }
}
