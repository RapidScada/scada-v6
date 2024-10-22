// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Server.Modules.ModDbExport.Config;

namespace Scada.Server.Modules.ModDbExport.Logic.Queries
{
    /// <summary>
    /// Represents a query filter that combines channel, device, and object filters.
    /// <para>Представляет фильтр запроса, который объединяет фильтры каналов, устройств и объектов.</para>
    /// </summary>
    internal class CombinedFilter
    {
        /// <summary>
        /// Gets or sets a value indicating whether the filter should be applied.
        /// </summary>
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// Gets the channel numbers.
        /// </summary>
        public HashSet<int> CnlNums { get; } = [];


        /// <summary>
        /// Initializes the filter.
        /// </summary>
        public void Init(QueryOptions queryOptions, ConfigDatabase configDatabase)
        {
            ArgumentNullException.ThrowIfNull(queryOptions, nameof(queryOptions));
            ArgumentNullException.ThrowIfNull(configDatabase, nameof(configDatabase));

            if (queryOptions.Filter.IsEmpty)
                return;

            Enabled = true;
            CnlNums.UnionWith(queryOptions.Filter.CnlNums);

            // extract channels from object filter
            if (queryOptions.Filter.ObjNums.Count > 0)
            {
                List<int> cnlNumsByObj = [];

                foreach (int objNum in queryOptions.Filter.ObjNums)
                {
                    foreach (Cnl cnl in configDatabase.CnlTable
                        .Select(new TableFilter("ObjNum", objNum), true)
                        .Where(c => c.Active))
                    {
                        for (int i = 0, len = cnl.GetDataLength(); i < len; i++)
                        {
                            cnlNumsByObj.Add(cnl.CnlNum + i);
                        }
                    }
                }

                if (CnlNums.Count > 0)
                    CnlNums.IntersectWith(cnlNumsByObj);
                else
                    CnlNums.UnionWith(cnlNumsByObj);
            }

            // extract channels from device filter
            if (queryOptions.Filter.DeviceNums.Count > 0)
            {
                List<int> cnlNumsByDevice = [];

                foreach (int deviceNum in queryOptions.Filter.DeviceNums)
                {
                    foreach (Cnl cnl in configDatabase.CnlTable
                        .Select(new TableFilter("DeviceNum", deviceNum), true)
                        .Where(c => c.Active))
                    {
                        for (int i = 0, len = cnl.GetDataLength(); i < len; i++)
                        {
                            cnlNumsByDevice.Add(cnl.CnlNum + i);
                        }
                    }
                }

                if (CnlNums.Count > 0)
                    CnlNums.IntersectWith(cnlNumsByDevice);
                else
                    CnlNums.UnionWith(cnlNumsByDevice);
            }
        }
    }
}
