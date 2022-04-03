// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Admin.Extensions.ExtWirenBoard.Code
{
    /// <summary>
    /// Represents ecently selected parameters.
    /// <para>Представляет недавно выбранные параметры.</para>
    /// </summary>
    internal class RecentSelection
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RecentSelection()
        {
            Reset();
        }


        /// <summary>
        /// Gets or sets the instance name.
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        /// Gets or sets the communication line number.
        /// </summary>
        public int CommLineNum { get; set; }

        /// <summary>
        /// Gets or sets the IP address of the Wiren Board.
        /// </summary>
        public string WirenBoardIP { get; set; }

        /// <summary>
        /// Gets or sets the object number.
        /// </summary>
        public int ObjNum { get; set; }


        /// <summary>
        /// Resets the selected parameters.
        /// </summary>
        public void Reset()
        {
            InstanceName = "";
            CommLineNum = 0;
            WirenBoardIP = "";
            ObjNum = 0;
        }
    }
}
