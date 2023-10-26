namespace Scada.Server.TotpLib.Models
{
    public class TotpSetup
    {
        /// <summary>
        ///     如果无法使用QR代码，则需要此代码来设置Google身份验证器。
        /// </summary>
        public string ManualSetupKey { get; set; }

        /// <summary>
        ///     二维码图片base64
        /// </summary>
        public string QrCodeImageBase64 { get; set; }

        /// <summary>
        ///     二维码图片内容 适合前端自己生成二维码
        /// </summary>
        public string QrCodeImageContent { get; set; }
    }
}