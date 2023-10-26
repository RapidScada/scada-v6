using Scada.Server.TotpLib.Interface;
using Scada.Server.TotpLib.Models;

namespace Scada.Server.TotpLib
{
    public class Totp : ITotp
    {
        private readonly ITotpGenerator _totpGenerator;
        private readonly ITotpSetupGenerator _totpSetupGenerator;
        private readonly ITotpValidator _totpValidator;

        public Totp(ITotpSetupGenerator totpSetupGenerator)
        {
            _totpSetupGenerator = totpSetupGenerator;
            _totpGenerator = new TotpGenerator();
            _totpValidator = new TotpValidator(_totpGenerator);
        }

        /// <summary>
        ///     生成一个二维码内容，适合前端自己生成二维码 以便Authenticator 扫描添加
        /// </summary>
        /// <param name="issuer">应用程序或者公司名称 英文 中文可能无法显示.</param>
        /// <param name="accountIdentity">
        ///     用户名或者有效可以显示到Authenticator中
        /// </param>
        /// <param name="accountSecretKey">
        ///     密钥最好每个人都是不同的随机生成或者GUID，校验TOTP时需传入
        /// </param>
        /// <returns>返回手动设置key和二维码内容 适合前端自己生成二维码.</returns>
        public TotpSetup GenerateUrl(string issuer, string accountIdentity, string accountSecretKey)
        {
            return _totpSetupGenerator.GenerateUrl(issuer, accountIdentity, accountSecretKey);
        }

        /// <summary>
        ///     生成一个bbbase64二维码图片 以便Authenticator 扫描添加
        /// </summary>
        /// <param name="issuer">应用程序或者公司名称 英文 中文可能无法显示.</param>
        /// <param name="accountIdentity">
        ///     用户名或者有效可以显示到Authenticator中
        /// </param>
        /// <param name="accountSecretKey">
        ///     密钥最好每个人都是不同的随机生成或者GUID，校验TOTP时需传入
        /// </param>
        /// <param name="pixelsPerModule">二维码每个点的像素大小默认5.</param>
        /// <returns>返回手动设置key和二维码base64.</returns>
        public TotpSetup GenerateBase64(string issuer, string accountIdentity, string accountSecretKey, int pixelsPerModule = 5)
        {
            return _totpSetupGenerator.GenerateBase64(issuer, accountIdentity, accountSecretKey, pixelsPerModule);
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="accountSecretKey"></param>
        /// <param name="clientTotp"></param>
        /// <param name="timeToleranceInSeconds"></param>
        /// <returns></returns>
        public bool Validate(string accountSecretKey, int clientTotp, int timeToleranceInSeconds = 60)
        {
            return _totpValidator.Validate(accountSecretKey, clientTotp, timeToleranceInSeconds);
        }
    }
}
