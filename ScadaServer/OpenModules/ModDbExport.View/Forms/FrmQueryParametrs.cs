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
            QueryOptions = null;
        }
 
        /// <summary>
        /// Gets or sets the query options.
        /// </summary>
        internal QueryOptions QueryOptions { get; set; }

        /// <summary>
        /// Gets or sets the kind of db.
        /// </summary>
        //internal KnownDBMS DBMS { get; set; }

        private void FrmQueryParametrs_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            
            lvParametrs.Items.Clear();
           
            QueryInfo queryInfo = new(QueryOptions);

            IDictionary<string, string> names = queryInfo.GetSqlParameters();

            foreach (KeyValuePair<string, string> pair in names)
            {
                lvParametrs.Items.Add(pair.Key + pair.Value);
            }
        }
    }
}
