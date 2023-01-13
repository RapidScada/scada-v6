// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Dbms;
using Scada.Forms;
using Scada.Server.Modules.ModDbExport.Config;

namespace Scada.Server.Modules.ModDbExport.View.Forms
{
    /// <summary>
    /// Represents a form for show a query parameters.
    /// <para>Представляет форму для просмотра параметров запроса.</para>
    /// </summary>
    public partial class FrmQueryParametrs : Form
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmQueryParametrs()
        {
            InitializeComponent();

            SetColumnNames();
            QueryOptions = null;
            DBMS = KnownDBMS.Undefined;
        }


        /// <summary>
        /// Gets or sets the query options.
        /// </summary>
        internal QueryOptions QueryOptions { get; set; }

        /// <summary>
        /// Gets or sets the database to generate query parameters.
        /// </summary>
        internal KnownDBMS DBMS { get; set; }


        /// <summary>
        /// Sets the column names for the translation.
        /// </summary>
        private void SetColumnNames()
        {
            colParamsName.Name = "colParamsName";
            colParams.Name = "colParams";
        }

        /// <summary>
        /// Fills the list view with query parameters.
        /// </summary>
        private void FillQueryParams()
        {
            lvParameters.BeginUpdate();
            lvParameters.Items.Clear();

            QueryInfo queryInfo = new(QueryOptions, DBMS);
            IEnumerable<QueryParam> queryParams = queryInfo.GetSqlParameters();

            foreach (QueryParam queryParam in queryParams)
            {
                lvParameters.Items.Add(new ListViewItem(new string[] { queryParam.Name, queryParam.Descr }));
            }

            lvParameters.EndUpdate();
        }


        private void FrmQueryParametrs_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);

            FillQueryParams();
        }
    }
}
