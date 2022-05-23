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
 * Module   : Webstation Application
 * Summary  : Represents a cached configuration database for the web application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

using Scada.Data.Entities;
using Scada.Data.Models;
using System.Collections.Generic;

namespace Scada.Web
{
    /// <summary>
    /// Represents a cached configuration database for the web application.
    /// <para>Представляет кэшированную базу конфигурации для веб-приложения.</para>
    /// </summary>
    public class ConfigDatabase : ConfigDataset
    {
        /// <summary>
        /// Defines a method that compares two views by ordinal number and ID.
        /// </summary>
        private class ViewComparer : IComparer<View>
        {
            public int Compare(View x, View y)
            {
                int compareResult = (x.Ord ?? 0).CompareTo(y.Ord ?? 0);
                return compareResult == 0 ? x.ViewID.CompareTo(y.ViewID) : compareResult;
            }
        }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConfigDatabase()
            : base()
        {
            RightMatrix = new RightMatrix();
            Enums = new EnumDict();
            SortedViews = new List<View>();
        }


        /// <summary>
        /// Gets the access rights.
        /// </summary>
        public RightMatrix RightMatrix { get; }

        /// <summary>
        /// Gets the enumerations.
        /// </summary>
        public EnumDict Enums { get; }

        /// <summary>
        /// Gets the sorted view entities.
        /// </summary>
        public List<View> SortedViews { get; }


        /// <summary>
        /// Initializes data objects based on the configuration database tables.
        /// </summary>
        public void Init()
        {
            RightMatrix.Init(this);
            Enums.Init(this);

            SortedViews.AddRange(ViewTable.Enumerate());
            SortedViews.Sort(new ViewComparer());
        }
    }
}
