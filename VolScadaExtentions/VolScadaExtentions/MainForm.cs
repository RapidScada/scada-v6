using Scada.Data.Adapters;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Lang;
using System.Windows.Forms;
using System.Linq;
using Scada.Admin.App.Forms.Tables;
using Scada;
using Scada.Protocol;
using System.Text;

namespace VolScadaExtentions
{
    public partial class MainForm : Form
    {
        BaseTable<User> baseTable;
        private string baseDir = Path.Combine("C:\\SCADAV6_Vol\\BaseDAT", "user.dat");
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            dataGridView.AutoGenerateColumns = false;
            dataGridView.AllowUserToAddRows = false;
            this.txtDatPath.Text = baseDir;
            this.lbltips.Text = "提示：将设置的明文密码放到ScadaWeb或ScadaComm的Connection Options的Password输入框中即可";
        }

        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    txtDatPath.Text = Path.Combine(folderBrowserDialog.SelectedPath, "user.dat");
                }
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            var fileName = this.txtDatPath.Text.Trim();
            if (string.IsNullOrEmpty(fileName)) { MessageBox.Show("请选择dat路径"); return; }
            if (!File.Exists(fileName)) { MessageBox.Show("dat文件不存在，请重新选择路径"); return; }
            baseTable = new BaseTable<User>("UserID", CommonPhrases.UserTable);
            BaseTableAdapter adapter = new BaseTableAdapter
            {
                FileName = fileName
            };
            adapter.Fill(baseTable);
            var sysaccount = baseTable.EnumerateItems().Cast<User>().Where(x => x.UserID < 10).ToList();
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = sysaccount;
            dataGridView.DataSource = bindingSource;
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowInd = e.RowIndex;
            if (e.ColumnIndex == 3)// "修改"按钮 
            {
                DataGridViewRow row = dataGridView.Rows[rowInd];
                var userId = (int)row.Cells[baseTable.PrimaryKey].Value;
                FrmPassword frmPassword = new()
                {
                    UserID = (int)row.Cells[baseTable.PrimaryKey].Value
                };

                if (frmPassword.ShowDialog() == DialogResult.OK)
                {
                    var newPwd = frmPassword.PasswordHash;
                    var fileName = this.txtDatPath.Text.Trim();

                    User user = baseTable.GetItem(userId);
                    user.Password = newPwd;

                    BaseTableAdapter adapter = new BaseTableAdapter
                    {
                        FileName = fileName
                    };
                    adapter.Update(baseTable);
                    row.Cells["Password"].Value = newPwd;
                    MessageBox.Show("修改成功");
                }
            }
        }

        private void btnAgentPwd_Click(object sender, EventArgs e)
        {
            FrmAgentPassword frmPassword = new();

            if (frmPassword.ShowDialog() == DialogResult.OK)
            {
                var newPwd = frmPassword.PasswordEncrypt ;
                Clipboard.SetText(newPwd);
                MessageBox.Show("密码已复制到剪贴板，粘贴到ScadaAgentConfig.xml中的Password！");
                var fileName = this.txtDatPath.Text.Trim();

            }
        }
    }
}
