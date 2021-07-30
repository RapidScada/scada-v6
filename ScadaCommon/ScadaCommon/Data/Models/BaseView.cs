/*
 * Copyright 2021 Rapid Software LLC
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
 * Module   : ScadaCommon
 * Summary  : Represents the base class for views
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2011
 * Modified : 2021
 */

using Scada.Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents the base class for views.
    /// <para>Представляет базовый класс представлений.</para>
    /// </summary>
    public abstract class BaseView
    {
        /// <summary>
        /// The view path separator.
        /// </summary>
        protected static readonly char[] PathSeparator = { '\\', '/' };


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BaseView(View viewEntity)
        {
            ViewEntity = viewEntity ?? throw new ArgumentNullException(nameof(viewEntity));
            StoredOnServer = true;
            Args = ParseArgs();
            Title = GetTitle();
            Resources = null;
            CnlNumList = new List<int>();
            CnlNumSet = new HashSet<int>();
            OutCnlNumList = new List<int>();
            OutCnlNumSet = new HashSet<int>();
        }


        /// <summary>
        /// Gets the view entity stored in the configuration database.
        /// </summary>
        public View ViewEntity { get; }

        /// <summary>
        /// Gets a value indicating whether to download a view from the server.
        /// </summary>
        public bool StoredOnServer { get; }

        /// <summary>
        /// Gets the view arguments.
        /// </summary>
        public Dictionary<string, string> Args { get; }

        /// <summary>
        /// Gets the view title.
        /// </summary>
        public string Title { get; protected set; }

        /// <summary>
        /// Gets the view resources. Key is a resource name, value is a path relative to the view directory.
        /// </summary>
        public Dictionary<string, string> Resources { get; protected set; }

        /// <summary>
        /// Gets the ordered no-duplicates list of input channel numbers included in the view.
        /// </summary>
        public List<int> CnlNumList { get; protected set; }

        /// <summary>
        /// Gets the set of input channel numbers included in the view.
        /// </summary>
        public HashSet<int> CnlNumSet { get; protected set; }

        /// <summary>
        /// Gets the ordered no-duplicates list of output channel numbers included in the view.
        /// </summary>
        public List<int> OutCnlNumList { get; protected set; }

        /// <summary>
        /// Gets the set of output channel numbers included in the view.
        /// </summary>
        public HashSet<int> OutCnlNumSet { get; protected set; }


        /// <summary>
        /// Parses the view arguments.
        /// </summary>
        protected virtual Dictionary<string, string> ParseArgs()
        {
            Dictionary<string, string> args = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(ViewEntity.Args))
            {
                string[] parts = ViewEntity.Args.Split('&');

                foreach (string part in parts)
                {
                    string key;
                    string val;
                    int idx = part.IndexOf("=");

                    if (idx >= 0)
                    {
                        key = part.Substring(0, idx).Trim();
                        val = part.Substring(idx + 1).Trim();
                    }
                    else
                    {
                        key = part.Trim();
                        val = "";
                    }

                    args[key] = val;
                }
            }

            return args;
        }

        /// <summary>
        /// Gets the view title.
        /// </summary>
        protected virtual string GetTitle()
        {
            ParsePath(ViewEntity, out _, out string viewTitle);
            return viewTitle;
        }

        /// <summary>
        /// Adds the input channel number to the list and set.
        /// </summary>
        protected void AddCnlNum(int cnlNum)
        {
            if (cnlNum > 0 && CnlNumSet.Add(cnlNum))
            {
                int index = CnlNumList.BinarySearch(cnlNum);
                if (index < 0)
                    CnlNumList.Insert(~index, cnlNum);
            }
        }

        /// <summary>
        /// Adds the output channel number to the list and set.
        /// </summary>
        protected void AddOutCnlNum(int outCnlNum)
        {
            if (outCnlNum > 0 && OutCnlNumSet.Add(outCnlNum))
            {
                int index = OutCnlNumList.BinarySearch(outCnlNum);
                if (index < 0)
                    OutCnlNumList.Insert(~index, outCnlNum);
            }
        }


        /// <summary>
        /// Prepares the view before loading.
        /// </summary>
        public virtual void Prepare()
        {
        }

        /// <summary>
        /// Loads the view from the specified stream.
        /// </summary>
        public virtual void LoadView(Stream stream)
        {
        }

        /// <summary>
        /// Loads the view resource specified in the reference.
        /// </summary>
        public virtual void LoadResource(string resourceName, Stream stream)
        {
        }

        /// <summary>
        /// Builds the view after loading the view itself and all required resources.
        /// </summary>
        public virtual void Build()
        {
        }

        /// <summary>
        /// Binds the view to the configuration database.
        /// </summary>
        public virtual void Bind(BaseDataSet baseDataSet)
        {
        }

        /// <summary>
        /// Parses the view path and title.
        /// </summary>
        public static void ParsePath(View viewEntity, out string[] pathParts, out string viewTitle)
        {
            if (viewEntity == null)
                throw new ArgumentNullException(nameof(viewEntity));

            string GetLastPart(string[] parts)
            {
                return parts.Length > 0 ? parts[parts.Length - 1] : "";
            }

            if (!string.IsNullOrEmpty(viewEntity.Title) && viewEntity.Title.IndexOfAny(PathSeparator) >= 0)
            {
                pathParts = viewEntity.Title.Split(PathSeparator, StringSplitOptions.RemoveEmptyEntries);
                viewTitle = GetLastPart(pathParts);
            }
            else if (!string.IsNullOrEmpty(viewEntity.Path))
            {
                pathParts = viewEntity.Path.Split(PathSeparator, StringSplitOptions.RemoveEmptyEntries);
                viewTitle = ScadaUtils.FirstNonEmpty(viewEntity.Title, GetLastPart(pathParts)) ?? "";
            }
            else if (!string.IsNullOrEmpty(viewEntity.Title))
            {
                pathParts = new string[] { viewEntity.Title };
                viewTitle = viewEntity.Title;
            }
            else
            {
                pathParts = Array.Empty<string>();
                viewTitle = "";
            }
        }
    }
}
