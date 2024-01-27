/*
 * Copyright 2024 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaWebCommon
 * Summary  : Provides the examples for development purposes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2023
 */

namespace Scada.Web.TreeView
{
    /// <summary>
    /// Provides the examples for development purposes.
    /// <para>Предоставляет примеры для целей разработки.</para>
    /// </summary>
    public static class TreeViewExample
    {
        /// <summary>
        /// Gets the main menu example.
        /// </summary>
        public static List<MenuItem> GetMainMenu()
        {
            List<MenuItem> menuItems = new()
            {
                new MenuItem
                {
                    Text = "Reports",
                    Url = "Reports.html",
                    Level = 0
                }
            };

            MenuItem pluginsItem = new() { Text = "Plugins", Level = 0 };
            menuItems.Add(pluginsItem);

            pluginsItem.Subitems.Add(new MenuItem
            {
                Parent = pluginsItem,
                Text = "Installed",
                Url = "InstalledPlugins.html",
                Level = 1
            });

            pluginsItem.Subitems.Add(new MenuItem
            {
                Parent = pluginsItem,
                Text = "Download",
                Url = "DownloadPlugins.html",
                Level = 1
            });

            menuItems.Add(new MenuItem
            {
                Parent = pluginsItem,
                Text = "About",
                Url = "About.html",
                Level = 0
            });

            return menuItems;
        }

        /// <summary>
        /// Gets the view explorer example.
        /// </summary>
        public static List<ViewNode> GetViewExplorer()
        {
            List<ViewNode> viewNodes = new();

            int viewID = 0;
            ViewNode rootNode = new(++viewID) { Text = "Hello World", Level = 0 };
            viewNodes.Add(rootNode);

            ViewNode CreateViewNode(int level)
            {
                return new ViewNode(++viewID)
                {
                    Text = "View " + viewID,
                    Url = WebPath.GetViewPath(viewID).PrependTilde(),
                    ViewFrameUrl = "~/Main/TableView/" + viewID,
                    Level = level
                };
            }

            rootNode.ChildNodes.Add(CreateViewNode(1));
            rootNode.ChildNodes.Add(CreateViewNode(1));
            rootNode.ChildNodes.Add(CreateViewNode(1));

            ViewNode folderNode = new(++viewID) { Text = "Folder", Level = 1 };
            rootNode.ChildNodes.Add(folderNode);

            folderNode.ChildNodes.Add(CreateViewNode(2));
            folderNode.ChildNodes.Add(CreateViewNode(2));

            return viewNodes;
        }
    }
}
