// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Entities;
using Scada.Data.Models;

namespace Scada.Web.Plugins.PlgWebPage.Code
{
    /// <summary>
    /// Represents a chart view.
    /// <para>Представляет представление графика.</para>
    /// </summary>
    public class WebPageView : ViewBase
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public WebPageView(View viewEntity)
            : base(viewEntity)
        {
            StoredOnServer = false;
        }
    }
}
