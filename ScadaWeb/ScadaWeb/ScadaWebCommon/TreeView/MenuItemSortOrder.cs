/*
 * Copyright 2024 Rapid Software LLC
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
 * Summary  : Specifies the menu item sort orders
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2021
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Web.TreeView
{
    /// <summary>
    /// Specifies the menu item sort orders.
    /// <para>Задает порядок сортировки пунктов меню..</para>
    /// </summary>
    public static class MenuItemSortOrder
    {
        public const int First = 0;
        public const int Last = 1000;
    }
}
