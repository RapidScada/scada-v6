// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using Scada.Web.Config;

namespace Scada.Web.Plugins.PlgMain.Code
{
    /// <summary>
    /// Contains plugin data.
    /// <para>Содержит данные плагина.</para>
    /// </summary>
    public class PluginContext
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PluginContext()
        {
            Options = new PluginOptions(new OptionList());
        }


        /// <summary>
        /// Gets the plugin options.
        /// </summary>
        public PluginOptions Options { get; private set; }


        /// <summary>
        /// Loads the plugin options.
        /// </summary>
        public void LoadOptions(WebConfig webConfig)
        {
            Options = new PluginOptions(webConfig.GetOptions("Main"))
            {
                RefreshRate = webConfig.DisplayOptions.RefreshRate
            };
        }

        /// <summary>
        /// Gets the options of the specified table view, or the default options.
        /// </summary>
        public TableOptions GetTableOptions(TableView tableView)
        {
            if (tableView == null || tableView.Options.UseDefault)
            {
                return new TableOptions
                {
                    UseDefault = true,
                    ArchiveCode = Options.TableArchiveCode,
                    Period = Options.TablePeriod,
                    ChartArgs = ""
                };
            }
            else
            {
                return tableView.Options;
            }
        }
    }
}
