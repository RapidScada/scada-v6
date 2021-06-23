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
        /// Represents a reference to a view resource.
        /// </summary>
        public class ResourceReference
        {
            /// <summary>
            /// Gets or sets the resource name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the resource path relative to the view directory.
            /// </summary>
            public string Path { get; set; }
        }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BaseView(View viewEntity)
        {
            ViewEntity = viewEntity ?? throw new ArgumentNullException(nameof(viewEntity));
            StoredOnServer = true;
            Args = new SortedList<string, string>();
            ResourceReferences = null;
            CnlNumList = new List<int>();
            CnlNumSet = new HashSet<int>();
            OutCnlNumList = new List<int>();
            OutCnlNumSet = new HashSet<int>();

            ParseArgs(ViewEntity.Args);
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
        public SortedList<string, string> Args { get; }

        /// <summary>
        /// Gets the references to view resources.
        /// </summary>
        public List<ResourceReference> ResourceReferences { get; protected set; }

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
        protected virtual void ParseArgs(string args)
        {
            if (!string.IsNullOrEmpty(args))
            {
                string[] parts = args.Split('&');

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

                    Args[key] = val;
                }
            }
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
        public virtual void LoadResource(ResourceReference reference, Stream stream)
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
    }
}
