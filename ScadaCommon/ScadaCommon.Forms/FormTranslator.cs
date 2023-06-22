// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using System.Collections;

namespace Scada.Forms
{
    /// <summary>
    /// Translates Windows forms and controls.
    /// <para>Переводит формы и элементы управления Windows.</para>
    /// </summary>
    public static class FormTranslator
    {
        /// <summary>
        /// The default translator options.
        /// </summary>
        private static readonly FormTranslatorOptions DefaultOptions = new();
        /// <summary>
        /// The default translator options for translating controls.
        /// </summary>
        private static readonly FormTranslatorOptions DefaultOptionsForControls = new() { SkipUserControls = false };


        /// <summary>
        /// Recursively translates controls.
        /// </summary>
        private static void Translate(ICollection controls, Dictionary<string, ControlPhrases> controlDict,
            FormTranslatorOptions options)
        {
            if (controls == null)
                return;

            foreach (object elem in controls)
            {
                ControlPhrases controlPhrases;

                if (elem is Control control)
                {
                    // skip user controls
                    if (options.SkipUserControls && elem is UserControl)
                        continue;

                    // process regular control
                    if (!string.IsNullOrEmpty(control.Name) /*some controls do not have a name*/ &&
                        controlDict.TryGetValue(control.Name, out controlPhrases))
                    {
                        if (controlPhrases.Text != null)
                            control.Text = controlPhrases.Text;

                        if (controlPhrases.ToolTip != null && options.ToolTip != null)
                            options.ToolTip.SetToolTip(control, controlPhrases.ToolTip);

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
                        Translate(menuStrip.Items, controlDict, options);
                    }
                    else if (elem is ToolStrip toolStrip)
                    {
                        Translate(toolStrip.Items, controlDict, options);
                    }
                    else if (elem is DataGridView dataGridView)
                    {
                        Translate(dataGridView.Columns, controlDict, options);
                    }
                    else if (elem is ListView listView)
                    {
                        Translate(listView.Columns, controlDict, options);
                        Translate(listView.Groups, controlDict, options);
                    }

                    // process child controls
                    if (control.HasChildren)
                        Translate(control.Controls, controlDict, options);
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
                            Translate(dropDownItem.DropDownItems, controlDict, options);
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
        public static void Translate(Form form, string dictName, FormTranslatorOptions options = null)
        {
            if (form != null && Locale.Dictionaries.TryGetValue(dictName, out LocaleDict localeDict))
            {
                options ??= DefaultOptions;
                Dictionary<string, ControlPhrases> controlDict = ControlPhrases.GetControlDict(localeDict);

                // translate form title
                if (controlDict.TryGetValue("this", out ControlPhrases controlPhrases) && controlPhrases.Text != null)
                    form.Text = controlPhrases.Title;

                // translate controls
                Translate(form.Controls, controlDict, options);

                // translate context menus
                if (options.ContextMenus != null)
                    Translate(options.ContextMenus, controlDict, options);
            }
        }

        /// <summary>
        /// Translates the control using the specified dictionary.
        /// </summary>
        public static void Translate(Control control, string dictName, FormTranslatorOptions options = null)
        {
            if (control != null && Locale.Dictionaries.TryGetValue(dictName, out LocaleDict localeDict))
            {
                options ??= DefaultOptionsForControls;
                Dictionary<string, ControlPhrases> controlDict = ControlPhrases.GetControlDict(localeDict);
                Translate(new Control[] { control }, controlDict, options);

                if (options.ContextMenus != null)
                    Translate(options.ContextMenus, controlDict, options);
            }
        }
    }
}
