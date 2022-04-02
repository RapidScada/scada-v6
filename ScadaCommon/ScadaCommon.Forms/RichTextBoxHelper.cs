// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Scada.Forms
{
    /// <summary>
    /// Provides helper methods for adding messages to a RichTextBox control.
    /// <para>Предоставляет вспомогательные методы для добавления сообщений в элемент управления RichTextBox.</para>
    /// </summary>
    public class RichTextBoxHelper
    {
        private readonly Action<string> writeMessageAction;
        private readonly Action<string, Color> writeColoredMessageAction;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RichTextBoxHelper(RichTextBox richTextBox)
        {
            writeMessageAction = s => WriteMessage(s);
            writeColoredMessageAction = (s, c) => WriteMessage(s, c);

            RichTextBox = richTextBox ?? throw new ArgumentNullException(nameof(richTextBox));
            SuccessColor = Color.Green;
            WarningColor = Color.Orange;
            ErrorColor = Color.Red;
        }


        /// <summary>
        /// Gets the rich text box control.
        /// </summary>
        public RichTextBox RichTextBox { get; }

        /// <summary>
        /// Gets or sets the color to display successful messages.
        /// </summary>
        public Color SuccessColor { get; set; }

        /// <summary>
        /// Gets or sets the color to display warnings.
        /// </summary>
        public Color WarningColor { get; set; }

        /// <summary>
        /// Gets or sets the color to display errors.
        /// </summary>
        public Color ErrorColor { get; set; }



        /// <summary>
        /// Writes the message.
        /// </summary>
        public void WriteMessage(string text)
        {
            if (RichTextBox.IsDisposed)
                return;

            if (RichTextBox.InvokeRequired)
            {
                RichTextBox.Invoke(writeMessageAction, text);
            }
            else
            {
                RichTextBox.AppendText(text);
                RichTextBox.AppendText(Environment.NewLine);
            }
        }

        /// <summary>
        /// Writes the message with the specified text color.
        /// </summary>
        public void WriteMessage(string text, Color color)
        {
            if (RichTextBox.IsDisposed)
                return;

            if (RichTextBox.InvokeRequired)
            {
                RichTextBox.Invoke(writeColoredMessageAction, text, color);
            }
            else
            {
                RichTextBox.SelectionStart = RichTextBox.TextLength;
                RichTextBox.SelectionLength = 0;

                RichTextBox.SelectionColor = color;
                RichTextBox.AppendText(text);
                RichTextBox.AppendText(Environment.NewLine);
                RichTextBox.SelectionColor = RichTextBox.ForeColor;
            }
        }

        /// <summary>
        /// Writes the successful message.
        /// </summary>
        public void WriteSuccess(string text)
        {
            WriteMessage(text, SuccessColor);
        }

        /// <summary>
        /// Writes the warning message.
        /// </summary>
        public void WriteWarning(string text)
        {
            WriteMessage(text, WarningColor);
        }

        /// <summary>
        /// Writes the error message.
        /// </summary>
        public void WriteError(string text)
        {
            WriteMessage(text, ErrorColor);
        }

        /// <summary>
        /// Writes an empty line.
        /// </summary>
        public void WriteLine()
        {
            WriteMessage("");
        }

        /// <summary>
        /// Clears the rich text box control.
        /// </summary>
        public void Clear()
        {
            RichTextBox.Clear();
        }
    }
}
