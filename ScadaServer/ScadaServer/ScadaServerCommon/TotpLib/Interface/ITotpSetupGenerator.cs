using Scada.Server.TotpLib.Models;

namespace Scada.Server.TotpLib.Interface
{
    public interface ITotpSetupGenerator
    {
        /// <summary>
        ///     Generates an object you will need so that the user can setup his Google Authenticator to be used with your app.
        /// </summary>
        /// <param name="issuer">Your app name or company for example.</param>
        /// <param name="accountIdentity">
        ///     Name, Email or Id of the user, without spaces, this will be shown in google
        ///     authenticator.
        /// </param>
        /// <param name="accountSecretKey">
        ///     A secret key which will be used to generate one time passwords. This key is the same
        ///     needed for validating a passed TOTP.
        /// </param>
        /// <param name="pixelsPerModule">Use Https on google api or not.</param>
        /// <returns>TotpSetup with ManualSetupKey and QrCodeBase64.</returns>
        TotpSetup GenerateBase64(string issuer, string accountIdentity, string accountSecretKey, int pixelsPerModule = 5);


        /// <summary>
        ///     Generates an object you will need so that the user can setup his Google Authenticator to be used with your app.
        /// </summary>
        /// <param name="issuer">Your app name or company for example.</param>
        /// <param name="accountIdentity">
        ///     Name, Email or Id of the user, without spaces, this will be shown in google
        ///     authenticator.
        /// </param>
        /// <param name="accountSecretKey">
        ///     A secret key which will be used to generate one time passwords. This key is the same
        ///     needed for validating a passed TOTP.
        /// </param>
        /// <returns>TotpSetup with ManualSetupKey and QrCodeContent.</returns>
        TotpSetup GenerateUrl(string issuer, string accountIdentity, string accountSecretKey);
    }
}