using Scada.Admin.Config;
using Scada.Admin.Extensions.ExtImport.Code;
using Scada.Admin.Extensions.ExtImport.Controls;
using Scada.Admin.Project;
using Scada.Comm.Devices;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Forms;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtImport.Forms
{
    public partial class FrmCnlImport : Form
    {
        private readonly IAdminContext adminContext;
        private readonly ScadaProject project;
        private readonly RecentSelection recentSelection;
        private int step;

        private FrmCnlImport()
        {
            InitializeComponent();
        }

        public FrmCnlImport(IAdminContext adminContext, ScadaProject project, RecentSelection recentSelection)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.recentSelection = recentSelection ?? throw new ArgumentNullException(nameof(recentSelection));
            step = 1;
        }


        private void ApplyStep(int offset)
        {
            step += offset;

            if (step < 1)
                step = 1;
            else if (step > 3)
                step = 3;

            CtrlCnlImport1.Visible = false;
            CtrlCnlImport2.Visible = false;
            CtrlCnlImport3.Visible = false;
            btnBack.Visible = false;
            btnNext.Visible = false;
            btnCreate.Visible = false;

            switch (step)
            {
                case 1:
                    lblStep.Text = ExtensionPhrases.ImportCnlsStep1 ?? "Import Cnls Step1";
                    CtrlCnlImport1.Visible = true;
                    btnNext.Visible = true;

                    CtrlCnlImport1.SetFocus();
                    btnNext.Enabled = CtrlCnlImport1.StatusOK;
                    break;
                case 2:
                    lblStep.Text = ExtensionPhrases.ImportCnlsStep2 ?? "Import Cnls Step2";
                    CtrlCnlImport2.Visible = true;
                    btnBack.Visible = true;
                    btnNext.Visible = true;

                    CtrlCnlImport2.DeviceName = CtrlCnlImport1.SelectedDevice?.Name;
                    CtrlCnlImport2.SetFocus();

                    break;
                case 3:
                    lblStep.Text = ExtensionPhrases.ImportCnlsStep3 ?? "Import Cnls Step3";
                    CtrlCnlImport3.Visible = true;
                    btnCreate.Visible = true;
                    btnCreate.Enabled = CtrlCnlImport3.FileSelected;
                    btnBack.Visible = true;

					if (CtrlCnlImport1.StatusOK)
					{
						CtrlCnlImport3.ResetCnlNums(CtrlCnlImport1.CnlPrototypes.Count);
						
					}
					
					break;
            }
        }
       
     

        private void FrmCnlImport_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(CtrlCnlImport1, CtrlCnlImport1.GetType().FullName);
            FormTranslator.Translate(CtrlCnlImport2, CtrlCnlImport2.GetType().FullName);
            FormTranslator.Translate(CtrlCnlImport3, CtrlCnlImport3.GetType().FullName);

            CtrlCnlImport1.Init(adminContext, project, recentSelection);
            CtrlCnlImport2.Init(project, recentSelection);
            CtrlCnlImport3.Init(adminContext, project);

            ApplyStep(0);
        }
        private void CtrlCnlImport3_SelectedDeviceChanged(object sender, EventArgs e)
        {
            btnCreate.Enabled = CtrlCnlImport3.FileSelected;
        }
        private void ctrlCnlCreate1_SelectedDeviceChanged(object sender, EventArgs e)
        {
            if (step == 1)
                btnNext.Enabled = CtrlCnlImport1.StatusOK;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            ApplyStep(-1);
        }


        private void btnNext_Click(object sender, EventArgs e)
        {
            ApplyStep(1);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (CtrlCnlImport1.StatusOK)
            {

                int? objNum = CtrlCnlImport2.ObjNum;
                int deviceNum = CtrlCnlImport1.SelectedDevice.DeviceNum;

                Dictionary<string, object> deviceInfoDict = new Dictionary<string, object>();
                deviceInfoDict.Add("cnlNum", CtrlCnlImport3.StartCnlNum);
                deviceInfoDict.Add("deviceName", CtrlCnlImport1.SelectedDevice.Name);
                deviceInfoDict.Add("objNum", objNum);
                deviceInfoDict.Add("deviceNum", deviceNum);
                deviceInfoDict.Add("Prototypes", CtrlCnlImport1.CnlPrototypes);


                //remove cnls
                //if (new FrmCnlMerge(project, deviceInfoDict, CtrlCnlImport3._dictio, CtrlCnlImport3).ShowDialog() == DialogResult.OK)
                //{

                //    DialogResult = DialogResult.OK;
                //}
            }
        }
    }
}


