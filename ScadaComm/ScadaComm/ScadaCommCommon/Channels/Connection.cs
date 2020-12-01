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
 * Module   : ScadaCommCommon
 * Summary  : Represents the base class for device connection
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2020
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// Represents the base class for device connection.
    /// <para>Представляет базовый класс соединения с КП.</para>
    /// </summary>
    public abstract class Connection
    {
        /// <summary>
        /// Gets a value indicating whether the connection is established.
        /// </summary>
        public virtual bool Connected
        {
            get
            {
                return true;
            }
        }
    }
}
