// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Dbms;
using Scada.Forms;

namespace Scada.Comm.Drivers.DrvGoogleBigQueue.View
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
        }


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

            QueryInfo queryInfo = new();
            var queryParams = queryInfo.GetAllParams();
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
