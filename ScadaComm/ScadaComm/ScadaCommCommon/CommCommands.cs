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
 * Module   : ScadaCommCommon
 * Summary  : Specifies the application commands supported by Communicator
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Comm
{
    /// <summary>
    /// Specifies the application commands supported by Communicator.
    /// <para>Задает команды приложения, поддерживаемые Коммуникатором.</para>
    /// </summary>
    public static class CommCommands
    {
        public const string StartLine = "Comm.StartLine";
        public const string StopLine = "Comm.StopLine";
        public const string RestartLine = "Comm.RestartLine";
        public const string PollDevice = "Comm.PollDevice";
    }
}
