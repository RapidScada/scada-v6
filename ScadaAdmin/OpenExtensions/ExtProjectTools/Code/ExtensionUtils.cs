// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Project;
using Scada.Data.Tables;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtProjectTools.Code
{
    /// <summary>
    /// The class provides helper methods for the extension.
    /// <para>Класс, предоставляющий вспомогательные методы для расширения.</para>
    /// </summary>
    internal static class ExtensionUtils
    {
        /// <summary>
        /// Normalizes the source IDs for importing into the configuration database table.
        /// </summary>
        public static void NormalizeIdRange(int minID, int maxID, 
            ref int srcStartID, ref int srcEndID, int destStartID, out int idOffset)
        {
            idOffset = destStartID - srcStartID;

            if (idOffset > 0)
                srcEndID = Math.Min(srcEndID, maxID - idOffset);
            else
                srcStartID = Math.Max(srcStartID, minID + idOffset);
        }

        /// <summary>
        /// Fills the combo box with the tables.
        /// </summary>
        public static void FillTableList(ComboBox comboBox, ConfigDatabase configDatabase, Type selectedItemType)
        {
            try
            {
                comboBox.BeginUpdate();
                int selectedIndex = 0;
                int index = 0;

                foreach (IBaseTable baseTable in configDatabase.AllTables.OrderBy(t => t.Title))
                {
                    if (baseTable.ItemType == selectedItemType)
                        selectedIndex = index;

                    comboBox.Items.Add(new BaseTableItem { BaseTable = baseTable });
                    index++;
                }

                comboBox.SelectedIndex = selectedIndex;
            }
            finally
            {
                comboBox.EndUpdate();
            }
        }
    }
}
