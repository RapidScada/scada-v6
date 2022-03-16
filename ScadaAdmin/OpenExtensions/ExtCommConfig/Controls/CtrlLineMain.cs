// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
using Scada.Admin.Project;
using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.Drivers;
using Scada.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Controls
{
    /// <summary>
    /// Represents a control for editing main communication line options.
    /// <para>Представляет элемент управления для редактирования основных параметров линии связи.</para>
    /// </summary>
    public partial class CtrlLineMain : UserControl
    {
        /// <summary>
        /// Represents a communication channel type.
        /// </summary>
        private class ChannelTypeItem
        {
            public string TypeCode { get; set; }
            public string TypeName { get; set; }
            public string Driver { get; set; }
            public bool IsEmpty => string.IsNullOrEmpty(Driver);
            public override string ToString() => TypeName;
        }

        private static List<ChannelTypeItem> sharedChannelTypes = null; // the channel types shared between lines
        private IAdminContext adminContext;      // the Administrator context
        private CommApp commApp;                 // the Communicator application in a project
        private bool changing;                   // controls are being changed programmatically
        private ChannelConfig channelConfigCopy; // the edited copy of the communication channel configuration


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlLineMain()
        {
            InitializeComponent();
            adminContext = null;
            commApp = null;
            changing = false;
            channelConfigCopy = null;
        }


        /// <summary>
        /// Validates that the control is initialized.
        /// </summary>
        private void ValidateInit()
        {
            if (adminContext == null)
                throw new InvalidOperationException("Administrator context must not be null.");

            if (commApp == null)
                throw new InvalidOperationException("Communicator application must not be null.");
        }

        /// <summary>
        /// Fills the list of the channel types.
        /// </summary>
        private void FillChannelTypeList()
        {
            // fill shared list
            if (sharedChannelTypes == null)
            {
                ValidateInit();
                sharedChannelTypes = new List<ChannelTypeItem>();
                DirectoryInfo dirInfo = new(adminContext.AppDirs.LibDir);

                sharedChannelTypes.Add(new ChannelTypeItem
                {
                    TypeCode = "",
                    TypeName = ExtensionPhrases.UndefinedChannelType,
                    Driver = ""
                });

                if (dirInfo.Exists)
                {
                    foreach (FileInfo fileInfo in
                        dirInfo.EnumerateFiles("DrvCnl*.View.dll", SearchOption.TopDirectoryOnly))
                    {
                        string driverCode = ScadaUtils.RemoveFileNameSuffixes(fileInfo.Name);

                        if (ExtensionUtils.GetDriverView(adminContext, commApp, driverCode, 
                            out DriverView driverView, out string message))
                        {
                            if (driverView.CanCreateChannel && 
                                driverView.ChannelTypes is ICollection<ChannelTypeName> channelTypeNames)
                            {
                                foreach (ChannelTypeName channelTypeName in channelTypeNames)
                                {
                                    sharedChannelTypes.Add(new ChannelTypeItem
                                    {
                                        TypeCode = channelTypeName.Code,
                                        TypeName = channelTypeName.Name,
                                        Driver = driverCode
                                    });
                                }
                            }
                        }
                        else
                        {
                            adminContext.ErrLog.WriteError(message);
                        }
                    }
                }
            }

            // fill combo box
            try
            {
                cbChannelType.BeginUpdate();
                cbChannelType.Items.Clear();

                foreach (ChannelTypeItem item in sharedChannelTypes.OrderBy(x => x.IsEmpty ? "" : x.TypeName))
                {
                    cbChannelType.Items.Add(item);
                }
            }
            finally
            {
                cbChannelType.EndUpdate();
            }
        }

        /// <summary>
        /// Selects the list box item corresponding to the specified arguments.
        /// </summary>
        private void SelectChannelType(string typeCode, string driver)
        {
            bool found = false;
            int itemIndex = 0;

            foreach (ChannelTypeItem item in cbChannelType.Items)
            {
                if (item.TypeCode == typeCode && item.Driver == driver)
                {
                    cbChannelType.SelectedIndex = itemIndex;
                    found = true;
                    break;
                }

                itemIndex++;
            }

            // add new item if not found
            if (!found)
            {
                cbChannelType.SelectedIndex = cbChannelType.Items.Add(new ChannelTypeItem
                {
                    TypeCode = typeCode,
                    TypeName = typeCode + ", " + driver,
                    Driver = driver
                });
            }
        }

        /// <summary>
        /// Raises a ConfigChanged event.
        /// </summary>
        private void OnConfigChanged()
        {
            ConfigChanged?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// Initializes the control.
        /// </summary>
        public void Init(IAdminContext adminContext, CommApp commApp)
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.commApp = commApp ?? throw new ArgumentNullException(nameof(commApp));
        }

        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        public void ConfigToControls(LineConfig lineConfig)
        {
            if (lineConfig == null)
                throw new ArgumentNullException(nameof(lineConfig));

            changing = true;

            chkActive.Checked = lineConfig.Active;
            chkIsBound.Checked = lineConfig.IsBound;
            numCommLineNum.SetValue(lineConfig.CommLineNum);
            txtName.Text = lineConfig.Name;

            LineOptions lineOptions = lineConfig.LineOptions;
            numReqRetries.SetValue(lineOptions.ReqRetries);
            numCycleDelay.SetValue(lineOptions.CycleDelay);
            chkCmdEnabled.Checked = lineOptions.CmdEnabled;
            chkPollAfterCmd.Checked = lineOptions.PollAfterCmd;
            chkDetailedLog.Checked = lineOptions.DetailedLog;

            channelConfigCopy = lineConfig.Channel.DeepClone();
            SelectChannelType(channelConfigCopy.TypeCode, channelConfigCopy.Driver);
            txtChannelOptions.Text = channelConfigCopy.CustomOptions.ToString();

            changing = false;
        }

        /// <summary>
        /// Sets the configuration according to the controls.
        /// </summary>
        public void ControlsToConfig(LineConfig lineConfig)
        {
            if (lineConfig == null)
                throw new ArgumentNullException(nameof(lineConfig));

            lineConfig.Active = chkActive.Checked;
            lineConfig.IsBound = chkIsBound.Checked;
            lineConfig.CommLineNum = Convert.ToInt32(numCommLineNum.Value);
            lineConfig.Name = txtName.Text;

            LineOptions lineOptions = lineConfig.LineOptions;
            lineOptions.ReqRetries = Convert.ToInt32(numReqRetries.Value);
            lineOptions.CycleDelay = Convert.ToInt32(numCycleDelay.Value);
            lineOptions.CmdEnabled = chkCmdEnabled.Checked;
            lineOptions.PollAfterCmd = chkPollAfterCmd.Checked;
            lineOptions.DetailedLog = chkDetailedLog.Checked;

            if (channelConfigCopy != null)
            {
                ChannelConfig channelConfig = lineConfig.Channel;
                channelConfig.TypeCode = channelConfigCopy.TypeCode;
                channelConfig.Driver = channelConfigCopy.Driver;
                channelConfig.CustomOptions.Clear();
                channelConfigCopy.CustomOptions.CopyTo(channelConfig.CustomOptions);
            }
        }


        /// <summary>
        /// Occurs when the configuration changes.
        /// </summary>
        public event EventHandler ConfigChanged;


        private void CtrlLineMain_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FillChannelTypeList();
        }

        private void control_Changed(object sender, EventArgs e)
        {
            if (!changing)
                OnConfigChanged();
        }

        private void cbChannelType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChannelTypeItem item = cbChannelType.SelectedItem as ChannelTypeItem;
            btnChannelProperties.Enabled = item != null && !item.IsEmpty;

            if (!changing && item != null && channelConfigCopy != null)
            {
                channelConfigCopy.TypeCode = item.TypeCode;
                channelConfigCopy.Driver = item.Driver;
                OnConfigChanged();
            }
        }

        private void btnChannelProperties_Click(object sender, EventArgs e)
        {
            // show communication channel properties
            if (cbChannelType.SelectedItem is ChannelTypeItem item)
            {
                if (string.IsNullOrEmpty(item.Driver))
                {
                    ScadaUiUtils.ShowError(ExtensionPhrases.DriverNotSpecified);
                }
                else if (!ExtensionUtils.GetDriverView(adminContext, commApp, item.Driver,
                    out DriverView driverView, out string message))
                {
                    ScadaUiUtils.ShowError(message);
                }
                else if (!driverView.CanCreateChannel)
                {
                    ScadaUiUtils.ShowError(ExtensionPhrases.ChannelNotSupported);
                }
                else if (driverView.CreateChannelView(channelConfigCopy) is not ChannelView channelView)
                {
                    ScadaUiUtils.ShowError(ExtensionPhrases.UnableCreateChannelView);
                }
                else if (!channelView.CanShowProperties)
                {
                    ScadaUiUtils.ShowInfo(ExtensionPhrases.NoChannelProperties);
                }
                else if (channelView.ShowProperties())
                {
                    txtChannelOptions.Text = channelConfigCopy.CustomOptions.ToString();
                    OnConfigChanged();
                }
            }
        }
    }
}
