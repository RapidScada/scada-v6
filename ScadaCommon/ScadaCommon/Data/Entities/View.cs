﻿/*
 * Copyright 2025 Rapid Software LLC
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
 * Summary  : Represents a view as an entity of the configuration database
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2020
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using System;

namespace Scada.Data.Entities
{
    /// <summary>
    /// Represents a view as an entity of the configuration database.
    /// <para>Представление как сущность базы конфигурации.</para>
    /// </summary>
    [Serializable]
    public class View
    {
        public int ViewID { get; set; }

        public string Path { get; set; }

        public string Args { get; set; }

        public int? ViewTypeID { get; set; }

        public string Title { get; set; }

        public int? Ord { get; set; }

        public bool Hidden { get; set; }

        public int? ObjNum { get; set; }
    }
}
