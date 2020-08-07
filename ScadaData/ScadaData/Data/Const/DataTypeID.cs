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
 * Summary  : Specifies the data type IDs
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Data.Const
{
    /// <summary>
    /// Specifies the data type IDs.
    /// <para>Задает идентификаторы типов данных.</para>
    /// </summary>
    public static class DataTypeID
    {
        public const int Double = 0;
        public const int Int64 = 1;
        public const int UInt64 = 2;
        public const int ASCII = 3;
        public const int Unicode = 4;
    }
}
