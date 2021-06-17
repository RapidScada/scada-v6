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
 * Module   : PlgMain
 * Summary  : Represents a table view specification
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2021
 */

using System;

namespace Scada.Web.Plugins.PlgMain.Code
{
    /// <summary>
    /// Represents a table view specification.
    /// <para>Представляет спецификацию табличного представления.</para>
    /// </summary>
    public class TableViewSpec : ViewSpec
    {
        /// <summary>
        /// Gets the view type code.
        /// </summary>
        public override string TypeCode => "TableView";

        /// <summary>
        /// Gets the extension of view files.
        /// </summary>
        public override string FileExtension => "tbl";

        /// <summary>
        /// Gets the view icon URL.
        /// </summary>
        public override string IconUrl => "~/plugins/Main/images/table-icon.png";

        /// <summary>
        /// Gets the view type.
        /// </summary>
        public override Type ViewType => typeof(TableView);


        /// <summary>
        /// Gets the view frame URL.
        /// </summary>
        public override string GetFrameUrl(int viewID) => "~/Main/TableView/" + viewID;
    }
}
