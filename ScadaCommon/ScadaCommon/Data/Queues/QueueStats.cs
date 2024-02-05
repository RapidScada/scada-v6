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
 * Module   : ScadaCommon
 * Summary  : Represents queue statistics
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

using Scada.Lang;
using System;
using System.Text;

namespace Scada.Data.Queues
{
    /// <summary>
    /// Represents queue statistics.
    /// <para>Представляет статистику очереди.</para>
    /// </summary>
    public class QueueStats
    {
        private string title;
        private string underline;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public QueueStats()
        {
            Title = "";
            Enabled = false;
            HasError = false;
            ExportedItems = 0;
            SkippedItems = 0;
            MaxQueueSize = 0;
        }


        /// <summary>
        /// Gets or sets the queue title.
        /// </summary>
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value ?? "";
                underline = new string('-', title.Length);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the queue is in use.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a data transfer error has occurred.
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// Gets or sets the number of exported items.
        /// </summary>
        public int ExportedItems { get; set; }

        /// <summary>
        /// Gets or sets the number of skipped items.
        /// </summary>
        public int SkippedItems { get; set; }

        /// <summary>
        /// Gets or sets the maximum queue size.
        /// </summary>
        public int MaxQueueSize { get; set; }


        /// <summary>
        /// Appends information to the string builder.
        /// </summary>
        public void AppendInfo(StringBuilder sbInfo, int? queueSize)
        {
            if (sbInfo == null)
                throw new ArgumentNullException(nameof(sbInfo));

            sbInfo
                .AppendLine(title)
                .AppendLine(underline);

            if (Locale.IsRussian)
            {
                if (Enabled)
                {
                    sbInfo
                        .Append("Состояние      : ").AppendLine(HasError ? "ошибка" : "норма")
                        .Append("В очереди      : ").Append(queueSize).Append(" из ").Append(MaxQueueSize).AppendLine()
                        .Append("Экспортировано : ").Append(ExportedItems).AppendLine()
                        .Append("Пропущено      : ").Append(SkippedItems).AppendLine()
                        .AppendLine();
                }
                else
                {
                    sbInfo
                        .Append("Состояние : отключено").AppendLine()
                        .AppendLine();
                }
            }
            else
            {
                if (Enabled)
                {
                    sbInfo
                        .Append("Status   : ").AppendLine(HasError ? "Error" : "Normal")
                        .Append("In queue : ").Append(queueSize).Append(" of ").Append(MaxQueueSize).AppendLine()
                        .Append("Exported : ").Append(ExportedItems).AppendLine()
                        .Append("Skipped  : ").Append(SkippedItems).AppendLine()
                        .AppendLine();
                }
                else
                {
                    sbInfo
                        .Append("Status : Disabled").AppendLine()
                        .AppendLine();
                }
            }
        }

        /// <summary>
        /// Appends information to the string builder as a single line.
        /// </summary>
        public void AppendShortInfo(StringBuilder sbInfo, int? queueSize, int titleWidth)
        {
            if (sbInfo == null)
                throw new ArgumentNullException(nameof(sbInfo));

            sbInfo
                .Append(title.PadRight(titleWidth))
                .Append(": ");

            if (Locale.IsRussian)
            {
                if (Enabled)
                {
                    sbInfo
                        .Append(queueSize).Append(" из ").Append(MaxQueueSize)
                        .Append(", пропущено ").Append(SkippedItems).AppendLine();
                }
                else
                {
                    sbInfo.AppendLine("отключено");
                }
            }
            else
            {
                if (Enabled)
                {
                    sbInfo
                        .Append(queueSize).Append(" of ").Append(MaxQueueSize)
                        .Append(", skipped ").Append(SkippedItems).AppendLine();
                }
                else
                {
                    sbInfo.AppendLine("disabled");
                }
            }
        }
    }
}
