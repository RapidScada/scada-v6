namespace Scada.Admin.Extensions.ExtWebConfig.Forms
{
    partial class FrmApplicationOptions
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
            this.lbTabs = new System.Windows.Forms.ListBox();
            this.ctrlDisplayOptions = new Scada.Admin.Extensions.ExtWebConfig.Control.CtrlDisplayOptions();
            this.ctrlGeneralOptions = new Scada.Admin.Extensions.ExtWebConfig.Control.CtrlGeneralOptions();
            this.ctrlLoginOptions = new Scada.Admin.Extensions.ExtWebConfig.Control.CtrlLoginOptions();
            this.ctrlPluginAssignment = new Scada.Admin.Extensions.ExtWebConfig.Control.CtrlPluginAssignment();
            this.ctrlConnectionOptions = new Scada.Admin.Extensions.ExtWebConfig.Control.CtrlConnectionOptions();
            this.SuspendLayout();
            // 
            // lbTabs
            // 
            this.lbTabs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbTabs.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbTabs.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbTabs.FormattingEnabled = true;
            this.lbTabs.IntegralHeight = false;
            this.lbTabs.ItemHeight = 25;
            this.lbTabs.Items.AddRange(new object[] {
            "General Options",
            "Connection Options",
            "Login Options",
            "Display Options",
            "Plugin Assignment"});
            this.lbTabs.Location = new System.Drawing.Point(0, 0);
            this.lbTabs.Name = "lbTabs";
            this.lbTabs.Size = new System.Drawing.Size(150, 561);
            this.lbTabs.TabIndex = 0;
            this.lbTabs.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbTabs_DrawItem);
            this.lbTabs.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.lbTabs_MeasureItem);
            this.lbTabs.SelectedIndexChanged += new System.EventHandler(this.lbTabs_SelectedIndexChanged);
            // 
            // ctrlDisplayOptions
            // 
            this.ctrlDisplayOptions.Location = new System.Drawing.Point(159, 12);
            this.ctrlDisplayOptions.Name = "ctrlDisplayOptions";
            this.ctrlDisplayOptions.Size = new System.Drawing.Size(550, 200);
            this.ctrlDisplayOptions.TabIndex = 2;
            this.ctrlDisplayOptions.OptionsChanged += new System.EventHandler(this.control_Changed);
            // 
            // ctrlGeneralOptions
            // 
            this.ctrlGeneralOptions.Location = new System.Drawing.Point(159, 12);
            this.ctrlGeneralOptions.Name = "ctrlGeneralOptions";
            this.ctrlGeneralOptions.Size = new System.Drawing.Size(550, 250);
            this.ctrlGeneralOptions.TabIndex = 3;
            this.ctrlGeneralOptions.OptionsChanged += new System.EventHandler(this.control_Changed);
            // 
            // ctrlLoginOptions
            // 
            this.ctrlLoginOptions.Location = new System.Drawing.Point(159, 12);
            this.ctrlLoginOptions.Name = "ctrlLoginOptions";
            this.ctrlLoginOptions.Size = new System.Drawing.Size(550, 200);
            this.ctrlLoginOptions.TabIndex = 4;
            this.ctrlLoginOptions.OptionsChanged += new System.EventHandler(this.control_Changed);
            // 
            // ctrlPluginAssignment
            // 
            this.ctrlPluginAssignment.Location = new System.Drawing.Point(159, 12);
            this.ctrlPluginAssignment.Name = "ctrlPluginAssignment";
            this.ctrlPluginAssignment.Size = new System.Drawing.Size(550, 200);
            this.ctrlPluginAssignment.TabIndex = 5;
            this.ctrlPluginAssignment.OptionsChanged += new System.EventHandler(this.control_Changed);
            // 
            // ctrlConnectionOptions
            // 
            this.ctrlConnectionOptions.Location = new System.Drawing.Point(159, 12);
            this.ctrlConnectionOptions.Name = "ctrlConnectionOptions";
            this.ctrlConnectionOptions.Size = new System.Drawing.Size(550, 400);
            this.ctrlConnectionOptions.TabIndex = 1;
            this.ctrlConnectionOptions.OptionsChanged += new System.EventHandler(this.control_Changed);
            // 
            // FrmApplicationOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 561);
            this.Controls.Add(this.ctrlConnectionOptions);
            this.Controls.Add(this.ctrlPluginAssignment);
            this.Controls.Add(this.ctrlLoginOptions);
            this.Controls.Add(this.ctrlGeneralOptions);
            this.Controls.Add(this.ctrlDisplayOptions);
            this.Controls.Add(this.lbTabs);
            this.Name = "FrmApplicationOptions";
            this.Text = "Application Options";
            this.Load += new System.EventHandler(this.FrmApplicationOptions_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ListBox lbTabs;
        private Control.CtrlDisplayOptions ctrlDisplayOptions;
        private Control.CtrlGeneralOptions ctrlGeneralOptions;
        private Control.CtrlLoginOptions ctrlLoginOptions;
        private Control.CtrlPluginAssignment ctrlPluginAssignment;
        private Control.CtrlConnectionOptions ctrlConnectionOptions;
    }
}