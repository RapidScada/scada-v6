// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtWirenBoard.Code;
using Scada.Admin.Project;
using Scada.Data.Entities;

namespace Scada.Admin.Extensions.ExtWirenBoard.Controls
{
    /// <summary>
    /// Represents a control for selecting entity IDs.
    /// <para>Представляет элемент управления для выбора идентификаторов сущностей.</para>
    /// </summary>
    internal partial class CtrlEntityID : UserControl
    {
        private readonly IAdminContext adminContext;      // the Administrator context
        private readonly ScadaProject project;            // the project under development
        private readonly RecentSelection recentSelection; // the recently selected parameters


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
        public CtrlEntityID(IAdminContext adminContext, ScadaProject project, RecentSelection recentSelection)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.recentSelection = recentSelection ?? throw new ArgumentNullException(nameof(recentSelection));
        }


        /// <summary>
        /// Gets the starting device number.
        /// </summary>
        public int StartDeviceNum => Convert.ToInt32(numStartDeviceNum.Value);

        /// <summary>
        /// Gets the starting channel number.
        /// </summary>
        public int StartCnlNum => Convert.ToInt32(numStartCnlNum.Value);

        /// <summary>
        /// Gets the selected object number.
        /// </summary>
        public int? ObjNum => cbObj.SelectedValue is int objNum && objNum > 0 ? objNum : null;


        /// <summary>
        /// Fills the combo box with the objects.
        /// </summary>
        private void FillObjectList()
        {
            List<Obj> objs = new(project.ConfigDatabase.ObjTable.ItemCount + 1);
            objs.Add(new Obj { ObjNum = 0, Name = " " });
            objs.AddRange(project.ConfigDatabase.ObjTable.Enumerate().OrderBy(obj => obj.Name));

            cbObj.ValueMember = "ObjNum";
            cbObj.DisplayMember = "Name";
            cbObj.DataSource = objs;

            try { cbObj.SelectedValue = recentSelection.ObjNum; }
            catch { cbObj.SelectedValue = 0; }
        }

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
            numStartDeviceNum.Value = project.ConfigDatabase.DeviceTable.GetNextPk();
            numStartCnlNum.Value = ConfigBuilder.AdjustCnlNum(adminContext.AppConfig.ChannelNumberingOptions, 
                project.ConfigDatabase.CnlTable.GetNextPk());
        }

        /// <summary>
        /// Remembers the selected values.
        /// </summary>
        public void RememberRecentSelection()
        {
            recentSelection.ObjNum = cbObj.SelectedValue is int objNum ? objNum : 0;
        }


        private void CtrlEntityID_Load(object sender, EventArgs e)
        {
            FillObjectList();
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
