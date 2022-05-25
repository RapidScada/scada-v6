/*
 * Copyright 2022 Rapid Software LLC
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
 * Summary  : Contains view explorer nodes available to a user
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2022
 */

using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Web.Plugins;
using Scada.Web.Services;
using Scada.Web.TreeView;
using System;
using System.Collections.Generic;
using System.IO;

namespace Scada.Web.Users
{
    /// <summary>
    /// Contains view explorer nodes available to a user.
    /// <para>Содержит узлы дерева представлений, доступные пользователю.</para>
    /// </summary>
    public class UserViews
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public UserViews()
        {
            WebContext = null;
            ViewNodes = new List<ViewNode>();
        }


        /// <summary>
        /// Gets or sets the web application context for the current initialization operation.
        /// </summary>
        private IWebContext WebContext { get; set; }

        /// <summary>
        /// Gets the root view nodes, which can contain child nodes.
        /// </summary>
        public List<ViewNode> ViewNodes { get; }


        /// <summary>
        /// Creates a branch of view nodes corresponding to the view path.
        /// </summary>
        private ViewNode CreateBranch(View viewEntity)
        {
            // split view path
            ViewBase.ParsePath(viewEntity, out string[] pathParts, out string nodeText);

            if (string.IsNullOrEmpty(nodeText))
                return null;

            // build branch
            if (pathParts.Length > 1)
            {
                string shortPath = pathParts[0];
                ViewNode rootNode = new() { Text = shortPath, ShortPath = shortPath };
                ViewNode currentNode = rootNode;

                for (int i = 1, len = pathParts.Length - 1; i < len; i++)
                {
                    shortPath = pathParts[i];
                    ViewNode childNode = new() { Text = shortPath, ShortPath = shortPath };
                    currentNode.ChildNodes.Add(childNode);
                    currentNode = childNode;
                }

                currentNode.ChildNodes.Add(CreateViewNode(viewEntity, nodeText, pathParts[^1]));
                return rootNode;
            }
            else
            {
                return CreateViewNode(viewEntity, nodeText, pathParts[0]);
            }
        }

        /// <summary>
        /// Creates a view node.
        /// </summary>
        private ViewNode CreateViewNode(View viewEntity, string text, string shortPath)
        {
            int viewID = viewEntity.ViewID;

            ViewNode viewNode = new(viewID)
            {
                Text = text,
                ShortPath = shortPath,
                SortOrder = viewEntity.Ord ?? 0
            };

            if (viewID > 0 &&
                WebContext.GetViewSpec(viewEntity) is ViewSpec viewSpec)
            {
                viewNode.IconUrl = viewSpec.IconUrl;
                viewNode.Url = WebPath.GetViewPath(viewID).PrependTilda();
                viewNode.ViewFrameUrl = viewSpec.GetFrameUrl(viewID);
                viewNode.DataAttrs.Add("viewFrameUrl", viewNode.ViewFrameUrl);
            }

            return viewNode;
        }

        /// <summary>
        /// Merges the view nodes recursively.
        /// </summary>
        private static void MergeViewNodes(List<ViewNode> existingNodes, List<ViewNode> addedNodes, int level)
        {
            if (addedNodes == null)
                return;

            foreach (ViewNode addedNode in addedNodes)
            {
                addedNode.Level = level;
                int nodeIndex = addedNode.ViewID > 0 ? -1 : FindViewNodeByPath(existingNodes, addedNode.ShortPath);

                if (nodeIndex >= 0)
                {
                    // merge
                    if (addedNode.ChildNodes.Count > 0)
                    {
                        ViewNode existingNode = existingNodes[nodeIndex];

                        if (existingNode.ChildNodes.Count > 0)
                        {
                            // add child nodes recursively
                            MergeViewNodes(existingNode.ChildNodes, addedNode.ChildNodes, level + 1);
                        }
                        else
                        {
                            // simply add child nodes
                            existingNode.ChildNodes.AddRange(addedNode.ChildNodes);
                            SetViewNodeLevels(addedNode.ChildNodes, level + 1);
                        }
                    }
                }
                else
                {
                    // add view node and its child nodes
                    existingNodes.Add(addedNode);
                    SetViewNodeLevels(addedNode.ChildNodes, level + 1);
                }
            }
        }

        /// <summary>
        /// Finds a node by the specified short path.
        /// </summary>
        private static int FindViewNodeByPath(List<ViewNode> nodes, string shortPath)
        {
            if (nodes != null)
            {
                // search from end
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    if (string.Equals(nodes[i].ShortPath, shortPath, StringComparison.OrdinalIgnoreCase))
                        return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Sets the nesting levels of the view nodes recursively.
        /// </summary>
        private static void SetViewNodeLevels(List<ViewNode> nodes, int level)
        {
            if (nodes != null)
            {
                foreach (ViewNode node in nodes)
                {
                    node.Level = level;
                    SetViewNodeLevels(node.ChildNodes, level + 1);
                }
            }
        }

        /// <summary>
        /// Finds a non-empty view node recursively.
        /// </summary>
        private static ViewNode FindNonEmptyViewNode(List<ViewNode> nodes)
        {
            if (nodes != null)
            {
                foreach (ViewNode node in nodes)
                {
                    if (node.IsEmpty)
                    {
                        if (FindNonEmptyViewNode(node.ChildNodes) is ViewNode childNode)
                            return childNode;
                    }
                    else
                    {
                        return node;
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// Initializes the user views.
        /// </summary>
        public void Init(IWebContext webContext, UserRights userRights)
        {
            if (webContext == null)
                throw new ArgumentNullException(nameof(webContext));
            if (userRights == null)
                throw new ArgumentNullException(nameof(userRights));

            try
            {
                WebContext = webContext;

                foreach (View viewEntity in webContext.ConfigDatabase.SortedViews)
                {
                    if (!viewEntity.Hidden &&
                        userRights.GetRightByObj(viewEntity.ObjNum).View &&
                        CreateBranch(viewEntity) is ViewNode branchRootNode)
                    {
                        MergeViewNodes(ViewNodes, new List<ViewNode> { branchRootNode }, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при инициализации представлений пользователя" :
                    "Error initializing user views");
            }
        }

        /// <summary>
        /// Gets the ID of the first non-empty view node.
        /// </summary>
        public int? GetFirstViewID()
        {
            return FindNonEmptyViewNode(ViewNodes)?.ViewID;
        }
    }
}
