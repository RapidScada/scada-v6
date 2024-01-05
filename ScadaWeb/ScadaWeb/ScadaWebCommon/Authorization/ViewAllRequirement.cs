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
 * Summary  : Represents a requirement to have rights to view data of any object
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

using Microsoft.AspNetCore.Authorization;

namespace Scada.Web.Authorization
{
    /// <summary>
    /// Represents a requirement to have rights to view data of any object.
    /// <para>Представляет требование иметь права на просмотр данных любого объекта.</para>
    /// </summary>
    public class ViewAllRequirement : IAuthorizationRequirement
    {
    }
}
