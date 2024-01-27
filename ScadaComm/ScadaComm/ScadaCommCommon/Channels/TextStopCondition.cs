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
 * Module   : ScadaCommEngine
 * Summary  : Represents a condition to stop reading data in a text format
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using System;
using System.Collections.Generic;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// Represents a condition to stop reading data in a text format.
    /// <para>Представляет условие остановки считывания данных в текстовом формате.</para>
    /// </summary>
    public class TextStopCondition
    {
        /// <summary>
        /// The condition for reading one line.
        /// </summary>
        public static readonly TextStopCondition OneLine = new TextStopCondition(1);


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected TextStopCondition()
        {
            StopEndings = null;
            MaxLineCount = int.MaxValue;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TextStopCondition(string[] stopEndings, int maxLineCount)
        {
            StopEndings = stopEndings ?? throw new ArgumentNullException(nameof(stopEndings));
            MaxLineCount = maxLineCount;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TextStopCondition(string stopEnding, int maxLineCount)
        {
            StopEndings = new string[] { stopEnding };
            MaxLineCount = maxLineCount;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TextStopCondition(params string[] stopEndings)
        {
            StopEndings = stopEndings;
            MaxLineCount = int.MaxValue;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TextStopCondition(int maxLineCount)
        {
            StopEndings = null;
            MaxLineCount = maxLineCount;
        }


        /// <summary>
        /// Gets the line endings that stop reading.
        /// </summary>
        public string[] StopEndings { get; protected set; }

        /// <summary>
        /// Gets the maximum number of lines to read.
        /// </summary>
        public int MaxLineCount { get; protected set; }


        /// <summary>
        /// Checks if the stop condition is satisfied.
        /// </summary>
        public virtual bool CheckCondition(List<string> lines, string lastLine)
        {
            bool stopReceived = false;

            if (lines.Count >= MaxLineCount)
            {
                stopReceived = true;
            }
            else if (StopEndings != null)
            {
                lastLine = lastLine.TrimEnd(CommUtils.NewLineChars);

                for (int i = 0, len = StopEndings.Length; i < len && !stopReceived; i++)
                {
                    stopReceived = lastLine.EndsWith(StopEndings[i], StringComparison.OrdinalIgnoreCase);
                }
            }

            return stopReceived;
        }
    }
}
