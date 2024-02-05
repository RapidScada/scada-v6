﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using System.Text;

namespace Scada.Forms
{
    /// <summary>
    /// Provides displaying and updating log.
    /// <para>Обеспечивает отображение и обновление журнала.</para>
    /// </summary>
    public class LogBox
    {
        /// <summary>
        /// The default size of displayed part of a log in bytes.
        /// </summary>
        protected const int DefaultLogViewSize = 10240;
        /// <summary>
        /// The brush for sent data.
        /// </summary>
        protected static readonly Brush SentDataBrush = new SolidBrush(Color.Blue);
        /// <summary>
        /// The brush for received data.
        /// </summary>
        protected static readonly Brush ReceivedDataBrush = new SolidBrush(Color.Purple);
        /// <summary>
        /// The brush for acknowledgement.
        /// </summary>
        protected static readonly Brush AckBrush = new SolidBrush(Color.Green);
        /// <summary>
        /// The brush for error.
        /// </summary>
        protected static readonly Brush ErrorBrush = new SolidBrush(Color.Red);

        /// <summary>
        /// The control to display a log.
        /// </summary>
        protected readonly ListBox listBox;
        /// <summary>
        /// The local file name of the log.
        /// </summary>
        protected string logFileName;
        /// <summary>
        /// The last write time of the local log file (UTC).
        /// </summary>
        protected DateTime logFileAge;
        /// <summary>
        /// Top padding of line text if the list box is drawn manually.
        /// </summary>
        protected int itemPaddingTop;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public LogBox(ListBox listBox, bool colorize = false)
        {
            this.listBox = listBox ?? throw new ArgumentNullException(nameof(listBox));
            listBox.KeyDown += ListBox_KeyDown;
            logFileName = "";
            logFileAge = DateTime.MinValue;
            itemPaddingTop = 0;

            if (colorize)
            {
                listBox.DrawMode = DrawMode.OwnerDrawFixed;
                listBox.DrawItem += ListBox_DrawItem;
            }

            FullLogView = false;
            LogViewSize = DefaultLogViewSize;
            AutoScroll = false;
            Colorize = colorize;
        }


        /// <summary>
        /// Gets or sets a value indicating whether to load a full log.
        /// </summary>
        public bool FullLogView { get; set; }

        /// <summary>
        /// Gets or sets the size of displayed part of a log in bytes.
        /// </summary>
        public int LogViewSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to automatically scroll down the content of the text box.
        /// </summary>
        public bool AutoScroll { get; set; }

        /// <summary>
        /// Gets a value indicating whether lines should be colorized depending on key words.
        /// </summary>
        public bool Colorize { get; protected set; }

        /// <summary>
        /// Gets or sets the local file name of the log.
        /// </summary>
        public string LogFileName
        {
            get
            {
                return logFileName;
            }
            set
            {
                logFileName = value;
                logFileAge = DateTime.MinValue;
            }
        }


        /// <summary>
        /// Reads lines from the log file.
        /// </summary>
        private List<string> ReadLines()
        {
            using FileStream fileStream = new(LogFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            List<string> lines = new();
            long offset = FullLogView ? 0 : Math.Max(0, fileStream.Length - LogViewSize);

            if (fileStream.Seek(offset, SeekOrigin.Begin) == offset)
            {
                using StreamReader reader = new(fileStream, Encoding.UTF8);
                bool skipFirstLine = offset != 0;

                while (!reader.EndOfStream)
                {
                    if (skipFirstLine)
                        skipFirstLine = false;
                    else
                        lines.Add(reader.ReadLine());
                }
            }

            return lines;
        }

        /// <summary>
        /// Chooses the appropriate brush depending on the line prefix.
        /// </summary>
        private static Brush ChooseBrush(string line, bool itemSelected)
        {
            if (itemSelected)
            {
                return SystemBrushes.HighlightText;
            }
            else if (line.StartsWith("send", StringComparison.OrdinalIgnoreCase) ||
                line.StartsWith("отправка", StringComparison.OrdinalIgnoreCase))
            {
                return SentDataBrush;
            }
            else if (line.StartsWith("receive", StringComparison.OrdinalIgnoreCase) ||
                line.StartsWith("приём", StringComparison.OrdinalIgnoreCase))
            {
                return ReceivedDataBrush;
            }
            else if (line.StartsWith("ok", StringComparison.OrdinalIgnoreCase))
            {
                return AckBrush;
            }
            else if (line.StartsWith("error", StringComparison.OrdinalIgnoreCase) ||
                line.StartsWith("ошибка", StringComparison.OrdinalIgnoreCase))
            {
                return ErrorBrush;
            }
            else
            {
                return SystemBrushes.WindowText;
            }
        }


        /// <summary>
        /// Sets the first line of the list box.
        /// </summary>
        public virtual void SetFirstLine(string s)
        {
            try
            {
                listBox.BeginUpdate();
                listBox.Items.Clear();
                listBox.Items.Add(s);
            }
            finally
            {
                listBox.EndUpdate();
            }
        }

        /// <summary>
        /// Sets the list box lines.
        /// </summary>
        public virtual void SetLines(ICollection<string> lines)
        {
            ArgumentNullException.ThrowIfNull(lines, nameof(lines));
            Graphics graphics = null;

            try
            {
                listBox.BeginUpdate();

                if (listBox.DrawMode == DrawMode.OwnerDrawFixed)
                {
                    graphics = listBox.CreateGraphics();
                    int lineHeight = (int)graphics.MeasureString("0", listBox.Font).Height;
                    itemPaddingTop = (listBox.ItemHeight - lineHeight) / 2;
                }

                int newLineCnt = lines.Count;
                int selIndex = listBox.SelectedIndex;
                int topIndex = listBox.TopIndex;
                int maxLineWidth = 0;

                listBox.Items.Clear();

                foreach (string line in lines)
                {
                    listBox.Items.Add(line);

                    if (graphics != null)
                    {
                        int lineWidth = (int)graphics.MeasureString(line, listBox.Font).Width;
                        maxLineWidth = Math.Max(maxLineWidth, lineWidth);
                    }
                }

                if (newLineCnt > 0)
                {
                    if (AutoScroll)
                    {
                        listBox.SelectedIndex = -1;
                        listBox.TopIndex = newLineCnt - 1;
                    }
                    else
                    {
                        listBox.SelectedIndex = Math.Min(selIndex, newLineCnt - 1);
                        listBox.TopIndex = Math.Min(topIndex, newLineCnt - 1);
                    }
                }

                if (graphics != null)
                    listBox.HorizontalExtent = maxLineWidth;
            }
            finally
            {
                listBox.EndUpdate();
                graphics?.Dispose();
            }
        }

        /// <summary>
        /// Refresh log content from file.
        /// </summary>
        public void RefreshFromFile()
        {
            try
            {
                if (File.Exists(LogFileName))
                {
                    DateTime newLogFileAge = File.GetLastWriteTimeUtc(LogFileName);

                    if (logFileAge != newLogFileAge)
                    {
                        List<string> lines = ReadLines();

                        if (lines.Count == 0)
                            lines.Add(CommonPhrases.NoData);

                        SetLines(lines);
                        logFileAge = newLogFileAge;
                    }
                }
                else
                {
                    SetFirstLine(CommonPhrases.FileNotFound);
                    logFileAge = DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
                SetFirstLine(ex.Message);
                logFileAge = DateTime.MinValue;
            }
        }


        private void ListBox_KeyDown(object sender, KeyEventArgs e)
        {
            // copy the selected lines
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.C)
            {
                StringBuilder selectedText = new();

                foreach (object item in listBox.SelectedItems)
                {
                    selectedText.AppendLine(item.ToString());
                }

                if (selectedText.Length > 0)
                    Clipboard.SetText(selectedText.ToString());
            }
        }

        private void ListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                string line = (string)listBox.Items[e.Index];
                Brush brush = ChooseBrush(line, e.State.HasFlag(DrawItemState.Selected));

                e.DrawBackground();
                e.Graphics.DrawString(line, listBox.Font, brush, e.Bounds.Left, e.Bounds.Top + itemPaddingTop);
                e.DrawFocusRectangle();
            }
        }
    }
}
