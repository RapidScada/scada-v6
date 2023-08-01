using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Admin.Extensions.ExtImport.Code;
using Scada.Comm.Devices;
using System.Xml.Linq;
using Scada.Forms;


namespace Scada.Admin.Extensions.ExtImport.Forms
{
    public partial class FrmCnlsMerge : Form
    {

        private Dictionary<string, List<string>> dictio;
        private IAdminContext adminContext; // the Administrator context
        private ScadaProject project;       // the project under development
        private Controls.CtrlImport3 CtrlImport3;
        private Controls.CtrlImport2 CtrlImport2;
        private Controls.CtrlImport1 CtrlImport1;
        private CheckBox _headerCheckBox1 = new CheckBox();
        private CheckBox _headerCheckBox2 = new CheckBox();

        private readonly Dictionary<int, string> cnlTypeDictionary = ConfigDictionaries.CnlTypeDictionary;
        private readonly Dictionary<int, string> dataTypeDictionary = ConfigDictionaries.DataTypeDictionary;


		private FrmCnlsMerge()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
        }

        public void Init(IAdminContext adminContext, ScadaProject project)
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.project = project ?? throw new ArgumentNullException(nameof(project));

        }

        public FrmCnlsMerge(ScadaProject project, Controls.CtrlImport1 ctrlImport1, Controls.CtrlImport2 ctrlImport2, Controls.CtrlImport3 ctrlImport3) : this()
        {
            this.project = project;
            this.CtrlImport1 = ctrlImport1;
            this.CtrlImport2 = ctrlImport2;
            this.CtrlImport3 = ctrlImport3;
            setDictio(ctrlImport3._dictio);
                
            gridViewFiller();
          
        }

        public void setDictio(Dictionary<string, List<string>> dictio)
        {
            this.dictio = dictio;
        }
        public void gridViewFiller()
        {
            dataGridView1.Rows.Clear();

			List<Cnl>  channelPrototypes = CreateChannels();
            
            foreach (var prototype in channelPrototypes)
            {
                int rowIndex = dataGridView1.Rows.Add(); 
                DataGridViewRow row = dataGridView1.Rows[rowIndex];

                
                var cnlNum = prototype.CnlNum;
                var projectItem = project.ConfigDatabase.CnlTable.GetItem(cnlNum);

                string projectCnlName = projectItem != null ? projectItem.Name : "";
                string projectCnlType = projectItem != null ? cnlTypeDictionary[projectItem.CnlTypeID] : "";
                string projectDataType = (projectItem != null && projectItem.DataTypeID.HasValue) ? dataTypeDictionary[projectItem.DataTypeID.Value] : "";
                string projectTagCode = projectItem != null ? projectItem.TagCode : "";
                string prototypeCnlType = cnlTypeDictionary[prototype.CnlTypeID];
                string prototypeDataType = prototype.DataTypeID.HasValue ? dataTypeDictionary[prototype.DataTypeID.Value] : "";

                row.Cells[0].Value = prototype.CnlNum;  
                row.Cells[1].Value = false;             
                row.Cells[2].Value = prototype.Name;    
                row.Cells[3].Value = prototypeDataType; 
                row.Cells[4].Value = prototypeCnlType;  
                row.Cells[5].Value = prototype.TagCode; 
                row.Cells[6].Value = "";                
                row.Cells[7].Value = false;             
                row.Cells[8].Value = projectCnlName;    
                row.Cells[9].Value = projectDataType;  
                row.Cells[10].Value = projectCnlType;   
                row.Cells[11].Value = projectTagCode;

                row.Cells[6].Value = prototype;
            }

            dataGridView1.Columns[6].Visible = false;
            
        }

        
        private bool AddSelectedChannels()
        {
            List<Cnl> selectedChannels = new List<Cnl>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells[1].Value) == true)
                {
                    Cnl cnl = (Cnl)row.Cells[6].Value;

                    selectedChannels.Add(cnl);
                }
            }
            if(selectedChannels.Count > 0)
            {
                AddChannels(selectedChannels);
                return true;
            }
            else
            {
                ScadaUiUtils.ShowWarning(ExtensionPhrases.SelectWarning);
				return false;
			}
            
        }


        /// <summary>
        /// Creates channels based on the channel prototypes.
        /// </summary>
        private List<Cnl> CreateChannels()
        {
            List<Cnl> cnls = new();
            int cnlNum = CtrlImport3.StartCnlNum;
            string name, separator, prefix, suffix;
            CtrlImport3.CnlNameFormat.TryGetValue("separator", out separator);
            CtrlImport3.CnlNameFormat.TryGetValue("prefix", out prefix);
            CtrlImport3.CnlNameFormat.TryGetValue("suffix", out suffix);

            int? objNum = CtrlImport2.ObjNum;
            int deviceNum = CtrlImport1.SelectedDevice.DeviceNum;

            foreach (CnlPrototype cnlPrototype in CtrlImport1.CnlPrototypes)
            {
                if (!string.IsNullOrWhiteSpace(prefix) || !string.IsNullOrWhiteSpace(suffix))
                {
                    name = prefix switch
                    {
                        "DeviceName" => CtrlImport3.DeviceName,
                        "TagCode" => cnlPrototype.TagCode,
                        "TagNumber" => cnlPrototype.TagNum.ToString(),
                        "Type" => cnlPrototype.CnlTypeID.ToString(),
                        _ => prefix
                    };
                    name += separator;
                    name += suffix switch
                    {
                        "DeviceName" => CtrlImport3.DeviceName,
                        "TagCode" => cnlPrototype.TagCode,
                        "TagNumber" => cnlPrototype.TagNum.ToString(),
                        "Type" => cnlPrototype.CnlTypeID.ToString(),
                        _ => ""
                    };
                }
                else
                {
                    name = CtrlImport3.DeviceName + "-" + cnlPrototype.TagCode;
                }

                cnls.Add(new Cnl
                {
                    CnlNum = cnlNum,
                    Active = cnlPrototype.Active,
                    Name = name,
                    DataTypeID = cnlPrototype.DataTypeID,
                    DataLen = cnlPrototype.DataLen,
                    CnlTypeID = cnlPrototype.CnlTypeID,
                    ObjNum = objNum,
                    DeviceNum = deviceNum,
                    TagNum = cnlPrototype.TagNum,
                    TagCode = cnlPrototype.TagCode,
                    FormulaEnabled = cnlPrototype.FormulaEnabled,
                    InFormula = cnlPrototype.InFormula,
                    OutFormula = cnlPrototype.OutFormula,
                    FormatID = project.ConfigDatabase.GetFormatByCode(cnlPrototype.FormatCode)?.FormatID,
                    QuantityID = project.ConfigDatabase.GetQuantityByCode(cnlPrototype.QuantityCode)?.QuantityID,
                    UnitID = project.ConfigDatabase.GetUnitByCode(cnlPrototype.UnitCode)?.UnitID,
                    LimID = null,
                    ArchiveMask = cnlPrototype.ArchiveMask,
                    EventMask = cnlPrototype.EventMask
                });

                int dataLength = cnlPrototype.GetDataLength();
                if (cnlNum > ConfigDatabase.MaxID - dataLength)
                    break;
                cnlNum += dataLength;
                name = "";
            }

            return cnls;
        }

		/// <summary>
		/// Add cnls 
		/// </summary>
		/// <param name="cnls"></param>
		private void AddChannels(List<Cnl> cnls)
		{
			if (cnls == null || cnls.Count <= 0)
			{
				return;
			}

			cnls.ForEach(cnl => project.ConfigDatabase.CnlTable.AddItem(cnl));
			project.ConfigDatabase.CnlTable.Modified = true;
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
            {
                DataGridViewCheckBoxCell currentCheckbox = (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                DataGridViewCheckBoxCell otherCheckbox = (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 1 ? 7 : 1];

                if ((bool)currentCheckbox.Value == true && (bool)otherCheckbox.Value == false)
                {
                    currentCheckbox.Value = false;

                    otherCheckbox.Style.BackColor = Color.White;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Style.BackColor = Color.White;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 2].Style.BackColor = Color.White;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 3].Style.BackColor = Color.White;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 4].Style.BackColor = Color.White;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 5].Style.BackColor = Color.White;
                    currentCheckbox.Style.BackColor = Color.White;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 1 ? 8 : 2].Style.BackColor = Color.White;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 1 ? 9 : 3].Style.BackColor = Color.White;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 1 ? 10 : 4].Style.BackColor = Color.White;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 1 ? 11 : 5].Style.BackColor = Color.White;
                }
                else
                {
                    if ((bool)currentCheckbox.Value == true)
                    {
                        otherCheckbox.Value = true;
                        currentCheckbox.Value = false;

                        otherCheckbox.Style.BackColor = Color.LightGreen;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Style.BackColor = Color.LightGreen;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 2].Style.BackColor = Color.LightGreen;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 3].Style.BackColor = Color.LightGreen;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 4].Style.BackColor = Color.LightGreen;

                        currentCheckbox.Style.BackColor = Color.PaleVioletRed;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 0 ? 8 : 2].Style.BackColor = Color.PaleVioletRed;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 0 ? 9 : 3].Style.BackColor = Color.PaleVioletRed;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 0 ? 10 : 4].Style.BackColor = Color.PaleVioletRed;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 0 ? 11 : 5].Style.BackColor = Color.PaleVioletRed;
                    }
                    else
                    {
                        otherCheckbox.Value = false;
                        currentCheckbox.Value = true;

                        otherCheckbox.Style.BackColor = Color.PaleVioletRed;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 1 ? 8 : 2].Style.BackColor = Color.PaleVioletRed;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 1 ? 9 : 3].Style.BackColor = Color.PaleVioletRed;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 1 ? 10 : 4].Style.BackColor = Color.PaleVioletRed;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 1 ? 11 : 5].Style.BackColor = Color.PaleVioletRed;

                        currentCheckbox.Style.BackColor = Color.LightGreen;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Style.BackColor = Color.LightGreen;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 2].Style.BackColor = Color.LightGreen;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 3].Style.BackColor = Color.LightGreen;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 4].Style.BackColor = Color.LightGreen;

                    }
                }

            }
        }


        private void _headerCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (_headerCheckBox1.Checked)
            {
                _headerCheckBox2.Checked = false;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.Cells[1].ReadOnly)
                    {
                        row.Cells[1].Value = true;

                        row.Cells[1].Style.BackColor = Color.LightGreen;
                        row.Cells[2].Style.BackColor = Color.LightGreen;
                        row.Cells[3].Style.BackColor = Color.LightGreen;
                        row.Cells[4].Style.BackColor = Color.LightGreen;
                        row.Cells[5].Style.BackColor = Color.LightGreen;

                        row.Cells[7].Style.BackColor = Color.PaleVioletRed;
                        row.Cells[8].Style.BackColor = Color.PaleVioletRed;
                        row.Cells[9].Style.BackColor = Color.PaleVioletRed;
                        row.Cells[10].Style.BackColor = Color.PaleVioletRed;
                        row.Cells[11].Style.BackColor = Color.PaleVioletRed;
                    }
                }
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.Cells[1].ReadOnly)
                    {
                        row.Cells[1].Value = false;

                        row.Cells[1].Style.BackColor = Color.White;
                        row.Cells[2].Style.BackColor = Color.White;
                        row.Cells[3].Style.BackColor = Color.White;
                        row.Cells[4].Style.BackColor = Color.White;
                        row.Cells[5].Style.BackColor = Color.White;

                        row.Cells[7].Style.BackColor = Color.White;
                        row.Cells[8].Style.BackColor = Color.White;
                        row.Cells[9].Style.BackColor = Color.White;
                        row.Cells[10].Style.BackColor = Color.White;
                        row.Cells[11].Style.BackColor = Color.White;
                    }
                }
            }
        }

        private void _headerCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (_headerCheckBox2.Checked)
            {
                _headerCheckBox1.Checked = false;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.Cells[7].ReadOnly)
                    {
                        row.Cells[7].Value = true;

                        row.Cells[1].Style.BackColor = Color.PaleVioletRed;
                        row.Cells[2].Style.BackColor = Color.PaleVioletRed;
                        row.Cells[3].Style.BackColor = Color.PaleVioletRed;
                        row.Cells[4].Style.BackColor = Color.PaleVioletRed;
                        row.Cells[5].Style.BackColor = Color.PaleVioletRed;

                        row.Cells[7].Style.BackColor = Color.LightGreen;
                        row.Cells[8].Style.BackColor = Color.LightGreen;
                        row.Cells[9].Style.BackColor = Color.LightGreen;
                        row.Cells[10].Style.BackColor = Color.LightGreen;
                        row.Cells[11].Style.BackColor = Color.LightGreen;
                    }
                }
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.Cells[7].ReadOnly)
                    {
                        row.Cells[7].Value = false;

                        row.Cells[1].Style.BackColor = Color.White;
                        row.Cells[2].Style.BackColor = Color.White;
                        row.Cells[3].Style.BackColor = Color.White;
                        row.Cells[4].Style.BackColor = Color.White;
                        row.Cells[5].Style.BackColor = Color.White;

                        row.Cells[7].Style.BackColor = Color.White;
                        row.Cells[8].Style.BackColor = Color.White;
                        row.Cells[9].Style.BackColor = Color.White;
                        row.Cells[10].Style.BackColor = Color.White;
                        row.Cells[11].Style.BackColor = Color.White;
                    }
                }
            }
        }

        private void FrmCnlMerge_Load(object sender, EventArgs e)
        {
            SetCheckboxLocation(_headerCheckBox1, 1);
            SetCheckboxLocation(_headerCheckBox2, 7);

            SetLabelLocation(lblSource, -1, 5);
            SetLabelLocation(lblDestination, 6, 9);

            dataGridView1.Controls.Add(_headerCheckBox1);
            _headerCheckBox1.CheckedChanged += _headerCheckBox1_CheckedChanged;
            dataGridView1.Controls.Add(_headerCheckBox2);
            _headerCheckBox2.CheckedChanged += _headerCheckBox2_CheckedChanged;
            dataGridView1.ColumnWidthChanged += DataGridView1_ColumnWidthChanged;


        }

        private void DataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            SetCheckboxLocation(_headerCheckBox1, 1);
            SetCheckboxLocation(_headerCheckBox2, 7);

            SetLabelLocation(lblSource, -1, 5);
            SetLabelLocation(lblDestination, 6, 9);
        }

        private void SetCheckboxLocation(System.Windows.Forms.CheckBox ck, int columnIndex)
        {
            Rectangle headerCellRectangle = this.dataGridView1.GetCellDisplayRectangle(columnIndex, -1, true);

            ck.Location = new Point(headerCellRectangle.X + (headerCellRectangle.Width / 2) - 8, headerCellRectangle.Y + 2);
            ck.BackColor = Color.Transparent;
            ck.Size = new Size(18, 18);
        }

        private void SetLabelLocation(System.Windows.Forms.Label lbl, int columnStartIndex, int columnEndIndex)
        {
            Rectangle headerCell1Rectangle = this.dataGridView1.GetCellDisplayRectangle(columnStartIndex, -1, true);
            Rectangle headerCell2Rectangle = this.dataGridView1.GetCellDisplayRectangle(columnEndIndex, -1, true);

            lbl.Location = new Point(headerCell1Rectangle.X + dataGridView1.Location.X, lbl.Location.Y);
            lbl.Size = new Size((headerCell2Rectangle.X + dataGridView1.Location.X + headerCell2Rectangle.Width) - headerCell1Rectangle.X, 21);
        }

        /// <summary>
        /// Create or update cnl from file
        /// </summary>
        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            if(AddSelectedChannels())
                DialogResult = DialogResult.OK;
        }

    }
}
