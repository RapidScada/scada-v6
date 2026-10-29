/*
 * Copyright 2026 Rapid Software LLC
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
 * Summary  : Creates device tags from a string
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2026
 * Modified : 2026
 */

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Creates device tags from a string.
    /// <para>Создает теги устройства из строки.</para>
    /// </summary>
    public static class TagParser
    {
        /// <summary>
        /// Extracts the code and name from the device tag definition.
        /// </summary>
        public static void Parse(string s, out string code, out string name)
        {
            code = "";
            name = "";

            if (!string.IsNullOrEmpty(s))
            {
                int idx1 = s.IndexOf('[');
                int idx2 = s.IndexOf(']');

                if (idx1 >= 0 && idx1 < idx2)
                {
                    code = s.Substring(idx1 + 1, idx2 - idx1 - 1).Trim();
                    name = s.Substring(idx2 + 1).Trim();
                }
                else
                {
                    code = name = s.Trim();
                }
            }
        }

        /// <summary>
        /// Creates a device tag with the code and name taken from the specified string.
        /// </summary>
        public static DeviceTag Parse(string s)
        {
            Parse(s, out string code, out string name);
            return new DeviceTag(code, name);
        }
    }
}
