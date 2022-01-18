/*
 * Copyright 2022 Rapid Software LLC
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
 * Summary  : Represents rights to access some entity
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2021
 */

using Scada.Data.Entities;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents rights to access some entity.
    /// <para>Представляет права на доступ к некоторой сущности.</para>
    /// </summary>
    public struct Right
    {
        /// <summary>
        /// Represents an instance that has no rights.
        /// </summary>
        public static readonly Right Empty = new Right(false, false);
        /// <summary>
        /// Represents an instance that has full rights.
        /// </summary>
        public static readonly Right Full = new Right(true, true);


        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        public Right(bool viewRight, bool ctrlRight)
        {
            View = viewRight;
            Control = ctrlRight;
        }

        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        public Right(ObjRight objRight)
        {
            View = objRight.View;
            Control = objRight.Control;
        }


        /// <summary>
        /// Gets or sets the right to view.
        /// </summary>
        public bool View { get; set; }

        /// <summary>
        /// Gets or sets the right to control.
        /// </summary>
        public bool Control { get; set; }
    }
}
