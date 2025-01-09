﻿/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Summary  : The class contains utility methods for the schemes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2019
 */

using System.Reflection;

namespace Scada.Scheme
{
    /// <summary>
    /// The class contains utility methods for the schemes.
    /// <para>Класс, содержащий вспомогательные методы для схем.</para>
    /// </summary>
    public static class SchemeUtils
    {
        /// <summary>
        /// The schemes version.
        /// </summary>
        public const string SchemeVersion = "5.3.1.1";
        /// <summary>
        /// The color name which means that color depends on channel status.
        /// </summary>
        public const string StatusColor = "Status";

        /// <summary>
        /// Checks that the view stamps are matched.
        /// </summary>
        public static bool ViewStampsMatched(long browserViewStamp, long serverViewStamp)
        {
            return serverViewStamp > 0 && (browserViewStamp == 0 || browserViewStamp == serverViewStamp);
        }
    }
}
