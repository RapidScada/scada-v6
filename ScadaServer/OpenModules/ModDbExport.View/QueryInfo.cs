// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Dbms;
using Scada.Lang;
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
        public QueryInfo(QueryOptions queryOptions, KnownDBMS knownDBMS)
        {
            QueryOptions = queryOptions ?? throw new ArgumentNullException(nameof(queryOptions));
            KnownDBMS = knownDBMS;
        }


        /// <summary>
        /// Gets the query options.
        /// </summary>
        public QueryOptions QueryOptions { get; }

        /// <summary>
        /// Gets the database to generate query parameters.
        /// </summary>
        public KnownDBMS KnownDBMS { get; }


        /// <summary>
        /// Gets data query parameters.
        /// </summary>
        private IEnumerable<QueryParam> GetDataParams()
        {
            dynamic dict = Locale.GetDictionary(GetType().FullName + ".Data");

            List<QueryParam> queryParams = new()
            {
                new QueryParam("@timestamp", dict.Timestamp),
            };

            if (QueryOptions.SingleQuery)
            {
                foreach (int cnlNum in QueryOptions.Filter.CnlNums)
                {
                    queryParams.Add(new QueryParam("@val" + cnlNum, string.Format(dict.ChannelValue, cnlNum)));
                    queryParams.Add(new QueryParam("@stat" + cnlNum, string.Format(dict.ChannelStatus, cnlNum)));
                }

                if (KnownDBMS != KnownDBMS.Oracle)
                {
                    queryParams.Add(new QueryParam("@objNum", dict.FirstObject));
                    queryParams.Add(new QueryParam("@deviceNum", dict.FirstDevice));
                }
            }
            else
            {
                queryParams.Add(new QueryParam("@cnlNum", dict.CnlNum));
                queryParams.Add(new QueryParam("@val", dict.Val));
                queryParams.Add(new QueryParam("@stat", dict.Stat));

                if (KnownDBMS != KnownDBMS.Oracle)
                {
                    queryParams.Add(new QueryParam("@objNum", dict.ObjNum));
                    queryParams.Add(new QueryParam("@deviceNum", dict.DeviceNum));
                }
            }

            return queryParams;
        }

        /// <summary>
        /// Gets event query parameters.
        /// </summary>
        private IEnumerable<QueryParam> GetEventParams()
        {
            dynamic dict = Locale.GetDictionary(GetType().FullName + ".Event");

            List<QueryParam> queryParams = new()
            {
                new QueryParam("@eventID", dict.EventID),
                new QueryParam("@timestamp", dict.Timestamp),
                new QueryParam("@hidden", dict.Hidden),
                new QueryParam("@cnlNum", dict.CnlNum),
                new QueryParam("@objNum", dict.ObjNum),
                new QueryParam("@deviceNum", dict.DeviceNum),
                new QueryParam("@prevCnlVal", dict.PrevCnlVal),
                new QueryParam("@prevCnlStat", dict.PrevCnlStat),
                new QueryParam("@cnlVal", dict.CnlVal),
                new QueryParam("@cnlStat", dict.CnlStat),
                new QueryParam("@severity", dict.Severity),
                new QueryParam("@ackRequired", dict.AckRequired),
                new QueryParam("@ack", dict.Ack),
                new QueryParam("@ackTimestamp", dict.AckTimestamp),
                new QueryParam("@ackUserID", dict.AckUserID),
                new QueryParam("@textFormat", dict.TextFormat),
                new QueryParam("@eventText", dict.EventText),
                new QueryParam("@eventData", dict.EventData)
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

        /// <summary>
        /// Gets command query parameters.
        /// </summary>
        private IEnumerable<QueryParam> GetCommandParams()
        {
            dynamic dict = Locale.GetDictionary(GetType().FullName + ".Command");

            List<QueryParam> queryParams = new()
            {
                new QueryParam("@commandID", dict.CommandID),
                new QueryParam("@creationTime", dict.CreationTime),
                new QueryParam("@clientName", dict.ClientName),
                new QueryParam("@userID", dict.UserID),
                new QueryParam("@cnlNum", dict.CnlNum),
                new QueryParam("@objNum", dict.ObjNum),
                new QueryParam("@deviceNum", dict.DeviceNum),
                new QueryParam("@cmdNum", dict.CmdNum),
                new QueryParam("@cmdCode", dict.CmdCode),
                new QueryParam("@cmdVal", dict.CmdVal),
                new QueryParam("@cmdData", dict.CmdData)
            };

            return queryParams;
        }

        /// <summary>
        /// Updates the query parameters in case of using Oracle.
        /// </summary>
        private static void UpdateParamsForOracle(IEnumerable<QueryParam> queryParams)
        {
            int paramNum = 1;

            foreach (QueryParam param in queryParams)
            {
                param.Name = ":" + paramNum;
                paramNum++;
            }
        }

        /// <summary>
        /// Gets the parameters available in the SQL request.
        /// </summary>
        public IEnumerable<QueryParam> GetSqlParameters()
        {
            IEnumerable<QueryParam> queryParams = QueryOptions.DataKind switch
            {
                DataKind.Current or DataKind.Historical => GetDataParams(),
                DataKind.Event => GetEventParams(),
                DataKind.EventAck => GetEventAckParams(),
                DataKind.Command => GetCommandParams(),
                _ => new List<QueryParam>()
            };

            if (KnownDBMS == KnownDBMS.Oracle)
                UpdateParamsForOracle(queryParams);

            return queryParams;
        }
    }
}
