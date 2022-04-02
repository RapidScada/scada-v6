// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Project;

namespace Scada.Admin.Extensions.ExtWirenBoard.Controls
{
    /// <summary>
    /// Represents a control for selecting entity IDs.
    /// <para>Представляет элемент управления для выбора идентификаторов сущностей.</para>
    /// </summary>
    internal partial class CtrlEntityID : UserControl
    {
        private readonly IAdminContext adminContext; // the Administrator context
        private readonly ScadaProject project;       // the project under development


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private CtrlEntityID()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlEntityID(IAdminContext adminContext, ScadaProject project)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
        }


        /// <summary>
        /// Gets the starting device number.
        /// </summary>
        public int StartDeviceNum => Convert.ToInt32(numStartDeviceNum.Value);

        /// <summary>
        /// Gets the adjusted starting channel number.
        /// </summary>
        public int StartCnlNum => AdjustCnlNum(Convert.ToInt32(numStartCnlNum.Value));


        /// <summary>
        /// Sets the input focus.
        /// </summary>
        public void SetFocus()
        {
            numStartDeviceNum.Select();
        }

        /// <summary>
        /// Automatically assigns device and channel numbers.
        /// </summary>
        public void AssignIDs()
        {
            numStartDeviceNum.Value = project.ConfigBase.DeviceTable.GetNextPk();
            numStartCnlNum.Value = AdjustCnlNum(project.ConfigBase.CnlTable.GetNextPk());
        }

        /// <summary>
        /// 
        /// </summary>
        public static int AdjustCnlNum(int cnlNum)
        {
            return cnlNum;
        }


        private void btnDeviceMap_Click(object sender, EventArgs e)
        {
            // send message to generate device map
            adminContext.MessageToExtensions(new MessageEventArgs
            {
                Message = KnownExtensionMessage.GenerateDeviceMap
            });
        }

        private void btnCnlMap_Click(object sender, EventArgs e)
        {
            // send message to generate channel map
            adminContext.MessageToExtensions(new MessageEventArgs
            {
                Message = KnownExtensionMessage.GenerateChannelMap,
                Arguments = new Dictionary<string, object> { { "GroupByDevices", true } }
            });
        }
    }
}
