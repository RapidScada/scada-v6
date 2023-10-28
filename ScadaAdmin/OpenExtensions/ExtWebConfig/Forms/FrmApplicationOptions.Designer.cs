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
            lbTabs = new ListBox();
            ctrlDisplayOptions = new Control.CtrlDisplayOptions();
            ctrlGeneralOptions = new Control.CtrlGeneralOptions();
            ctrlLoginOptions = new Control.CtrlLoginOptions();
            ctrlPluginAssignment = new Control.CtrlPluginAssignment();
            ctrlConnectionOptions = new Control.CtrlConnectionOptions();
            SuspendLayout();
            // 
            // lbTabs
            // 
            lbTabs.BorderStyle = BorderStyle.None;
            lbTabs.Dock = DockStyle.Left;
            lbTabs.DrawMode = DrawMode.OwnerDrawVariable;
            lbTabs.FormattingEnabled = true;
            lbTabs.IntegralHeight = false;
            lbTabs.ItemHeight = 25;
            lbTabs.Items.AddRange(new object[] { "General Options", "Connection Options", "Login Options", "Display Options", "Plugin Assigment" });
            lbTabs.Location = new Point(0, 0);
            lbTabs.Name = "lbTabs";
            lbTabs.Size = new Size(150, 636);
            lbTabs.TabIndex = 0;
            lbTabs.DrawItem += lbTabs_DrawItem;
            lbTabs.MeasureItem += lbTabs_MeasureItem;
            lbTabs.SelectedIndexChanged += lbTabs_SelectedIndexChanged;
            // 
            // ctrlDisplayOptions
            // 
            ctrlDisplayOptions.Location = new Point(159, 14);
            ctrlDisplayOptions.Name = "ctrlDisplayOptions";
            ctrlDisplayOptions.Size = new Size(550, 227);
            ctrlDisplayOptions.TabIndex = 2;
            ctrlDisplayOptions.OptionsChanged += control_Changed;
            // 
            // ctrlGeneralOptions
            // 
            ctrlGeneralOptions.Location = new Point(159, 14);
            ctrlGeneralOptions.Name = "ctrlGeneralOptions";
            ctrlGeneralOptions.Size = new Size(550, 283);
            ctrlGeneralOptions.TabIndex = 3;
            ctrlGeneralOptions.OptionsChanged += control_Changed;
            // 
            // ctrlLoginOptions
            // 
            ctrlLoginOptions.Location = new Point(159, 14);
            ctrlLoginOptions.Name = "ctrlLoginOptions";
            ctrlLoginOptions.Size = new Size(550, 374);
            ctrlLoginOptions.TabIndex = 4;
            ctrlLoginOptions.OptionsChanged += control_Changed;
            // 
            // ctrlPluginAssignment
            // 
            ctrlPluginAssignment.Location = new Point(159, 14);
            ctrlPluginAssignment.Name = "ctrlPluginAssignment";
            ctrlPluginAssignment.Size = new Size(550, 227);
            ctrlPluginAssignment.TabIndex = 5;
            ctrlPluginAssignment.OptionsChanged += control_Changed;
            // 
            // ctrlConnectionOptions
            // 
            ctrlConnectionOptions.Location = new Point(159, 14);
            ctrlConnectionOptions.Name = "ctrlConnectionOptions";
            ctrlConnectionOptions.Size = new Size(550, 453);
            ctrlConnectionOptions.TabIndex = 1;
            ctrlConnectionOptions.OptionsChanged += control_Changed;
            // 
            // FrmApplicationOptions
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(734, 636);
            Controls.Add(ctrlConnectionOptions);
            Controls.Add(ctrlPluginAssignment);
            Controls.Add(ctrlLoginOptions);
            Controls.Add(ctrlGeneralOptions);
            Controls.Add(ctrlDisplayOptions);
            Controls.Add(lbTabs);
            Name = "FrmApplicationOptions";
            Text = "Application Options";
            Load += FrmApplicationOptions_Load;
            ResumeLayout(false);
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