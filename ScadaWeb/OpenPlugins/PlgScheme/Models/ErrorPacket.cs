// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Scada.Web.Plugins.PlgScheme.Models
{
    /// <summary>
    /// Represents a package containing scheme errors.
    /// <para>Представляет пакет, содержащий ошибки схемы.</para>
    /// </summary>
    public class ErrorPacket : SchemePacket
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ErrorPacket(SchemeView schemeView)
        {
            ArgumentNullException.ThrowIfNull(schemeView, nameof(schemeView));
            ViewStamp = schemeView.ViewStamp.ToString();
            Errors = schemeView.LoadErrors;
        }

        /// <summary>
        /// Gets the scheme errors.
        /// </summary>
        public List<string> Errors { get; }
    }
}
