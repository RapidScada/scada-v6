// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Config;
using Scada.Admin.Project;
using Scada.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Scada.Admin.Extensions.ExtImport.Controls
{
	/// <summary>
	/// Represents a control for selecting channel numbers when creating channels.
	/// <para>Представляет элемент управления для выбора номеров каналов при создании каналов.</para>
	/// </summary>
	public partial class CtrlCnlCreate3 : UserControl
	{
		private IAdminContext adminContext; // the Administrator context
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private ScadaProject project;       // the project under development
		private int lastStartCnlNum;        // the last calculated start channel number
		private int lastCnlCnt;             // the last specified number of channels
		
		public bool lastCheckState { get; internal set; }
		public event EventHandler SelectedFileChanged;
		public Dictionary<string, List<string>> _dictio = new Dictionary<string, List<string>>();
		public bool FileSelected { get; internal set; }
		private string _mnemonique;
		private string _adress;
		private string _type;
		private string _comment;
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		public CtrlCnlCreate3()
		{
			InitializeComponent();
		}

		private void OnSelectedFileChanged()
		{
			SelectedFileChanged?.Invoke(this, EventArgs.Empty);
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
		OnSelectedFileChanged();
		}

		/// <summary>
		/// Sets the input focus.
		/// </summary>
		public void SetFocus()
		{
			numStartCnlNum.Select();
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			txtPathFile.Enabled = radioButton1.Checked;
			txtPathFile.Visible = radioButton1.Checked;
			btnSelectFile.Enabled = radioButton1.Checked;
			btnSelectFile.Visible = radioButton1.Checked;
		}
		private void rdoEnableImport_MouseClick(object sender, MouseEventArgs e)
		{
			//if (lastCheckState)
			//{
			//	radioButton1.Checked = false;
			//}

			lastCheckState = radioButton1.Checked;
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

			string prefix = Regex.Split(_adress, @"[0-9]").First(); //Regex.Replace(_adress, @"[^0-9]", "");
																	// DG
			_adress = new string(_adress.SkipWhile(x => !char.IsDigit(x)).ToArray());

			//setFormatType(columns[2]);
			_comment = columns[3].Replace("\"", "");

			//add in dictionary

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

			foreach (string colum in colums)
			{
				if (String.IsNullOrEmpty(colum) && colums[4] != colum)
				{
					isAVar = false;
				}
			}

			if (isAVar)
			{
				_mnemonique = colums[0];
				_adress = colums[1];
				_adress = new string(_adress.SkipWhile(x => !char.IsDigit(x)).ToArray());
				//setFormatType(colums[2]);
				_comment = colums[3].Replace("\"", "");
				string prefix = Regex.Split(_adress, @"[0-9]").First();

				//add in dictionary

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

				//add in dictionary
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


	}
}
