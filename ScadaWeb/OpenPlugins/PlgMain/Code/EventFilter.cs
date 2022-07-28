// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Web.Users;

namespace Scada.Web.Plugins.PlgMain.Code
{
    /// <summary>
    /// Represents a filter for selecting events.
    /// <para>Представляет фильтр для выбора событий.</para>
    /// </summary>
    internal class EventFilter : DataFilter
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventFilter(int limit)
            : base(typeof(Event))
        {
            Limit = limit;
            OriginBegin = false;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventFilter(int limit, UserRights rights)
            : this(limit)
        {
            ArgumentNullException.ThrowIfNull(rights, nameof(rights));

            if (!rights.ViewAll)
                AddCondition("ObjNum", FilterOperator.In, rights.GetAvailableObjs().ToList());
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EventFilter(int limit, ViewBase view)
            : this(limit)
        {
            ArgumentNullException.ThrowIfNull(view, nameof(view));
            AddCondition("CnlNum", FilterOperator.In, view.CnlNumList);
        }
    }
}
