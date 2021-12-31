// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Entities;
using Scada.Data.Models;
using System;

namespace Scada.Web.Plugins.PlgScheme.Models
{
    /// <summary>
    /// Represents channel properties for a scheme.
    /// <para>Представляет свойства канала для схемы.</para>
    /// </summary>
    public class CnlProps
    {
        public CnlProps(Cnl cnl, BaseDataSet baseDataSet)
        {
            ArgumentNullException.ThrowIfNull(cnl, nameof(cnl));
            ArgumentNullException.ThrowIfNull(baseDataSet, nameof(baseDataSet));

            CnlNum = cnl.CnlNum;
            JoinLen = cnl.IsString() ? cnl.GetDataLength() : 1;
            Unit = cnl.UnitID.HasValue ? baseDataSet.UnitTable.GetItem(cnl.UnitID.Value)?.Name : null;
        }

        public int CnlNum { get; set; }

        public int JoinLen { get; set; }

        public string Unit { get; set; }
    }
}
