/*
 * Copyright 2023 Rapid Software LLC
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
 * Modified : 2023
 */

using Scada.Data.Entities;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.IO;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents the base class for views.
    /// <para>Представляет базовый класс представлений.</para>
    /// </summary>
    public abstract class ViewBase
    {
        /// <summary>
        /// The view path separator.
        /// </summary>
        protected static readonly char[] PathSeparator = { '\\', '/' };


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ViewBase(View viewEntity)
        {
            ViewEntity = viewEntity ?? throw new ArgumentNullException(nameof(viewEntity));
            ViewStamp = ScadaUtils.GenerateUniqueID();
            StoredOnServer = true;
            Args = ParseArgs();
            Title = GetTitle();
            Resources = new List<ViewResource>();
            CnlNumList = new List<int>();
            CnlNumSet = new HashSet<int>();
        }


        /// <summary>
        /// Gets the view entity stored in the configuration database.
        /// </summary>
        public View ViewEntity { get; }

        /// <summary>
        /// Gets the view stamp to check view integrity during loading from cache.
        /// </summary>
        public long ViewStamp { get; }

        /// <summary>
        /// Gets a value indicating whether to download a view from the server.
        /// </summary>
        public bool StoredOnServer { get; protected set; }

        /// <summary>
        /// Gets the view arguments.
        /// </summary>
        public IDictionary<string, string> Args { get; }

        /// <summary>
        /// Gets the view title.
        /// </summary>
        public string Title { get; protected set; }

        /// <summary>
        /// Gets the view resources.
        /// </summary>
        public List<ViewResource> Resources { get; protected set; }

        /// <summary>
        /// Gets the ordered no-duplicates list of channel numbers included in the view.
        /// </summary>
        public List<int> CnlNumList { get; protected set; }

        /// <summary>
        /// Gets the set of channel numbers included in the view.
        /// </summary>
        public HashSet<int> CnlNumSet { get; protected set; }


        /// <summary>
        /// Parses the view arguments.
        /// </summary>
        protected virtual IDictionary<string, string> ParseArgs()
        {
            return ScadaUtils.ParseArgs(ViewEntity.Args, '&');
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
        /// Adds the channel number to the view.
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
        /// Adds the channel numbers to the view.
        /// </summary>
        protected void AddCnlNums(IEnumerable<int> cnlNums)
        {
            if (cnlNums != null)
            {
                foreach (int cnlNum in cnlNums)
                {
                    AddCnlNum(cnlNum);
                }
            }
        }

        /// <summary>
        /// Adds additional channel numbers for the channels representing arrays.
        /// </summary>
        protected void AddCnlNumsForArrays(BaseTable<Cnl> cnlTable)
        {
            if (cnlTable == null)
                throw new ArgumentNullException(nameof(cnlTable));

            List<int> cnlNumsToAdd = new List<int>();

            foreach (int cnlNum in CnlNumList)
            {
                if (cnlTable.GetItem(cnlNum) is Cnl cnl && cnl.IsArray())
                {
                    for (int i = 1, len = cnl.DataLen.Value; i < len; i++)
                    {
                        cnlNumsToAdd.Add(cnl.CnlNum + i);
                    }
                }
            }

            AddCnlNums(cnlNumsToAdd);
        }

        /// <summary>
        /// Adds the channel to the view.
        /// </summary>
        protected void AddCnl(Cnl cnl)
        {
            if (cnl != null)
            {
                AddCnlNum(cnl.CnlNum);

                for (int i = 1, len = cnl.GetDataLength(); i < len; i++)
                {
                    AddCnlNum(cnl.CnlNum + i);
                }
            }
        }


        /// <summary>
        /// Loads the view from the specified stream.
        /// </summary>
        public virtual void LoadView(Stream stream)
        {
        }

        /// <summary>
        /// Loads the view resource from the specified stream.
        /// </summary>
        public virtual void LoadResource(ViewResource resource, Stream stream)
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
        public virtual void Bind(ConfigDataset configDataset)
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
