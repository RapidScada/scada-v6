// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Lang;
using Scada.Server.Modules.ModArcInfluxDb.Config;

namespace Scada.Server.Modules.ModArcInfluxDb.View.Forms
{
    /// <summary>
    /// Represents a connection manager form.
    /// <para>Представляет форму менеджера соединений.</para>
    /// </summary>
    public partial class FrmConnManager : Form
    {
        private readonly string configFileName;      // the module configuration file name
        private readonly ModuleConfig moduleConfig;  // the module configuration
        private bool changing;                       // controls are being changed programmatically


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmConnManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmConnManager(string configDir)
            : this()
        {
            configFileName = Path.Combine(configDir, ModuleConfig.DefaultFileName);
            moduleConfig = new ModuleConfig();
            changing = false;
        }


        /// <summary>
        /// Gets the connection names.
        /// </summary>
        public string[] ConnectionNames => moduleConfig.Connections.Keys.ToArray();


        /// <summary>
        /// Loads the module configuration.
        /// </summary>
        private void LoadConfig()
        {
            if (File.Exists(configFileName) && !moduleConfig.Load(configFileName, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);
        }

        /// <summary>
        /// Saves the module configuration.
        /// </summary>
        private bool SaveConfig()
        {
            if (moduleConfig.Save(configFileName, out string errMsg))
            {
                return true;
            }
            else
            {
                ScadaUiUtils.ShowError(errMsg);
                return false;
            }
        }

        /// <summary>
        /// Fills the connection list according to the configuration.
        /// </summary>
        private void FillConnList()
        {
            lvConn.BeginUpdate();
            lvConn.Items.Clear();

            foreach (KeyValuePair<string, ConnectionOptions> pair in moduleConfig.Connections)
            {
                lvConn.Items.Add(CreateConnectionItem(pair.Value));
            }

            lvConn.EndUpdate();

            if (lvConn.Items.Count > 0)
            {
                lvConn.Items[0].Selected = true;
            }
            else
            {
                btnDeleteConn.Enabled = false;
                ShowConnOptions(null);
            }
        }

        /// <summary>
        /// Shows the connection options according to the select connection item.
        /// </summary>
        private void ShowConnOptions(ConnectionOptions connectionOptions)
        {
            if (connectionOptions == null)
            {
                gbOptions.Enabled = false;
                txtName.Text = "";
                txtUrl.Text = "";
                txtToken.Text = "";
                txtUsername.Text = "";
                txtPassword.Text = "";
                txtBucket.Text = "";
                txtOrg.Text = "";
            }
            else
            {
                gbOptions.Enabled = true;
                txtName.Text = connectionOptions.Name;
                txtUrl.Text = connectionOptions.Url;
                txtToken.Text = connectionOptions.Token;
                txtUsername.Text = connectionOptions.Username;
                txtPassword.Text = connectionOptions.Password;
                txtBucket.Text = connectionOptions.Bucket;
                txtOrg.Text = connectionOptions.Org;
            }
        }

        /// <summary>
        /// Retrieves the connections from the connection list to the module configuration.
        /// </summary>
        private void RetrieveConnections()
        {
            moduleConfig.Connections.Clear();

            foreach (ListViewItem item in lvConn.Items)
            {
                ConnectionOptions options = (ConnectionOptions)item.Tag;
                moduleConfig.Connections[options.Name] = options;
            }
        }

        /// <summary>
        /// Gets the selected list view item and the corresponding configuration.
        /// </summary>
        private bool GetSelectedItem(out ListViewItem item, out ConnectionOptions connectionOptions)
        {
            if (lvConn.SelectedItems.Count > 0)
            {
                item = lvConn.SelectedItems[0];
                connectionOptions = (ConnectionOptions)item.Tag;
                return true;
            }
            else
            {
                item = null;
                connectionOptions = null;
                return false;
            }
        }

        /// <summary>
        /// Creates a new list view item that represents the specified connection.
        /// </summary>
        private static ListViewItem CreateConnectionItem(ConnectionOptions options)
        {
            return new ListViewItem
            {
                Text = string.IsNullOrEmpty(options.Name) ? CommonPhrases.UnnamedConnection : options.Name,
                Tag = options
            };
        }


        private void FrmConnManager_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            ActiveControl = lvConn;
            LoadConfig();
            FillConnList();
        }

        private void btnNewConn_Click(object sender, EventArgs e)
        {
            // add new connection
            lvConn.Items
                .Add(CreateConnectionItem(new ConnectionOptions
                {
                    Name = CommonPhrases.NewConnection
                }))
                .Selected = true;

            txtName.Focus();
        }

        private void btnDeleteConn_Click(object sender, EventArgs e)
        {
            // delete selected connection
            lvConn.RemoveSelectedItem();
        }

        private void lvConn_SelectedIndexChanged(object sender, EventArgs e)
        {
            // show selected connection options
            changing = true;
            GetSelectedItem(out _, out ConnectionOptions connectionOptions);
            ShowConnOptions(connectionOptions);
            changing = false;

            // enable or disable button
            btnDeleteConn.Enabled = connectionOptions != null;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            // update selected connection name
            if (!changing && GetSelectedItem(out ListViewItem item, out ConnectionOptions connectionOptions))
            {
                connectionOptions.Name = txtName.Text;

                item.Text = string.IsNullOrEmpty(txtName.Text)
                    ? CommonPhrases.UnnamedConnection
                    : txtName.Text;
            }
        }

        private void txtUrl_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out _, out ConnectionOptions connectionOptions))
                connectionOptions.Url = txtUrl.Text;
        }

        private void txtToken_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out _, out ConnectionOptions connectionOptions))
                connectionOptions.Token = txtToken.Text;
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out _, out ConnectionOptions connectionOptions))
                connectionOptions.Username = txtUsername.Text;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out _, out ConnectionOptions connectionOptions))
                connectionOptions.Password = txtPassword.Text;
        }

        private void txtBucket_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out _, out ConnectionOptions connectionOptions))
                connectionOptions.Bucket = txtBucket.Text;
        }

        private void txtOrg_TextChanged(object sender, EventArgs e)
        {
            if (!changing && GetSelectedItem(out _, out ConnectionOptions connectionOptions))
                connectionOptions.Org = txtOrg.Text;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            RetrieveConnections();

            if (SaveConfig())
                DialogResult = DialogResult.OK;
        }
    }
}

