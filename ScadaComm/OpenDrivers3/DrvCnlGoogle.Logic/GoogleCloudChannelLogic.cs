using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.Drivers.DrvGoogle.Common;
using Scada.Comm.Drivers.DrvGoogle.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Comm.Drivers.DrvCnlGoogle.Logic
{
    public class GoogleCloudChannelLogic : ChannelLogic, IGoogleCloudChannel
    {
        public GoogleCloudOptions GoogleCloudOptions { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public GoogleCloudChannelLogic(ILineContext lineContext, ChannelConfig channelConfig)
            : base(lineContext, channelConfig)
        {
            GoogleCloudOptions = new GoogleCloudOptions(channelConfig.CustomOptions);
        }
    }
}
