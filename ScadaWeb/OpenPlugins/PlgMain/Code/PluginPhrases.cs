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
        public static string PrevDate { get; private set; }
        public static string SelDate { get; private set; }
        public static string MinusOneDay { get; private set; }
        public static string ItemColumn { get; private set; }
        public static string CurrentColumn { get; private set; }
        public static string InCnlTip { get; private set; }
        public static string OutCnlTip { get; private set; }
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
            PrevDate = dict["PrevDate"];
            SelDate = dict["SelDate"];
            MinusOneDay = dict["MinusOneDay"];
            ItemColumn = dict["ItemColumn"];
            CurrentColumn = dict["CurrentColumn"];
            InCnlTip = dict["InCnlTip"];
            OutCnlTip = dict["OutCnlTip"];
            DeviceTip = dict["DeviceTip"];
            ObjTip = dict["ObjTip"];
            QuantityTip = dict["QuantityTip"];
            UnitTip = dict["UnitTip"];
            SendCommandTip = dict["SendCommandTip"];

            dict = Locale.GetDictionary("Scada.Web.Plugins.PlgMain.Code.EventWindowSpec");
            EventWindowTitle = dict["EventWindowTitle"];
        }
    }
}
