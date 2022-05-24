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
 * Module   : ScadaWebCommon
 * Summary  : Represents an object item that is available to a user
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

using Scada.Data.Entities;
using System;

namespace Scada.Web.Models
{
    /// <summary>
    /// Represents an object item that is available to a user.
    /// <para>Представляет элемент объекта, доступный пользователю.</para>
    /// </summary>
    public class ObjItem
    {
        /// <summary>
        /// The prefix indicating an item level.
        /// </summary>
        public const string LevelPrefix = "--";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ObjItem(Obj obj, int level)
        {
            ArgumentNullException.ThrowIfNull(obj, nameof(obj));
            Level = level;
        }


        /// <summary>
        /// Gets the object number.
        /// </summary>
        public int ObjNum { get; }

        /// <summary>
        /// Gets the item text.
        /// </summary>
        public string Text { get; }


        /// <summary>
        /// Gets the nesting level.
        /// </summary>
        public int Level { get; }
    }
}
