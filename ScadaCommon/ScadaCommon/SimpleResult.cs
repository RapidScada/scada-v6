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
 * Summary  : Represents a result of a method or action
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

namespace Scada
{
    /// <summary>
    /// Represents a result of a method or action.
    /// <para>Представляет результат метода или действия.</para>
    /// </summary>
    public class SimpleResult
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SimpleResult()
        {
            Ok = false;
            Msg = "";
        }


        /// <summary>
        /// Gets or sets a value indicating whether the result is successful.
        /// </summary>
        public bool Ok { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Msg { get; set; }


        /// <summary>
        /// Creates a successfull result.
        /// </summary>
        public static SimpleResult Success()
        {
            return new SimpleResult
            {
                Ok = true,
                Msg = ""
            };
        }

        /// <summary>
        /// Creates a failed result.
        /// </summary>
        public static SimpleResult Fail(string msg)
        {
            return new SimpleResult
            {
                Ok = false,
                Msg = msg
            };
        }
    }
}
