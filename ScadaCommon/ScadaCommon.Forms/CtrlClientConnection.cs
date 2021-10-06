// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Forms
{
    /// <summary>
    /// Represents a control for editing client connection options.
    /// <para>Представляет элемент управления для редактирования параметров соединения клиента.</para>
    /// </summary>
    public partial class CtrlClientConnection : UserControl
    {
        private ConnectionOptions connectionOptions;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlClientConnection()
        {
            InitializeComponent();
            connectionOptions = null;
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
                    connectionOptions = null; // to suppress event
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
                return txtName.Enabled;
            }
            set
            {
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
                return txtInstance.Enabled;
            }
            set
            {
                txtInstance.Enabled = value;
            }
        }


        /// <summary>
        /// Sets input focus.
        /// </summary>
        public void SetFocus()
        {
            txtName.Select();
        }

    }
}
