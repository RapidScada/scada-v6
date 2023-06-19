// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Config;
using Scada.Admin.Extensions.ExtImport.Code;
using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Scada.Admin.Extensions.ExtImport.Controls
{

    public partial class CtrlImport3 : UserControl
    {
        private IAdminContext adminContext; // the Administrator context
        private OpenFileDialog openFileDialog1;
        private ScadaProject project;       // the project under development
        private int lastStartCnlNum;        // the last calculated start channel number
        private int lastCnlCnt;             // the last specified number of channels

        public bool lastCheckState { get; internal set; }
        public bool lastCheckState2 { get; internal set; }
        public event EventHandler SelectedFileChanged;
        public event EventHandler rdbCheckStateChanged;

        public Dictionary<string, List<string>> _dictio = new Dictionary<string, List<string>>();
		private readonly List<Obj> prefixesAndSuffixes = ConfigDictionaries.prefixesAndSuffixes;
		public bool FileSelected { get; internal set; }
        private string _mnemonique;
        private string _adress;
        private string _type;
        private string _comment;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlImport3()
        {
            InitializeComponent();


        }

        private void OnSelectedFileChanged()
        {
            SelectedFileChanged?.Invoke(this, EventArgs.Empty);
        }
        private void OnRdbCheckStateChanged()
        {
            rdbCheckStateChanged?.Invoke(this, EventArgs.Empty);
        }

        

        /// <summary>
        /// Gets or sets the selected device name.
        /// </summary>
        public string DeviceName
        {
            get
            {
                return txtDevice.Text;
            }
            set
            {
                txtDevice.Text = value ?? "";
            }
        }

        private Dictionary<string, string> _cnlNameFormat = new Dictionary<string, string>();
        public IReadOnlyDictionary<string, string> CnlNameFormat
        {
            get { return new ReadOnlyDictionary<string, string>(_cnlNameFormat); }
        }


        /// <summary>
        /// Gets the start channel number.
        /// </summary>
        public int StartCnlNum => Convert.ToInt32(numStartCnlNum.Value);


        /// <summary>
        /// Calculates a start channel number.
        /// </summary>
        private bool CalcStartCnlNum(int cnlCnt, out int startCnlNum)
        {
            ChannelNumberingOptions options = adminContext.AppConfig.ChannelNumberingOptions;
            startCnlNum = options.Multiplicity + options.Shift;
            int prevCnlNum = 0;

            foreach (int cnlNum in project.ConfigDatabase.CnlTable.EnumerateKeys())
            {
                if (prevCnlNum < startCnlNum && startCnlNum <= cnlNum)
                {
                    if (startCnlNum + cnlCnt + options.Gap <= cnlNum)
                        return true;
                    else
                        startCnlNum += options.Multiplicity;
                }

                prevCnlNum = cnlNum;
            }

            return startCnlNum <= ushort.MaxValue;
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        public void Init(IAdminContext adminContext, ScadaProject project)
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            openFileDialog1 = new OpenFileDialog();
            lastStartCnlNum = 1;
            lastCnlCnt = 0;
            FileSelected = false;
            lastCheckState = false;
            radioButton2.Checked = true;
            OnSelectedFileChanged();
            OnRdbCheckStateChanged();
            initCmbNameFormat();

        }

        /// <summary>
        /// Sets the input focus.
        /// </summary>
        public void SetFocus()
        {
            numStartCnlNum.Select();
            cbBoxSuffix.Select();
            cbBoxPrefix.Select();
            txtSeparator.Select();
        }

        /// <summary>
        /// Sets the channel numbers by default.
        /// </summary>
        public void ResetCnlNums(int cnlCnt)
        {
            lastStartCnlNum = 1;
            lastCnlCnt = cnlCnt;

            if (cnlCnt > 0)
            {
                gbCnlNums.Enabled = true;

                if (CalcStartCnlNum(cnlCnt, out int startCnlNum))
                    lastStartCnlNum = startCnlNum;
            }
            else
            {
                gbCnlNums.Enabled = false;
            }

            numStartCnlNum.SetValue(lastStartCnlNum);
            numEndCnlNum.SetValue(lastStartCnlNum + lastCnlCnt - 1);
        }
        private void initCmbNameFormat()
        {
            List<Obj> listPrefix = prefixesAndSuffixes.Select(item => new Obj { ObjNum = item.ObjNum, Name = item.Name }).ToList();
            cbBoxPrefix.Items.Clear();
            cbBoxPrefix.DataSource = listPrefix;
            cbBoxPrefix.ValueMember = "ObjNum";
            cbBoxPrefix.DisplayMember = "Name";

            List<Obj> listSuffix = prefixesAndSuffixes.Select(item => new Obj { ObjNum = item.ObjNum, Name = item.Name }).ToList();
            cbBoxSuffix.Items.Clear();
            cbBoxSuffix.DataSource = listSuffix;
            cbBoxSuffix.ValueMember = "ObjNum";
            cbBoxSuffix.DisplayMember = "Name";

            cbBoxPrefix.SelectedIndex = 1;
            cbBoxSuffix.SelectedIndex = 2;

        }

        private void cbBoxPrefix_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Obj obj = cbBoxPrefix.SelectedItem as Obj;
            if (obj != null)
            {
                _cnlNameFormat["prefix"] = obj.Name;
            }
        }

        private void cbBoxSuffix_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Obj obj2 = cbBoxSuffix.SelectedItem as Obj;
            if (obj2 != null)
            {

                _cnlNameFormat["suffix"] = obj2.Name;
            }
        }


        private void numStartCnlNum_ValueChanged(object sender, EventArgs e)
        {
            int startCnlNum = Convert.ToInt32(numStartCnlNum.Value);
            numEndCnlNum.SetValue(startCnlNum + lastCnlCnt - 1);
        }

        private void btnMap_Click(object sender, EventArgs e)
        {
            // send message to generate map
            adminContext.MessageToExtensions(new MessageEventArgs
            {
                Message = KnownExtensionMessage.GenerateChannelMap,
                Arguments = new Dictionary<string, object> { { "GroupByDevices", true } }
            });
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (lastStartCnlNum > 0)
                numStartCnlNum.SetValue(lastStartCnlNum);
        }

       
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            txtPathFile.Enabled = radioButton1.Checked;
            btnSelectFile.Enabled = radioButton1.Checked;
            OnRdbCheckStateChanged();
        }
        private void rdoEnableImport_MouseClick(object sender, MouseEventArgs e)
        {
            lastCheckState = radioButton1.Checked;
            OnRdbCheckStateChanged();
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            lastCheckState2 = radioButton2.Checked;
            OnRdbCheckStateChanged();

        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fichiers texte (*.txt)|*.txt|Fichiers SCY (*.scy)|*.scy";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                string fileSelected = openFileDialog.FileName;

                txtPathFile.Text = fileSelected;

                readFile(fileSelected);
            }


        }
        private void readFile(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                if (Path.GetExtension(fileName) == ".txt" || Path.GetExtension(fileName) == ".TXT")
                {
                    int count = 0;
                    bool isPL7 = false;
                    _dictio.Clear();
                    FileSelected = true;
                    OnSelectedFileChanged();

                    while (!sr.EndOfStream)
                    {
                        if (count == 0)
                        {
                            string firstLine = sr.ReadLine();
                            if (firstLine.StartsWith("%")) isPL7 = true;

                            if (isPL7) readTxtPL7(firstLine);

                            else readTxtControlExpert(firstLine);
                            count++;
                        }

                        string line = sr.ReadLine();

                        if (isPL7) readTxtPL7(line);

                        else readTxtControlExpert(line);
                    }
                }

                else if (Path.GetExtension(fileName) == ".SCY")
                {
                    FileSelected = true;
                    OnSelectedFileChanged();
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        readSCYPL7(line);
                    }
                }
                else
                {
                    FileSelected = false;
                    OnSelectedFileChanged();
                }
            }
        }

        private void readTxtPL7(string l)
        {
            string[] columns = l.Split('\t');
            _mnemonique = columns[1];
            _adress = columns[0];

            string prefix = Regex.Split(_adress, @"[0-9]").First();

            _adress = new string(_adress.SkipWhile(x => !char.IsDigit(x)).ToArray());

            _comment = columns[3].Replace("\"", "");


            List<string> list = new List<string>();
            list.Add(_mnemonique);
            list.Add(columns[2]);
            list.Add(_comment);
            list.Add(prefix);

            _dictio.Add(_adress, list);
        }

        private void readTxtControlExpert(string l)
        {
            string[] colums = l.Split("\t");

            bool isAVar = true;

            if (colums[1] == "")
            {
                isAVar = false;
            }

            if (isAVar)
            {
                _mnemonique = colums[0];
                _adress = colums[1];
                _adress = new string(_adress.SkipWhile(x => !char.IsDigit(x)).ToArray());
                _comment = colums[3].Replace("\"", "");
                string prefix = Regex.Split(_adress, @"[0-9]").First();

                List<string> list = new List<string>();
                list.Add(_mnemonique);
                list.Add(colums[2]);
                list.Add(_comment);
                list.Add(prefix);

                if (!_dictio.ContainsKey(_adress))
                    _dictio.Add(_adress, list);
            }
        }

        private void readSCYPL7(string l)
        {
            string[] splitInTwo = l.Split(" AT ");

            if (splitInTwo.Length >= 2)
            {
                _mnemonique = splitInTwo[0];

                string[] splitAdress = splitInTwo[1].Split(" : ");
                _adress = splitAdress[0];

                string[] splitType = splitAdress[1].Split(" (*");
                _type = splitType[0];

                string[] splitComment = splitType[1].Split('*');
                _comment = splitComment[0].Replace("\"", "");

                string prefix = Regex.Split(_adress, @"[0-9]").First();

                List<string> list = new List<string>
                {
                    _mnemonique,
                    _type,
                    _comment,
                    prefix,
                };

                _dictio.Add(_adress, list);
                Console.WriteLine(_dictio.Count);
            }
        }

        private void txtSeparator_TextChanged(object sender, EventArgs e)
        {
            string separator = string.IsNullOrEmpty(txtSeparator.Text) ? string.Empty : txtSeparator.Text;
            _cnlNameFormat["separator"] = separator;

        }


    }
}
