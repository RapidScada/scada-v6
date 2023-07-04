// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions;
using Scada.Admin.Lang;
using Scada.Admin.Project;
using Scada.Agent;
using Scada.Forms;
using Scada.Lang;
using Scada.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Scada.Admin.Forms
{
    /// <summary>
    /// Represents a registration form.
    /// <para>Представляет форму регистрации.</para>
    /// </summary>
    public partial class FrmRegistration : Form
    {
        /// <summary>
        /// The computer code file name.
        /// </summary>
        public const string CompCodeFileName = "CompCode.txt";

        private readonly IAdminContext adminContext; // the Administrator context
        private readonly ProjectApp projectApp;      // the application containing the registered module
        private readonly string regKeyFileName;      // the registration key file name


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmRegistration()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmRegistration(IAdminContext adminContext, ProjectApp projectApp,
            string productCode, string productName) : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.projectApp = projectApp ?? throw new ArgumentNullException(nameof(projectApp));
            regKeyFileName = Path.Combine(projectApp.ConfigDir, productCode + "_Reg.xml");

            ctrlRegistration.ProductCode = productCode;
            ctrlRegistration.ProductName = productName;
        }


        /// <summary>
        /// Requests a computer code from Agent.
        /// </summary>
        private void RequestCompCode()
        {
            try
            {
                if (adminContext.MainForm.GetAgentClient(false) is IAgentClient agentClient)
                {
                    ctrlRegistration.ComputerCode = AdminPhrases.FileLoading;
                    DateTime newerThan = DateTime.MinValue;
                    ICollection<string> lines;

                    lock (agentClient.SyncRoot)
                    {
                        agentClient.ReadTextFile(
                            new RelativePath(projectApp.ServiceApp, AppFolder.Log, CompCodeFileName),
                            ref newerThan, out lines);
                    }

                    ctrlRegistration.ComputerCode = lines != null && lines.Count > 0
                        ? lines.First()
                        : CommonPhrases.NoData;
                }
                else
                {
                    ctrlRegistration.ComputerCode = AdminPhrases.AgentNotEnabled;
                }
            }
            catch (Exception ex)
            {
                ctrlRegistration.ComputerCode = ex.Message;
            }
        }

        /// <summary>
        /// Loads a registration key.
        /// </summary>
        private void LoadRegKey()
        {
            try
            {
                if (File.Exists(regKeyFileName))
                {
                    XmlDocument xmlDoc = new();
                    xmlDoc.Load(regKeyFileName);
                    ctrlRegistration.RegistrationKey = xmlDoc.DocumentElement.Name == "RegKey" ?
                        xmlDoc.DocumentElement.InnerText.Trim() : "";
                }
            }
            catch (Exception ex)
            {
                ScadaUiUtils.ShowError(ScadaUtils.BuildErrorMessage(ex, AdminPhrases.LoadRegKeyError));
            }
        }

        /// <summary>
        /// Saves the registration key.
        /// </summary>
        private bool SaveRegKey()
        {
            try
            {
                XmlDocument xmlDoc = new();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement xmlElem = xmlDoc.CreateElement("RegKey");
                xmlDoc.AppendChild(xmlElem);
                xmlElem.InnerText = ctrlRegistration.RegistrationKey.Trim();

                xmlDoc.Save(regKeyFileName);
                return true;
            }
            catch (Exception ex)
            {
                ScadaUiUtils.ShowError(ScadaUtils.BuildErrorMessage(ex, AdminPhrases.SaveRegKeyError));
                return false;
            }
        }


        private async void FrmRegistration_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlRegistration, ctrlRegistration.GetType().FullName);
            LoadRegKey();
            ctrlRegistration.SetFocus();
            await Task.Run(() => RequestCompCode());
        }

        private async void ctrlRegistration_RefreshCompCode(object sender, EventArgs e)
        {
            await Task.Run(() => RequestCompCode());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveRegKey())
                DialogResult = DialogResult.OK;
        }
    }
}
