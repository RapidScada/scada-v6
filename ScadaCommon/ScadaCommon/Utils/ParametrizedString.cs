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
 * Module   : ScadaCommon
 * Summary  : Represents a parameterized string
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2026
 * Modified : 2026
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Utils
{
    /// <summary>
    /// Represents a parameterized string.
    /// <para>Представляет параметризованную строку.</para>
    /// </summary>
    public class ParametrizedString
    {
        /// <summary>
        /// Represents a string parameter.
        /// </summary>
        private class StringParameter
        {
            public string Name { get; set; }
            public string Value { get; set; } = "";
        }

        /// <summary>
        /// Represents a string part.
        /// </summary>
        private class StringPart
        {
            public bool IsConstant { get { return Parameter == null; } }
            public string Value { get; set; } = "";
            public StringParameter Parameter { get; set; } = null;
        }


        private readonly Dictionary<string, StringParameter> stringParameters;
        private readonly List<StringPart> stringParts;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ParametrizedString(string source, string beginSymbol, string endSymbol)
        {
            stringParameters = new Dictionary<string, StringParameter>();
            stringParts = new List<StringPart>();
            ParseString(source, beginSymbol, endSymbol);
        }


        /// <summary>
        /// Parses the specified string into parts and parameters.
        /// </summary>
        private void ParseString(string source, string beginSymbol, string endSymbol)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(beginSymbol) || string.IsNullOrEmpty(endSymbol))
            {
                stringParts.Add(new StringPart { Value = source ?? "" });
                return;
            }

            int idx = 0;

            while (idx < source.Length)
            {
                int beginIdx = source.IndexOf(beginSymbol, idx);

                if (beginIdx < 0)
                {
                    stringParts.Add(new StringPart { Value = source.Substring(idx) });
                    break;
                }
                else
                {
                    int paramNameIdx = beginIdx + beginSymbol.Length;
                    int endIdx = source.IndexOf(endSymbol, paramNameIdx);

                    if (endIdx < 0)
                    {
                        stringParts.Add(new StringPart { Value = source.Substring(idx) });
                        break;
                    }
                    else if (paramNameIdx < endIdx)
                    {
                        string paramName = source.Substring(paramNameIdx, endIdx - paramNameIdx).Trim();

                        if (!string.IsNullOrEmpty(paramName))
                        {
                            if (!stringParameters.TryGetValue(paramName, out StringParameter param))
                            {
                                param = new StringParameter { Name = paramName };
                                stringParameters.Add(paramName, param);
                            }

                            stringParts.Add(new StringPart { Parameter = param });
                        }
                    }

                    idx = endIdx + endSymbol.Length;
                }
            }
        }

        /// <summary>
        /// Sets the parameter value.
        /// </summary>
        public void SetParameter(string name, string value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            if (stringParameters.TryGetValue(name, out StringParameter stringParameter))
            {
                stringParameter.Value = value;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (StringPart stringPart in stringParts)
            {
                if (stringPart.IsConstant)
                {
                    sb.Append(stringPart.Value);
                }
                else if (stringPart.Parameter != null)
                {
                    sb.Append(stringPart.Parameter.Value);
                }
            }

            return sb.ToString();
        }
    }
}
