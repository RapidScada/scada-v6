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
 * Module   : ScadaCommon.Forms
 * Summary  : The class provides helper methods that works with Windows Forms
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2021
 */

using Scada.Lang;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Scada.Forms
{
    /// <summary>
    /// The class provides helper methods that works with Windows Forms.
    /// <para>Класс, предоставляющий вспомогательные методы, которые работают с Windows Forms.</para>
    /// </summary>
    public static class ScadaUiUtils
    {
        /// <summary>
        /// The threshold for the number of table rows for selecting the autosize mode.
        /// </summary>
        private const int GridAutoResizeBoundary = 100;
        /// <summary>
        /// The maximum column width in DataGridView in pixels.
        /// </summary>
        private const int MaxColumnWidth = 500;


        /// <summary>
        /// Shows an informational message.
        /// </summary>
        public static void ShowInfo(string text)
        {
            MessageBox.Show(text?.Trim(), CommonPhrases.InfoCaption,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Shows an error message.
        /// </summary>
        public static void ShowError(string text)
        {
            MessageBox.Show(text?.Trim(), CommonPhrases.ErrorCaption,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Shows a warning message.
        /// </summary>
        public static void ShowWarning(string text)
        {
            MessageBox.Show(text?.Trim(), CommonPhrases.WarningCaption,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Sets the value of the NumericUpDown control within its valid range.
        /// </summary>
        public static void SetValue(this NumericUpDown numericUpDown, decimal val)
        {
            if (val < numericUpDown.Minimum)
                numericUpDown.Value = numericUpDown.Minimum;
            else if (val > numericUpDown.Maximum)
                numericUpDown.Value = numericUpDown.Maximum;
            else
                numericUpDown.Value = val;
        }

        /// <summary>
        /// Sets the time of the DateTimePicker control.
        /// </summary>
        public static void SetTime(this DateTimePicker dateTimePicker, DateTime time)
        {
            DateTime date = dateTimePicker.MinDate;
            dateTimePicker.Value = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
        }

        /// <summary>
        /// Sets the time of the DateTimePicker control.
        /// </summary>
        public static void SetTime(this DateTimePicker dateTimePicker, TimeSpan timeSpan)
        {
            DateTime date = dateTimePicker.MinDate;
            dateTimePicker.Value = new DateTime(date.Year, date.Month, date.Day).Add(timeSpan);
        }

        /// <summary>
        /// Sets the file dialog filter suppressing possible exception.
        /// </summary>
        public static void SetFilter(this FileDialog fileDialog, string filter)
        {
            try { fileDialog.Filter = filter; }
            catch { }
        }

        /// <summary>
        /// Adjusts the width of all columns using the mode depending on row number.
        /// </summary>
        public static void AutoSizeColumns(this DataGridView dataGridView)
        {
            if (dataGridView.RowCount <= GridAutoResizeBoundary)
                dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            else
                dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Width > MaxColumnWidth)
                    column.Width = MaxColumnWidth;
            }
        }

        /// <summary>
        /// Tests whether the specified area is visible on any of the available screens.
        /// </summary>
        public static bool AreaIsVisible(int x, int y, int width, int height)
        {
            Rectangle rect = new(x, y, width, height);
            return Screen.AllScreens.Any(screen => screen.Bounds.IntersectsWith(rect));
        }
    }
}
