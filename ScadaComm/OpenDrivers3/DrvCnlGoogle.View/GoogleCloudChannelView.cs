using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.Drivers.DrvCnlGoogle.View.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Comm.Drivers.DrvCnlGoogle.View
{
    internal class GoogleCloudChannelView : ChannelView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public GoogleCloudChannelView(DriverView parentView, ChannelConfig channelConfig)
            : base(parentView, channelConfig)
        {
            CanShowProperties = true;
        }

        /// <summary>
        /// Shows a modal dialog box for editing communication channel properties.
        /// </summary>
        public override bool ShowProperties()
        {
            return new FrmGoogleCloudChannelOptions(ChannelConfig).ShowDialog() == DialogResult.OK;
        }
    }
}
