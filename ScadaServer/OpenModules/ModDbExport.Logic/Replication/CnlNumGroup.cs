// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Server.Modules.ModDbExport.Logic.Replication
{
    /// <summary>
    /// Represents a group of channel numbers to export.
    /// <para>Представляет группу номеров каналов для экспорта.</para>
    /// </summary>
    internal class CnlNumGroup
    {
        private int[] cnlNums;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlNumGroup(int length)
        {
            cnlNums = new int[length];

            QueryID = 0;
            SingleQuery = null;
        }


        /// <summary>
        /// Gets the channel numbers.
        /// </summary>
        public int[] CnlNums => cnlNums;

        /// <summary>
        /// Gets the ID of the query that exports the channels of the group.
        /// </summary>
        public int QueryID { get; init; }

        /// <summary>
        /// Gets a value indicating whether the group is intended for the specified type of query.
        /// </summary>
        public bool? SingleQuery { get; init; }
        
        
        /// <summary>
        /// Changes the number of elements of the channel array.
        /// </summary>
        public void Resize(int newSize)
        {
            Array.Resize(ref cnlNums, newSize);
        }
    }
}
