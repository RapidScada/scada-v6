// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvGoogleBigQueue.View
{
    /// <summary>
    /// Contains name and description of a query parameter.
    /// <para>Содержит наименование и описание параметра запроса.</para>
    /// </summary>
    internal class QueryParam
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public QueryParam(string name, string descr)
        {
            Name = name;
            Descr = descr;
        }


        /// <summary>
        /// Gets or sets the parameter name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the parameter description.
        /// </summary>
        public string Descr { get; }
    }
}
