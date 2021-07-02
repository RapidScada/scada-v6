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
 * Summary  : Represents formatted input channel data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Data.Entities;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents formatted input channel data.
    /// <para>Представляет форматированные данные входного канала.</para>
    /// </summary>
    public struct CnlDataFormatted
    {
        /// <summary>
        /// The default color for displaying input channel values.
        /// </summary>
        public const string DefaultColor = "Black";


        /// <summary>
        /// Gets or sets the displayed channel value.
        /// </summary>
        public string DispVal { get; set; }

        /// <summary>
        /// Gets or sets the main color of the channel value.
        /// </summary>
        public string Color1 { get; set; }

        /// <summary>
        /// Gets or sets the second color of the channel value.
        /// </summary>
        public string Color2 { get; set; }

        /// <summary>
        /// Gets or sets the background color of the channel value.
        /// </summary>
        public string Color3 { get; set; }


        /// <summary>
        /// Sets the default colors.
        /// </summary>
        public void SetColorsToDefault()
        {
            Color1 = DefaultColor;
            Color2 = DefaultColor;
            Color3 = DefaultColor;
        }

        /// <summary>
        /// Sets colors according to the channel status entity.
        /// </summary>
        public void SetColors(CnlStatus cnlStatus)
        {
            if (cnlStatus == null)
            {
                SetColorsToDefault();
            }
            else
            {
                Color1 = string.IsNullOrEmpty(cnlStatus.MainColor) ? DefaultColor : cnlStatus.MainColor;
                Color2 = string.IsNullOrEmpty(cnlStatus.SecondColor) ? DefaultColor : cnlStatus.SecondColor;
                Color3 = string.IsNullOrEmpty(cnlStatus.BackColor) ? DefaultColor : cnlStatus.BackColor;
            }
        }
    }
}
