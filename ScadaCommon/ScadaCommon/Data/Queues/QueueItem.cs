﻿/*
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
 * Summary  : Represents a queue item
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using System;

namespace Scada.Data.Queues
{
    /// <summary>
    /// Represents a queue item.
    /// <para>Представляет элемент очереди.</para>
    /// </summary>
    public class QueueItem<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public QueueItem(DateTime creationTime, T value)
        {
            CreationTime = creationTime;
            Value = value ?? throw new ArgumentNullException("value");
        }

        /// <summary>
        /// Gets the creation time of the item (UTC).
        /// </summary>
        public DateTime CreationTime { get; }

        /// <summary>
        /// Gets the item value.
        /// </summary>
        public T Value { get; }
    }
}
