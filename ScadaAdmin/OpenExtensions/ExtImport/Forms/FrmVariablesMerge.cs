using Scada.Admin.Project;
using Scada.Data.Entities;
using System.Xml;
using Scada.Comm.Drivers.DrvModbus.Config;
using Scada.Comm.Drivers.DrvModbus.Protocol;
using System.Text.RegularExpressions;
using Scada.Admin.Extensions.ExtImport.Code;
using Scada.Comm.Devices;
using Scada.Data.Const;
using System.Xml.Linq;
using System;
using Scada.Forms;

namespace Scada.Admin.Extensions.ExtImport.Forms
{
    public partial class FrmVariablesMerge : Form
    {

        private Dictionary<string, List<string>> dictio;
        private Dictionary<string, Dictionary<string, string>> _oldDictio = new Dictionary<string, Dictionary<string, string>>();
        private Dictionary<string, Dictionary<string, string>> _newDictio = new Dictionary<string, Dictionary<string, string>>();

        private List<string> _listOfKey = new List<string>();
        private IAdminContext adminContext; // the Administrator context
        private ScadaProject project;       // the project under development

        string selectedDeviceName;
        Dictionary<string, ElemType> elemTypeDico;
        private CheckBox _headerCheckBox1 = new CheckBox();
        private CheckBox _headerCheckBox2 = new CheckBox();
        private Controls.CtrlImport3 CtrlImport3;
        private Controls.CtrlImport2 CtrlImport2;
        private Controls.CtrlImport1 CtrlImport1;


        private Dictionary<string, int> cnlDataType = ConfigDictionaries.CnlDataType;

        private FrmVariablesMerge()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            this.elemTypeDico = ConfigDictionaries.ElemTypeDictionary;
        }
       
        public void Init(IAdminContext adminContext, ScadaProject project)
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.project = project ?? throw new ArgumentNullException(nameof(project));

        }

      
        public FrmVariablesMerge(ScadaProject project, Controls.CtrlImport1 ctrlImport1, Controls.CtrlImport2 ctrlImport2, Controls.CtrlImport3 ctrlImport3) : this()
        {

            this.project = project;
            this.selectedDeviceName = ctrlImport3.DeviceName;
            this.CtrlImport1 = ctrlImport1;
            this.CtrlImport2 = ctrlImport2;
            this.CtrlImport3 = ctrlImport3;
          
            setDictio(ctrlImport3._dictio);

            List<Cnl> listCnl = new List<Cnl>();
            Dictionary<int, string> dataTypes = new Dictionary<int, string>();
            dataTypes = project.ConfigDatabase.DataTypeTable.ToDictionary(x => x.DataTypeID, x => x.Name);

            foreach (Cnl cnl in project.ConfigDatabase.CnlTable)
            {
                string address = cnl.TagNum.ToString();
                Dictionary<string, string> data = new Dictionary<string, string>
                {
                    { ExtensionPhrases.DestMneColName, cnl.TagCode },
                    { ExtensionPhrases.DestTypeColName, dataTypes.ContainsKey(cnl.DataTypeID ?? 0) ? dataTypes[cnl.DataTypeID ?? 0] : "" },
                    { ExtensionPhrases.DestCmentColName, cnl.Name },
                    { "NumCnl", cnl.CnlNum.ToString()}
                };
                if (!String.IsNullOrEmpty(address) && !_oldDictio.ContainsKey(address))
                {
                    _oldDictio.Add(address, data);
                }

            }
            

				gridViewFiller();
   
        }


        public void setDictio(Dictionary<string, List<string>> dictio)
        {
            this.dictio = dictio;
        }

        public void gridViewFiller()
        {
            Dictionary<string, Dictionary<string, string>> importDictio = new Dictionary<string, Dictionary<string, string>>();

            foreach (KeyValuePair<string, List<string>> kvp in dictio)
            {
                var innerDict = new Dictionary<string, string>
                {
                    {ExtensionPhrases.SrcMneColName, kvp.Value[0]},
                    {ExtensionPhrases.SrcTypeColName, kvp.Value[1]},
                    {ExtensionPhrases.SrcCmentColName, kvp.Value[2]}
                };

                importDictio[kvp.Key] = innerDict;
            }

            foreach (KeyValuePair<string, Dictionary<string, string>> kvp in importDictio)
            {
                bool rowAdded = false;

                foreach (KeyValuePair<string, Dictionary<string, string>> kvpOld in _oldDictio)
                {
                    if (kvp.Key == kvpOld.Key)
                    {
                        dataGridView1.Rows.Add(kvp.Key, false, kvp.Value[ExtensionPhrases.SrcMneColName], kvp.Value[ExtensionPhrases.SrcTypeColName], kvp.Value[ExtensionPhrases.SrcCmentColName], "", false, kvpOld.Value[ExtensionPhrases.DestMneColName], kvpOld.Value[ExtensionPhrases.DestTypeColName], kvpOld.Value[ExtensionPhrases.DestCmentColName]);
                        _listOfKey.Add(kvp.Key);
                        rowAdded = true;
                    }
                }

                if (!rowAdded)
                {
                    dataGridView1.Rows.Add(kvp.Key, false, kvp.Value[ExtensionPhrases.SrcMneColName], kvp.Value[ExtensionPhrases.SrcTypeColName], kvp.Value[ExtensionPhrases.SrcCmentColName], "", false, "", "", "");
                    _listOfKey.Add(kvp.Key);
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[6].ReadOnly = true;
                }
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
            {
                DataGridViewCheckBoxCell currentCheckbox = (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                DataGridViewCheckBoxCell otherCheckbox = (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 1 ? 6 : 1];

                if ((bool)currentCheckbox.Value == true && (bool)otherCheckbox.Value == false)
                {
                    currentCheckbox.Value = false;

                    otherCheckbox.Style.BackColor = Color.White;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Style.BackColor = Color.White;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 2].Style.BackColor = Color.White;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 3].Style.BackColor = Color.White;
                    currentCheckbox.Style.BackColor = Color.White;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 1 ? 7 : 2].Style.BackColor = Color.White;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 1 ? 8 : 3].Style.BackColor = Color.White;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 1 ? 9 : 4].Style.BackColor = Color.White;
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
                        currentCheckbox.Style.BackColor = Color.PaleVioletRed;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 0 ? 7 : 2].Style.BackColor = Color.PaleVioletRed;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 0 ? 8 : 3].Style.BackColor = Color.PaleVioletRed;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 0 ? 9 : 4].Style.BackColor = Color.PaleVioletRed;
                    }
                    else
                    {
                        otherCheckbox.Value = false;
                        currentCheckbox.Value = true;

                        otherCheckbox.Style.BackColor = Color.PaleVioletRed;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 1 ? 7 : 2].Style.BackColor = Color.PaleVioletRed;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 1 ? 8 : 3].Style.BackColor = Color.PaleVioletRed;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 1 ? 9 : 4].Style.BackColor = Color.PaleVioletRed;
                        currentCheckbox.Style.BackColor = Color.LightGreen;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Style.BackColor = Color.LightGreen;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 2].Style.BackColor = Color.LightGreen;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 3].Style.BackColor = Color.LightGreen;
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

                        row.Cells[6].Style.BackColor = Color.PaleVioletRed;
                        row.Cells[7].Style.BackColor = Color.PaleVioletRed;
                        row.Cells[8].Style.BackColor = Color.PaleVioletRed;
                        row.Cells[9].Style.BackColor = Color.PaleVioletRed;
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

                        row.Cells[6].Style.BackColor = Color.White;
                        row.Cells[7].Style.BackColor = Color.White;
                        row.Cells[8].Style.BackColor = Color.White;
                        row.Cells[9].Style.BackColor = Color.White;
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
                    if (!row.Cells[6].ReadOnly)
                    {
                        row.Cells[6].Value = true;

                        row.Cells[1].Style.BackColor = Color.PaleVioletRed;
                        row.Cells[2].Style.BackColor = Color.PaleVioletRed;
                        row.Cells[3].Style.BackColor = Color.PaleVioletRed;
                        row.Cells[4].Style.BackColor = Color.PaleVioletRed;

                        row.Cells[6].Style.BackColor = Color.LightGreen;
                        row.Cells[7].Style.BackColor = Color.LightGreen;
                        row.Cells[8].Style.BackColor = Color.LightGreen;
                        row.Cells[9].Style.BackColor = Color.LightGreen;
                    }
                }
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.Cells[6].ReadOnly)
                    {
                        row.Cells[6].Value = false;

                        row.Cells[1].Style.BackColor = Color.White;
                        row.Cells[2].Style.BackColor = Color.White;
                        row.Cells[3].Style.BackColor = Color.White;
                        row.Cells[4].Style.BackColor = Color.White;

                        row.Cells[6].Style.BackColor = Color.White;
                        row.Cells[7].Style.BackColor = Color.White;
                        row.Cells[8].Style.BackColor = Color.White;
                        row.Cells[9].Style.BackColor = Color.White;
                    }
                }
            }
        }

        private void FrmCnlMerge_Load(object sender, EventArgs e)
        {
            SetCheckboxLocation(_headerCheckBox1, 1);
            SetCheckboxLocation(_headerCheckBox2, 6);

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
            SetCheckboxLocation(_headerCheckBox2, 6);

            SetLabelLocation(lblSource, -1, 5);
            SetLabelLocation(lblDestination, 6, 9);
        }

        private void SetCheckboxLocation(CheckBox ck, int columnIndex)
        {
            Rectangle headerCellRectangle = this.dataGridView1.GetCellDisplayRectangle(columnIndex, -1, true);

            ck.Location = new Point(headerCellRectangle.X + (headerCellRectangle.Width / 2) - 8, headerCellRectangle.Y + 2);
            ck.BackColor = Color.Transparent;
            ck.Size = new Size(18, 18);
        }

        private void SetLabelLocation(Label lbl, int columnStartIndex, int columnEndIndex)
        {
            Rectangle headerCell1Rectangle = this.dataGridView1.GetCellDisplayRectangle(columnStartIndex, -1, true);
            Rectangle headerCell2Rectangle = this.dataGridView1.GetCellDisplayRectangle(columnEndIndex, -1, true);

            lbl.Location = new Point(headerCell1Rectangle.X + dataGridView1.Location.X, lbl.Location.Y);
            lbl.Size = new Size((headerCell2Rectangle.X + dataGridView1.Location.X + headerCell2Rectangle.Width) - headerCell1Rectangle.X, 21);
        }


       
        private Dictionary<string, List<string>> generateDictionnary()
        {
            var dico = new Dictionary<string, List<string>>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells[1].Value) == true)
                {
                    var key = row.Cells[0].Value.ToString();
                    var list = new List<string>
            {
                row.Cells[2].Value.ToString(),
                row.Cells[3].Value.ToString(),
                row.Cells[4].Value.ToString()
            };
                    dico[key] = list;
                }
                else if (Convert.ToBoolean(row.Cells[6].Value) == true) 
                {
                    var key = row.Cells[0].Value.ToString();
                    var list = new List<string>
            {
                row.Cells[7].Value.ToString(),
                row.Cells[8].Value.ToString(),
                row.Cells[9].Value.ToString()
            };
                    dico[key] = list;
                }
            }

            return dico;
        }
        public Dictionary<string, List<string>> StoreDictioData()
        {
            Dictionary<string, List<string>> resultDictionnary = new Dictionary<string, List<string>>();

            foreach (KeyValuePair<string, List<string>> kvp in dictio)
            {
                var list = new List<string>
                {
                    kvp.Value[0], 
                    kvp.Value[1], 
                    kvp.Value[2]  
                };

                resultDictionnary[kvp.Key] = list;
            }

            return resultDictionnary;
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
        /// <summary>
        /// Create or update cnl from file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addChannelsAfterImport()
        {
            _newDictio = ProcessDataGridView(dataGridView1);
            CtrlImport3.ResetCnlNums(_newDictio.Count());
            string deviceName = CtrlImport3.DeviceName;
            int? objNum = CtrlImport2.ObjNum;
            int deviceNum = CtrlImport1.SelectedDevice.DeviceNum;
            int cnlNum = CtrlImport3.StartCnlNum;
            string name;
            CtrlImport3.CnlNameFormat.TryGetValue("separator", out string separator);
            CtrlImport3.CnlNameFormat.TryGetValue("prefix", out string prefix);
            CtrlImport3.CnlNameFormat.TryGetValue("suffix", out string suffix);
            List<Cnl> cnls = new();

            foreach (var kvp in _newDictio)
            {
                string address = Regex.Replace(kvp.Key, @"[^0-9]", "");
                Dictionary<string, string> data = kvp.Value;
                CnlPrototype cnlPrototype = ((List<CnlPrototype>)CtrlImport1.CnlPrototypes)[0];

                Cnl cnl;


                if (_oldDictio.TryGetValue(address, out var oldData) && project.ConfigDatabase.CnlTable.GetItem(int.Parse(oldData["NumCnl"])) != null)
                {
                    cnl = project.ConfigDatabase.CnlTable.GetItem(int.Parse(oldData["NumCnl"]));
                }
                else
                {
                    cnl = new Cnl
                    {
                        CnlNum = cnlNum,
                        Active = cnlPrototype.Active,
                        DataLen = cnlPrototype.DataLen,
                        CnlTypeID = cnlPrototype.CnlTypeID,
                        DeviceNum = deviceNum,
                        FormulaEnabled = cnlPrototype.FormulaEnabled,
                        InFormula = cnlPrototype.InFormula,
                        OutFormula = cnlPrototype.OutFormula,
                        ArchiveMask = cnlPrototype.ArchiveMask,
                        EventMask = cnlPrototype.EventMask
                    };
                    int dataLength = cnlPrototype.GetDataLength();
                    if (cnlNum > ConfigDatabase.MaxID - dataLength)
                    {
                        break;
                    }

                    cnlNum += dataLength;
                }


                cnl.TagCode = address;
                cnl.DataTypeID = cnlDataType.GetValueOrDefault(data["type"]);
                cnl.ObjNum = objNum;

                if (!string.IsNullOrWhiteSpace(prefix) || !string.IsNullOrWhiteSpace(suffix))
                {
                    name = prefix switch
                    {
                        "DeviceName" => CtrlImport3.DeviceName,
                        "TagCode" => address,
                        "TagNumber" => cnlPrototype.TagNum.ToString(),
                        "Type" => cnlPrototype.CnlTypeID.ToString(),
                        _ => prefix
                    };
                    name += separator;
                    name += suffix switch
                    {
                        "DeviceName" => CtrlImport3.DeviceName,
                        "TagCode" => address,
                        "TagNumber" => cnlPrototype.TagNum.ToString(),
                        "Type" => cnlPrototype.CnlTypeID.ToString(),
                        _ => ""
                    };
                }
                else
                {
                    name = CtrlImport3.DeviceName + "-" + address;
                }

                cnl.Name = name;


                cnl.FormatID = project.ConfigDatabase.GetFormatByCode(cnlPrototype.FormatCode)?.FormatID;
                cnl.QuantityID = project.ConfigDatabase.GetQuantityByCode(cnlPrototype.QuantityCode)?.QuantityID;
                cnl.UnitID = project.ConfigDatabase.GetUnitByCode(cnlPrototype.UnitCode)?.UnitID;

                cnls.Add(cnl);

            }

            AddChannels(cnls);
            DialogResult = DialogResult.OK;
        }


        /// <summary>
        /// Get merge selection
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        private Dictionary<string, Dictionary<string, string>> ProcessDataGridView(DataGridView dgv)
        {
            var resultDict = new Dictionary<string, Dictionary<string, string>>();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (Convert.ToBoolean(row.Cells[1].Value)) 
                {
                    AddRowToDict(row, resultDict, 2, 3, 4, true);
                }
                else if (Convert.ToBoolean(row.Cells[6].Value)) 
                {
                    AddRowToDict(row, resultDict, 7, 8, 9, false);
                }
            }

            return resultDict;
        }

        private void AddRowToDict(DataGridViewRow row, Dictionary<string, Dictionary<string, string>> dict, int cell1, int cell2, int cell3, bool isLeft)
        {
            var key = row.Cells[0].Value.ToString();
            var data = new Dictionary<string, string>
            {
                {"tagCode", row.Cells[cell1].Value.ToString()},
                {"type", row.Cells[cell2].Value.ToString()},
                {"name", row.Cells[cell3].Value.ToString()},
                {"numCnl", isLeft ? "" : _oldDictio.ContainsKey(key) ? _oldDictio[key]["NumCnl"] : ""}
            };

            dict[key] = data;
        }

        /// <summary>
        /// Generate xml device variable
        /// </summary>
        /// <param name="dico"></param>
        /// <returns></returns>

        private DeviceTemplate generateDeviceTemplateFromDictionnary(Dictionary<string, List<string>> dico)
        {
            DeviceTemplate template = new DeviceTemplate();

            ElemGroupConfig newElemenGroup = new ElemGroupConfig();

            Dictionary<int, string> dataTypes = new Dictionary<int, string>();
            dataTypes = project.ConfigDatabase.DataTypeTable.ToDictionary(x => x.DataTypeID, x => x.Name);
            string previousPrefix = "";
            ElemType previousType = ElemType.Undefined;
            for (int index = 0; index < dico.Count; index++)
            {
                List<string> entry = dico.ElementAt(index).Value;
                var prefix = dictio[dico.ElementAt(index).Key][3] ?? "";
                ElemConfig newElem = new ElemConfig();
                string newType = elemTypeDico.Keys.Contains(entry[1]) ? entry[1] : cnlDataType.FirstOrDefault(t => t.Value == dataTypes.FirstOrDefault(dt => dt.Value == entry[1]).Key).Key;
                newElem.ElemType = elemTypeDico.Keys.Contains(newType) ? elemTypeDico[newType] : ElemType.Undefined;
                newElem.ByteOrder = newElem.ElemType == ElemType.UShort ? "01" : "0123";
                newElem.Name = entry[0];
                newElem.TagCode = dico.ElementAt(index).Key;
                newElemenGroup.DataBlock = DataBlock.HoldingRegisters;
                if (index == 0 || prefix != previousPrefix || newElem.ElemType != previousType || (prefix == "%MW" && newElemenGroup.Elems.Count == 125) || (prefix == "%M" && newElemenGroup.Elems.Count == 2000))
                {
                    if (index > 0)
                    {
                        template.ElemGroups.Add(newElemenGroup);
                    }
                    newElemenGroup = new ElemGroupConfig();
                    newElemenGroup.Address = int.Parse(Regex.Replace(dico.ElementAt(index).Key, @"[^0-9]", ""));
                }
                previousPrefix = prefix;
                previousType = newElem.ElemType;
                newElemenGroup.Elems.Add(newElem);
            }
            template.ElemGroups.Add(newElemenGroup);
            return template;
        }

        /// <summary>
        /// Save import config from merge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            var dico = generateDictionnary();

            if ( dico.Count != 0)
            {
                DeviceTemplate template = this.generateDeviceTemplateFromDictionnary(dico);

                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);
                XmlElement rootElem = xmlDoc.CreateElement("DeviceTemplate");
                xmlDoc.AppendChild(rootElem);
                DeviceTemplateOptions options = new DeviceTemplateOptions();
                options.SaveToXml(rootElem.AppendElem("Options"));
                XmlElement elemGroupsElem = rootElem.AppendElem("ElemGroups");
                foreach (ElemGroupConfig elemGroupConfig in template.ElemGroups)
                {
                    elemGroupConfig.SaveToXml(elemGroupsElem.AppendElem("ElemGroup"));
                }

                if (selectedDeviceName == null)
                {
                    selectedDeviceName = "New Template.xml";
                }
                saveFileDialog1.FileName = selectedDeviceName;

                saveFileDialog1.InitialDirectory = string.Format("{0}\\Instances\\Default\\ScadaComm\\Config", this.project.ProjectDir);
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.Create))
                    using (StreamWriter sw = new StreamWriter(s))
                    {
                        xmlDoc.Save(sw);
                    }
                    addChannelsAfterImport();
                }

            }
            else
            {
				ScadaUiUtils.ShowWarning(ExtensionPhrases.SelectWarning);
			}

           
        }
    }
}
