﻿namespace Scada.Admin.Extensions.ExtServerConfig.Forms
{
    partial class FrmModules
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
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnProperties = new System.Windows.Forms.Button();
            this.btnDeactivate = new System.Windows.Forms.Button();
            this.btnActivate = new System.Windows.Forms.Button();
            this.lbActiveModules = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lbUnusedModules = new System.Windows.Forms.ListBox();
            this.pnlTopLeft = new System.Windows.Forms.Panel();
            this.lblUnusedModules = new System.Windows.Forms.Label();
            this.pnlTopRight = new System.Windows.Forms.Panel();
            this.btnRegister = new System.Windows.Forms.Button();
            this.lblActiveModules = new System.Windows.Forms.Label();
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
            this.txtDescr.TabIndex = 2;
            // 
            // lblDescr
            // 
            this.lblDescr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDescr.AutoSize = true;
            this.lblDescr.Location = new System.Drawing.Point(9, 301);
            this.lblDescr.Name = "lblDescr";
            this.lblDescr.Size = new System.Drawing.Size(67, 15);
            this.lblDescr.TabIndex = 1;
            this.lblDescr.Text = "Description";
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
            // lbActiveModules
            // 
            this.lbActiveModules.ColumnWidth = 150;
            this.lbActiveModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbActiveModules.HorizontalScrollbar = true;
            this.lbActiveModules.IntegralHeight = false;
            this.lbActiveModules.ItemHeight = 15;
            this.lbActiveModules.Location = new System.Drawing.Point(408, 47);
            this.lbActiveModules.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lbActiveModules.MultiColumn = true;
            this.lbActiveModules.Name = "lbActiveModules";
            this.lbActiveModules.Size = new System.Drawing.Size(402, 232);
            this.lbActiveModules.TabIndex = 3;
            this.lbActiveModules.SelectedIndexChanged += new System.EventHandler(this.lbActiveModules_SelectedIndexChanged);
            this.lbActiveModules.DoubleClick += new System.EventHandler(this.lbActiveModules_DoubleClick);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.lbUnusedModules, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.lbActiveModules, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.pnlTopLeft, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.pnlTopRight, 1, 0);
            this.tableLayoutPanel.Location = new System.Drawing.Point(12, 9);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(810, 279);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // lbUnusedModules
            // 
            this.lbUnusedModules.ColumnWidth = 150;
            this.lbUnusedModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbUnusedModules.HorizontalScrollbar = true;
            this.lbUnusedModules.IntegralHeight = false;
            this.lbUnusedModules.ItemHeight = 15;
            this.lbUnusedModules.Location = new System.Drawing.Point(0, 47);
            this.lbUnusedModules.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lbUnusedModules.MultiColumn = true;
            this.lbUnusedModules.Name = "lbUnusedModules";
            this.lbUnusedModules.Size = new System.Drawing.Size(402, 232);
            this.lbUnusedModules.Sorted = true;
            this.lbUnusedModules.TabIndex = 1;
            this.lbUnusedModules.SelectedIndexChanged += new System.EventHandler(this.lbUnusedModules_SelectedIndexChanged);
            this.lbUnusedModules.DoubleClick += new System.EventHandler(this.lbUnusedModules_DoubleClick);
            // 
            // pnlTopLeft
            // 
            this.pnlTopLeft.Controls.Add(this.lblUnusedModules);
            this.pnlTopLeft.Controls.Add(this.btnActivate);
            this.pnlTopLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTopLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlTopLeft.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.pnlTopLeft.Name = "pnlTopLeft";
            this.pnlTopLeft.Size = new System.Drawing.Size(402, 47);
            this.pnlTopLeft.TabIndex = 0;
            // 
            // lblUnusedModules
            // 
            this.lblUnusedModules.AutoSize = true;
            this.lblUnusedModules.Location = new System.Drawing.Point(-3, 0);
            this.lblUnusedModules.Name = "lblUnusedModules";
            this.lblUnusedModules.Size = new System.Drawing.Size(99, 15);
            this.lblUnusedModules.TabIndex = 0;
            this.lblUnusedModules.Text = "Unused modules:";
            // 
            // pnlTopRight
            // 
            this.pnlTopRight.Controls.Add(this.btnRegister);
            this.pnlTopRight.Controls.Add(this.lblActiveModules);
            this.pnlTopRight.Controls.Add(this.btnDeactivate);
            this.pnlTopRight.Controls.Add(this.btnProperties);
            this.pnlTopRight.Controls.Add(this.btnMoveUp);
            this.pnlTopRight.Controls.Add(this.btnMoveDown);
            this.pnlTopRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTopRight.Location = new System.Drawing.Point(408, 0);
            this.pnlTopRight.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.pnlTopRight.Name = "pnlTopRight";
            this.pnlTopRight.Size = new System.Drawing.Size(402, 47);
            this.pnlTopRight.TabIndex = 1;
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
            // lblActiveModules
            // 
            this.lblActiveModules.AutoSize = true;
            this.lblActiveModules.Location = new System.Drawing.Point(-3, 0);
            this.lblActiveModules.Name = "lblActiveModules";
            this.lblActiveModules.Size = new System.Drawing.Size(92, 15);
            this.lblActiveModules.TabIndex = 0;
            this.lblActiveModules.Text = "Active modules:";
            // 
            // FrmModules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 531);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.txtDescr);
            this.Controls.Add(this.lblDescr);
            this.Name = "FrmModules";
            this.Text = "Modules";
            this.Load += new System.EventHandler(this.FrmModules_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            this.pnlTopLeft.ResumeLayout(false);
            this.pnlTopLeft.PerformLayout();
            this.pnlTopRight.ResumeLayout(false);
            this.pnlTopRight.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDescr;
        private System.Windows.Forms.Label lblDescr;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnProperties;
        private System.Windows.Forms.Button btnDeactivate;
        private System.Windows.Forms.Button btnActivate;
        private System.Windows.Forms.ListBox lbActiveModules;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.ListBox lbUnusedModules;
        private System.Windows.Forms.Panel pnlTopLeft;
        private System.Windows.Forms.Panel pnlTopRight;
        private System.Windows.Forms.Label lblUnusedModules;
        private System.Windows.Forms.Label lblActiveModules;
        private System.Windows.Forms.Button btnRegister;
    }
}