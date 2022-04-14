// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Client;
using Scada.Lang;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Forms.Controls
{
    /// <summary>
    /// Represents a control for editing client connection options.
    /// <para>Представляет элемент управления для редактирования параметров соединения клиента.</para>
    /// </summary>
    public partial class CtrlClientConnection : UserControl
    {
        private ConnectionOptions connectionOptions;
        private bool nameEnabled;
        private bool instanceEnabled;
        private bool secretKeyChanged;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlClientConnection()
        {
            InitializeComponent();

            connectionOptions = null;
            nameEnabled = txtName.Enabled;
            instanceEnabled = txtInstance.Enabled;
            secretKeyChanged = false;
        }


        /// <summary>
        /// Gets or sets the connection options being edited.
        /// </summary>
        [Browsable(false)]
        public ConnectionOptions ConnectionOptions
        {
            get
            {
                return connectionOptions;
            }
            set
            {
                connectionOptions = null; // to suppress event

                if (value == null)
                {
                    gbConnectionOptions.Enabled = false;
                    txtName.Text = "";
                    txtHost.Text = "";
                    numPort.Value = numPort.Minimum;
                    numTimeout.Value = numTimeout.Minimum;
                    txtUsername.Text = "";
                    txtPassword.Text = "";
                    txtInstance.Text = "";
                    txtSecretKey.Text = "";
                }
                else
                {
                    gbConnectionOptions.Enabled = true;
                    txtName.Text = value.Name;
                    txtHost.Text = value.Host;
                    numPort.Value = value.Port;
                    numTimeout.Value = value.Timeout;
                    txtUsername.Text = value.Username;
                    txtPassword.Text = value.Password;
                    txtInstance.Text = value.Instance;
                    txtSecretKey.Text = ScadaUtils.BytesToHex(value.SecretKey);
                }

                connectionOptions = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the name control can respond to user interaction.
        /// </summary>
        public bool NameEnabled
        {
            get
            {
                return nameEnabled;
            }
            set
            {
                nameEnabled = value;
                txtName.Enabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the instance control can respond to user interaction.
        /// </summary>
        public bool InstanceEnabled
        {
            get
            {
                return instanceEnabled;
            }
            set
            {
                instanceEnabled = value;
                txtInstance.Enabled = value;
            }
        }


        /// <summary>
        /// Raises a ConnectionOptionsChanged event.
        /// </summary>
        private void OnConnectionOptionsChanged()
        {
            ConnectionOptionsChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises a NameChanged event.
        /// </summary>
        private void OnNameChanged()
        {
            NameChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises a NameValidated event.
        /// </summary>
        private void OnNameValidated()
        {
            NameValidated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Sets input focus.
        /// </summary>
        public void SetFocus()
        {
            txtName.Select();
        }


        /// <summary>
        /// Occurs when the connection options change.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler ConnectionOptionsChanged;

        /// <summary>
        /// Occurs when the connection name changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler NameChanged;

        /// <summary>
        /// Occurs when the connection name is finished validating.
        /// </summary>
        [Category("Focus")]
        public event EventHandler NameValidated;


        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                connectionOptions.Name = txtName.Text;
                OnConnectionOptionsChanged();
                OnNameChanged();
            }
        }

        private void txtName_Validated(object sender, EventArgs e)
        {
            if (connectionOptions != null)
                OnNameValidated();
        }

        private void txtHost_TextChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                connectionOptions.Host = txtHost.Text;
                OnConnectionOptionsChanged();
            }
        }

        private void numPort_ValueChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                connectionOptions.Port = Convert.ToInt32(numPort.Value);
                OnConnectionOptionsChanged();
            }
        }

        private void numTimeout_ValueChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                connectionOptions.Timeout = Convert.ToInt32(numTimeout.Value);
                OnConnectionOptionsChanged();
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                connectionOptions.Username = txtUsername.Text;
                OnConnectionOptionsChanged();
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                connectionOptions.Password = txtPassword.Text;
                OnConnectionOptionsChanged();
            }
        }

        private void txtInstance_TextChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                connectionOptions.Instance = txtInstance.Text;
                OnConnectionOptionsChanged();
            }
        }

        private void txtSecretKey_TextChanged(object sender, EventArgs e)
        {
            secretKeyChanged = true;
        }

        private void txtSecretKey_Enter(object sender, EventArgs e)
        {
            txtSecretKey.UseSystemPasswordChar = false;
            secretKeyChanged = false;
        }

        private void txtSecretKey_Leave(object sender, EventArgs e)
        {
            // otherwise the Tab key does not work
            Action action = () => { txtSecretKey.UseSystemPasswordChar = true; };
            Task.Run(() => { Invoke(action); });
        }

        private void txtSecretKey_Validating(object sender, CancelEventArgs e)
        {
            if (connectionOptions != null && secretKeyChanged)
            {
                if (ScadaUtils.HexToBytes(txtSecretKey.Text, out byte[] secretKey) &&
                    secretKey.Length == ScadaUtils.SecretKeySize)
                {
                    connectionOptions.SecretKey = secretKey;
                    txtSecretKey.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
                    OnConnectionOptionsChanged();
                }
                else
                {
                    txtSecretKey.ForeColor = Color.Red;
                    ScadaUiUtils.ShowError(CommonPhrases.InvalidSecretKey);
                }
            }
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            txtSecretKey.Text = Clipboard.GetText();
            secretKeyChanged = true;
            txtSecretKey_Validating(null, null);
        }
    }
}
