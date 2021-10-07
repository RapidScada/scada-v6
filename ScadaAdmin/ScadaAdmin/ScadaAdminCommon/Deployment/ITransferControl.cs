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
 * Module   : ScadaAdminCommon
 * Summary  : Defines functionality to control configuration transfer
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

namespace Scada.Admin.Deployment
{
    /// <summary>
    /// Defines functionality to control configuration transfer.
    /// <para>Определяет функциональность контроля передачи конфигурации.</para>
    /// </summary>
    public interface ITransferControl
    {
        /// <summary>
        /// Gets a value indicating whether the transfer is canceled by a user.
        /// </summary>
        bool IsCanceled { get; }

        /// <summary>
        /// Writes the message to a terminal.
        /// </summary>
        void WriteMessage(string text);

        /// <summary>
        /// Sets the transfer progress in percent.
        /// </summary>
        void SetProgress(double percent);

        /// <summary>
        /// Sets the transfer result.
        /// </summary>
        void SetResult(bool successful);
    }
}
