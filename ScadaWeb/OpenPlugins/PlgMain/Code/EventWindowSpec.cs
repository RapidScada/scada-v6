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
 * Summary  : Represents an event window specification
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2021
 */

namespace Scada.Web.Plugins.PlgMain.Code
{
    /// <summary>
    /// Represents an event window specification.
    /// <para>Представляет спецификацию окна событий.</para>
    /// </summary>
    public class EventWindowSpec : DataWindowSpec
    {
        /// <summary>
        /// Gets the window title.
        /// </summary>
        public override string Title => "Events";

        /// <summary>
        /// Gets the URL of the window frame.
        /// </summary>
        public override string Url => "~/Main/Events";
    }
}
