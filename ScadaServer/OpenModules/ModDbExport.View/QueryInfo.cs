// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Server.Modules.ModDbExport.Config;

namespace Scada.Server.Modules.ModDbExport.View
{
    /// <summary>
    /// Represents information associated with a query.
    /// <para>Представляет информацию, связанную с запросом.</para>
    /// </summary>
    internal class QueryInfo
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public QueryInfo(QueryOptions queryOptions)
        {
            QueryOptions = queryOptions ?? throw new ArgumentNullException(nameof(queryOptions));
        }

        /// <summary>
        /// Gets the query options.
        /// </summary>
        public QueryOptions QueryOptions { get; }

        /// <summary>
        /// Gets the parameters available in the SQL request.
        /// </summary>
        public IDictionary<string, string> GetSqlParameters()
        {
            return new Dictionary<string, string>() 
            { 
                { "@test", "Test parameter" } 
            };
        }
    }
}
