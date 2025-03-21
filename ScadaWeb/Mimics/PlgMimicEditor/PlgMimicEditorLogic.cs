// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Scada.Data.Entities;
using Scada.Lang;
using Scada.Log;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgMimicEditor.Code;
using Scada.Web.Services;
using Scada.Web.TreeView;
using Scada.Web.Users;

namespace Scada.Web.Plugins.PlgMimicEditor
{
    /// <summary>
    /// Implements the plugin logic.
    /// <para>Реализует логику плагина.</para>
    /// </summary>
    public class PlgMimicEditorLogic : PluginLogic
    {
        private readonly EditorManager editorManager;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PlgMimicEditorLogic(IWebContext webContext)
            : base(webContext)
        {
            Info = new EditorPluginInfo();
            editorManager = new EditorManager(webContext);
        }


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, Code, out string errMsg))
                Log.WriteError(WebPhrases.PluginMessage, Code, errMsg);

            EditorPhrases.Init();
        }

        /// <summary>
        /// Loads configuration.
        /// </summary>
        public override void LoadConfig()
        {
            editorManager.LoadConfig();
            editorManager.ObtainComponents();
        }

        /// <summary>
        /// Adds services to the DI container.
        /// </summary>
        public override void AddServices(IServiceCollection services)
        {
            services.AddSingleton(editorManager);
        }

        /// <summary>
        /// Gets menu items available for the specified user.
        /// </summary>
        public override List<MenuItem> GetUserMenuItems(User user, UserRights userRights)
        {
            if (!userRights.Full)
                return null;

            MenuItem parentItem = new()
            { 
                Text = EditorPhrases.EditorMenuItem,
                SortOrder = MenuItemSortOrder.First
            };

            parentItem.Subitems.AddRange(
            [
                new() { 
                    Text = EditorPhrases.MimicsMenuItem,
                    Url = "~/MimicEditor/MimicList",
                    SortOrder = MenuItemSortOrder.First
                }
            ]);

            return [parentItem];
        }
    }
}
