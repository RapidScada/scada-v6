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
 * Summary  : Represents an exception raised when the user cannot access services
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2022
 */

using Scada.Web.Lang;

namespace Scada.Web.Authorization
{
    /// <summary>
    /// Represents an exception raised when the user cannot access services.
    /// <para>Представляет исключение, возникающее, когда пользователь не может получить доступ к службам.</para>
    /// </summary>
    public class AccessDeniedException : ApiException
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AccessDeniedException() 
            : base(WebPhrases.AccessDenied)
        {
        }
    }
}
