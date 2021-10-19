// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Forms
{
    /// <summary>
    /// Translates Windows forms and controls.
    /// <para>Переводит формы и элементы управления Windows.</para>
    /// </summary>
    public static class FormTranslator
    {
        /// <summary>
        /// Recursively translates controls.
        /// </summary>
        private static void Translate(ICollection controls, ToolTip toolTip, 
            Dictionary<string, ControlPhrases> controlDict)
        {
            if (controls == null)
                return;

            foreach (object elem in controls)
            {
                ControlPhrases controlPhrases;

                if (elem is Control control)
                {
                    // process regular control
                    if (!string.IsNullOrEmpty(control.Name) /*some controls do not have a name*/ &&
                        controlDict.TryGetValue(control.Name, out controlPhrases))
                    {
                        if (controlPhrases.Text != null)
                            control.Text = controlPhrases.Text;

                        if (controlPhrases.ToolTip != null && toolTip != null)
                            toolTip.SetToolTip(control, controlPhrases.ToolTip);

                        if (controlPhrases.Items != null)
                        {
                            int itemCnt = controlPhrases.Items.Count;

                            if (elem is ComboBox comboBox)
                            {
                                for (int i = 0, cnt = Math.Min(comboBox.Items.Count, itemCnt); i < cnt; i++)
                                {
                                    string itemText = controlPhrases.Items[i];
                                    if (itemText != null)
                                        comboBox.Items[i] = itemText;
                                }
                            }
                            else if (elem is ListBox listBox)
                            {
                                for (int i = 0, cnt = Math.Min(listBox.Items.Count, itemCnt); i < cnt; i++)
                                {
                                    string itemText = controlPhrases.Items[i];
                                    if (itemText != null)
                                        listBox.Items[i] = itemText;
                                }
                            }
                            else if (elem is ListView listView)
                            {
                                for (int i = 0, cnt = Math.Min(listView.Items.Count, itemCnt); i < cnt; i++)
                                {
                                    string itemText = controlPhrases.Items[i];
                                    if (itemText != null)
                                        listView.Items[i].Text = itemText;
                                }
                            }
                        }
                    }

                    // process nested elements
                    if (elem is MenuStrip menuStrip)
                    {
                        Translate(menuStrip.Items, toolTip, controlDict);
                    }
                    else if (elem is ToolStrip toolStrip)
                    {
                        Translate(toolStrip.Items, toolTip, controlDict);
                    }
                    else if (elem is DataGridView dataGridView)
                    {
                        Translate(dataGridView.Columns, toolTip, controlDict);
                    }
                    else if (elem is ListView listView)
                    {
                        Translate(listView.Columns, toolTip, controlDict);
                        Translate(listView.Groups, toolTip, controlDict);
                    }

                    // process child controls
                    if (control.HasChildren)
                        Translate(control.Controls, toolTip, controlDict);
                }
                else
                {
                    // not inherited from Control
                    if (elem is ToolStripItem toolStripItem)
                    {
                        // menu
                        if (controlDict.TryGetValue(toolStripItem.Name, out controlPhrases))
                        {
                            if (controlPhrases.Text != null)
                                toolStripItem.Text = controlPhrases.Text;
                            if (controlPhrases.ToolTip != null)
                                toolStripItem.ToolTipText = controlPhrases.ToolTip;
                        }

                        // submenu
                        if (elem is ToolStripDropDownItem dropDownItem && dropDownItem.HasDropDownItems)
                            Translate(dropDownItem.DropDownItems, toolTip, controlDict);
                    }
                    else if (elem is DataGridViewColumn column)
                    {
                        if (controlDict.TryGetValue(column.Name, out controlPhrases) && controlPhrases.Text != null)
                            column.HeaderText = controlPhrases.Text;
                    }
                    else if (elem is ColumnHeader columnHeader)
                    {
                        if (controlDict.TryGetValue(columnHeader.Name, out controlPhrases) && controlPhrases.Text != null)
                            columnHeader.Text = controlPhrases.Text;
                    }
                    else if (elem is ListViewGroup listViewGroup)
                    {
                        if (controlDict.TryGetValue(listViewGroup.Name, out controlPhrases) && controlPhrases.Text != null)
                            listViewGroup.Header = controlPhrases.Text;
                    }
                }
            }
        }

        /// <summary>
        /// Translates the form using the specified dictionary.
        /// </summary>
        public static void Translate(Form form, string dictName, 
            ToolTip toolTip = null, params ContextMenuStrip[] contextMenus)
        {
            if (form != null && Locale.Dictionaries.TryGetValue(dictName, out LocaleDict localeDict))
            {
                Dictionary<string, ControlPhrases> controlDict = ControlPhrases.GetControlDict(localeDict);

                // translate form title
                if (controlDict.TryGetValue("this", out ControlPhrases controlPhrases) && controlPhrases.Text != null)
                    form.Text = controlPhrases.Title;

                // translate controls
                Translate(form.Controls, toolTip, controlDict);

                // translate context menus
                if (contextMenus != null)
                    Translate(contextMenus, null, controlDict);
            }
        }

        /// <summary>
        /// Translates the control using the specified dictionary.
        /// </summary>
        public static void Translate(Control control, string dictName, ToolTip toolTip = null)
        {
            if (control != null && Locale.Dictionaries.TryGetValue(dictName, out LocaleDict localeDict))
            {
                Translate(new Control[] { control }, toolTip, ControlPhrases.GetControlDict(localeDict));
            }
        }
    }
}
