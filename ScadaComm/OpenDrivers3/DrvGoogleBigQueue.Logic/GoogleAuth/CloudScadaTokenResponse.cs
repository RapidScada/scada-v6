using Google.Apis.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Comm.Drivers.DrvGoogleBigQueue.Logic.GoogleAuth
{
    /// <summary>
    /// Token响应对象
    /// </summary>
    public class CloudScadaTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonProperty("expiry")]
        public DateTime Expiry { get; set; } = DateTime.MinValue;

        /// <summary>
        /// 判断token过期
        /// </summary>
        public bool IsExpired(IClock clock) =>
            AccessToken == null || Expiry <= clock.UtcNow;

        /// <summary>
        /// Returns true if the token is expired or it's so close to expiring that it shouldn't be used.
        /// </summary>
        /// <remarks>If a token response doesn't have at least one of <see cref="TokenResponse.AccessToken"/>
        /// or <see cref="TokenResponse.IdToken"/> set then it's considered expired.
        /// If <see cref="TokenResponse.ExpiresInSeconds"/> is null, the token is also considered expired. </remarks>
        internal bool IsEffectivelyExpired(IClock clock) =>
            AccessToken == null || Expiry <= clock.UtcNow;

    }
}
