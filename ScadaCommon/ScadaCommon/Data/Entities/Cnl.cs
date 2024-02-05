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
 * Summary  : Represents a channel as an entity of the configuration database
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2023
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using System;

namespace Scada.Data.Entities
{
    /// <summary>
    /// Represents a channel as an entity of the configuration database.
    /// <para>Представляет канал как сущность базы конфигурации.</para>
    /// </summary>
    [Serializable]
    public class Cnl
    {
        public int CnlNum { get; set; }

        public bool Active { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int? DataTypeID { get; set; }

        public int? DataLen { get; set; }

        public int CnlTypeID { get; set; }

        public int? ObjNum { get; set; }

        public int? DeviceNum { get; set; }

        public int? TagNum { get; set; }

        public string TagCode { get; set; }

        public bool FormulaEnabled { get; set; }

        public string InFormula { get; set; }

        public string OutFormula { get; set; }

        public int? FormatID { get; set; }

        public int? OutFormatID { get; set; }

        public int? QuantityID { get; set; }

        public int? UnitID { get; set; }

        public int? LimID { get; set; }

        public int? ArchiveMask { get; set; }

        public int? EventMask { get; set; }
    }
}
