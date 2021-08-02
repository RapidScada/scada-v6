// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMain.Code
{
    /// <summary>
    /// Represents an event window specification.
    /// <para>Представляет спецификацию окна событий.</para>
    /// </summary>
    public class EventWindowSpec : DataWindowSpec
    {
        /// <summary>
        /// Gets the window title.
        /// </summary>
        public override string Title => PluginPhrases.EventWindowTitle;

        /// <summary>
        /// Gets the URL of the window frame.
        /// </summary>
        public override string Url => "~/Main/Events";
    }
}
