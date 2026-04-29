/*
 * Copyright 2025 Rapid Software LLC
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
 * Summary  : Represents a one page of the list
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2025
 */

namespace Scada.Web.Components
{
    /// <summary>
    /// Represents a one page of a list.
    /// <para>Представляет одну страницу списка.</para>
    /// </summary>
    public class PaginatedList<T> : List<T>
    {
        /// <summary>
        /// The default page size.
        /// </summary>
        public const int DefaultPageSize = 10;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PaginatedList()
            : this(DefaultPageSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PaginatedList(int pageSize)
            : base(pageSize)
        {
            if (pageSize <= 0)
                throw new ArgumentException("Page size must be positive.", nameof(pageSize));

            PageSize = pageSize;
            PageIndex = 0;
            PageCount = 0;
            TotalItems = 0;
        }


        /// <summary>
        /// Gets the page size.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Gets the index of the current page.
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        /// Gets the total number of pages.
        /// </summary>
        public int PageCount { get; private set; }

        /// <summary>
        /// Gets the total number of items.
        /// </summary>
        public int TotalItems { get; private set; }


        /// <summary>
        /// Calculates the page count.
        /// </summary>
        private int CalculatePageCount(int itemCount)
        {
            return (int)Math.Ceiling(itemCount / (double)PageSize);
        }

        /// <summary>
        /// Keeps the index unchanged or adjusts it according to the number of pages.
        /// </summary>
        private static int FixPageIndex(int pageIndex, int pageCount)
        {
            return pageCount > 0 ? Math.Clamp(pageIndex, 0, pageCount - 1) : 0;
        }

        /// <summary>
        /// Populates the list with the items of the specified page.
        /// </summary>
        public void Fill(List<T> source, int pageIndex)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));
            TotalItems = source.Count;
            PageCount = CalculatePageCount(TotalItems);
            PageIndex = FixPageIndex(pageIndex, PageCount);

            int startIndex = PageIndex * PageSize;
            int endIndex = Math.Min(startIndex + PageSize, TotalItems);

            for (int i = startIndex; i < endIndex; i++)
            {
                Add(source[i]);
            }
        }

        /// <summary>
        /// Populates the list with the items of the specified page.
        /// </summary>
        public void Fill(IQueryable<T> source, int pageIndex)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));
            TotalItems = source.Count();
            PageCount = CalculatePageCount(TotalItems);
            PageIndex = FixPageIndex(pageIndex, PageCount);
            AddRange(source
                .Skip(PageIndex * PageSize)
                .Take(PageSize));
        }
    }
}
