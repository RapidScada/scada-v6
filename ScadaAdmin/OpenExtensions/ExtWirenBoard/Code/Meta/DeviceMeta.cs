// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Admin.Extensions.ExtWirenBoard.Code.Meta
{
    /// <summary>
    /// Represents information associated with a device connected to Wiren Board.
    /// <para>Представляет информацию, связанную с устройством, подключенным к Wiren Board.</para>
    /// </summary>
    /// <remarks>See https://github.com/wirenboard/conventions</remarks>
    internal class DeviceMeta
    {
        public string Driver { get; set; }

        public Title Title { get; set; }

        public string Name { get; set; }
    }
}
