using Scada.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Comm.Drivers.DrvDsScadaServer.View.Forms
{
    public partial class FrmScadaServerDSO : Form
    {
        public FrmScadaServerDSO()
        {
            InitializeComponent();
        }

        private void FrmScadaServerDSO_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
        }
    }
}
