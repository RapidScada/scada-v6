// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.MultiDb;
using Scada.Server.Modules.ModDbExport.Config;

namespace Scada.Server.Modules.ModDbExport.Logic.Queries
{
    /// <summary>
    /// Contains queries grouped by classes.
    /// <para>Содержит запросы, сгруппированные по классам.</para>
    /// </summary>
    internal class ClassifiedQueries
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ClassifiedQueries()
        {
            CurDataQueries = new List<DataQuery>();
            HistDataQueries = new List<DataQuery>();
            EventQueries = new List<EventQuery>();
            EventAckQueries = new List<EventAckQuery>();
            CmdQueries = new List<CmdQuery>();
        }


        /// <summary>
        /// Gets the current data queries.
        /// </summary>
        public List<DataQuery> CurDataQueries { get; }

        /// <summary>
        /// Gets the historical data queries.
        /// </summary>
        public List<DataQuery> HistDataQueries { get; }

        /// <summary>
        /// Gets the event queries.
        /// </summary>
        public List<EventQuery> EventQueries { get; }

        /// <summary>
        /// Gets the event acknowledgement queries.
        /// </summary>
        public List<EventAckQuery> EventAckQueries { get; }

        /// <summary>
        /// Gets the command queries.
        /// </summary>
        public List<CmdQuery> CmdQueries { get; }


        /// <summary>
        /// Creates queries according to the configuration.
        /// </summary>
        public void CreateQueries(QueryOptionList queryOptionList, DataSource dataSource)
        {
            ArgumentNullException.ThrowIfNull(queryOptionList, nameof(queryOptionList));
            ArgumentNullException.ThrowIfNull(dataSource, nameof(dataSource));

            foreach (QueryOptions queryOptions in queryOptionList)
            {
                if (queryOptions.Active)
                {
                    switch (queryOptions.DataKind)
                    {
                        case DataKind.Current:
                            CurDataQueries.Add(new DataQuery(queryOptions, dataSource));
                            break;

                        case DataKind.Historical:
                            HistDataQueries.Add(new DataQuery(queryOptions, dataSource));
                            break;

                        case DataKind.Event:
                            EventQueries.Add(new EventQuery(queryOptions, dataSource));
                            break;

                        case DataKind.EventAck:
                            EventAckQueries.Add(new EventAckQuery(queryOptions, dataSource));
                            break;

                        case DataKind.Command:
                            CmdQueries.Add(new CmdQuery(queryOptions, dataSource));
                            break;
                    }
                }
            }
        }
    }
}
