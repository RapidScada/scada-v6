// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;

namespace Scada.Admin.Extensions.ExtWirenBoard.Code.Meta
{
    /// <summary>
    /// Represents a device or control title.
    /// <para>Представляет наименование устройства или элемента.</para>
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    internal struct Title
    {
        public string En { get; set; }

        public string Ru { get; set; }

        public override string ToString() => ScadaUtils.FirstNonEmpty(En, Ru);
    }
}
