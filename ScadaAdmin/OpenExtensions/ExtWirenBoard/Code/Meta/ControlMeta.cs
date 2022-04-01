// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Admin.Extensions.ExtWirenBoard.Code.Meta
{
    /// <summary>
    /// Represents information associated with a device control.
    /// <para>Представляет информацию, связанную с элементом устройства.</para>
    /// </summary>
    internal class ControlMeta
    {
        public string Type { get; set; }

        public string Units { get; set; }

        public double Min { get; set; }

        public double Max { get; set; }

        public double Precision { get; set; }

        public int Order { get; set; }

        public bool ReadOnly { get; set; }

        public Title Title { get; set; }

        public string Name { get; set; }
    }
}
