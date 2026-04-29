/*
 * Copyright 2025 Rapid Software LLC
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
 * Summary  : Specifies the common keys to pass temporary data between HTTP requests when performing a redirect
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2025
 * Modified : 2025
 */

namespace Scada.Web.Pages
{
    /// <summary>
    /// Specifies the common keys to pass temporary data between HTTP requests when performing a redirect.
    /// <para>Задаёт общие ключи для передачи временных данных между HTTP-запросами при перенаправлении.</para>
    /// </summary>
    public static class TempDataKey
    {
        /// <summary>
        /// Error message displayed to a user.
        /// </summary>
        public const string ErrorMessage = nameof(ErrorMessage);
    }
}
