namespace Scada.Admin.Extensions.ExtWirenBoard.Controls
{
    partial class CtrlLineSelect
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblInstance = new System.Windows.Forms.Label();
            this.cbInstance = new System.Windows.Forms.ComboBox();
            this.lblLine = new System.Windows.Forms.Label();
            this.cbLine = new System.Windows.Forms.ComboBox();
            this.lblWirenBoardIP = new System.Windows.Forms.Label();
            this.txtWirenBoardIP = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblInstance
            // 
            this.lblInstance.AutoSize = true;
            this.lblInstance.Location = new System.Drawing.Point(9, 9);
            this.lblInstance.Name = "lblInstance";
            this.lblInstance.Size = new System.Drawing.Size(51, 15);
            this.lblInstance.TabIndex = 0;
            this.lblInstance.Text = "Instance";
            // 
            // cbInstance
            // 
            this.cbInstance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbInstance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInstance.FormattingEnabled = true;
            this.cbInstance.Location = new System.Drawing.Point(12, 27);
            this.cbInstance.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.cbInstance.Name = "cbInstance";
            this.cbInstance.Size = new System.Drawing.Size(476, 23);
            this.cbInstance.TabIndex = 1;
            this.cbInstance.SelectedIndexChanged += new System.EventHandler(this.cbInstance_SelectedIndexChanged);
            // 
            // lblLine
            // 
            this.lblLine.AutoSize = true;
            this.lblLine.Location = new System.Drawing.Point(9, 60);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(116, 15);
            this.lblLine.TabIndex = 2;
            this.lblLine.Text = "Communication line";
            // 
            // cbLine
            // 
            this.cbLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLine.FormattingEnabled = true;
            this.cbLine.Location = new System.Drawing.Point(12, 78);
            this.cbLine.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.cbLine.Name = "cbLine";
            this.cbLine.Size = new System.Drawing.Size(476, 23);
            this.cbLine.TabIndex = 3;
            this.cbLine.SelectedIndexChanged += new System.EventHandler(this.cbLine_SelectedIndexChanged);
            // 
            // lblWirenBoardIP
            // 
            this.lblWirenBoardIP.AutoSize = true;
            this.lblWirenBoardIP.Location = new System.Drawing.Point(9, 111);
            this.lblWirenBoardIP.Name = "lblWirenBoardIP";
            this.lblWirenBoardIP.Size = new System.Drawing.Size(128, 15);
            this.lblWirenBoardIP.TabIndex = 4;
            this.lblWirenBoardIP.Text = "Wiren Board IP address";
            // 
            // txtWirenBoardIP
            // 
            this.txtWirenBoardIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWirenBoardIP.Location = new System.Drawing.Point(12, 129);
            this.txtWirenBoardIP.Name = "txtWirenBoardIP";
            this.txtWirenBoardIP.Size = new System.Drawing.Size(476, 23);
            this.txtWirenBoardIP.TabIndex = 5;
            // 
            // CtrlLineSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtWirenBoardIP);
            this.Controls.Add(this.lblWirenBoardIP);
            this.Controls.Add(this.cbLine);
            this.Controls.Add(this.lblLine);
            this.Controls.Add(this.cbInstance);
            this.Controls.Add(this.lblInstance);
            this.Name = "CtrlLineSelect";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.Size = new System.Drawing.Size(500, 200);
            this.Load += new System.EventHandler(this.CtrlLineSelect_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblInstance;
        private ComboBox cbInstance;
        private Label lblLine;
        private ComboBox cbLine;
        private Label lblWirenBoardIP;
        private TextBox txtWirenBoardIP;
    }
}
