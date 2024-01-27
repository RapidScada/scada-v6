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
 * Module   : ScadaCommon.Log
 * Summary  : Provides logging functionality
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2021
 */

using System;

namespace Scada.Log
{
    /// <summary>
    /// Provides logging functionality.
    /// <para>Обеспечивает функциональность логгирования.</para>
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Writes the message of the specified type to the log.
        /// </summary>
        void WriteMessage(string text, LogMessageType messageType);

        /// <summary>
        /// Writes the informational message to the log.
        /// </summary>
        void WriteInfo(string text, params object[] args);

        /// <summary>
        /// Writes the action to the log.
        /// </summary>
        void WriteAction(string text, params object[] args);

        /// <summary>
        /// Writes the warning message to the log.
        /// </summary>
        void WriteWarning(string text, params object[] args);

        /// <summary>
        /// Writes the error to the log.
        /// </summary>
        void WriteError(string text, params object[] args);

        /// <summary>
        /// Writes the error to the log.
        /// </summary>
        void WriteError(Exception ex, string text = "", params object[] args);

        /// <summary>
        /// Writes the specified line to the log.
        /// </summary>
        void WriteLine(string text = "", params object[] args);

        /// <summary>
        /// Writes a divider to the log.
        /// </summary>
        void WriteBreak();
    }
}
