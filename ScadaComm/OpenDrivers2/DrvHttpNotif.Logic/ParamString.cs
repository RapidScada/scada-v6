// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvHttpNotif.Config;
using Scada.Utils;
using System.Web;

namespace Scada.Comm.Drivers.DrvHttpNotif.Logic
{
    /// <summary>
    /// Represents a parameterized string with parameter encoding features.
    /// <para>Представляет параметризованную строку с возможностями кодирования параметров.</para>
    /// </summary>
    internal class ParamString : ParameterizedString
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ParamString(string source, string beginSymbol, string endSymbol)
            : base(source, beginSymbol, endSymbol)
        {
        }

        /// <summary>
        /// Sets the parameter value escaped by the specified method.
        /// </summary>
        public void SetParameter(string name, string value, EscapingMethod escapingMethod)
        {
            SetParameter(name, escapingMethod switch
            {
                EscapingMethod.EncodeUrl => HttpUtility.UrlEncode(value), // WebUtility.UrlEncode or Uri.EscapeDataString
                EscapingMethod.EncodeJson => HttpUtility.JavaScriptStringEncode(value, false),
                _ => value
            });
        }

        /// <summary>
        /// Sets all parameters contained by the parameterized string.
        /// </summary>
        public void SetAllParameters(IDictionary<string, string> args, EscapingMethod escapingMethod)
        {
            ArgumentNullException.ThrowIfNull(args);

            foreach (string name in GetParameterNames())
            {
                string value = args.GetValueAsString(name);
                SetParameter(name, value, escapingMethod);
            }
        }
    }
}
