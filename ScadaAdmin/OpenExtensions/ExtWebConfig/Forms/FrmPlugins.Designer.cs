namespace Scada.Admin.Extensions.ExtWebConfig.Forms
{
    partial class FrmPlugins
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtDescr = new System.Windows.Forms.TextBox();
            this.lblDescr = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lbUnusedPlugins = new System.Windows.Forms.ListBox();
            this.lbActivePlugins = new System.Windows.Forms.ListBox();
            this.pnlTopLeft = new System.Windows.Forms.Panel();
            this.lblUnusedPlugins = new System.Windows.Forms.Label();
            this.btnActivate = new System.Windows.Forms.Button();
            this.pnlTopRight = new System.Windows.Forms.Panel();
            this.btnProperties = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.lblActivePlugins = new System.Windows.Forms.Label();
            this.btnDeactivate = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.pnlTopLeft.SuspendLayout();
            this.pnlTopRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDescr
            // 
            this.txtDescr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescr.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtDescr.Location = new System.Drawing.Point(12, 319);
            this.txtDescr.Multiline = true;
            this.txtDescr.Name = "txtDescr";
            this.txtDescr.ReadOnly = true;
            this.txtDescr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescr.Size = new System.Drawing.Size(810, 200);
            this.txtDescr.TabIndex = 0;
            // 
            // lblDescr
            // 
            this.lblDescr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDescr.AutoSize = true;
            this.lblDescr.Location = new System.Drawing.Point(9, 301);
            this.lblDescr.Name = "lblDescr";
            this.lblDescr.Size = new System.Drawing.Size(67, 15);
            this.lblDescr.TabIndex = 2;
            this.lblDescr.Text = "Description";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.lbUnusedPlugins, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.lbActivePlugins, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.pnlTopLeft, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.pnlTopRight, 1, 0);
            this.tableLayoutPanel.Location = new System.Drawing.Point(12, 9);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(810, 279);
            this.tableLayoutPanel.TabIndex = 5;
            // 
            // lbUnusedPlugins
            // 
            this.lbUnusedPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbUnusedPlugins.HorizontalScrollbar = true;
            this.lbUnusedPlugins.IntegralHeight = false;
            this.lbUnusedPlugins.ItemHeight = 15;
            this.lbUnusedPlugins.Location = new System.Drawing.Point(0, 47);
            this.lbUnusedPlugins.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lbUnusedPlugins.MultiColumn = true;
            this.lbUnusedPlugins.Name = "lbUnusedPlugins";
            this.lbUnusedPlugins.Size = new System.Drawing.Size(402, 232);
            this.lbUnusedPlugins.Sorted = true;
            this.lbUnusedPlugins.TabIndex = 2;
            this.lbUnusedPlugins.SelectedIndexChanged += new System.EventHandler(this.lbUnusedPlugins_SelectedIndexChanged);
            this.lbUnusedPlugins.DoubleClick += new System.EventHandler(this.lbUnusedPlugins_DoubleClick);
            // 
            // lbActivePlugins
            // 
            this.lbActivePlugins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbActivePlugins.HorizontalScrollbar = true;
            this.lbActivePlugins.IntegralHeight = false;
            this.lbActivePlugins.ItemHeight = 15;
            this.lbActivePlugins.Location = new System.Drawing.Point(408, 47);
            this.lbActivePlugins.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lbActivePlugins.MultiColumn = true;
            this.lbActivePlugins.Name = "lbActivePlugins";
            this.lbActivePlugins.Size = new System.Drawing.Size(402, 232);
            this.lbActivePlugins.TabIndex = 1;
            this.lbActivePlugins.SelectedIndexChanged += new System.EventHandler(this.lbActivePlugins_SelectedIndexChanged);
            this.lbActivePlugins.DoubleClick += new System.EventHandler(this.lbActivePlugins_DoubleClick);
            // 
            // pnlTopLeft
            // 
            this.pnlTopLeft.Controls.Add(this.lblUnusedPlugins);
            this.pnlTopLeft.Controls.Add(this.btnActivate);
            this.pnlTopLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTopLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlTopLeft.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.pnlTopLeft.Name = "pnlTopLeft";
            this.pnlTopLeft.Size = new System.Drawing.Size(402, 47);
            this.pnlTopLeft.TabIndex = 0;
            // 
            // lblUnusedPlugins
            // 
            this.lblUnusedPlugins.AutoSize = true;
            this.lblUnusedPlugins.Location = new System.Drawing.Point(-3, 0);
            this.lblUnusedPlugins.Name = "lblUnusedPlugins";
            this.lblUnusedPlugins.Size = new System.Drawing.Size(92, 15);
            this.lblUnusedPlugins.TabIndex = 1;
            this.lblUnusedPlugins.Text = "Unused plugins:";
            // 
            // btnActivate
            // 
            this.btnActivate.Location = new System.Drawing.Point(0, 18);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(100, 23);
            this.btnActivate.TabIndex = 1;
            this.btnActivate.Text = "Activate";
            this.btnActivate.UseVisualStyleBackColor = true;
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // pnlTopRight
            // 
            this.pnlTopRight.Controls.Add(this.btnProperties);
            this.pnlTopRight.Controls.Add(this.btnRegister);
            this.pnlTopRight.Controls.Add(this.lblActivePlugins);
            this.pnlTopRight.Controls.Add(this.btnDeactivate);
            this.pnlTopRight.Controls.Add(this.btnMoveUp);
            this.pnlTopRight.Controls.Add(this.btnMoveDown);
            this.pnlTopRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTopRight.Location = new System.Drawing.Point(408, 0);
            this.pnlTopRight.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.pnlTopRight.Name = "pnlTopRight";
            this.pnlTopRight.Size = new System.Drawing.Size(402, 47);
            this.pnlTopRight.TabIndex = 1;
            // 
            // btnProperties
            // 
            this.btnProperties.Location = new System.Drawing.Point(258, 18);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(80, 23);
            this.btnProperties.TabIndex = 4;
            this.btnProperties.Text = "Properties";
            this.btnProperties.UseVisualStyleBackColor = true;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(344, 18);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(90, 23);
            this.btnRegister.TabIndex = 5;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // lblActivePlugins
            // 
            this.lblActivePlugins.AutoSize = true;
            this.lblActivePlugins.Location = new System.Drawing.Point(-3, 0);
            this.lblActivePlugins.Name = "lblActivePlugins";
            this.lblActivePlugins.Size = new System.Drawing.Size(85, 15);
            this.lblActivePlugins.TabIndex = 0;
            this.lblActivePlugins.Text = "Active plugins:";
            // 
            // btnDeactivate
            // 
            this.btnDeactivate.Location = new System.Drawing.Point(0, 18);
            this.btnDeactivate.Name = "btnDeactivate";
            this.btnDeactivate.Size = new System.Drawing.Size(80, 23);
            this.btnDeactivate.TabIndex = 1;
            this.btnDeactivate.Text = "Deactivate";
            this.btnDeactivate.UseVisualStyleBackColor = true;
            this.btnDeactivate.Click += new System.EventHandler(this.btnDeactivate_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Location = new System.Drawing.Point(86, 18);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(80, 23);
            this.btnMoveUp.TabIndex = 2;
            this.btnMoveUp.Text = "Move Up";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Location = new System.Drawing.Point(172, 18);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(80, 23);
            this.btnMoveDown.TabIndex = 3;
            this.btnMoveDown.Text = "Move Down";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // FrmPlugins
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 531);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.lblDescr);
            this.Controls.Add(this.txtDescr);
            this.Name = "FrmPlugins";
            this.Text = "Plugins";
            this.Load += new System.EventHandler(this.FrmPlugins_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            this.pnlTopLeft.ResumeLayout(false);
            this.pnlTopLeft.PerformLayout();
            this.pnlTopRight.ResumeLayout(false);
            this.pnlTopRight.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtDescr;
        private Label lblDescr;
        private TableLayoutPanel tableLayoutPanel;
        private ListBox lbUnusedPlugins;
        private ListBox lbActivePlugins;
        private Panel pnlTopLeft;
        private Label lblUnusedPlugins;
        private Button btnActivate;
        private Panel pnlTopRight;
        private Button btnRegister;
        private Label lblActivePlugins;
        private Button btnDeactivate;
        private Button btnMoveUp;
        private Button btnMoveDown;
        private Button btnProperties;
    }
}