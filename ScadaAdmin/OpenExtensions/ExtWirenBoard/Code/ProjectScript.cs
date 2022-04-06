// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtWirenBoard.Properties;

namespace Scada.Admin.Extensions.ExtWirenBoard.Code
{
    /// <summary>
    /// Contains scripts required for Wiren Board projects.
    /// <para>Содержит скрипты, необходимые для проектов Wiren Board.</para>
    /// </summary>
    internal static class ProjectScript
    {
        /// <summary>
        /// The name of the script in the Scripts table.
        /// </summary>
        public const string CsScriptName = "Wiren Board";

        /// <summary>
        /// The source code of the script in the Scripts table.
        /// </summary>
        public const string CsScriptSource =
            "public CnlData WB_CalcControl()\n" +
            "{\n" +
            "  CnlData lastData = Data(CnlNum - 2);\n" +
            "  CnlData errorData = Data(CnlNum - 1);\n" +
            "  return lastData.IsDefined && errorData.IsDefined && errorData.Val > 0\n" +
            "    ? NewData(lastData.Val, CnlStatusID.Unreliable)\n" +
            "    : lastData;\n" +
            "}";

        /// <summary>
        /// The JavaScript file name.
        /// </summary>
        public const string JsFileName = "wb_error.js";

        /// <summary>
        /// The JavaScript source code.
        /// </summary>
        public static readonly string JsSource = Resources.wb_error;
    }
}
