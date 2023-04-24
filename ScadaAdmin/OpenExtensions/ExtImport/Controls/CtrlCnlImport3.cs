using Scada.Admin.Extensions.ExtImport.Code;
using Scada.Admin.Project;
using Scada.Data.Entities;
using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtImport.Controls
{
	public partial class CtrlCnlImport3 : UserControl
	{
		private IAdminContext adminContext; // the Administrator context
		private ScadaProject project;       // the project under development
		private OpenFileDialog openFileDialog;
		private CtrlCnlImport4 ctrlCnlImport4;

		public CtrlCnlImport3()
		{
			InitializeComponent();

			ctrlCnlImport4 = new CtrlCnlImport4
			{
				Dock = DockStyle.Fill,
				Visible = false // masquer le contrôle au début
			};
			Controls.Add(btnSelectFile);

			Controls.Add(ctrlCnlImport4);
		}

		public void Init(IAdminContext adminContext, ScadaProject project)
		{
			this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
			this.project = project ?? throw new ArgumentNullException(nameof(project));
		}

		private void BtnImport_Click(object sender, EventArgs e)
		{
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string fileName = openFileDialog.FileName;
				ImportChannels(fileName);
				UpdateControlVisibility(true); // rendre le contrôle visible après l'importation du fichier
			}
		}
		private void UpdateControlVisibility(bool fileSelected)
		{
			ctrlCnlImport4.Visible = fileSelected;
		}
		private void ImportChannels(string fileName)
		{
			if (File.Exists(fileName))
			{
				// Read the file and add the data to the DataGridView in ctrlCnlImport4
				using (StreamReader reader = new StreamReader(fileName))
				{
					string line;

					while ((line = reader.ReadLine()) != null)
					{
						// Parse the line to get the old and new data (adapt this to your specific data format)
						string[] columns = line.Split(';');
						string oldData = columns[0];
						string newData = columns[1];

						// Add the data to the DataGridView
						//ctrlCnlImport4.dataGridView.Rows.Add(newData, oldData, false);
					}
				}

				MessageBox.Show($"Importing channels from {fileName}");
			}
			else
			{
				MessageBox.Show($"File not found: {fileName}");
			}
		}

		private void CtrlCnlImport3_Load(object sender, EventArgs e)
		{

		}
	}
}