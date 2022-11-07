// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Doc.Code
{
    /// <summary>
    /// Provides tables of contents.
    /// <para>Предоставляет оглавления.</para>
    /// </summary>
    public class TocManager
    {
        private readonly Dictionary<string, Toc> tocs; // the tables of contents accessed by short file name


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TocManager()
        {
            tocs = new Dictionary<string, Toc>();
        }


        /// <summary>
        /// Gets the table of contents corresponding to the web page.
        /// </summary>
        public Toc GetToc(string pagePath)
        {
            return null;
        }
    }
}
