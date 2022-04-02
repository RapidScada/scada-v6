// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Admin.Extensions.ExtWirenBoard.Code
{
    /// <summary>
    /// The phrases used by the extension.
    /// <para>Фразы, используемые расширением.</para>
    /// </summary>
    internal class ExtensionPhrases
    {
        // Scada.Admin.Extensions.ExtWirenBoard.Controls.CtrlDeviceTree
        public static string SelectDevice { get; private set; }

        // Scada.Admin.Extensions.ExtWirenBoard.Controls.CtrlLineSelect
        public static string CommLineRequired { get; private set; }
        public static string MqttLineRequired { get; private set; }
        public static string WirenBoardIpRequired { get; private set; }

        // Scada.Admin.Extensions.ExtWirenBoard.Forms.FrmWirenBoardWizard
        public static string Step1Descr { get; private set; }
        public static string Step2Descr { get; private set; }
        public static string Step3Descr { get; private set; }
        public static string Step4Descr { get; private set; }
        public static string Step5Descr { get; private set; }
        public static string CreateConfigCompleted { get; private set; }
        public static string CreateConfigError { get; private set; }


        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtWirenBoard.Controls.CtrlDeviceTree");
            SelectDevice = dict[nameof(SelectDevice)];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtWirenBoard.Controls.CtrlLineSelect");
            CommLineRequired = dict[nameof(CommLineRequired)];
            MqttLineRequired = dict[nameof(MqttLineRequired)];
            WirenBoardIpRequired = dict[nameof(WirenBoardIpRequired)];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtWirenBoard.Forms.FrmWirenBoardWizard");
            Step1Descr = dict[nameof(Step1Descr)];
            Step2Descr = dict[nameof(Step2Descr)];
            Step3Descr = dict[nameof(Step3Descr)];
            Step4Descr = dict[nameof(Step4Descr)];
            Step5Descr = dict[nameof(Step5Descr)];
            CreateConfigCompleted = dict[nameof(CreateConfigCompleted)];
            CreateConfigError = dict[nameof(CreateConfigError)];
        }
    }
}
