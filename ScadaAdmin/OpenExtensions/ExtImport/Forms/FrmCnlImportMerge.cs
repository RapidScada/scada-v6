using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Comm.Devices;
using System.Xml.Linq;


namespace Scada.Admin.Extensions.ExtImport.Forms
{
	public partial class FrmCnlImportMerge : Form
	{

		private Dictionary<string, List<string>> dictio;
		private IAdminContext adminContext; // the Administrator context
		private ScadaProject project;       // the project under development
		private Controls.CtrlCnlCreate3 CtrlCnlCreate3;
		private Controls.CtrlCnlCreate2 CtrlCnlCreate2;
		private Controls.CtrlCnlCreate1 CtrlCnlCreate1;
		private CheckBox _headerCheckBox1 = new CheckBox();
		private CheckBox _headerCheckBox2 = new CheckBox();


		private Dictionary<string, int> cnlDataType = new Dictionary<string, int>
			{
				{"BOOL", 1 },
				{"EBOOL", 1 },
				{"REAL", 0 },
				{"FLOAT", 0 },
				{"INT", 1 },
				{"LONG", 1 },
				{"SHORT", 1 },
				{"DWORD", 1 },
				{"QWORD", 1 },
				{"UNDEFINED", 3 }, // or any other default value
                {"WORD", 1 },
			};
	
		private Dictionary<int, string> cnlDataTypeDictionary = new Dictionary<int, string>
		{
			{1, "BOOL"},
			{2, "EBOOL"},
			{3, "REAL"},
			{4, "FLOAT"},
			{5, "INT"},
			{6, "LONG"},
			{7, "SHORT"},
			{8, "DWORD"},
			{9, "QWORD"},
			{10, "UNDEFINED"},
			{11, "WORD"}
		};
		private Dictionary<int, string> cnlTypeDictionary = new Dictionary<int, string>
		{
			{1, "Input"},
			{2, "Input/output"},
			{3, "Calculated"},
			{4, "Calculated/output"},
			{5, "Output"}
		};
		private Dictionary<int, string> dataTypeDictionary = new Dictionary<int, string>
		{
			{0, "Double"},
			{1, "Integer"},
			{2, "ASCII string"},
			{3, "Unicode string"}
		};

		private FrmCnlImportMerge()
		{
			InitializeComponent();
			dataGridView1.AutoGenerateColumns = false;
		}
	
		
		public void Init(IAdminContext adminContext, ScadaProject project)
		{
			this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
			this.project = project ?? throw new ArgumentNullException(nameof(project));

		}

	
		public FrmCnlImportMerge(ScadaProject project, Controls.CtrlCnlCreate1 ctrlCnlCreate1, Controls.CtrlCnlCreate2 ctrlCnlCreate2, Controls.CtrlCnlCreate3 ctrlCnlCreate3) : this()
		{
			this.project = project;
			this.CtrlCnlCreate1 = ctrlCnlCreate1; 
			this.CtrlCnlCreate2 = ctrlCnlCreate2;
			this.CtrlCnlCreate3 = ctrlCnlCreate3; 
			setDictio(ctrlCnlCreate3._dictio);
			gridViewFiller();
		}

		public static List<Cnl> GetMatchingChannels(IEnumerable<Cnl> cnlTable, Dictionary<string, List<string>> dictio)
		{
			List<Cnl> matchingCnls = new List<Cnl>();

			foreach (Cnl cnl in cnlTable)
			{
				string tagCode = cnl.TagCode.ToString();

				if (dictio.ContainsKey(tagCode))
				{
					matchingCnls.Add(cnl);
				}
			}

			return matchingCnls;
		}
		public static List<Cnl> GetAllChannels(IEnumerable<Cnl> cnlTable)
		{
			List<Cnl> channels = new List<Cnl>();

			foreach (Cnl cnl in cnlTable)
			{
				channels.Add(cnl);
			}

			return channels;
		}


		public void setDictio(Dictionary<string, List<string>> dictio)
		{
			this.dictio = dictio;
		}
		public void gridViewFiller()
		{
			// Clear all existing rows
			dataGridView1.Rows.Clear();

			List<Cnl> channelPrototypes;

			// Create channel prototypes
			if (dictio.Count != 0)
			{
				channelPrototypes = this.CreateChannelsFromFille(StoreDictioData());
			}
			else
			{
				channelPrototypes = CreateChannels();
			}

			// Loop through each prototype and add a new row to the dataGridView
			foreach (var prototype in channelPrototypes)
			{
				int rowIndex = dataGridView1.Rows.Add(); // Add a new row and get its index
				DataGridViewRow row = dataGridView1.Rows[rowIndex];

				// Populate the row with the relevant data
				var cnlNum = prototype.CnlNum;
				var projectItem = project.ConfigDatabase.CnlTable.GetItem(cnlNum);

				string projectCnlName = projectItem != null ? projectItem.Name : "";
				string projectCnlType = projectItem != null ? cnlTypeDictionary[projectItem.CnlTypeID] : "";
				string projectDataType = (projectItem != null && projectItem.DataTypeID.HasValue) ? dataTypeDictionary[projectItem.DataTypeID.Value] : "";
				string projectTagCode = projectItem != null ? projectItem.TagCode : "";
				string prototypeCnlType = cnlTypeDictionary[prototype.CnlTypeID];
				string prototypeDataType = prototype.DataTypeID.HasValue ? dataTypeDictionary[prototype.DataTypeID.Value] : "";

				row.Cells[0].Value = prototype.CnlNum;  // Number
				row.Cells[1].Value = false;             // Checkbox
				row.Cells[2].Value = prototype.Name;    // Name
				row.Cells[3].Value = prototypeDataType; // Type
				row.Cells[4].Value = prototypeCnlType;  // Channel Type
				row.Cells[5].Value = prototype.TagCode; // Tag Code
				row.Cells[6].Value = "";                // Empty column
				row.Cells[7].Value = false;             // Checkbox
				row.Cells[8].Value = projectCnlName;    // Name
				row.Cells[9].Value = projectDataType;   // Type
				row.Cells[10].Value = projectCnlType;   // Channel Type
				row.Cells[11].Value = projectTagCode;   // Tag Code

				// Store the Cnl object in a hidden column
				row.Cells[6].Value = prototype;
			}

			// Hide the column containing the Cnl objects
			dataGridView1.Columns[6].Visible = false;
		}
		private void AddSelectedChannels()
		{
			List<Cnl> selectedChannels = new List<Cnl>();

			// Loop through all rows of the dataGridView
			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				// Check if the checkbox in the first column (index 1) is checked
				if (Convert.ToBoolean(row.Cells[1].Value) == true)
				{
					// Retrieve the Cnl object from the hidden column
					Cnl cnl = (Cnl)row.Cells[6].Value;

					// Add the Cnl object to the list
					selectedChannels.Add(cnl);
				}
			}

			// Use the AddChannels function to add the selected channels
			AddChannels(selectedChannels);
		}
		

		private List<Cnl> CreateChannelsFromFille(Dictionary<string, List<string>> store)
		{
			List<Cnl> cnls = new List<Cnl>();
			int cnlNum = CtrlCnlCreate3.StartCnlNum;
			string namePrefix = CtrlCnlCreate1.SelectedDevice.Name;
			int? objNum = CtrlCnlCreate2.ObjNum;
			int deviceNum = CtrlCnlCreate1.SelectedDevice.DeviceNum;

			foreach (KeyValuePair<string, List<string>> element in store)
			{
				string tagCode = element.Key;
				CnlPrototype cnlPrototype = CtrlCnlCreate1.CnlPrototypes.FirstOrDefault(); // or some other way to get the prototype

				if (cnlPrototype != null)
				{
					cnls.Add(new Cnl
					{
						CnlNum = cnlNum,
						Active = cnlPrototype.Active,
						Name = namePrefix + cnlPrototype.Name,
						DataTypeID = cnlPrototype.DataTypeID,
						DataLen = cnlPrototype.DataLen,
						CnlTypeID = cnlPrototype.CnlTypeID,
						ObjNum = objNum,
						DeviceNum = deviceNum,
						TagNum = cnlPrototype.TagNum,
						TagCode = tagCode,
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

					cnlNum++; // Increment cnlNum for each element in store
				}
			}

			return cnls;
		}
		public Dictionary<string, List<string>> StoreDictioData()
		{
			Dictionary<string, List<string>> resultDictionnary = new Dictionary<string, List<string>>();

			// Parcourir dictio et ajouter des éléments à resultDictionnary
			foreach (KeyValuePair<string, List<string>> kvp in dictio)
			{
				var list = new List<string>
		{
			kvp.Value[0], // Mnemonique
            kvp.Value[1], // Type
            kvp.Value[2]  // Descr
        };

				resultDictionnary[kvp.Key] = list;
			}

			return resultDictionnary;
		}


		/// <summary>
		/// Creates channels based on the channel prototypes.
		/// </summary>
		private List<Cnl> CreateChannels()
		{
			List<Cnl> cnls = new();
			int cnlNum = CtrlCnlCreate3.StartCnlNum;
			string name, separator,prefix, suffix;
			CtrlCnlCreate3.CnlNameFormat.TryGetValue("separator", out separator);
			CtrlCnlCreate3.CnlNameFormat.TryGetValue("prefix", out prefix);
			CtrlCnlCreate3.CnlNameFormat.TryGetValue("suffix", out suffix);
		


			int? objNum = CtrlCnlCreate2.ObjNum;
			int deviceNum = CtrlCnlCreate1.SelectedDevice.DeviceNum;

			foreach (CnlPrototype cnlPrototype in CtrlCnlCreate1.CnlPrototypes)
			{

				name = prefix switch
				{
					"DeviceName" => CtrlCnlCreate3.DeviceName,
					"TagCode" => cnlPrototype.TagCode,
					"TagNumber" => cnlPrototype.TagNum.ToString(),
					"Type" => cnlPrototype.CnlTypeID.ToString(),
					_ => prefix
				};
				name += separator;
				name += suffix switch
				{
					"DeviceName" => CtrlCnlCreate3.DeviceName,
					"TagCode" => cnlPrototype.TagCode,
					"TagNumber" => cnlPrototype.TagNum.ToString(),
					"Type" => cnlPrototype.CnlTypeID.ToString(),
					_ => ""
				};
				cnls.Add(new Cnl
				{
					CnlNum = cnlNum,
					Active = cnlPrototype.Active,
					Name =name,
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


		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
			{
				DataGridViewCheckBoxCell currentCheckbox = (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
				DataGridViewCheckBoxCell otherCheckbox = (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 1 ? 7 : 1];

				if ((bool)currentCheckbox.Value == true && (bool)otherCheckbox.Value == false)
				{
					currentCheckbox.Value = false;

					//color
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

						//color
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

						//color
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

						//color
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

						//color
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

						//color
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

						//color
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
		/// 
		private void btnAdd_Click_1(object sender, EventArgs e)
		{
			AddSelectedChannels();
			DialogResult = DialogResult.OK;
		}
		
	
	}
}
