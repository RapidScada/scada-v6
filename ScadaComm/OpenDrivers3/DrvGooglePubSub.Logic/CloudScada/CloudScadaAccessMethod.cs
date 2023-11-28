using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Comm.Drivers.DrvGooglePubSub.Logic
{
    public class CloudScadaAccessMethod : IAccessMethod
    {
        public string GetAccessToken(HttpRequestMessage request)
        {
            Console.WriteLine($"[CloudScadaAccessMethod]GetAccessToken: {request.Content}");
            return "";
        }

        public void Intercept(HttpRequestMessage request, string accessToken)
        {
            Console.WriteLine($"[CloudScadaAccessMethod]Intercept: {accessToken}");
        }
    }
}
