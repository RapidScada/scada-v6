
namespace Scada.Admin.App.Forms.Tables
{
    partial class FrmCnl
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
            tabControl = new System.Windows.Forms.TabControl();
            pageGeneral = new System.Windows.Forms.TabPage();
            txtOutFormula = new System.Windows.Forms.TextBox();
            lblOutFormula = new System.Windows.Forms.Label();
            txtInFormula = new System.Windows.Forms.TextBox();
            lblInFormula = new System.Windows.Forms.Label();
            chkFormulaEnabled = new System.Windows.Forms.CheckBox();
            txtTagCode = new System.Windows.Forms.TextBox();
            lblTagCode = new System.Windows.Forms.Label();
            txtTagNum = new System.Windows.Forms.TextBox();
            lblTagNum = new System.Windows.Forms.Label();
            cbDevice = new System.Windows.Forms.ComboBox();
            txtDeviceNum = new System.Windows.Forms.TextBox();
            lblDevice = new System.Windows.Forms.Label();
            cbObj = new System.Windows.Forms.ComboBox();
            txtObjNum = new System.Windows.Forms.TextBox();
            lblObj = new System.Windows.Forms.Label();
            cbCnlType = new System.Windows.Forms.ComboBox();
            lblCnlType = new System.Windows.Forms.Label();
            txtDataLen = new System.Windows.Forms.TextBox();
            lblDataLen = new System.Windows.Forms.Label();
            cbDataType = new System.Windows.Forms.ComboBox();
            lblDataType = new System.Windows.Forms.Label();
            txtCode = new System.Windows.Forms.TextBox();
            lblCode = new System.Windows.Forms.Label();
            txtName = new System.Windows.Forms.TextBox();
            lblName = new System.Windows.Forms.Label();
            txtCnlNum = new System.Windows.Forms.TextBox();
            lblCnlNum = new System.Windows.Forms.Label();
            chkActive = new System.Windows.Forms.CheckBox();
            pageDisplay = new System.Windows.Forms.TabPage();
            cbUnit = new System.Windows.Forms.ComboBox();
            lblUnit = new System.Windows.Forms.Label();
            cbQuantity = new System.Windows.Forms.ComboBox();
            lblQuantity = new System.Windows.Forms.Label();
            cbOutFormat = new System.Windows.Forms.ComboBox();
            lblOutFormat = new System.Windows.Forms.Label();
            cbFormat = new System.Windows.Forms.ComboBox();
            lblFormat = new System.Windows.Forms.Label();
            pageLim = new System.Windows.Forms.TabPage();
            txtDeadband = new System.Windows.Forms.TextBox();
            lblDeadband = new System.Windows.Forms.Label();
            txtHiHi = new System.Windows.Forms.TextBox();
            lblHiHi = new System.Windows.Forms.Label();
            txtHigh = new System.Windows.Forms.TextBox();
            lblHigh = new System.Windows.Forms.Label();
            txtLow = new System.Windows.Forms.TextBox();
            lblLow = new System.Windows.Forms.Label();
            txtLoLo = new System.Windows.Forms.TextBox();
            lblLoLo = new System.Windows.Forms.Label();
            chkShared = new System.Windows.Forms.CheckBox();
            btnCreateLim = new System.Windows.Forms.Button();
            cbLim = new System.Windows.Forms.ComboBox();
            lblLim = new System.Windows.Forms.Label();
            pageArchives = new System.Windows.Forms.TabPage();
            bmArchive = new Scada.Forms.Controls.CtrlBitmask();
            pageEvents = new System.Windows.Forms.TabPage();
            bmEvent = new Scada.Forms.Controls.CtrlBitmask();
            pnlBottom = new System.Windows.Forms.Panel();
            btnCancel = new System.Windows.Forms.Button();
            btnOK = new System.Windows.Forms.Button();
            tabControl.SuspendLayout();
            pageGeneral.SuspendLayout();
            pageDisplay.SuspendLayout();
            pageLim.SuspendLayout();
            pageArchives.SuspendLayout();
            pageEvents.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(pageGeneral);
            tabControl.Controls.Add(pageDisplay);
            tabControl.Controls.Add(pageLim);
            tabControl.Controls.Add(pageArchives);
            tabControl.Controls.Add(pageEvents);
            tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl.Location = new System.Drawing.Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(484, 454);
            tabControl.TabIndex = 0;
            // 
            // pageGeneral
            // 
            pageGeneral.Controls.Add(txtOutFormula);
            pageGeneral.Controls.Add(lblOutFormula);
            pageGeneral.Controls.Add(txtInFormula);
            pageGeneral.Controls.Add(lblInFormula);
            pageGeneral.Controls.Add(chkFormulaEnabled);
            pageGeneral.Controls.Add(txtTagCode);
            pageGeneral.Controls.Add(lblTagCode);
            pageGeneral.Controls.Add(txtTagNum);
            pageGeneral.Controls.Add(lblTagNum);
            pageGeneral.Controls.Add(cbDevice);
            pageGeneral.Controls.Add(txtDeviceNum);
            pageGeneral.Controls.Add(lblDevice);
            pageGeneral.Controls.Add(cbObj);
            pageGeneral.Controls.Add(txtObjNum);
            pageGeneral.Controls.Add(lblObj);
            pageGeneral.Controls.Add(cbCnlType);
            pageGeneral.Controls.Add(lblCnlType);
            pageGeneral.Controls.Add(txtDataLen);
            pageGeneral.Controls.Add(lblDataLen);
            pageGeneral.Controls.Add(cbDataType);
            pageGeneral.Controls.Add(lblDataType);
            pageGeneral.Controls.Add(txtCode);
            pageGeneral.Controls.Add(lblCode);
            pageGeneral.Controls.Add(txtName);
            pageGeneral.Controls.Add(lblName);
            pageGeneral.Controls.Add(txtCnlNum);
            pageGeneral.Controls.Add(lblCnlNum);
            pageGeneral.Controls.Add(chkActive);
            pageGeneral.Location = new System.Drawing.Point(4, 24);
            pageGeneral.Name = "pageGeneral";
            pageGeneral.Padding = new System.Windows.Forms.Padding(5);
            pageGeneral.Size = new System.Drawing.Size(476, 426);
            pageGeneral.TabIndex = 0;
            pageGeneral.Text = "General";
            pageGeneral.UseVisualStyleBackColor = true;
            // 
            // txtOutFormula
            // 
            txtOutFormula.Location = new System.Drawing.Point(38, 395);
            txtOutFormula.Name = "txtOutFormula";
            txtOutFormula.Size = new System.Drawing.Size(430, 23);
            txtOutFormula.TabIndex = 27;
            // 
            // lblOutFormula
            // 
            lblOutFormula.AutoSize = true;
            lblOutFormula.Location = new System.Drawing.Point(5, 399);
            lblOutFormula.Name = "lblOutFormula";
            lblOutFormula.Size = new System.Drawing.Size(27, 15);
            lblOutFormula.TabIndex = 26;
            lblOutFormula.Text = "Out";
            // 
            // txtInFormula
            // 
            txtInFormula.Location = new System.Drawing.Point(38, 366);
            txtInFormula.Name = "txtInFormula";
            txtInFormula.Size = new System.Drawing.Size(430, 23);
            txtInFormula.TabIndex = 25;
            // 
            // lblInFormula
            // 
            lblInFormula.AutoSize = true;
            lblInFormula.Location = new System.Drawing.Point(5, 370);
            lblInFormula.Name = "lblInFormula";
            lblInFormula.Size = new System.Drawing.Size(17, 15);
            lblInFormula.TabIndex = 24;
            lblInFormula.Text = "In";
            // 
            // chkFormulaEnabled
            // 
            chkFormulaEnabled.AutoSize = true;
            chkFormulaEnabled.Location = new System.Drawing.Point(8, 341);
            chkFormulaEnabled.Name = "chkFormulaEnabled";
            chkFormulaEnabled.Size = new System.Drawing.Size(70, 19);
            chkFormulaEnabled.TabIndex = 23;
            chkFormulaEnabled.Text = "Formula";
            chkFormulaEnabled.UseVisualStyleBackColor = true;
            chkFormulaEnabled.CheckedChanged += chkFormulaEnabled_CheckedChanged;
            // 
            // txtTagCode
            // 
            txtTagCode.Location = new System.Drawing.Point(114, 312);
            txtTagCode.Name = "txtTagCode";
            txtTagCode.Size = new System.Drawing.Size(354, 23);
            txtTagCode.TabIndex = 22;
            // 
            // lblTagCode
            // 
            lblTagCode.AutoSize = true;
            lblTagCode.Location = new System.Drawing.Point(111, 294);
            lblTagCode.Name = "lblTagCode";
            lblTagCode.Size = new System.Drawing.Size(54, 15);
            lblTagCode.TabIndex = 21;
            lblTagCode.Text = "Tag code";
            // 
            // txtTagNum
            // 
            txtTagNum.Location = new System.Drawing.Point(8, 312);
            txtTagNum.Name = "txtTagNum";
            txtTagNum.Size = new System.Drawing.Size(100, 23);
            txtTagNum.TabIndex = 20;
            // 
            // lblTagNum
            // 
            lblTagNum.AutoSize = true;
            lblTagNum.Location = new System.Drawing.Point(5, 294);
            lblTagNum.Name = "lblTagNum";
            lblTagNum.Size = new System.Drawing.Size(70, 15);
            lblTagNum.TabIndex = 19;
            lblTagNum.Text = "Tag number";
            // 
            // cbDevice
            // 
            cbDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbDevice.FormattingEnabled = true;
            cbDevice.Location = new System.Drawing.Point(114, 268);
            cbDevice.Name = "cbDevice";
            cbDevice.Size = new System.Drawing.Size(354, 23);
            cbDevice.TabIndex = 18;
            cbDevice.SelectedIndexChanged += cbDevice_SelectedIndexChanged;
            // 
            // txtDeviceNum
            // 
            txtDeviceNum.Location = new System.Drawing.Point(8, 268);
            txtDeviceNum.Name = "txtDeviceNum";
            txtDeviceNum.ReadOnly = true;
            txtDeviceNum.Size = new System.Drawing.Size(100, 23);
            txtDeviceNum.TabIndex = 17;
            // 
            // lblDevice
            // 
            lblDevice.AutoSize = true;
            lblDevice.Location = new System.Drawing.Point(5, 250);
            lblDevice.Name = "lblDevice";
            lblDevice.Size = new System.Drawing.Size(42, 15);
            lblDevice.TabIndex = 16;
            lblDevice.Text = "Device";
            // 
            // cbObj
            // 
            cbObj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbObj.FormattingEnabled = true;
            cbObj.Location = new System.Drawing.Point(114, 224);
            cbObj.Name = "cbObj";
            cbObj.Size = new System.Drawing.Size(354, 23);
            cbObj.TabIndex = 15;
            cbObj.SelectedIndexChanged += cbObj_SelectedIndexChanged;
            // 
            // txtObjNum
            // 
            txtObjNum.Location = new System.Drawing.Point(8, 224);
            txtObjNum.Name = "txtObjNum";
            txtObjNum.ReadOnly = true;
            txtObjNum.Size = new System.Drawing.Size(100, 23);
            txtObjNum.TabIndex = 14;
            // 
            // lblObj
            // 
            lblObj.AutoSize = true;
            lblObj.Location = new System.Drawing.Point(5, 206);
            lblObj.Name = "lblObj";
            lblObj.Size = new System.Drawing.Size(42, 15);
            lblObj.TabIndex = 13;
            lblObj.Text = "Object";
            // 
            // cbCnlType
            // 
            cbCnlType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbCnlType.FormattingEnabled = true;
            cbCnlType.Location = new System.Drawing.Point(8, 180);
            cbCnlType.Name = "cbCnlType";
            cbCnlType.Size = new System.Drawing.Size(460, 23);
            cbCnlType.TabIndex = 12;
            // 
            // lblCnlType
            // 
            lblCnlType.AutoSize = true;
            lblCnlType.Location = new System.Drawing.Point(5, 162);
            lblCnlType.Name = "lblCnlType";
            lblCnlType.Size = new System.Drawing.Size(77, 15);
            lblCnlType.TabIndex = 11;
            lblCnlType.Text = "Channel type";
            // 
            // txtDataLen
            // 
            txtDataLen.Location = new System.Drawing.Point(368, 136);
            txtDataLen.Name = "txtDataLen";
            txtDataLen.Size = new System.Drawing.Size(100, 23);
            txtDataLen.TabIndex = 10;
            // 
            // lblDataLen
            // 
            lblDataLen.AutoSize = true;
            lblDataLen.Location = new System.Drawing.Point(365, 118);
            lblDataLen.Name = "lblDataLen";
            lblDataLen.Size = new System.Drawing.Size(68, 15);
            lblDataLen.TabIndex = 9;
            lblDataLen.Text = "Data length";
            // 
            // cbDataType
            // 
            cbDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbDataType.FormattingEnabled = true;
            cbDataType.Location = new System.Drawing.Point(8, 136);
            cbDataType.Name = "cbDataType";
            cbDataType.Size = new System.Drawing.Size(354, 23);
            cbDataType.TabIndex = 8;
            // 
            // lblDataType
            // 
            lblDataType.AutoSize = true;
            lblDataType.Location = new System.Drawing.Point(5, 118);
            lblDataType.Name = "lblDataType";
            lblDataType.Size = new System.Drawing.Size(57, 15);
            lblDataType.TabIndex = 7;
            lblDataType.Text = "Data type";
            // 
            // txtCode
            // 
            txtCode.Location = new System.Drawing.Point(8, 92);
            txtCode.Name = "txtCode";
            txtCode.Size = new System.Drawing.Size(460, 23);
            txtCode.TabIndex = 6;
            // 
            // lblCode
            // 
            lblCode.AutoSize = true;
            lblCode.Location = new System.Drawing.Point(5, 74);
            lblCode.Name = "lblCode";
            lblCode.Size = new System.Drawing.Size(35, 15);
            lblCode.TabIndex = 5;
            lblCode.Text = "Code";
            // 
            // txtName
            // 
            txtName.Location = new System.Drawing.Point(114, 48);
            txtName.Name = "txtName";
            txtName.Size = new System.Drawing.Size(354, 23);
            txtName.TabIndex = 4;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new System.Drawing.Point(113, 30);
            lblName.Name = "lblName";
            lblName.Size = new System.Drawing.Size(39, 15);
            lblName.TabIndex = 3;
            lblName.Text = "Name";
            // 
            // txtCnlNum
            // 
            txtCnlNum.Location = new System.Drawing.Point(8, 48);
            txtCnlNum.Name = "txtCnlNum";
            txtCnlNum.Size = new System.Drawing.Size(100, 23);
            txtCnlNum.TabIndex = 2;
            // 
            // lblCnlNum
            // 
            lblCnlNum.AutoSize = true;
            lblCnlNum.Location = new System.Drawing.Point(5, 30);
            lblCnlNum.Name = "lblCnlNum";
            lblCnlNum.Size = new System.Drawing.Size(51, 15);
            lblCnlNum.TabIndex = 1;
            lblCnlNum.Text = "Number";
            // 
            // chkActive
            // 
            chkActive.AutoSize = true;
            chkActive.Location = new System.Drawing.Point(8, 8);
            chkActive.Name = "chkActive";
            chkActive.Size = new System.Drawing.Size(59, 19);
            chkActive.TabIndex = 0;
            chkActive.Text = "Active";
            chkActive.UseVisualStyleBackColor = true;
            // 
            // pageDisplay
            // 
            pageDisplay.Controls.Add(cbUnit);
            pageDisplay.Controls.Add(lblUnit);
            pageDisplay.Controls.Add(cbQuantity);
            pageDisplay.Controls.Add(lblQuantity);
            pageDisplay.Controls.Add(cbOutFormat);
            pageDisplay.Controls.Add(lblOutFormat);
            pageDisplay.Controls.Add(cbFormat);
            pageDisplay.Controls.Add(lblFormat);
            pageDisplay.Location = new System.Drawing.Point(4, 24);
            pageDisplay.Name = "pageDisplay";
            pageDisplay.Padding = new System.Windows.Forms.Padding(5);
            pageDisplay.Size = new System.Drawing.Size(476, 432);
            pageDisplay.TabIndex = 4;
            pageDisplay.Text = "Display";
            pageDisplay.UseVisualStyleBackColor = true;
            // 
            // cbUnit
            // 
            cbUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbUnit.FormattingEnabled = true;
            cbUnit.Location = new System.Drawing.Point(8, 111);
            cbUnit.Name = "cbUnit";
            cbUnit.Size = new System.Drawing.Size(460, 23);
            cbUnit.TabIndex = 7;
            // 
            // lblUnit
            // 
            lblUnit.AutoSize = true;
            lblUnit.Location = new System.Drawing.Point(5, 93);
            lblUnit.Name = "lblUnit";
            lblUnit.Size = new System.Drawing.Size(29, 15);
            lblUnit.TabIndex = 6;
            lblUnit.Text = "Unit";
            // 
            // cbQuantity
            // 
            cbQuantity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbQuantity.FormattingEnabled = true;
            cbQuantity.Location = new System.Drawing.Point(8, 67);
            cbQuantity.Name = "cbQuantity";
            cbQuantity.Size = new System.Drawing.Size(460, 23);
            cbQuantity.TabIndex = 5;
            // 
            // lblQuantity
            // 
            lblQuantity.AutoSize = true;
            lblQuantity.Location = new System.Drawing.Point(5, 49);
            lblQuantity.Name = "lblQuantity";
            lblQuantity.Size = new System.Drawing.Size(53, 15);
            lblQuantity.TabIndex = 4;
            lblQuantity.Text = "Quantity";
            // 
            // cbOutFormat
            // 
            cbOutFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbOutFormat.FormattingEnabled = true;
            cbOutFormat.Location = new System.Drawing.Point(241, 23);
            cbOutFormat.Name = "cbOutFormat";
            cbOutFormat.Size = new System.Drawing.Size(227, 23);
            cbOutFormat.TabIndex = 3;
            // 
            // lblOutFormat
            // 
            lblOutFormat.AutoSize = true;
            lblOutFormat.Location = new System.Drawing.Point(238, 5);
            lblOutFormat.Name = "lblOutFormat";
            lblOutFormat.Size = new System.Drawing.Size(103, 15);
            lblOutFormat.TabIndex = 2;
            lblOutFormat.Text = "Command format";
            // 
            // cbFormat
            // 
            cbFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbFormat.FormattingEnabled = true;
            cbFormat.Location = new System.Drawing.Point(8, 23);
            cbFormat.Name = "cbFormat";
            cbFormat.Size = new System.Drawing.Size(227, 23);
            cbFormat.TabIndex = 1;
            // 
            // lblFormat
            // 
            lblFormat.AutoSize = true;
            lblFormat.Location = new System.Drawing.Point(5, 5);
            lblFormat.Name = "lblFormat";
            lblFormat.Size = new System.Drawing.Size(45, 15);
            lblFormat.TabIndex = 0;
            lblFormat.Text = "Format";
            // 
            // pageLim
            // 
            pageLim.Controls.Add(txtDeadband);
            pageLim.Controls.Add(lblDeadband);
            pageLim.Controls.Add(txtHiHi);
            pageLim.Controls.Add(lblHiHi);
            pageLim.Controls.Add(txtHigh);
            pageLim.Controls.Add(lblHigh);
            pageLim.Controls.Add(txtLow);
            pageLim.Controls.Add(lblLow);
            pageLim.Controls.Add(txtLoLo);
            pageLim.Controls.Add(lblLoLo);
            pageLim.Controls.Add(chkShared);
            pageLim.Controls.Add(btnCreateLim);
            pageLim.Controls.Add(cbLim);
            pageLim.Controls.Add(lblLim);
            pageLim.Location = new System.Drawing.Point(4, 24);
            pageLim.Name = "pageLim";
            pageLim.Padding = new System.Windows.Forms.Padding(5);
            pageLim.Size = new System.Drawing.Size(476, 432);
            pageLim.TabIndex = 3;
            pageLim.Text = "Limits";
            pageLim.UseVisualStyleBackColor = true;
            // 
            // txtDeadband
            // 
            txtDeadband.Location = new System.Drawing.Point(8, 136);
            txtDeadband.Name = "txtDeadband";
            txtDeadband.ReadOnly = true;
            txtDeadband.Size = new System.Drawing.Size(110, 23);
            txtDeadband.TabIndex = 13;
            // 
            // lblDeadband
            // 
            lblDeadband.AutoSize = true;
            lblDeadband.Location = new System.Drawing.Point(5, 118);
            lblDeadband.Name = "lblDeadband";
            lblDeadband.Size = new System.Drawing.Size(61, 15);
            lblDeadband.TabIndex = 12;
            lblDeadband.Text = "Deadband";
            // 
            // txtHiHi
            // 
            txtHiHi.Location = new System.Drawing.Point(356, 92);
            txtHiHi.Name = "txtHiHi";
            txtHiHi.ReadOnly = true;
            txtHiHi.Size = new System.Drawing.Size(112, 23);
            txtHiHi.TabIndex = 11;
            // 
            // lblHiHi
            // 
            lblHiHi.AutoSize = true;
            lblHiHi.Location = new System.Drawing.Point(353, 74);
            lblHiHi.Name = "lblHiHi";
            lblHiHi.Size = new System.Drawing.Size(86, 15);
            lblHiHi.TabIndex = 10;
            lblHiHi.Text = "High high";
            // 
            // txtHigh
            // 
            txtHigh.Location = new System.Drawing.Point(240, 92);
            txtHigh.Name = "txtHigh";
            txtHigh.ReadOnly = true;
            txtHigh.Size = new System.Drawing.Size(110, 23);
            txtHigh.TabIndex = 9;
            // 
            // lblHigh
            // 
            lblHigh.AutoSize = true;
            lblHigh.Location = new System.Drawing.Point(237, 74);
            lblHigh.Name = "lblHigh";
            lblHigh.Size = new System.Drawing.Size(33, 15);
            lblHigh.TabIndex = 8;
            lblHigh.Text = "High";
            // 
            // txtLow
            // 
            txtLow.Location = new System.Drawing.Point(124, 92);
            txtLow.Name = "txtLow";
            txtLow.ReadOnly = true;
            txtLow.Size = new System.Drawing.Size(110, 23);
            txtLow.TabIndex = 7;
            // 
            // lblLow
            // 
            lblLow.AutoSize = true;
            lblLow.Location = new System.Drawing.Point(121, 74);
            lblLow.Name = "lblLow";
            lblLow.Size = new System.Drawing.Size(29, 15);
            lblLow.TabIndex = 6;
            lblLow.Text = "Low";
            // 
            // txtLoLo
            // 
            txtLoLo.Location = new System.Drawing.Point(8, 92);
            txtLoLo.Name = "txtLoLo";
            txtLoLo.ReadOnly = true;
            txtLoLo.Size = new System.Drawing.Size(110, 23);
            txtLoLo.TabIndex = 5;
            // 
            // lblLoLo
            // 
            lblLoLo.AutoSize = true;
            lblLoLo.Location = new System.Drawing.Point(5, 74);
            lblLoLo.Name = "lblLoLo";
            lblLoLo.Size = new System.Drawing.Size(81, 15);
            lblLoLo.TabIndex = 4;
            lblLoLo.Text = "Low low";
            // 
            // chkShared
            // 
            chkShared.AutoSize = true;
            chkShared.Location = new System.Drawing.Point(8, 52);
            chkShared.Name = "chkShared";
            chkShared.Size = new System.Drawing.Size(151, 19);
            chkShared.TabIndex = 3;
            chkShared.Text = "Show only shared limits";
            chkShared.UseVisualStyleBackColor = true;
            chkShared.CheckedChanged += chkShared_CheckedChanged;
            // 
            // btnCreateLim
            // 
            btnCreateLim.Location = new System.Drawing.Point(393, 23);
            btnCreateLim.Name = "btnCreateLim";
            btnCreateLim.Size = new System.Drawing.Size(75, 23);
            btnCreateLim.TabIndex = 2;
            btnCreateLim.Text = "Create";
            btnCreateLim.UseVisualStyleBackColor = true;
            btnCreateLim.Click += btnCreateLim_Click;
            // 
            // cbLim
            // 
            cbLim.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbLim.FormattingEnabled = true;
            cbLim.Location = new System.Drawing.Point(8, 23);
            cbLim.Name = "cbLim";
            cbLim.Size = new System.Drawing.Size(379, 23);
            cbLim.TabIndex = 1;
            cbLim.SelectedIndexChanged += cbLim_SelectedIndexChanged;
            // 
            // lblLim
            // 
            lblLim.AutoSize = true;
            lblLim.Location = new System.Drawing.Point(5, 5);
            lblLim.Name = "lblLim";
            lblLim.Size = new System.Drawing.Size(34, 15);
            lblLim.TabIndex = 0;
            lblLim.Text = "Limit";
            // 
            // pageArchives
            // 
            pageArchives.Controls.Add(bmArchive);
            pageArchives.Location = new System.Drawing.Point(4, 24);
            pageArchives.Name = "pageArchives";
            pageArchives.Padding = new System.Windows.Forms.Padding(5);
            pageArchives.Size = new System.Drawing.Size(476, 426);
            pageArchives.TabIndex = 1;
            pageArchives.Text = "Archives";
            pageArchives.UseVisualStyleBackColor = true;
            // 
            // bmArchive
            // 
            bmArchive.Location = new System.Drawing.Point(8, 5);
            bmArchive.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            bmArchive.MaskBits = null;
            bmArchive.MaskValue = 0;
            bmArchive.Name = "bmArchive";
            bmArchive.Size = new System.Drawing.Size(460, 413);
            bmArchive.TabIndex = 0;
            // 
            // pageEvents
            // 
            pageEvents.Controls.Add(bmEvent);
            pageEvents.Location = new System.Drawing.Point(4, 24);
            pageEvents.Name = "pageEvents";
            pageEvents.Padding = new System.Windows.Forms.Padding(5);
            pageEvents.Size = new System.Drawing.Size(476, 426);
            pageEvents.TabIndex = 2;
            pageEvents.Text = "Events";
            pageEvents.UseVisualStyleBackColor = true;
            // 
            // bmEvent
            // 
            bmEvent.Location = new System.Drawing.Point(8, 5);
            bmEvent.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            bmEvent.MaskBits = null;
            bmEvent.MaskValue = 0;
            bmEvent.Name = "bmEvent";
            bmEvent.Size = new System.Drawing.Size(460, 413);
            bmEvent.TabIndex = 0;
            // 
            // pnlBottom
            // 
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnOK);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Location = new System.Drawing.Point(0, 454);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(484, 41);
            pnlBottom.TabIndex = 1;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.Location = new System.Drawing.Point(397, 6);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnOK.Location = new System.Drawing.Point(316, 6);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(75, 23);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // FrmCnl
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(484, 495);
            Controls.Add(tabControl);
            Controls.Add(pnlBottom);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCnl";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Channel Properties";
            FormClosed += FrmInCnl_FormClosed;
            Load += FrmInCnl_Load;
            tabControl.ResumeLayout(false);
            pageGeneral.ResumeLayout(false);
            pageGeneral.PerformLayout();
            pageDisplay.ResumeLayout(false);
            pageDisplay.PerformLayout();
            pageLim.ResumeLayout(false);
            pageLim.PerformLayout();
            pageArchives.ResumeLayout(false);
            pageEvents.ResumeLayout(false);
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage pageGeneral;
        private System.Windows.Forms.TabPage pageArchives;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.TabPage pageEvents;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.TextBox txtCnlNum;
        private System.Windows.Forms.Label lblCnlNum;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ComboBox cbDataType;
        private System.Windows.Forms.Label lblDataType;
        private System.Windows.Forms.TextBox txtDataLen;
        private System.Windows.Forms.Label lblDataLen;
        private System.Windows.Forms.ComboBox cbCnlType;
        private System.Windows.Forms.Label lblCnlType;
        private System.Windows.Forms.Label lblObj;
        private System.Windows.Forms.TextBox txtObjNum;
        private System.Windows.Forms.ComboBox cbObj;
        private System.Windows.Forms.ComboBox cbDevice;
        private System.Windows.Forms.TextBox txtDeviceNum;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.TextBox txtTagNum;
        private System.Windows.Forms.Label lblTagNum;
        private System.Windows.Forms.Label lblTagCode;
        private System.Windows.Forms.TextBox txtTagCode;
        private System.Windows.Forms.CheckBox chkFormulaEnabled;
        private System.Windows.Forms.TextBox txtInFormula;
        private Scada.Forms.Controls.CtrlBitmask bmArchive;
        private Scada.Forms.Controls.CtrlBitmask bmEvent;
        private System.Windows.Forms.Label lblInFormula;
        private System.Windows.Forms.TextBox txtOutFormula;
        private System.Windows.Forms.Label lblOutFormula;
        private System.Windows.Forms.TabPage pageLim;
        private System.Windows.Forms.TextBox txtDeadband;
        private System.Windows.Forms.Label lblDeadband;
        private System.Windows.Forms.TextBox txtHiHi;
        private System.Windows.Forms.Label lblHiHi;
        private System.Windows.Forms.TextBox txtHigh;
        private System.Windows.Forms.Label lblHigh;
        private System.Windows.Forms.TextBox txtLow;
        private System.Windows.Forms.Label lblLow;
        private System.Windows.Forms.TextBox txtLoLo;
        private System.Windows.Forms.Label lblLoLo;
        private System.Windows.Forms.CheckBox chkShared;
        private System.Windows.Forms.ComboBox cbLim;
        private System.Windows.Forms.Label lblLim;
        private System.Windows.Forms.Button btnCreateLim;
        private System.Windows.Forms.TabPage pageDisplay;
        private System.Windows.Forms.ComboBox cbUnit;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.ComboBox cbQuantity;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.ComboBox cbFormat;
        private System.Windows.Forms.Label lblFormat;
        private System.Windows.Forms.ComboBox cbOutFormat;
        private System.Windows.Forms.Label lblOutFormat;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblCode;
    }
}