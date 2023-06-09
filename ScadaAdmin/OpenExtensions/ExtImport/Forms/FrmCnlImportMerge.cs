using Scada.Admin.Project;
using Scada.Data.Entities;
using System.Xml;
using Scada.Comm.Drivers.DrvModbus.Config;
using Scada.Comm.Drivers.DrvModbus.Protocol;
using System.Text.RegularExpressions;
using Scada.Comm.Devices;
using Scada.Data.Const;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtImport.Forms
{
	public partial class FrmCnlImportMerge : Form
	{

		private Dictionary<string, List<string>> dictio;
		//private Dictionary<string, List<string>> _oldDictio = new Dictionary<string, List<string>>();
		Dictionary<string, Dictionary<string, string>> _oldDictio = new Dictionary<string, Dictionary<string, string>>();

		//private Dictionary<string, List<string>> _newDictio = new Dictionary<string, List<string>>();
		private Dictionary<string, Dictionary<string, string>> _newDictio = new Dictionary<string, Dictionary<string, string>>();

		Dictionary<string, object> deviceInfoDict = new Dictionary<string, object>();

		private List<string> _listOfKey = new List<string>();
		private IAdminContext adminContext; // the Administrator context
		private ScadaProject project;       // the project under development
		private Controls.CtrlCnlCreate3 CtrlCnlCreate3;
		private Controls.CtrlCnlCreate2 CtrlCnlCreate2;
		private Controls.CtrlCnlCreate1 CtrlCnlCreate1;
		string selectedDeviceName;
		Dictionary<string, ElemType> elemTypeDico;
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
		//private Dictionary<string, int> cnlDataTypeDictionary = new Dictionary<string, int>
		//{
		//	{"BOOL", 1},
		//	{"EBOOL", 1},
		//	{"REAL", 0},
		//	{"FLOAT", 0},
		//	{"INT", 1},
		//	{"LONG", 1},
		//	{"SHORT", 1},
		//	{"DWORD", 1},
		//	{"QWORD", 1},
		//	{"UNDEFINED", 3},
		//	{"WORD", 1}
		//};
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

		private FrmCnlImportMerge()
		{
			InitializeComponent();
			dataGridView1.AutoGenerateColumns = false;
			this.elemTypeDico = new Dictionary<string, ElemType>
			{
				{"BOOL", ElemType.Bool },
				{"EBOOL", ElemType.Bool },
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
		private Dictionary<int, string> dataTypeDictionary = new Dictionary<int, string>
		{
			{0, "Double"},
			{1, "Integer"},
			{2, "ASCII string"},
			{3, "Unicode string"}
		};
		private Dictionary<int, string> cnlTypeDictionary = new Dictionary<int, string>
		{
			{1, "Input"},
			{2, "Input/output"},
			{3, "Calculated"},
			{4, "Calculated/output"},
			{5, "Output"}
		};



		/// <summary>
		/// 
		/// </summary>
		/// <param name="adminContext"></param>
		/// <param name="project"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public void Init(IAdminContext adminContext, ScadaProject project)
		{
			this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
			this.project = project ?? throw new ArgumentNullException(nameof(project));

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="project"></param>
		/// <param name="deviceInfoDict"></param>
		/// <param name="dictio"></param>
		/// <param name="CtrlCnlImport3"></param>

		public FrmCnlImportMerge(ScadaProject project, Controls.CtrlCnlCreate1 ctrlCnlCreate1, Controls.CtrlCnlCreate2 ctrlCnlCreate2, Controls.CtrlCnlCreate3 ctrlCnlCreate3) : this()
		{

			this.project = project;
			//this.deviceInfoDict = deviceInfoDict;
			//this.selectedDeviceName = deviceInfoDict.ContainsKey("deviceName") ? (string)deviceInfoDict["deviceName"] : "default value";
			this.CtrlCnlCreate1 = ctrlCnlCreate1; //?? throw new ArgumentNullException(nameof(project));
			this.selectedDeviceName = ctrlCnlCreate1.SelectedDevice.Name;
			this.CtrlCnlCreate2 = ctrlCnlCreate2;//?? throw new ArgumentNullException(nameof(project));
			this.CtrlCnlCreate3 = ctrlCnlCreate3;// ?? throw new ArgumentNullException(nameof(project)); 

			setDictio(ctrlCnlCreate3._dictio);


			List<Cnl> listCnl = new List<Cnl>();
			Dictionary<int, string> dataTypes = new Dictionary<int, string>();
			dataTypes = project.ConfigDatabase.DataTypeTable.ToDictionary(x => x.DataTypeID, x => x.Name);

			foreach (Cnl cnl in project.ConfigDatabase.CnlTable)
			{
				string address = cnl.TagNum.ToString();
				Dictionary<string, string> data = new Dictionary<string, string>
				{
					{ "Mnemonique", cnl.TagCode },
					{ "Type", dataTypes.ContainsKey(cnl.DataTypeID ?? 0) ? dataTypes[cnl.DataTypeID ?? 0] : "" },
					{ "Descr", cnl.Name },
					{ "NumCnl", cnl.CnlNum.ToString()}
				};
				if (!String.IsNullOrEmpty(address) && !_oldDictio.ContainsKey(address))
				{
					_oldDictio.Add(address, data);
				}


			}

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
		//public void gridViewFiller()
		//{
		//	// Clear all existing rows
		//	dataGridView1.Rows.Clear();
		//	List<Cnl> channelPrototypes;
		//	// Create channel prototypes
		//	if (dictio.Count != 0)
		//          {
		//		channelPrototypes = this.CreateChannelsFromFille(StoreDictioData());

		//	}
		//	else
		//	{
		//		channelPrototypes= CreateChannels();

		//	}


		//	foreach (var prototype in channelPrototypes)
		//	{
		//		var cnlNum = prototype.CnlNum;
		//		var projectItem = project.ConfigDatabase.CnlTable.GetItem(cnlNum);

		//		string projectCnlName = projectItem != null ? projectItem.Name : "";
		//		string projectCnlType = projectItem != null ? cnlTypeDictionary[projectItem.CnlTypeID] : "";
		//		//string projectDataType = projectItem != null ? cnlDataTypeDictionary[projectItem.DataTypeID] : "";
		//		string projectDataType = (projectItem != null && projectItem.DataTypeID.HasValue) ? dataTypeDictionary[projectItem.DataTypeID.Value] : "";

		//		string projectTagCode = projectItem != null ? projectItem.TagCode : "";

		//		string prototypeCnlType = cnlTypeDictionary[prototype.CnlTypeID];
		//		//string prototypeDataType = cnlDataTypeDictionary[prototype.DataTypeID];
		//		string prototypeDataType = prototype.DataTypeID.HasValue ? dataTypeDictionary[prototype.DataTypeID.Value] : "";


		//		dataGridView1.Rows.Add(
		//			prototype.CnlNum, // Number
		//			false, // Checkbox
		//			prototype.Name, // Name
		//			prototypeDataType, // Type
		//			prototypeCnlType, // Channel Type
		//			prototype.TagCode, // Tag Code
		//			"", // Empty column
		//			false, // Checkbox
		//			projectCnlName, // Name
		//			projectDataType, // Type
		//			projectCnlType, // Channel Type
		//			projectTagCode // Tag Code
		//		);
		//	}
		//}

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
			string namePrefix = CtrlCnlCreate1.SelectedDevice.Name;
			//adminContext.AppConfig.ChannelNumberingOptions.PrependDeviceName ?
			//CtrlCnlCreate1.SelectedDevice.Name + " - " : "";
			int? objNum = CtrlCnlCreate2.ObjNum;
			int deviceNum = CtrlCnlCreate1.SelectedDevice.DeviceNum;

			foreach (CnlPrototype cnlPrototype in CtrlCnlCreate1.CnlPrototypes)
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
				else if (Convert.ToBoolean(row.Cells[7].Value) == true) // Si la case de droite est cochée
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
		//private void btnAdd_Click_1(object sender, EventArgs e)
		//{
		//	_newDictio = ProcessDataGridView(dataGridView1);
		//	CtrlCnlCreate3.ResetCnlNums(_newDictio.Count());
		//	string deviceName = (string)deviceInfoDict["deviceName"];
		//	int? objNum = deviceInfoDict.ContainsKey("objNum") ? (int?)deviceInfoDict["objNum"] : null;
		//	int deviceNum = (int)deviceInfoDict["deviceNum"];
		//	int cnlNum = (int)deviceInfoDict["cnlNum"];

		//	List<Cnl> cnls = new();

		//	foreach (var kvp in _newDictio)
		//	{
		//		string address = kvp.Key;
		//		Dictionary<string, string> data = kvp.Value;
		//		CnlPrototype cnlPrototype = ((List<CnlPrototype>)deviceInfoDict["Prototypes"])[0]; // Assuming there is at least one prototype

		//		Cnl cnl;


		//		if (_oldDictio.TryGetValue(address, out var oldData) && project.ConfigDatabase.CnlTable.GetItem(int.Parse(oldData["NumCnl"])) != null)
		//		{
		//			cnl = project.ConfigDatabase.CnlTable.GetItem(int.Parse(oldData["NumCnl"]));
		//		}
		//		else
		//		{
		//			cnl = new Cnl
		//			{
		//				CnlNum = cnlNum,
		//				Active = cnlPrototype.Active,
		//				DataLen = cnlPrototype.DataLen,
		//				CnlTypeID = cnlPrototype.CnlTypeID,
		//				DeviceNum = deviceNum,
		//				FormulaEnabled = cnlPrototype.FormulaEnabled,
		//				InFormula = cnlPrototype.InFormula,
		//				OutFormula = cnlPrototype.OutFormula,
		//				ArchiveMask = cnlPrototype.ArchiveMask,
		//				EventMask = cnlPrototype.EventMask
		//			};
		//			int dataLength = cnlPrototype.GetDataLength();
		//			if (cnlNum > ConfigDatabase.MaxID - dataLength)
		//			{
		//				break;
		//			}

		//			cnlNum += dataLength; // Increment cnlNum for each new address by the length of data
		//		}
		//		//cnl.DataTypeID = cnlDataType.TryGetValue(data["type"], out int dataTypeID);
		//		cnl.DataTypeID = cnlDataType.GetValueOrDefault(data["type"]);
		//		cnl.ObjNum = objNum;
		//		cnl.Name = data["name"];
		//		//cnl.TagNum = int.Parse(address);
		//		cnl.TagNum = int.Parse(address.Contains(":") ? address.Split(':')[0] : address);
		//		cnl.TagCode = data["tagCode"];
		//		cnl.FormatID = project.ConfigDatabase.GetFormatByCode(cnlPrototype.FormatCode)?.FormatID;
		//		cnl.QuantityID = project.ConfigDatabase.GetQuantityByCode(cnlPrototype.QuantityCode)?.QuantityID;
		//		cnl.UnitID = project.ConfigDatabase.GetUnitByCode(cnlPrototype.UnitCode)?.UnitID;

		//		cnls.Add(cnl);

		//	}

		//	AddChannels(cnls);
		//	DialogResult = DialogResult.OK;
		//}


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
				if (Convert.ToBoolean(row.Cells[1].Value)) // Si la case de gauche est cochée
				{
					AddRowToDict(row, resultDict, 2, 3, 4, true);
				}
				else if (Convert.ToBoolean(row.Cells[7].Value)) // Si la case de droite est cochée
				{
					AddRowToDict(row, resultDict, 7, 8, 9, false);
				}
			}

			return resultDict;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="row"></param>
		/// <param name="dict"></param>
		/// <param name="cell1"></param>
		/// <param name="cell2"></param>
		/// <param name="cell3"></param>
		/// <param name="isLeft"></param>
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
			saveFileDialog1.InitialDirectory = string.Format("{0}\\Instances\\Default\\ScadaComm\\Config", this.project.ProjectDir);
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
