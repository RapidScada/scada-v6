using Scada.Forms;
using Scada.Server.Modules.ModDeviceAlarm.Config;
using System.Net;
using System.Net.Mail;

namespace Scada.Server.Modules.ModDeviceAlarm.View.Forms
{
    public partial class FrmSmtpTest : Form
    {
        private readonly SmtpClient smtpClient;    // sends emails
        private readonly int smtpTimeout = 25000;  //smtp timeout

        private EmailDeviceConfig emailConfig;
        public FrmSmtpTest()
        {
            InitializeComponent();
        }

        public FrmSmtpTest(EmailDeviceConfig emailDeviceConfig) 
            : this()
        {
            emailConfig = emailDeviceConfig;
            smtpClient = new SmtpClient();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            InitSnmpClient();
            MailMessage message = new MailMessage();
            try
            {
                message.From = new MailAddress(emailConfig.SenderAddress, emailConfig.SenderDisplayName);
            }
            catch
            {
                ScadaUiUtils.ShowError("Invalid sender address {0}", emailConfig.SenderAddress);
                return;
            }
            try
            {
                message.To.Add(new MailAddress(txtSendTo.Text.Trim()));
            }
            catch
            {
                ScadaUiUtils.ShowError("Invalid recipient address {0}", txtSendTo.Text);
                return;
            }
            try
            {
                message.Subject = txtSubject.Text;
                message.Body = txtContent.Text;

                smtpClient.Send(message);
            }
            catch(Exception ex)
            {
                ScadaUiUtils.ShowError("Send error {0},{1}", txtSendTo.Text,ex.Message);
                return;
            }
            ScadaUiUtils.ShowInfo($"Success send to {txtSendTo.Text}");

            DialogResult = DialogResult.OK;
        }


        /// <summary>
        /// Initializes the exporter.
        /// </summary>
        private void InitSnmpClient()
        {
            try
            {
                smtpClient.Host = emailConfig.Host;
                smtpClient.Port = emailConfig.Port;
                smtpClient.Credentials = string.IsNullOrEmpty(emailConfig.Password)
                    ? CredentialCache.DefaultNetworkCredentials
                    : new NetworkCredential(emailConfig.Username, emailConfig.Password);
                smtpClient.Timeout = smtpTimeout;
                smtpClient.EnableSsl = emailConfig.EnableSsl;
            }
            catch (Exception ex)
            {
                ScadaUiUtils.ShowError($"Error initializing smtp client: {ex.Message}");
            }
        }
    }
}
