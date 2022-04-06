// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Config;
using Scada.Admin.Project;
using Scada.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Controls
{
    /// <summary>
    /// Represents a control for selecting channel numbers when creating channels.
    /// <para>Представляет элемент управления для выбора номеров каналов при создании каналов.</para>
    /// </summary>
    public partial class CtrlCnlCreate3 : UserControl
    {
        private IAdminContext adminContext; // the Administrator context
        private ScadaProject project;       // the project under development
        private int lastStartCnlNum;        // the last calculated start channel number
        private int lastCnlCnt;             // the last specified number of channels


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlCnlCreate3()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets or sets the selected device name.
        /// </summary>
        public string DeviceName
        {
            get
            {
                return txtDevice.Text;
            }
            set
            {
                txtDevice.Text = value ?? "";
            }
        }

        /// <summary>
        /// Gets the start channel number.
        /// </summary>
        public int StartCnlNum => Convert.ToInt32(numStartCnlNum.Value);


        /// <summary>
        /// Calculates a start channel number.
        /// </summary>
        private bool CalcStartCnlNum(int cnlCnt, out int startCnlNum)
        {
            ChannelNumberingOptions options = adminContext.AppConfig.ChannelNumberingOptions;
            startCnlNum = options.Multiplicity + options.Shift;
            int prevCnlNum = 0;

            foreach (int cnlNum in project.ConfigBase.CnlTable.EnumerateKeys())
            {
                if (prevCnlNum < startCnlNum && startCnlNum <= cnlNum)
                {
                    if (startCnlNum + cnlCnt + options.Gap <= cnlNum)
                        return true;
                    else
                        startCnlNum += options.Multiplicity;
                }

                prevCnlNum = cnlNum;
            }

            return startCnlNum <= ushort.MaxValue;
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        public void Init(IAdminContext adminContext, ScadaProject project)
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            lastStartCnlNum = 1;
            lastCnlCnt = 0;
        }

        /// <summary>
        /// Sets the input focus.
        /// </summary>
        public void SetFocus()
        {
            numStartCnlNum.Select();
        }
        
        /// <summary>
        /// Sets the channel numbers by default.
        /// </summary>
        public void ResetCnlNums(int cnlCnt)
        {
            lastStartCnlNum = 1;
            lastCnlCnt = cnlCnt;

            if (cnlCnt > 0)
            {
                gbCnlNums.Enabled = true;

                if (CalcStartCnlNum(cnlCnt, out int startCnlNum))
                    lastStartCnlNum = startCnlNum;
            }
            else
            {
                gbCnlNums.Enabled = false;
            }

            numStartCnlNum.SetValue(lastStartCnlNum);
            numEndCnlNum.SetValue(lastStartCnlNum + lastCnlCnt - 1);
        }


        private void numStartCnlNum_ValueChanged(object sender, EventArgs e)
        {
            int startCnlNum = Convert.ToInt32(numStartCnlNum.Value);
            numEndCnlNum.SetValue(startCnlNum + lastCnlCnt - 1);
        }

        private void btnMap_Click(object sender, EventArgs e)
        {
            // send message to generate map
            adminContext.MessageToExtensions(new MessageEventArgs
            {
                Message = KnownExtensionMessage.GenerateChannelMap,
                Arguments = new Dictionary<string, object> { { "GroupByDevices", true } }
            });
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (lastStartCnlNum > 0)
                numStartCnlNum.SetValue(lastStartCnlNum);
        }
    }
}
