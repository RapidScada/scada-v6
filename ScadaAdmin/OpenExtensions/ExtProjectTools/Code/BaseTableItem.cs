// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Tables;

namespace Scada.Admin.Extensions.ExtProjectTools.Code
{
    /// <summary>
    /// Represents an item of the table list.
    /// <para>Представляет элемент списка таблиц.</para>
    internal class BaseTableItem
    {
        public IBaseTable BaseTable { get; init; }

        public override string ToString() => BaseTable.Title;
    }
}
