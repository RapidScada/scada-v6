using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Comm.Drivers.DrvGooglePubSub.Logic.CloudScada
{
    public class CloudScadaCredential : ICredential
    {
        private static object tokenLocker = new object();
        private HttpClient HttpClient;
        private CloudScadaTokenResponse _token;

        public static CloudScadaCredential Init(IAccessMethod accessMethod)
        {
            return new CloudScadaCredential(accessMethod);
        }

        public CloudScadaCredential(IAccessMethod accessMethod)
        {
            this.HttpClient = new HttpClient();
            this.HttpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        public Task<string> GetAccessTokenForRequestAsync(string authUri = null, CancellationToken cancellationToken = default)
        {
            lock (tokenLocker)
            {
                if (_token == null || _token.IsExpired)
                {
                    try
                    {
                        LogUtil.Info("[GetAccessTokenForRequestAsync] Get New AccessToken");
                        var accessTokenRes = this.GetAccessToken().ConfigureAwait(false).GetAwaiter().GetResult();
                        _token = JsonConvert.DeserializeObject<CloudScadaTokenResponse>(accessTokenRes);
                        if (_token != null)
                        {
                            _token.Expiry = _token.Expiry.AddHours(8).AddMinutes(-3);
                            LogUtil.Info($"Expiry: {_token.Expiry:G}");
                        }
                    }
                    catch (Exception ex)
                    {
                        this._token = null;
                        LogUtil.Info($"[GetAccessTokenForRequestAsync]Err: {ex.Message}");
                    }
                }
            }
            if (_token == null || _token.IsExpired) throw new ArgumentException("Cloud Scada Token is empty");
            //Console.WriteLine($"[GetAccessTokenForRequestAsync] Return AccessToken");
            return Task.FromResult(_token.AccessToken);
        }

        public void Initialize(ConfigurableHttpClient httpClient)
        {
            LogUtil.Info("[CloudScadaCredential]Initialize");
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        private async Task<string> GetAccessToken()
        {
            var ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var sign = GenerateSign(GlobalConfig.client_id, GlobalConfig.client_secret, $"{ts}");
            var queryParams = $"?key={GlobalConfig.key}&client_id={GlobalConfig.client_id}&ts={ts}&sign={sign}";
            var response = await this.HttpClient.GetAsync($"{GlobalConfig.api_url}{queryParams}");
            var responseMes = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return responseMes;
            }
            throw new ArgumentException(responseMes);
        }


        /// <summary>
        /// 签名
        /// </summary>
        private string GenerateSign(string clientID, string clientSecret, string ts)
        {
            var preSign = "cloudscada-connector-dev" + clientID + clientSecret + ts;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(preSign));
                string hashString = BitConverter.ToString(hashBytes).Replace("-", String.Empty).ToLower();
                LogUtil.Info($"preSign: {preSign}");
                LogUtil.Info($"sign: {hashString}");
                return hashString;
            }
        }
    }
}
