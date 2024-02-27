// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using Scada.Forms.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Scada.Forms.ComponentModel
{
    /// <summary>
    /// Represents a numeric range editor for PropertyGrid.
    /// <para>Представляет редактор числового диапазона для PropertyGrid.</para>
    /// </summary>
    public class RangeEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider?.GetService(typeof(IWindowsFormsEditorService)) is IWindowsFormsEditorService editorService &&
                value is ICollection<int> collection)
            {
                editorService.ShowDialog(new FrmRangeEdit
                {
                    Range = collection,
                    AllowEmpty = true
                });
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}
