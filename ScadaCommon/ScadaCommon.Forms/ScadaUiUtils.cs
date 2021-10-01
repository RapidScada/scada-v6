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
using Scada.Log;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
        public static void ShowInfo(string text, params object[] args)
        {
            MessageBox.Show(ScadaUtils.FormatText(text, args).Trim(), CommonPhrases.InfoCaption,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Shows an error message.
        /// </summary>
        public static void ShowError(string text, params object[] args)
        {
            MessageBox.Show(ScadaUtils.FormatText(text, args).Trim(), CommonPhrases.ErrorCaption,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Shows a warning message.
        /// </summary>
        public static void ShowWarning(string text, params object[] args)
        {
            MessageBox.Show(ScadaUtils.FormatText(text, args).Trim(), CommonPhrases.WarningCaption,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Writes the error to the log and displays a error message.
        /// </summary>
        public static void HandleError(this ILog log, string text)
        {
            log.WriteError(text);
            ShowError(text);
        }

        /// <summary>
        /// Writes the error to the log and displays a error message.
        /// </summary>
        public static void HandleError(this ILog log, Exception ex, string text = "", params object[] args)
        {
            string msg = ScadaUtils.BuildErrorMessage(ex, text, args);
            log.WriteError(msg);
            ShowError(msg);
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

        /// <summary>
        /// Loads a background image and hyperlink for the about form.
        /// </summary>
        public static bool LoadAboutForm(string exeDir, Form frmAbout, PictureBox pictureBox, Label lblLink,
            out bool imgLoaded, out string linkUrl, out string errMsg)
        {
            imgLoaded = false;
            linkUrl = CommonPhrases.WebsiteUrl;
            errMsg = "";

            // load background image if file exists
            try
            {
                string imgFileName = exeDir + "About.jpg";

                if (File.Exists(imgFileName))
                {
                    Image image = Image.FromFile(imgFileName);
                    pictureBox.Image = image;
                    imgLoaded = true;

                    // check and fix form size and image size
                    int width;
                    if (image.Width < 100)
                        width = 100;
                    else if (image.Width > 800)
                        width = 800;
                    else
                        width = image.Width;

                    int height;
                    if (image.Height < 100)
                        height = 100;
                    else if (image.Height > 600)
                        height = 600;
                    else
                        height = image.Height;

                    frmAbout.Width = pictureBox.Width = width;
                    frmAbout.Height = pictureBox.Height = height;
                }
            }
            catch (OutOfMemoryException)
            {
                errMsg = Locale.IsRussian ?
                    "Ошибка при загрузке изображения из файла:\r\nНекорректный формат файла." :
                    "Error loading image from file:\r\nIncorrect file format.";
            }
            catch (Exception ex)
            {
                errMsg = string.Format(Locale.IsRussian ?
                    "Ошибка при загрузке изображения из файла:\r\n{0}" :
                    "Error loading image from file:\r\n{0}", ex.Message);
            }

            if (errMsg == "")
            {
                // load hyperlink if file exists
                StreamReader reader = null;

                try
                {
                    string linkFileName = exeDir + "About.txt";

                    if (File.Exists(linkFileName))
                    {
                        reader = new StreamReader(linkFileName, Encoding.Default);
                        linkUrl = reader.ReadLine();

                        if (string.IsNullOrEmpty(linkUrl))
                        {
                            lblLink.Visible = false;
                        }
                        else
                        {
                            linkUrl = linkUrl.Trim();
                            string pos = reader.ReadLine();

                            if (!string.IsNullOrEmpty(pos))
                            {
                                string[] parts = pos.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                if (parts.Length >= 4 && 
                                    int.TryParse(parts[0], out int x) && 
                                    int.TryParse(parts[1], out int y) &&
                                    int.TryParse(parts[2], out int w) && 
                                    int.TryParse(parts[3], out int h))
                                {
                                    // check link location and size
                                    if (x < 0)
                                        x = 0;
                                    else if (x >= frmAbout.Width)
                                        x = frmAbout.Width - 1;
                                    if (y < 0)
                                        y = 0;
                                    else if (y >= frmAbout.Height)
                                        y = frmAbout.Height - 1;

                                    if (x + w >= frmAbout.Width)
                                        w = frmAbout.Width - x;
                                    if (w <= 0)
                                        w = 1;
                                    if (y + h >= frmAbout.Height)
                                        h = frmAbout.Height - y;
                                    if (h <= 0)
                                        h = 1;

                                    lblLink.Left = x;
                                    lblLink.Top = y;
                                    lblLink.Width = w;
                                    lblLink.Height = h;
                                    lblLink.Visible = true;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    linkUrl = "";
                    lblLink.Visible = false;
                    errMsg = string.Format(Locale.IsRussian ?
                        "Ошибка при загрузке гиперссылки из файла:\r\n{0}" :
                        "Error loading hyperlink from file:\r\n{0}", ex.Message);
                }
                finally
                {
                    reader?.Close();
                }
            }
            else
            {
                lblLink.Visible = false;
            }

            return errMsg == "";
        }

        /// <summary>
        /// Starts a new process.
        /// </summary>
        public static void StartProcess(string fileName, string arguments = "")
        {
            Process.Start(new ProcessStartInfo(fileName, arguments) { UseShellExecute = true });
        }
    }
}
