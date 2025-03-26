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
 * Summary  : Represents formatted channel data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2024
 */

using Scada.Data.Entities;
using System;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents formatted channel data.
    /// <para>Представляет форматированные данные канала.</para>
    /// </summary>
    public struct CnlDataFormatted
    {
        /// <summary>
        /// Gets or sets the displayed channel value.
        /// </summary>
        public string DispVal { get; set; }

        /// <summary>
        /// Gets or sets the main, second and background colors.
        /// </summary>
        public string[] Colors { get; set; }


        /// <summary>
        /// Gets the main color, or null if it does not exist.
        /// </summary>
        public string GetMainColor()
        {
            return GetMainColor(out string color) ? color : null;
        }

        /// <summary>
        /// Gets the main color.
        /// </summary>
        public bool GetMainColor(out string color)
        {
            if (Colors != null && Colors.Length > 0)
            {
                color = Colors[0];
                return true;
            }
            else
            {
                color = null;
                return false;
            }
        }

        /// <summary>
        /// Sets the main color keeping the others unchanged.
        /// </summary>
        public void SetMainColor(string color)
        {
            if (Colors == null || Colors.Length == 0)
                Colors = new string[] { color };
            else
                Colors[0] = color;
        }

        /// <summary>
        /// Sets the default colors.
        /// </summary>
        public void SetColorsToDefault()
        {
            Colors = Array.Empty<string>();
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
                Colors = new string[]
                {
                    cnlStatus.MainColor ?? "",
                    cnlStatus.SecondColor ?? "",
                    cnlStatus.BackColor ?? ""
                };
            }
        }
    }
}
