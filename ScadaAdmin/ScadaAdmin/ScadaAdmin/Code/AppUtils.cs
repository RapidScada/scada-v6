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
 * Module   : Administrator
 * Summary  : The class provides helper methods for the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2021
 */

using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.Text;
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
        /// Gets a list of archive bits.
        /// </summary>
        public static List<BitItem> GetArchiveBits(BaseTable<Archive> archiveTable)
        {
            if (archiveTable == null)
                throw new ArgumentNullException(nameof(archiveTable));

            List<BitItem> archiveBits = new();

            foreach (Archive archive in archiveTable.EnumerateItems())
            {
                archiveBits.Add(new BitItem(archive.Bit, archive.Name));
            }

            return archiveBits;
        }

        /// <summary>
        /// Gets a list of event bits.
        /// </summary>
        public static List<BitItem> GetEventBits()
        {
            return new List<BitItem>
            {
                new BitItem(EventMask.EnabledBit, AppPhrases.EventEnabled),
                new BitItem(EventMask.BeepBit, AppPhrases.EventBeep),
                new BitItem(EventMask.DataChangeBit, AppPhrases.DataChangeEvent),
                new BitItem(EventMask.ValueChangeBit, AppPhrases.ValueChangeEvent),
                new BitItem(EventMask.StatusChangeBit, AppPhrases.StatusChangeEvent),
                new BitItem(EventMask.CnlUndefinedBit, AppPhrases.CnlUndefinedEvent)
            };
        }

        /// <summary>
        /// Sets the check box state according to the cell value.
        /// </summary>
        public static void SetChecked(this CheckBox checkBox, DataGridViewCell cell)
        {
            if (cell == null)
                throw new ArgumentNullException(nameof(cell));
            
            checkBox.Checked = (bool)cell.Value;
        }

        /// <summary>
        /// Sets the text box according to the cell value.
        /// </summary>
        public static void SetText(this TextBox textBox, DataGridViewCell cell)
        {
            if (cell == null)
                throw new ArgumentNullException(nameof(cell));

            textBox.Text = Convert.ToString(cell.Value);
        }

        /// <summary>
        /// Sets the combo box value according to the cell value.
        /// </summary>
        public static void SetValue(this ComboBox comboBox, DataGridViewCell cell)
        {
            if (cell == null)
                throw new ArgumentNullException(nameof(cell));

            if (cell.OwningColumn is DataGridViewComboBoxColumn comboBoxColumn)
            {
                comboBox.DisplayMember = comboBoxColumn.DisplayMember;
                comboBox.ValueMember = comboBoxColumn.ValueMember;
                comboBox.DataSource = comboBoxColumn.DataSource;
                comboBox.SelectedValue = cell.Value;
            }
        }

        /// <summary>
        /// Appends the error text to the string builder.
        /// </summary>
        public static void AppendError(this StringBuilder stringBuilder, Label label, 
            string text, params object[] args)
        {
            if (label == null)
                throw new ArgumentNullException(nameof(label));

            stringBuilder.Append(label.Text).Append(": ").AppendLine(ScadaUtils.FormatText(text, args));
        }
    }
}
