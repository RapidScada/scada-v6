/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : ScadaData
 * Summary  : Represents an automated system event
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using System;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents an automated system event.
    /// <para>Представляет событие автоматизированной системы.</para>
    /// </summary>
    public class Event
    {
        public long EventID { get; set; }

        public DateTime Timestamp { get; set; }

        public bool Hidden { get; set; }

        public int CnlNum { get; set; }

        public int OutCnlNum { get; set; }

        public int ObjNum { get; set; }

        public int DeviceNum { get; set; }

        public double PrevCnlVal { get; set; }

        public int PrevCnlStat { get; set; }

        public double CnlVal { get; set; }

        public int CnlStat { get; set; }

        public int Severity { get; set; }

        public bool AckRequired { get; set; }

        public bool Ack { get; set; }

        public DateTime AckTimestamp { get; set; }

        public int AckUserID { get; set; }

        public EventTextFormat TextFormat { get; set; }

        public string Text { get; set; }

        public byte[] Data { get; set; }
    }
}
