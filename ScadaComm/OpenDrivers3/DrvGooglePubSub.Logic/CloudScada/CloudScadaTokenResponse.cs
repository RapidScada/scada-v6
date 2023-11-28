using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Comm.Drivers.DrvGooglePubSub.Logic
{
    /// <summary>
    /// Token响应对象
    /// </summary>
    public class CloudScadaTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonProperty("expiry")]
        public DateTime Expiry { get; set; } = System.DateTime.MinValue;



        /// <summary>
        /// 判断token过期
        /// </summary>
        [JsonIgnore]
        public bool IsExpired { get { return string.IsNullOrEmpty(AccessToken) || Expiry <= System.DateTime.Now; } }

    }
}
