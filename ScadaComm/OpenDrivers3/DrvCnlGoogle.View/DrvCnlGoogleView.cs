using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Forms;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Comm.Drivers.DrvCnlGoogle.View
{
    /// <summary>
    /// Implements the driver user interface.
    /// <para>Реализует пользовательский интерфейс драйвера.</para>
    /// </summary>
    public class DrvCnlGoogleView : DriverView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvCnlGoogleView()
            : base()
        {
            CanCreateChannel = true;
        }


        /// <summary>
        /// Gets the driver name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Google Cloud Communication Channel";
            }
        }

        /// <summary>
        /// Gets the driver description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return "Provides Google Cloud communication channel.";
            }
        }

        /// <summary>
        /// Gets the communication channel types provided by the driver.
        /// </summary>
        public override ICollection<ChannelTypeName> ChannelTypes
        {
            get
            {
                return new ChannelTypeName[] { new ChannelTypeName("GoogleCloud", "Google Cloud") };
            }
        }


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, "DrvCnlGoogle", out string errMsg))
                ScadaUiUtils.ShowError(errMsg);
        }

        /// <summary>
        /// Creates a new communication channel user interface.
        /// </summary>
        public override ChannelView CreateChannelView(ChannelConfig channelConfig)
        {
            return new GoogleCloudChannelView(this, channelConfig);
        }
    }
}
