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
 * Summary  : Represents a data transfer object that carries a method result from the server side to a client
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2022
 */

using System;

namespace Scada.Web.Api
{
    /// <summary>
    /// Represents a data transfer object that carries a method result from the server side to a client.
    /// <para>Представляет объект, передающий результат метода со стороны сервера клиенту.</para>
    /// </summary>
    public class Dto : SimpleResult
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Dto()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Dto(SimpleResult simpleResult)
        {
            ArgumentNullException.ThrowIfNull(simpleResult, nameof(simpleResult));
            Ok = simpleResult.Ok;
            Msg = simpleResult.Msg;
        }


        /// <summary>
        /// Creates a new data transfer object with the successfull result.
        /// </summary>
        public static new Dto Success()
        {
            return new Dto
            {
                Ok = true,
                Msg = ""
            };
        }

        /// <summary>
        /// Creates a new data transfer object with the failed result.
        /// </summary>
        public static new Dto Fail(string msg)
        {
            return new Dto
            {
                Ok = false,
                Msg = msg
            };
        }
    }
}
