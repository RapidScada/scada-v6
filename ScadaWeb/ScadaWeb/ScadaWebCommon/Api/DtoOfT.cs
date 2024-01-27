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
 * Module   : ScadaWebCommon
 * Summary  : Represents a data transfer object that carries data of the specified type from the server side to a client
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

namespace Scada.Web.Api
{
    /// <summary>
    /// Represents a data transfer object that carries data of the specified type from the server side to a client.
    /// <para>Представляет объект, передающий данные заданного типа со стороны сервера клиенту.</para>
    /// </summary>
    public class Dto<TData> : Dto
    {
        /// <summary>
        /// Gets or sets the data to transfer.
        /// </summary>
        public TData Data { get; set; }


        /// <summary>
        /// Creates a new data transfer object with the successfull result.
        /// </summary>
        public static Dto<TData> Success(TData data)
        {
            return new Dto<TData>
            {
                Ok = true,
                Msg = "",
                Data = data
            };
        }

        /// <summary>
        /// Creates a new data transfer object with the failed result.
        /// </summary>
        public static new Dto<TData> Fail(string msg)
        {
            return new Dto<TData>
            {
                Ok = false,
                Msg = msg,
                Data = default
            };
        }
    }
}
