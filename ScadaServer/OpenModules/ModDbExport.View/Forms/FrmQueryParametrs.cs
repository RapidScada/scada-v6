// Copyright (c) Rapid Software LLC. All rights reserved.

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


        private void FrmQueryParametrs_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            
            lvParametrs.BeginUpdate();
            lvParametrs.Items.Clear();
           
            QueryInfo queryInfo = new(QueryOptions, DBMS);
            IEnumerable<QueryParam> queryParamList = queryInfo.GetSqlParameters();

            foreach (QueryParam queryParam in queryParamList)
            {
                lvParametrs.Items.Add(new ListViewItem(new string[] { queryParam.Name, queryParam.Descr }));
            }

            lvParametrs.EndUpdate();
        }
    }
}
