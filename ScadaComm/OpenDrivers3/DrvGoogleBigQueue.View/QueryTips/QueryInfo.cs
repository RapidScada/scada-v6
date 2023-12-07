// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Dbms;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvGoogleBigQueue.View
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
        public QueryInfo()
        {

        }

        /// <summary>
        /// Gets event query parameters.
        /// </summary>
        public IEnumerable<QueryParam> GetAllParams()
        {
            dynamic dict = Locale.GetDictionary(GetType().FullName + ".AllParams");

            List<QueryParam> queryParams = new()
            {
                new QueryParam("timeserie_name", dict.TimeserieName),
                new QueryParam("timestamp", dict.Timestamp),
                new QueryParam("value", dict.Value),
                new QueryParam("@lastQueryTime", dict.LastQueryTime)
            };

            return queryParams;
        }

        /// <summary>
        /// Gets event acknowledgement query parameters.
        /// </summary>
        private IEnumerable<QueryParam> GetEventAckParams()
        {
            dynamic dict = Locale.GetDictionary(GetType().FullName + ".EventAck");

            List<QueryParam> queryParams = new()
            {
                new QueryParam("@eventID", dict.EventID),
                new QueryParam("@ackTimestamp", dict.AckTimestamp),
                new QueryParam("@ackUserID", dict.AckUserID)
            };

            return queryParams;
        }
    }
}
