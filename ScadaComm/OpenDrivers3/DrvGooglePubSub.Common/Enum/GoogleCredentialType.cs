using Scada.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Comm.Drivers.DrvGooglePubSub
{
    public enum GoogleCredentialType
    {
        [Description("应用默认凭据(ADC)")]
        ApplicationDefaultCredential = 0,

        [Description("自签名令牌")]
        CloudScadaAccessToken = 1,
    }
}
