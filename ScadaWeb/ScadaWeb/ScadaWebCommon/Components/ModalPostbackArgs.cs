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
 * Summary  : Describes modal dialog behavior after postback
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

namespace Scada.Web.Components
{
    /// <summary>
    /// Describes modal dialog behavior after postback.
    /// <para>Описывает поведение модального диалога после передачи.</para>
    /// </summary>
    public class ModalPostbackArgs
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModalPostbackArgs()
        {
            CloseModal = false;
            CloseDelay = 0;
            ModalResult = null;
            UpdateHeight = false;
            GrowOnly = false;
        }


        /// <summary>
        /// Gets or sets a value indicating whether a modal dialog should be closed.
        /// </summary>
        public bool CloseModal { get; set; }

        /// <summary>
        /// Gets or sets the delay before closing, seconds.
        /// </summary>
        public int CloseDelay { get; set; }

        /// <summary>
        /// Gets or sets the result of the modal dialog.
        /// </summary>
        public object ModalResult { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to update the height of the modal dialog.
        /// </summary>
        public bool UpdateHeight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the height of the modal dialog can only be increased.
        /// </summary>
        public bool GrowOnly { get; set; }
    }
}
