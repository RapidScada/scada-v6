// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Admin.Extensions.ExtDepAgent
{
    /// <summary>
    /// The phrases used by the extension.
    /// <para>Фразы, используемые расширением.</para>
    /// </summary>
    internal static class ExtensionPhrases
    {
        public static string AgentNotEnabled { get; private set; }

        static ExtensionPhrases()
        {
            if (Locale.IsRussian)
            {
                AgentNotEnabled = "Агент не включен в профиле развёртывания.";
            }
            else
            {
                AgentNotEnabled = "Agent is not enabled in the deployment profile.";
            }
        }
    }
}
