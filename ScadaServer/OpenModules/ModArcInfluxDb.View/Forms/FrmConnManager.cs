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
        private readonly string configFileName;     // the module configuration file name
        private readonly ModuleConfig moduleConfig; // the module configuration
        private bool sortRequired;                  // indicated whether to sort the list

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmConnManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmConnManager(string configDir)
            : this()
        {
            configFileName = Path.Combine(configDir, ModuleConfig.ConfigFileName);
            moduleConfig = new ModuleConfig();
            sortRequired = false;
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
                lvConn.Items[0].Selected = true;
        }
        
        /// <summary>
        /// Fills the connection options according to the select connection item.
        /// </summary>
        private void FillConnOptions(ListViewItem listViewItem)
        {
            ConnectionOptions options = listViewItem?.Tag as ConnectionOptions;

            txtName.Text = options.Name;
            txtUrl.Text = options.Url;
            txtToken.Text = options.Token;
            txtUsername.Text = options.Username;
            txtPassword.Text = options.Password;
            txtBucket.Text = options.Bucket;
            txtOrg.Text = options.Org;
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
            
            if (lvConn.Items.Count > 0)
                FillConnOptions(lvConn.SelectedItems[0]);
        }

        private void lvConn_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem listViewItem = lvConn.GetSelectedItem();
            
            if (listViewItem != null)
                FillConnOptions(listViewItem);
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

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            // update selected connection name
            sortRequired = true;

            if (lvConn.GetSelectedItem() is ListViewItem listViewItem)
            {
                ConnectionOptions options = (ConnectionOptions)listViewItem.Tag;
                options.Name = txtName.Text;

                listViewItem.Text = string.IsNullOrEmpty(txtName.Text)
                    ? CommonPhrases.UnnamedConnection
                    : txtName.Text;
            }
        }

        private void txtUrl_TextChanged(object sender, EventArgs e)
        {
            sortRequired = true;

            if (lvConn.GetSelectedItem() is ListViewItem listViewItem)
            {
                ConnectionOptions options = (ConnectionOptions)listViewItem.Tag;
                options.Url = txtUrl.Text;
            }
        }

        private void txtToken_TextChanged(object sender, EventArgs e)
        {
            sortRequired = true;

            if (lvConn.GetSelectedItem() is ListViewItem listViewItem)
            {
                ConnectionOptions options = (ConnectionOptions)listViewItem.Tag;
                options.Token = txtToken.Text;
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            sortRequired = true;

            if (lvConn.GetSelectedItem() is ListViewItem listViewItem)
            {
                ConnectionOptions options = (ConnectionOptions)listViewItem.Tag;
                options.Username = txtUsername.Text;
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            sortRequired = true;

            if (lvConn.GetSelectedItem() is ListViewItem listViewItem)
            {
                ConnectionOptions options = (ConnectionOptions)listViewItem.Tag;
                options.Password = txtPassword.Text;
            }
        }

        private void txtBucket_TextChanged(object sender, EventArgs e)
        {
            sortRequired = true;

            if (lvConn.GetSelectedItem() is ListViewItem listViewItem)
            {
                ConnectionOptions options = (ConnectionOptions)listViewItem.Tag;
                options.Bucket = txtBucket.Text;
            }
        }

        private void txtOrg_TextChanged(object sender, EventArgs e)
        {
            if (lvConn.GetSelectedItem() is ListViewItem listViewItem)
            {
                ConnectionOptions options = (ConnectionOptions)listViewItem.Tag;
                options.Org = txtOrg.Text;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            RetrieveConnections();

            if (SaveConfig())
                DialogResult = DialogResult.OK;
        }
    }
}
