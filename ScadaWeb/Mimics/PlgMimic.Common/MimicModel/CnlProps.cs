// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Entities;
using Scada.Data.Models;

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Represents channel properties used when displaying data.
    /// <para>Представляет свойства канала, используемые при отображении данных.</para>
    /// </summary>
    public class CnlProps
    {
        public CnlProps(int cnlNum, ConfigDataset configDataset)
            : this(configDataset.CnlTable.GetItem(cnlNum), configDataset)
        {
        }

        public CnlProps(Cnl cnl, ConfigDataset configDataset)
        {
            if (cnl == null)
            {
                JoinLen = 1;
                Unit = null;
            }
            else
            {
                JoinLen = cnl.GetJoinLength();
                Unit = cnl.UnitID.HasValue ? configDataset?.UnitTable.GetItem(cnl.UnitID.Value)?.Name : null;
            }
        }


        public int JoinLen { get; }

        public string Unit { get; }
    }
}
