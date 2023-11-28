using Google.Cloud.PubSub.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Comm.Drivers.DrvGooglePubSub
{
    public class SubscriptionRecord
    {
        public string ProjectId { get; set; } 
        public string SubscriptionId { get; set; } 

        public Task SubscribeTask { get; set; }

        /// <summary>
        /// Gets the auxiliary data.
        /// </summary>
        public object AuxData { get; init; }

        public SubscriberClient SubscriberClient { get; set; }

        public long MessageCount { get; set; } = 0;
    }
}
