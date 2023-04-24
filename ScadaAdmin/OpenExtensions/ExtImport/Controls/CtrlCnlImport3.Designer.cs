using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtImport.Controls
{
	partial class CtrlCnlImport3
	{
		/// <summary> 
		/// Variable nécessaire au concepteur.
		/// </summary>
		private IContainer components = null;
		private Button btnSelectFile;

		/// <summary> 
		/// Nettoyage des ressources utilisées.
		/// </summary>
		/// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Code généré par le Concepteur de composants

		/// <summary> 
		/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
		/// le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent()
		{
			btnSelectFile = new Button();
			openFileDialog = new OpenFileDialog();
			SuspendLayout();
			// 
			// btnSelectFile
			// 
			btnSelectFile.Location = new Point(0, 0);
			btnSelectFile.Name = "btnSelectFile";
			btnSelectFile.Size = new Size(75, 23);
			btnSelectFile.TabIndex = 0;
			btnSelectFile.Click += BtnImport_Click;
			// 
			// CtrlCnlImport3
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Margin = new Padding(3, 2, 3, 2);
			Name = "CtrlCnlImport3";
			Size = new Size(274, 151);
			Load += CtrlCnlImport3_Load;
			ResumeLayout(false);
		}

		#endregion
	}
}

