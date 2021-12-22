
namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    partial class FrmLineConfig
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
            this.ctrlLineMain = new Scada.Admin.Extensions.ExtCommConfig.Controls.CtrlLineMain();
            this.ctrlLineCustom = new Scada.Admin.Extensions.ExtCommConfig.Controls.CtrlLineCustom();
            this.ctrlLinePolling = new Scada.Admin.Extensions.ExtCommConfig.Controls.CtrlLinePolling();
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
            "Main Options",
            "Custom Options",
            "Device Polling"});
            this.lbTabs.Location = new System.Drawing.Point(0, 0);
            this.lbTabs.Name = "lbTabs";
            this.lbTabs.Size = new System.Drawing.Size(150, 561);
            this.lbTabs.TabIndex = 0;
            this.lbTabs.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbTabs_DrawItem);
            this.lbTabs.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.lbTabs_MeasureItem);
            this.lbTabs.SelectedIndexChanged += new System.EventHandler(this.lbTabs_SelectedIndexChanged);
            // 
            // ctrlLineMain
            // 
            this.ctrlLineMain.Location = new System.Drawing.Point(159, 12);
            this.ctrlLineMain.Name = "ctrlLineMain";
            this.ctrlLineMain.Size = new System.Drawing.Size(550, 550);
            this.ctrlLineMain.TabIndex = 1;
            this.ctrlLineMain.Visible = false;
            this.ctrlLineMain.ConfigChanged += new System.EventHandler(this.ctrlLineMain_ConfigChanged);
            // 
            // ctrlLineCustom
            // 
            this.ctrlLineCustom.Location = new System.Drawing.Point(150, 0);
            this.ctrlLineCustom.Name = "ctrlLineCustom";
            this.ctrlLineCustom.Size = new System.Drawing.Size(550, 450);
            this.ctrlLineCustom.TabIndex = 2;
            this.ctrlLineCustom.Visible = false;
            this.ctrlLineCustom.OptionsChanged += new System.EventHandler(this.ctrlLineCustom_OptionsChanged);
            // 
            // ctrlLinePolling
            // 
            this.ctrlLinePolling.Location = new System.Drawing.Point(150, 0);
            this.ctrlLinePolling.Name = "ctrlLinePolling";
            this.ctrlLinePolling.Size = new System.Drawing.Size(700, 550);
            this.ctrlLinePolling.TabIndex = 3;
            this.ctrlLinePolling.Visible = false;
            this.ctrlLinePolling.ConfigChanged += new System.EventHandler(this.ctrlLinePolling_ConfigChanged);
            this.ctrlLinePolling.LineConfigChanged += new System.EventHandler(this.ctrlLinePolling_LineConfigChanged);
            // 
            // FrmLineConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 561);
            this.Controls.Add(this.ctrlLinePolling);
            this.Controls.Add(this.ctrlLineCustom);
            this.Controls.Add(this.ctrlLineMain);
            this.Controls.Add(this.lbTabs);
            this.Name = "FrmLineConfig";
            this.Text = "Line Options";
            this.Load += new System.EventHandler(this.FrmLineOptions_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbTabs;
        private Controls.CtrlLineMain ctrlLineMain;
        private Controls.CtrlLineCustom ctrlLineCustom;
        private Controls.CtrlLinePolling ctrlLinePolling;
    }
}