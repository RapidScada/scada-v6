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
 * Module   : Administrator
 * Summary  : The class provides helper methods for the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2023
 */

using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// The class provides helper methods for the application.
    /// <para>Класс, предоставляющий вспомогательные методы для приложения.</para>
    /// </summary>
    internal static class AppUtils
    {
        /// <summary>
        /// The hyperlink to the documentation.
        /// </summary>
        public const string DocUrl = "https://rapidscada.net/docs";
        /// <summary>
        /// The hyperlink to the support in English.
        /// </summary>
        public const string SupportEnUrl = "https://forum.rapidscada.org/";
        /// <summary>
        /// The hyperlink to the support in Russian.
        /// </summary>
        public const string SupportRuUrl = "https://forum.rapidscada.ru/";


        /// <summary>
        /// Sets the check box state according to the cell value.
        /// </summary>
        public static void SetChecked(this CheckBox checkBox, DataGridViewCell cell)
        {
            ArgumentNullException.ThrowIfNull(cell, nameof(cell));
            checkBox.Checked = (bool)cell.Value;
        }

        /// <summary>
        /// Sets the text box according to the cell value.
        /// </summary>
        public static void SetText(this TextBox textBox, DataGridViewCell cell)
        {
            ArgumentNullException.ThrowIfNull(cell, nameof(cell));
            textBox.Text = Convert.ToString(cell.Value);
        }

        /// <summary>
        /// Sets the combo box value according to the cell value.
        /// </summary>
        public static void SetValue(this ComboBox comboBox, DataGridViewCell cell)
        {
            ArgumentNullException.ThrowIfNull(cell, nameof(cell));

            if (cell.OwningColumn is DataGridViewComboBoxColumn comboBoxColumn)
            {
                comboBox.DisplayMember = comboBoxColumn.DisplayMember;
                comboBox.ValueMember = comboBoxColumn.ValueMember;
                comboBox.DataSource = comboBoxColumn.DataSource;
                comboBox.SelectedValue = cell.Value;
            }
        }

        /// <summary>
        /// Gets the lowercase extension of the specified file, not including the period.
        /// </summary>
        public static string GetExtensionLower(string fileName)
        {
            return Path.GetExtension(fileName).TrimStart('.').ToLowerInvariant();
        }
    }
}
