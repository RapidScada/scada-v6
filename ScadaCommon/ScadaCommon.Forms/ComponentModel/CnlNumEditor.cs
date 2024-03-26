// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using Scada.ComponentModel;
using Scada.Data.Models;
using Scada.Forms.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Scada.Forms.ComponentModel
{
    /// <summary>
    /// Represents a channel number editor for PropertyGrid.
    /// <para>Представляет редактор номера канала для PropertyGrid.</para>
    /// </summary>
    public class CnlNumEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context?.Instance is IConfigDatasetAccessor configDatasetAccessor &&
                configDatasetAccessor.ConfigDataset is ConfigDataset configDataset &&
                provider?.GetService(typeof(IWindowsFormsEditorService)) is IWindowsFormsEditorService editorService &&
                value is int cnlNum)
            {
                FrmCnlSelect form = new(configDataset)
                {
                    MultiSelect = false,
                    SelectedCnlNum = cnlNum
                };

                if (editorService.ShowDialog(form) == DialogResult.OK)
                    return form.SelectedCnlNum;
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}
