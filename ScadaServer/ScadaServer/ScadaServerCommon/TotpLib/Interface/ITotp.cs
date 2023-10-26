using Scada.Server.TotpLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Server.TotpLib.Interface
{
    public interface ITotp
    {

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
        TotpSetup GenerateBase64(string issuer, string accountIdentity, string accountSecretKey, int pixelsPerModule = 5);

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
        TotpSetup GenerateUrl(string issuer, string accountIdentity, string accountSecretKey);

        /// <summary>
        ///     Validates a given TOTP.
        /// </summary>
        /// <param name="accountSecretKey">User's secret key. Same as used to create the setup.</param>
        /// <param name="clientTotp">Number provided by the user which has to be validated.</param>
        /// <param name="timeToleranceInSeconds">
        ///     Time tolerance in seconds. Default is 60 to acceppt 60 seconds before and after
        ///     now.
        /// </param>
        /// <returns>True or False if the validation was successful.</returns>
        bool Validate(string accountSecretKey, int clientTotp, int timeToleranceInSeconds = 60);
    }
}
