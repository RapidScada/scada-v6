using Scada.Admin.Project;
using Scada.Data.Entities;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Xml;
using Scada.Comm.Devices;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Threading.Channels;
using Scada.Comm.Drivers.DrvModbus.Config;
using Scada.Comm.Drivers.DrvModbus.Protocol;
using static System.Windows.Forms.Design.AxImporter;
using System.Windows.Forms.Design;
using static System.Windows.Forms.DataFormats;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Web;

namespace Scada.Admin.Extensions.ExtImport.Forms
{
    public partial class FrmCnlMerge : Form
    {
        private List<Cnl> cnls;
        private Dictionary<string, List<string>> dictio;
        private Dictionary<string, List<string>> _oldDictio = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> _newDictio = new Dictionary<string, List<string>>();
        private List<string> _listOfKey = new List<string>();
        private ScadaProject project;       // the project under development
        string selectedDeviceName;
        Dictionary<string, ElemType> elemTypeDico;
        private System.Windows.Forms.CheckBox _headerCheckBox1 = new System.Windows.Forms.CheckBox();
        private System.Windows.Forms.CheckBox _headerCheckBox2 = new System.Windows.Forms.CheckBox();

        private FrmCnlMerge()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            this.elemTypeDico = new Dictionary<string, ElemType>
            {
                { "BOOL", ElemType.Bool },
                { "EBOOL", ElemType.Bool },
                {"REAL", ElemType.Double },
                {"FLOAT", ElemType.Float },
                {"INT", ElemType.Int },
                {"LONG", ElemType.Long },
                {"SHORT", ElemType.Short },
                {"DWORD", ElemType.UInt },
                {"QWORD", ElemType.ULong },
                {"UNDEFINED", ElemType.Undefined },
                {"WORD", ElemType.UShort },
            };

        }

        public FrmCnlMerge(ScadaProject project, List<Cnl> cnls, Dictionary<string, List<string>> dictio, string selectedDeviceName) : this()
        {
            //bindingSource.DataSource = cnls ?? throw new ArgumentNullException(nameof(cnls));
            this.project = project;
            this.cnls = cnls;
            this.selectedDeviceName = selectedDeviceName;
            setDictio(dictio);


            Dictionary<int, string> dataTypes = new Dictionary<int, string>();
            dataTypes = project.ConfigDatabase.DataTypeTable.ToDictionary(x => x.DataTypeID, x => x.Name);

            foreach (Cnl cnl in project.ConfigDatabase.CnlTable)
            {
                List<string> list = new List<string>();

                string address = cnl.TagNum.ToString();
                list.Add(cnl.TagCode); // Mnemonique
                list.Add(dataTypes.ContainsKey(cnl.DataTypeID ?? 0) ? dataTypes[cnl.DataTypeID ?? 0] : ""); // Type
                list.Add(cnl.Name); // Descr

                if (!String.IsNullOrEmpty(address) && !_oldDictio.ContainsKey(address))
                    _oldDictio.Add(address, list);
            }

            gridViewFiller();
        }

        public void setDictio(Dictionary<string, List<string>> dictio)
        {
            this.dictio = dictio;
        }

        public void gridViewFiller()
        {
            foreach (KeyValuePair<string, List<string>> kvp in dictio)
            {
                bool rowAdded = false;
                foreach (KeyValuePair<string, List<string>> kvpOld in _oldDictio)
                {
                    if (kvp.Key == kvpOld.Key)
                    {
                        dataGridView1.Rows.Add(kvp.Key, false, kvp.Value[0], kvp.Value[1], kvp.Value[2], "", false, kvpOld.Value[0], kvpOld.Value[1], kvpOld.Value[2]);
                        _listOfKey.Add(kvp.Key);
                        rowAdded = true;
                    }
                }

                if (!rowAdded)
                {
                    dataGridView1.Rows.Add(kvp.Key, false, kvp.Value[0], kvp.Value[1], kvp.Value[2], "", false, "", "", "");
                    _listOfKey.Add(kvp.Key);
                    //dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[6].Style.BackColor = Color.Red;
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

                    //color
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

                        //color
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

                        //color
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

                        //color
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

                        //color
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

                        //color
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

                        //color
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

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private Dictionary<string, List<string>> generateDictionnary()
        {
            var dico = new Dictionary<string, List<string>>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(row.Cells[1].Value) == true) // Si la case de gauche est cochée
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
                else if (Convert.ToBoolean(row.Cells[6].Value) == true) // Si la case de droite est cochée
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


        private DeviceTemplate generateDeviceTemplateFromDictionnary(Dictionary<string, List<string>> dico)
        {
            DeviceTemplate template = new DeviceTemplate();

            ElemGroupConfig newElemenGroup = new ElemGroupConfig();
            int previousTagCode = 0;
            for (int index = 0; index < dico.Count; index++)
            {
                List<string> entry = dico.ElementAt(index).Value;
                int intTagCode = int.Parse(Regex.Split(dico.ElementAt(index).Key, @"[^0-9]").Last());
                if (index == 0 || previousTagCode != intTagCode - 1)
                {
                    if (index > 0)
                    {
                        template.ElemGroups.Add(newElemenGroup);
                    }
                    newElemenGroup = new ElemGroupConfig();
                    newElemenGroup.Address = int.Parse(Regex.Replace(dico.ElementAt(index).Key, @"[^0-9]", "")) - 1;
                }
                previousTagCode = intTagCode;
                ElemConfig newElem = new ElemConfig();
                newElem.Name = entry[0];
                newElem.TagCode = dico.ElementAt(index).Key;
                newElem.ElemType = elemTypeDico.Keys.Contains(entry[1]) ? elemTypeDico[entry[1]] : ElemType.Undefined;
                newElemenGroup.Elems.Add(newElem);
            }
            template.ElemGroups.Add(newElemenGroup);
            return template;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Generate dictionnary
            var dico = generateDictionnary();

            //Fill new DeviceTemplate with dictionnary
            DeviceTemplate template = this.generateDeviceTemplateFromDictionnary(dico);

            //Fill XML with DeviceTemplate
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

            //Define XML document name
            if (selectedDeviceName == null)
            {
                selectedDeviceName = "New Template.xml";
            }
            saveFileDialog1.FileName = selectedDeviceName;


            //Save file
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.Create))
                using (StreamWriter sw = new StreamWriter(s))
                {
                    xmlDoc.Save(sw);
                }
            }
        }
    }
}
