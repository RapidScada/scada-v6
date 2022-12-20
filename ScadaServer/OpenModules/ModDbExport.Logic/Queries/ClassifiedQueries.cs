// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
            CurDataQueries = new List<CurDataQuery>();
            HistDataQueries = new List<HistDataQuery>();
            EventQueries = new List<EventQuery>();
            EventAckQueries = new List<EventAckQuery>();
            CmdQueries = new List<CmdQuery>();
        }


        /// <summary>
        /// Gets the current data queries.
        /// </summary>
        public List<CurDataQuery> CurDataQueries { get; }

        /// <summary>
        /// Gets the historical data queries.
        /// </summary>
        public List<HistDataQuery> HistDataQueries { get; }

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
        public void CreateQueries(QueryOptionList queryOptionList)
        {
            foreach (QueryOptions queryOptions in queryOptionList)
            {
                if (queryOptions.Active)
                {
                    switch (queryOptions.DataKind)
                    {
                        case DataKind.Current:
                            CurDataQueries.Add(new CurDataQuery());
                            break;

                        case DataKind.Historical:
                            HistDataQueries.Add(new HistDataQuery());
                            break;

                        case DataKind.Event:
                            EventQueries.Add(new EventQuery());
                            break;

                        case DataKind.EventAck:
                            EventAckQueries.Add(new EventAckQuery());
                            break;

                        case DataKind.Command:
                            CmdQueries.Add(new CmdQuery());
                            break;
                    }
                }
            }
        }
    }
}
