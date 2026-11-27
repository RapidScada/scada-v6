/*
 * Copyright 2026 Rapid Software LLC
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
 * Summary  : Provides static members for a paginated list
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2026
 * Modified : 2026
 */

namespace Scada.Web.Components
{
    /// <summary>
    /// Provides static members for a paginated list.
    /// <para>Предоставляет статические члены для страничного списка.</para>
    /// </summary>
    public static class PaginatedList
    {
        /// <summary>
        /// The default page size.
        /// </summary>
        public const int DefaultPageSize = 10;
        /// <summary>
        /// The maximum allowed number of items per page.
        /// </summary>
        public const int MaxPageSize = 1000;


        /// <summary>
        /// Checks that the page size is within the allowed range.
        /// </summary>
        public static bool IsValidPageSize(int pageSize)
        {
            return 0 < pageSize && pageSize <= MaxPageSize;
        }

        /// <summary>
        /// Returns the page size clamped to the allowed range.
        /// </summary>
        public static int NormalizePageSize(int pageSize)
        {
            return pageSize <= 0 ? DefaultPageSize : Math.Min(pageSize, MaxPageSize);
        }

        /// <summary>
        /// Calculates the number of pages required to display the specified number of items.
        /// </summary>
        public static int GetPageCount(int totalItems, int pageSize)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(totalItems);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);

            return totalItems / pageSize + (totalItems % pageSize > 0 ? 1 : 0);
        }

        /// <summary>
        /// Keeps the index unchanged or adjusts it according to the number of pages.
        /// </summary>
        public static int FixPageIndex(int pageIndex, int pageCount)
        {
            return pageCount > 0 ? Math.Clamp(pageIndex, 0, pageCount - 1) : 0;
        }

        /// <summary>
        /// Creates and populates a paginated list from the list.
        /// </summary>
        public static PaginatedList<T> Create<T>(List<T> source, int pageIndex, int pageSize = DefaultPageSize)
        {
            ArgumentNullException.ThrowIfNull(source);

            PaginatedList<T> list = new(NormalizePageSize(pageSize));
            list.Fill(source, pageIndex);
            return list;
        }

        /// <summary>
        /// Creates and populates a paginated list from the query.
        /// </summary>
        public static PaginatedList<T> Create<T>(IQueryable<T> source, int pageIndex, int pageSize = DefaultPageSize)
        {
            ArgumentNullException.ThrowIfNull(source);

            PaginatedList<T> list = new(NormalizePageSize(pageSize));
            list.Fill(source, pageIndex);
            return list;
        }
    }
}
