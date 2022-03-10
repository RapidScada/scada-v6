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
        protected IWebContext WebContext { get; set; }

        /// <summary>
        /// Gets the root view nodes, which can contain child nodes.
        /// </summary>
        public List<ViewNode> ViewNodes { get; }


        /// <summary>
        /// Creates a branch of view nodes corresponding to the view path.
        /// </summary>
        protected ViewNode CreateBranch(View viewEntity)
        {
            // split view path
            BaseView.ParsePath(viewEntity, out string[] pathParts, out string nodeText);

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
        protected ViewNode CreateViewNode(View viewEntity, string text, string shortPath)
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
        protected void MergeViewNodes(List<ViewNode> existingNodes, List<ViewNode> addedNodes, int level)
        {
            if (addedNodes == null)
                return;

            addedNodes.Sort();

            foreach (ViewNode addedNode in addedNodes)
            {
                addedNode.Level = level;
                int ind = existingNodes.BinarySearch(addedNode);

                if (ind >= 0)
                {
                    // merge
                    ViewNode existingItem = existingNodes[ind];

                    if (existingItem.ChildNodes.Count > 0 && addedNode.ChildNodes.Count > 0)
                    {
                        // add child nodes recursively
                        MergeViewNodes(existingItem.ChildNodes, addedNode.ChildNodes, level + 1);
                    }
                    else
                    {
                        // simply add child nodes
                        addedNode.ChildNodes.Sort();
                        existingItem.ChildNodes.AddRange(addedNode.ChildNodes);
                        SetViewNodeLevels(addedNode.ChildNodes, level + 1);
                    }
                }
                else
                {
                    // insert the view node and its child nodes
                    addedNode.ChildNodes.Sort();
                    existingNodes.Insert(~ind, addedNode);
                    SetViewNodeLevels(addedNode.ChildNodes, level + 1);
                }
            }
        }

        /// <summary>
        /// Sets the nesting levels of the view nodes recursively.
        /// </summary>
        protected void SetViewNodeLevels(List<ViewNode> nodes, int level)
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
        protected ViewNode FindNonEmptyViewNode(List<ViewNode> viewNodes)
        {
            if (viewNodes != null)
            {
                foreach (ViewNode viewNode in viewNodes)
                {
                    if (viewNode.IsEmpty)
                    {
                        if (FindNonEmptyViewNode(viewNode.ChildNodes) is ViewNode node)
                            return node;
                    }
                    else
                    {
                        return viewNode;
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

                foreach (View viewEntity in webContext.BaseDataSet.ViewTable.EnumerateItems())
                {
                    if (!viewEntity.Hidden &&
                        userRights.GetRightByObj(viewEntity.ObjNum ?? 0).View &&
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
