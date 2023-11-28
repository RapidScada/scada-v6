using Scada.Comm.Channels;
using Scada.Comm.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Comm.Drivers.DrvCnlGoogle.Logic
{
    public class DrvCnlGoogleLogic : DriverLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvCnlGoogleLogic(ICommContext commContext)
            : base(commContext)
        {
        }

        /// <summary>
        /// Gets the driver code.
        /// </summary>
        public override string Code
        {
            get
            {
                return "DrvCnlGoogle";
            }
        }

        /// <summary>
        /// Creates a new communication channel.
        /// </summary>
        public override ChannelLogic CreateChannel(ILineContext lineContext, ChannelConfig channelConfig)
        {
            return channelConfig.TypeCode == "GoogleCloud"
                ? new GoogleCloudChannelLogic(lineContext, channelConfig)
                : null;
        }
    }
}
