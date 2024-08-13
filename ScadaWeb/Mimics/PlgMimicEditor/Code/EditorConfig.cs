// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;

namespace Scada.Web.Plugins.PlgMimicEditor.Code
{
    /// <summary>
    /// Represents an editor configuration.
    /// <para>Представляет конфигурацию редактора.</para>
    /// </summary>
    public class EditorConfig : ConfigBase
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = EditorPluginInfo.PluginCode + ".xml";
    }
}
