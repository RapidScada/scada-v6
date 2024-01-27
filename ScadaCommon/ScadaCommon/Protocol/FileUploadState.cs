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
 * Summary  : Specifies the possible state of an upload operation
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

namespace Scada.Protocol
{
    /// <summary>
    /// Specifies the possible state of an upload operation.
    /// <para>Задаёт возможное состояние операции загрузки.</para>
    /// </summary>
    public enum FileUploadState : byte
    {
        /// <summary>
        /// More data is available in the uploaded file.
        /// </summary>
        DataAvailable = 0,

        /// <summary>
        /// The end of the file has been reached.
        /// </summary>
        EndOfFile = 1,

        /// <summary>
        /// The upload operation has been canceled by a user.
        /// </summary>
        UploadCanceled = 2,
    }
}
