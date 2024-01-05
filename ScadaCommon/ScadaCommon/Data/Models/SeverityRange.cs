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
 * Module   : ScadaCommon
 * Summary  : Represents a severity range
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

using Scada.Data.Const;
using Scada.Lang;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents a severity range.
    /// <para>Представляет диапазон серьёзности.</para>
    /// </summary>
    public struct SeverityRange
    {
        /// <summary>
        /// The critical severity from 1 to 249.
        /// </summary>
        public static readonly SeverityRange Critical = 
            new SeverityRange(Severity.Critical, Severity.Major - 1, CommonPhrases.CriticalSeverity);

        /// <summary>
        /// The major severity from 250 to 499.
        /// </summary>
        public static readonly SeverityRange Major = 
            new SeverityRange(Severity.Major, Severity.Minor - 1, CommonPhrases.MajorSeverity);

        /// <summary>
        /// The minor severity from 500 to 749.
        /// </summary>
        public static readonly SeverityRange Minor = 
            new SeverityRange(Severity.Minor, Severity.Info - 1, CommonPhrases.MinorSeverity);

        /// <summary>
        /// The informational severity from 750 to 999.
        /// </summary>
        public static readonly SeverityRange Info = 
            new SeverityRange(Severity.Info, Severity.Max, CommonPhrases.InfoSeverity);


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private SeverityRange(int min, int max, string name)
        {
            Min = min;
            Max = max;
            Name = name;
        }


        /// <summary>
        /// Gets the minimum.
        /// </summary>
        public int Min { get; }

        /// <summary>
        /// Gets the maximum.
        /// </summary>
        public int Max { get; }

        /// <summary>
        /// Gets the severity name.
        /// </summary>
        public string Name { get; }


        /// <summary>
        /// Returns a string in range format
        /// </summary>
        public string ToRangeString()
        {
            return Min + "-" + Max;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0} ({1}...{2})", Name, Min, Max);
        }
    }
}
