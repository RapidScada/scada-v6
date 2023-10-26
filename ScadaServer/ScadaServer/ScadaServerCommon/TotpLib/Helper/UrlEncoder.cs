using System.Text;

namespace Scada.Server.TotpLib.Helper
{
    internal static class UrlEncoder
    {
        internal static string Encode(string value)
        {
            var result = new StringBuilder();
            var validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

            foreach (var symbol in value)
                if (validChars.IndexOf(symbol) != -1)
                    result.Append(symbol);
                else
                    result.Append('%' + string.Format("{0:X2}", (int)symbol));

            return result.ToString().Replace(" ", "%20");
        }
    }
}