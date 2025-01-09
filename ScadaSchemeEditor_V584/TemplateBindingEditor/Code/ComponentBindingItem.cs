﻿/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : Template Binding Editor
 * Summary  : Represents an editable item of component bindings
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Scheme.Template;

namespace Scada.Scheme.TemplateBindingEditor.Code
{
    /// <summary>
    /// Represents an editable item of component bindings.
    /// <para>Представляет редактируемый элемент привязки компонента.</para>
    /// </summary>
    internal class ComponentBindingItem : ComponentBinding
    {
        /// <summary>
        /// Gets or sets the display name of the compoment.
        /// </summary>
        public string CompDisplayName { get; set; }
    }
}
