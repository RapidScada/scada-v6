using Scada.Admin.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Scada.Admin.Extensions.ExtImport.Controls
{
	public partial class CtrlCnlImport4 : UserControl
	{

		private Dictionary<string, List<string>> dictio;
		private Dictionary<string, List<string>> _oldDictio = new Dictionary<string, List<string>>();
		private Dictionary<string, List<string>> _newDictio = new Dictionary<string, List<string>>();
		private List<string> _listOfKey = new List<string>();
		private IAdminContext adminContext; // the Administrator context
		private ScadaProject project;       // the project under development
		public CtrlCnlImport4()
		{
			InitializeComponent();

			xmlReader();
			gridViewFiller();
		}

		public void setDictio(Dictionary<string, List<string>> dictio)
		{
			this.dictio = dictio;
		}
		public void Init(IAdminContext adminContext, ScadaProject project)
		{
			this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
			this.project = project ?? throw new ArgumentNullException(nameof(project));

		}
		private void xmlReader()
		{
			string filePath = @"C:/Users/messiem/Documents/testsScada/testXML.xml";
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(filePath);

			// Récupérer les valeurs des balises XML
			XmlNodeList entries = xmlDoc.GetElementsByTagName("ENTREE");

			foreach (XmlNode entry in entries)
			{
				XmlNodeList childNodes = entry.ChildNodes;
				List<string> list = new List<string>();
				string adress = "";

				for (int i = 0; i < childNodes.Count; i++)
				{
					if (childNodes[i].Name == "Adress") adress = childNodes[i].InnerText;
					if (childNodes[i].Name == "Mnemonique") list.Add(childNodes[i].InnerText);
					if (childNodes[i].Name == "Type") list.Add(childNodes[i].InnerText);
					if (childNodes[i].Name == "Descr") list.Add(childNodes[i].InnerText);
				}

				if (!String.IsNullOrEmpty(adress))
					_oldDictio.Add(adress, list);
			}
		}

		private void gridViewFiller()
		{
			foreach (KeyValuePair<string, List<string>> kvp in dictio)
			{
				foreach (KeyValuePair<string, List<string>> kvpOld in _oldDictio)
				{
					if (kvp.Key == kvpOld.Key)
					{
						label2.Text = "Vous cherchez à importer des variables déjà présentes dans le système.";
						label3.Text = "Un choix de votre part est requis.";

						dataGridView1.Rows.Add(false, kvp.Key, "", false, kvpOld.Key);
						_listOfKey.Add(kvp.Key);
					}
				}
			}
		}



		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{
			// on va garder les descriptions déjà présentes dans le système

			foreach (KeyValuePair<string, List<string>> kvp in _newDictio)
			{
				foreach (KeyValuePair<string, List<string>> kvpOld in _oldDictio)
				{
					if (kvp.Key == kvpOld.Key)
						kvp.Value[2] = kvpOld.Value[2];
				}
			}
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
			{
				DataGridViewCheckBoxCell currentCheckbox = (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
				DataGridViewCheckBoxCell otherCheckbox = (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 0 ? 3 : 0];

				if ((bool)currentCheckbox.Value == true && (bool)otherCheckbox.Value == false)
				{
					currentCheckbox.Value = false;

					//color
					otherCheckbox.Style.BackColor = Color.White;
					dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Style.BackColor = Color.White;
					currentCheckbox.Style.BackColor = Color.White;
					dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 0 ? 4 : 1].Style.BackColor = Color.White;
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
						currentCheckbox.Style.BackColor = Color.PaleVioletRed;
						dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 0 ? 4 : 1].Style.BackColor = Color.PaleVioletRed;
					}
					else
					{
						otherCheckbox.Value = false;
						currentCheckbox.Value = true;

						//color
						otherCheckbox.Style.BackColor = Color.PaleVioletRed;
						dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 0 ? 4 : 1].Style.BackColor = Color.PaleVioletRed;
						currentCheckbox.Style.BackColor = Color.LightGreen;
						dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Style.BackColor = Color.LightGreen;
					}
				}

			}
		}

		private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
			{
				DataGridViewCheckBoxCell currentCheckbox = (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
				DataGridViewCheckBoxCell otherCheckbox = (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex == 0 ? 3 : 0];

				if ((bool)currentCheckbox.Value == true)
				{
					otherCheckbox.Value = false;
				}

			}
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox1.Checked)
			{
				checkBox2.Checked = false;

				foreach (DataGridViewRow row in dataGridView1.Rows)
				{
					row.Cells[0].Value = true;

					//color
					row.Cells[0].Style.BackColor = Color.LightGreen;
					row.Cells[1].Style.BackColor = Color.LightGreen;
					row.Cells[3].Style.BackColor = Color.PaleVioletRed;
					row.Cells[4].Style.BackColor = Color.PaleVioletRed;
				}
			}
			else
			{
				foreach (DataGridViewRow row in dataGridView1.Rows)
				{
					row.Cells[0].Value = false;

					//color
					row.Cells[0].Style.BackColor = Color.White;
					row.Cells[1].Style.BackColor = Color.White;
					row.Cells[3].Style.BackColor = Color.White;
					row.Cells[4].Style.BackColor = Color.White;
				}
			}
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox2.Checked)
			{
				checkBox1.Checked = false;

				foreach (DataGridViewRow row in dataGridView1.Rows)
				{
					row.Cells[3].Value = true;

					//color
					row.Cells[3].Style.BackColor = Color.LightGreen;
					row.Cells[4].Style.BackColor = Color.LightGreen;
					row.Cells[0].Style.BackColor = Color.PaleVioletRed;
					row.Cells[1].Style.BackColor = Color.PaleVioletRed;
				}
			}
			else
			{
				foreach (DataGridViewRow row in dataGridView1.Rows)
				{
					row.Cells[3].Value = false;

					//color
					row.Cells[0].Style.BackColor = Color.White;
					row.Cells[1].Style.BackColor = Color.White;
					row.Cells[3].Style.BackColor = Color.White;
					row.Cells[4].Style.BackColor = Color.White;
				}
			}
		}
	}
}
