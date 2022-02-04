// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMain.Models
{
    /// <summary>
    /// Represents a data transfer object containing a telecontrol command.
    /// <para>Представляет объект передачи данных, содержащий команду телеуправления.</para>
    /// </summary>
    public class CommandDTO
    {
        /// <summary>
        /// Gets or sets the channel number.
        /// </summary>
        public int CnlNum { get; set; }

        /// <summary>
        /// Gets or sets the command value.
        /// </summary>
        public double CmdVal { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the command data is in hexadecimal format.
        /// </summary>
        public bool IsHex { get; set; }

        /// <summary>
        /// Gets or sets the command binary data.
        /// </summary>
        public string CmdData { get; set; }
    }
}
