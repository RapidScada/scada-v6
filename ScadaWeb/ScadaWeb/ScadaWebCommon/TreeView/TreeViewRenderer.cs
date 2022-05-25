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
 * Summary  : Renders tree view HTML
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2021
 */

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Scada.Web.TreeView
{
    /// <summary>
    /// Renders tree view HTML.
    /// <para>Формирует HTML-код дерева.</para>
    /// </summary>
    public class TreeViewRenderer
    {
        /// <summary>
        /// Represents renderer options.
        /// </summary>
        public class Options
        {
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public Options()
            {
                ExpanderLeft = false;
                ShowIcons = false;
                FolderIconUrl = "";
                NodeIconUrl = "";
            }

            /// <summary>
            /// Gets or sets a value indicating whether expanders are located to the left.
            /// </summary>
            public bool ExpanderLeft { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether to show node icons.
            /// </summary>
            public bool ShowIcons { get; set; }
            /// <summary>
            /// Gets or sets the folder icon URL.
            /// </summary>
            public string FolderIconUrl { get; set; }
            /// <summary>
            /// Gets or sets the default node icon URL.
            /// </summary>
            public string NodeIconUrl { get; set; }
        }

        private readonly IUrlHelper urlHelper;
        private readonly Options options;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TreeViewRenderer(IUrlHelper urlHelper)
            : this(urlHelper, new Options())
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TreeViewRenderer(IUrlHelper urlHelper, Options options)
        {
            this.urlHelper = urlHelper ?? throw new ArgumentNullException(nameof(urlHelper));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            SelectedObject = null;
        }


        /// <summary>
        /// Gets or sets the selected object that is represented by a tree node.
        /// </summary>
        public object SelectedObject { get; set; }


        /// <summary>
        /// Renders HTML for the node data attributes.
        /// </summary>
        protected string RenderDataAttrs(IWebTreeNode webTreeNode)
        {
            const string DataAttrTemplate = " data-{0}='{1}'";

            StringBuilder sbAttr = new();
            sbAttr.AppendFormat(DataAttrTemplate, "level", webTreeNode.Level);

            if (webTreeNode.DataAttrs != null)
            {
                foreach (KeyValuePair<string, string> pair in webTreeNode.DataAttrs)
                {
                    if (!string.IsNullOrWhiteSpace(pair.Key))
                    {
                        string val = pair.Key.EndsWith("url", StringComparison.OrdinalIgnoreCase)
                            ? urlHelper.Content(pair.Value)
                            : pair.Value;
                        sbAttr.AppendFormat(DataAttrTemplate, pair.Key, val);
                    }
                }
            }

            return sbAttr.ToString();
        }

        /// <summary>
        /// Renders HTML for the tree view recursively.
        /// </summary>
        protected void RenderTreeView(IList treeNodes, bool topLevel, StringBuilder sbHtml)
        {
            sbHtml.AppendLine(topLevel ? 
                "<div class='tree-view'>" : 
                "<div class='child-nodes hidden'>");

            if (treeNodes != null)
            {
                foreach (object treeNode in treeNodes)
                {
                    if (treeNode is IWebTreeNode webTreeNode && !webTreeNode.IsHidden)
                    {
                        bool childrenExist = webTreeNode.Children.Count > 0;
                        bool urlIsEmpty = string.IsNullOrEmpty(webTreeNode.Url);
                        string nodeCssClass = 
                            (webTreeNode.Represents(SelectedObject) ? " selected" : "") + 
                            (!childrenExist && urlIsEmpty ? " disabled" : "");
                        string dataAttrs = RenderDataAttrs(webTreeNode);
                        string expanderCssClass = childrenExist ? "" : " empty";

                        string iconHtml;
                        string leftExpanderHtml;
                        string rightExpanderHtml;

                        if (options.ExpanderLeft)
                        {
                            leftExpanderHtml = $"<div class='expander left{expanderCssClass}'></div>";
                            rightExpanderHtml = "";
                        }
                        else
                        {
                            leftExpanderHtml = "";
                            rightExpanderHtml = $"<div class='expander right{expanderCssClass}'></div>";
                        }

                        if (options.ShowIcons)
                        {
                            string iconUrl = string.IsNullOrEmpty(webTreeNode.IconUrl) 
                                ? (childrenExist ? options.FolderIconUrl : options.NodeIconUrl) 
                                : webTreeNode.IconUrl;
                            iconHtml = $"<div class='icon'><img src='{urlHelper.Content(iconUrl)}' alt='' /></div>";
                        }
                        else
                        {
                            iconHtml = "";
                        }

                        sbHtml.AppendLine(
                            $"<a class='node{nodeCssClass}' href='{urlHelper.Content(webTreeNode.Url)}' {dataAttrs}>" +
                            $"<div class='node-parts'>" +
                            $"<div class='indent'></div>" +
                            leftExpanderHtml +
                            iconHtml +
                            $"<div class='text'>{HttpUtility.HtmlEncode(webTreeNode.Text)}</div>" +
                            rightExpanderHtml +
                            "</div></a>");


                        if (childrenExist)
                            RenderTreeView(webTreeNode.Children, false, sbHtml);
                    }
                }
            }

            sbHtml.AppendLine("</div>");
        }

        /// <summary>
        /// Renders HTML for the tree view that implement IWebTreeNode.
        /// </summary>
        public HtmlString RenderHtml(IList treeNodes)
        {
            StringBuilder sbHtml = new();
            RenderTreeView(treeNodes, true, sbHtml);
            return new HtmlString(sbHtml.ToString());
        }
    }
}
