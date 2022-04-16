// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvHttpNotif.Config;
using System.Collections.Generic;
using System.Web;

namespace Scada.Comm.Drivers.DrvHttpNotif.Logic
{
    /// <summary>
    /// Represents a parameterized string.
    /// <para>Представляет параметризованную строку.</para>
    /// </summary>
    internal class ParamString
    {
        /// <summary>
        /// Represents a string parameter.
        /// </summary>
        public class Param
        {
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public Param(string name)
            {
                Name = name;
                PartIndices = new List<int>();
            }

            /// <summary>
            /// Gets the parameter name.
            /// </summary>
            public string Name { get; }
            /// <summary>
            /// Gets the parameter indices among the parts of a string.
            /// </summary>
            public List<int> PartIndices { get; }
        }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ParamString(string sourceString, char paramBegin, char paramEnd)
        {
            Parse(sourceString, paramBegin, paramEnd);
        }


        /// <summary>
        /// Gets the string parts.
        /// </summary>
        public string[] StringParts { get; private set; }

        /// <summary>
        /// Gets the parameters accessed by name.
        /// </summary>
        public Dictionary<string, Param> Params { get; private set; }


        /// <summary>
        /// Parses the specified string, creates string parts and parameters.
        /// </summary>
        private void Parse(string sourceString, char paramBegin, char paramEnd)
        {
            List<string> stringParts = new List<string>();
            Dictionary<string, Param> stringParams = new Dictionary<string, Param>();

            // split the string into parts separated by curly braces { and }
            if (!string.IsNullOrEmpty(sourceString))
            {
                int ind = 0;
                int len = sourceString.Length;

                while (ind < len)
                {
                    int braceInd1 = sourceString.IndexOf(paramBegin, ind);
                    if (braceInd1 < 0)
                    {
                        stringParts.Add(sourceString.Substring(ind));
                        break;
                    }
                    else
                    {
                        int braceInd2 = sourceString.IndexOf(paramEnd, braceInd1 + 1);
                        int paramNameLen = braceInd2 - braceInd1 - 1;

                        if (paramNameLen <= 0)
                        {
                            stringParts.Add(sourceString.Substring(ind));
                            break;
                        }
                        else
                        {
                            string paramName = sourceString.Substring(braceInd1 + 1, paramNameLen);

                            if (!stringParams.TryGetValue(paramName, out Param param))
                            {
                                param = new Param(paramName);
                                stringParams.Add(paramName, param);
                            }

                            stringParts.Add(sourceString.Substring(ind, braceInd1 - ind));
                            param.PartIndices.Add(stringParts.Count);
                            stringParts.Add(""); // empty parameter value
                            ind = braceInd2 + 1;
                        }
                    }
                }
            }

            StringParts = stringParts.ToArray();
            Params = stringParams;
        }

        /// <summary>
        /// Sets the parameter value escaped by the specified method.
        /// </summary>
        private void SetParam(string name, string value, EscapingMethod escapingMethod)
        {
            if (Params.TryGetValue(name, out Param param))
            {
                if (escapingMethod == EscapingMethod.EncodeUrl)
                    value = HttpUtility.UrlEncode(value); // or WebUtility.UrlEncode or Uri.EscapeDataString
                else if (escapingMethod == EscapingMethod.EncodeJson)
                    value = HttpUtility.JavaScriptStringEncode(value, false);

                foreach (int index in param.PartIndices)
                {
                    StringParts[index] = value;
                }
            }
        }

        /// <summary>
        /// Resets the parameter values.
        /// </summary>
        public void ResetParams(IDictionary<string, string> args, EscapingMethod escapingMethod)
        {
            // clear all parts
            foreach (Param param in Params.Values)
            {
                foreach (int index in param.PartIndices)
                {
                    StringParts[index] = "";
                }
            }

            // set new values
            if (args != null)
            {
                foreach (var arg in args)
                {
                    SetParam(arg.Key, arg.Value, escapingMethod);
                }
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Join("", StringParts);
        }
    }
}
