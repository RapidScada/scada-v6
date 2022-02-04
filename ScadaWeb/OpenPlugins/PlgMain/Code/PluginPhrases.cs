// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Web.Plugins.PlgMain.Code
{
    /// <summary>
    /// The phrases used by the plugin.
    /// <para>Фразы, используемые плагином.</para>
    /// </summary>
    internal static class PluginPhrases
    {
        // Scada.Web.Plugins.PlgMain.Areas.Main.Pages.TableView
        public static string TableViewTitle { get; private set; }
        public static string PrevDate { get; private set; }
        public static string SelDate { get; private set; }
        public static string MinusOneDay { get; private set; }
        public static string ItemColumn { get; private set; }
        public static string CurrentColumn { get; private set; }
        public static string CnlTip { get; private set; }
        public static string DeviceTip { get; private set; }
        public static string ObjTip { get; private set; }
        public static string QuantityTip { get; private set; }
        public static string UnitTip { get; private set; }
        public static string SendCommandTip { get; private set; }

        // Scada.Web.Plugins.PlgMain.Code.EventWindowSpec
        public static string EventWindowTitle { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Web.Plugins.PlgMain.Areas.Main.Pages.TableView");
            TableViewTitle = dict[nameof(TableViewTitle)];
            PrevDate = dict[nameof(PrevDate)];
            SelDate = dict[nameof(SelDate)];
            MinusOneDay = dict[nameof(MinusOneDay)];
            ItemColumn = dict[nameof(ItemColumn)];
            CurrentColumn = dict[nameof(CurrentColumn)];
            CnlTip = dict[nameof(CnlTip)];
            DeviceTip = dict[nameof(DeviceTip)];
            ObjTip = dict[nameof(ObjTip)];
            QuantityTip = dict[nameof(QuantityTip)];
            UnitTip = dict[nameof(UnitTip)];
            SendCommandTip = dict[nameof(SendCommandTip)];

            dict = Locale.GetDictionary("Scada.Web.Plugins.PlgMain.Code.EventWindowSpec");
            EventWindowTitle = dict[nameof(EventWindowTitle)];
        }
    }
}
