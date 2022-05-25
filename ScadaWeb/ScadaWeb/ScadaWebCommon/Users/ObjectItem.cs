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
using Scada.Lang;
using System;
using System.Text;

namespace Scada.Web.Users
{
    /// <summary>
    /// Represents an item that corresponds to an object available to a user.
    /// <para>Представляет элемент, который соответствует объекту, доступному пользователю.</para>
    /// </summary>
    public class ObjectItem
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ObjectItem(Obj obj, int level)
        {
            ArgumentNullException.ThrowIfNull(obj, nameof(obj));
            ObjNum = obj.ObjNum;
            Level = level;

            if (level > 0)
            {
                Text = new StringBuilder()
                    .Append('-', level * 2)
                    .Append(' ')
                    .AppendFormat(CommonPhrases.EntityCaption, obj.ObjNum, obj.Name)
                    .ToString();
            }
            else
            {
                Text = string.Format(CommonPhrases.EntityCaption, obj.ObjNum, obj.Name);
            }
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
