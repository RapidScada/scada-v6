using Scada.Admin.Config;
using Scada.Admin.Project;
using Scada.Forms;
using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtImport.Controls
{
	public partial class CtrlCnlImport3 : UserControl
	{
		private IAdminContext adminContext; // the Administrator context
		private ScadaProject project;       // the project under development
		public CtrlCnlImport4 ctrlCnlImport4 { get; set; }
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		public Dictionary<string, List<string>> _dictio = new Dictionary<string, List<string>>();

		private string _mnemonique;
		private string _adress;
		private string _type;
		private string _comment;

		//public Dictionary<string, List<string>>  ImportedData { get; private set; }

		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		public CtrlCnlImport3()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Initializes the control.
		/// </summary>
		public void Init(IAdminContext adminContext, ScadaProject project)
		{
			this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
			this.project = project ?? throw new ArgumentNullException(nameof(project));
			openFileDialog1 = new OpenFileDialog();

		}

		/// <summary>
		/// Sets the input focus.
		/// </summary>
		public void SetFocus()
		{
			// You can set focus to a specific control, e.g., a TextBox or a Button
		}

		/// <summary>
		/// Imports channels based on user input.
		/// </summary>
		public void ImportChannels()
		{
			// Implement the import functionality based on user input
			// You may need to access controls in the designer file
		}
		public void ImportProject() { }

		private void btnImport_Click(object sender, EventArgs e)
		{
			//// Créer une nouvelle instance de OpenFileDialog
			//OpenFileDialog openFileDialog = new OpenFileDialog();

			//// Configurer les propriétés de OpenFileDialog
			//openFileDialog.Filter = "Fichiers texte (*.txt)|*.txt|Tous les fichiers (*.*)|*.*";
			//openFileDialog.Title = "Sélectionner un fichier texte à importer";

			//// Afficher la boîte de dialogue et vérifier si l'utilisateur a sélectionné un fichier
			//if (openFileDialog.ShowDialog() == DialogResult.OK)
			//{
			//	// Lire le contenu du fichier sélectionné
			//	string contenuFichier = File.ReadAllText(openFileDialog.FileName);

			//	// Analyser le contenu du fichier et stocker les données importées
			//	Dictionary<string, string> importedData = new Dictionary<string, string>();
			//	string[] lines = contenuFichier.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

			//	foreach (string line in lines)
			//	{
			//		string[] columns = line.Split(',');
			//		if (columns.Length >= 2)
			//		{
			//			Console.WriteLine(columns[0]);
			//			importedData.Add(columns[0], columns[1]);
			//		}
			//	}
			//	Console.WriteLine("Données importées : ");
			//	foreach (KeyValuePair<string, string> entry in importedData)
			//	{
			//		Console.WriteLine("Clé : " + entry.Key + " Valeur : " + entry.Value);
			//	}

			//	// Passer les données importées à CtrlCnlImport4
			//	if (CtrlCnlImport4Instance != null)
			//	{
			//		Console.WriteLine(importedData);
			//		CtrlCnlImport4Instance.SetDictio(importedData);
			//	}

			//	// Passer à l'étape suivante (CtrlCnlImport4) - SUPPRIMER CETTE LIGNE
			//	// ApplyStep(1);

			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Fichiers texte (*.txt)|*.txt|Fichiers SCY (*.scy)|*.scy";

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string fileSelected = openFileDialog.FileName;
				pathFile.Text = Path.GetFileNameWithoutExtension(fileSelected);
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

				if (Path.GetExtension(fileName) == ".SCY")
				{
					while (!sr.EndOfStream)
					{
						string line = sr.ReadLine();
						readSCYPL7(line);
					}
				}
			}
		}

		private void readTxtPL7(string l)
		{
			string[] columns = l.Split('\t');

			_mnemonique = columns[1];
			_adress = columns[0];
			setFormatType(columns[2]);
			_comment = columns[3];

			//add in dictionary

			List<string> list = new List<string>();
			list.Add(_mnemonique);
			list.Add(_type);
			list.Add(_comment);

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
				setFormatType(colums[2]);
				_comment = colums[3];

				//add in dictionary

				List<string> list = new List<string>();
				list.Add(_mnemonique);
				list.Add(_type);
				list.Add(_comment);

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
				_comment = splitComment[0];

				//add in dictionary

				List<string> list = new List<string>();
				list.Add(_mnemonique);
				list.Add(_type);
				list.Add(_comment);

				_dictio.Add(_adress, list);
				Console.WriteLine(_dictio.Count);
			}
		}

		private void setFormatType(string type)
		{
			switch (type)
			{
				case "DWORD":
					_type = "DINT";
					break;
				case "WORD":
					_type = "INT";
					break;
				default:
					break;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{

		}

		private void label1_Click(object sender, EventArgs e)
		{

		}
	}


}

