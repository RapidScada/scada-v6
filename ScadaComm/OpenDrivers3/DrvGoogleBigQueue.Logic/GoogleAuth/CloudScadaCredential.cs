using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Google.Apis.Util;
using Newtonsoft.Json;
using Scada.Comm.Drivers.DrvGoogle.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scada.Comm.Drivers.DrvGoogleBigQueue.Logic.GoogleAuth
{
    public class CloudScadaCredential : ICredential, ITokenAccessWithHeaders, IHttpExecuteInterceptor
    {
        private HttpClient HttpClient;
        private CloudScadaTokenRefreshManager refreshManager;
        public IAccessMethod AccessMethod { get; set; }

        private IGoogleCloudChannel googleCloudChannel;

        public CloudScadaCredential(IGoogleCloudChannel googleCloudChannel)
        {
            this.googleCloudChannel = googleCloudChannel.ThrowIfNull(nameof(googleCloudChannel));
            HttpClient = new HttpClient();
            HttpClient.Timeout = TimeSpan.FromMilliseconds(googleCloudChannel.GoogleCloudOptions.Timeout);
            AccessMethod = new BearerToken.AuthorizationHeaderAccessMethod();
            refreshManager = new CloudScadaTokenRefreshManager(RefreshTokenAsync, Google.Apis.Util.SystemClock.Default);
        }

        public async Task<string> GetAccessTokenForRequestAsync(string authUri = null, CancellationToken cancellationToken = default)
        {
            return await refreshManager.GetAccessTokenForRequestAsync(cancellationToken);
        }

        public void Initialize(ConfigurableHttpClient httpClient)
        {
            httpClient.MessageHandler.Credential = this;
        }


        public async Task<AccessTokenWithHeaders> GetAccessTokenWithHeadersForRequestAsync(string authUri = null, CancellationToken cancellationToken = default)
        {
            var token = await refreshManager.GetAccessTokenForRequestAsync(cancellationToken).ConfigureAwait(false);
            return new AccessTokenWithHeaders.Builder { }.Build(token);
        }
        /// <summary>
        /// 获取Token
        /// </summary>
        async Task<bool> RefreshTokenAsync(CancellationToken cancellationToken)
        {
            var cloudOpts = this.googleCloudChannel.GoogleCloudOptions;
            var ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var sign = GenerateSign(cloudOpts.ClientID, cloudOpts.ClientSecret, $"{ts}");
            var queryParams = $"?key={cloudOpts.ServerKey}&client_id={cloudOpts.ClientID}&ts={ts}&sign={sign}";
            var response = await HttpClient.GetAsync($"{cloudOpts.Server}{queryParams}").ConfigureAwait(false);
            var responseMes = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var token = JsonConvert.DeserializeObject<CloudScadaTokenResponse>(responseMes);
                refreshManager.Token = token;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 签名
        /// </summary>
        private string GenerateSign(string clientID, string clientSecret, string ts)
        {
            var preSign = "cloudscada-connector-dev" + clientID + clientSecret + ts;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(preSign));
                string hashString = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
                Console.WriteLine($"preSign: {preSign}");
                Console.WriteLine($"sign: {hashString}");
                return hashString;
            }
        }

        public async Task InterceptAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await GetAccessTokenWithHeadersForRequestAsync("", cancellationToken).ConfigureAwait(false);
            AccessMethod.Intercept(request, accessToken.AccessToken);
            accessToken.AddHeaders(request);
        }
    }

}
