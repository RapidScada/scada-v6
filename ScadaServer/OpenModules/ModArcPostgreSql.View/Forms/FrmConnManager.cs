// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using Scada.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Scada.Server.Modules.ModArcPostgreSql.View.Forms
{
    /// <summary>
    /// Represents a connection manager form.
    /// <para>Представляет форму менеджера соединений.</para>
    /// </summary>
    public partial class FrmConnManager : Form
    {
        /// <summary>
        /// Represents a connection item.
        /// </summary>
        private class ConnectionItem
        {
            public DbConnectionOptions ConnectionOptions { get; init; }
            public override string ToString() => 
                string.IsNullOrEmpty(ConnectionOptions.Name) ? "<Unnamed connection>" : ConnectionOptions.Name;
        }

        private readonly string configFileName;      // the module configuration file name
        private readonly ModuleConfig moduleConfig;  // the module configuration
        private ConnectionItem selectedItem;         // the currently selected connection item
        private bool sortRequired;                   // indicated whether to sort the list


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
            selectedItem = null;
            sortRequired = false;
        }


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
        /// Fills the connection list box.
        /// </summary>
        private void FillConnList()
        {
            lbConn.BeginUpdate();
            lbConn.Items.Clear();

            foreach (KeyValuePair<string, DbConnectionOptions> pair in moduleConfig.Connections)
            {
                lbConn.Items.Add(new ConnectionItem { ConnectionOptions = pair.Value });
            }

            lbConn.EndUpdate();

            if (lbConn.Items.Count > 0)
                lbConn.SelectedIndex = 0;
        }

        /// <summary>
        /// Shows the connection options.
        /// </summary>
        private void ShowConnectionOptions(ConnectionItem item)
        {
            if (item == null)
            {
                gbConnOptions.Enabled = false;
                txtName.Text = "";
                txtServer.Text = "";
                txtDatabase.Text = "";
                txtUsername.Text = "";
                txtPassword.Text = "";
                chkConnectionString.Checked = false;
                txtConnectionString.Text = "";
            }
            else
            {
                DbConnectionOptions options = item.ConnectionOptions;
                gbConnOptions.Enabled = true;
                txtName.Text = options.Name;

                if (string.IsNullOrEmpty(options.ConnectionString))
                {
                    txtServer.Text = options.Server;
                    txtDatabase.Text = options.Database;
                    txtUsername.Text = options.Username;
                    txtPassword.Text = options.Password;
                    chkConnectionString.Checked = false;
                }
                else
                {
                    chkConnectionString.Checked = true;
                    txtConnectionString.Text = options.ConnectionString;
                }
            }
        }

        /// <summary>
        /// Builds the connection string.
        /// </summary>
        private string BuildConnectionString(ConnectionItem item)
        {
            if (item?.ConnectionOptions is DbConnectionOptions options &&
                options.KnownDBMS == KnownDBMS.PostgreSQL)
            {
                return "aaa";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Refreshes the text of the selected list item.
        /// </summary>
        private void RefreshSelectedItem()
        {
            if (lbConn.SelectedIndex >= 0)
                lbConn.Items[lbConn.SelectedIndex] = lbConn.Items[lbConn.SelectedIndex];
        }


        private void FrmConnManager_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            LoadConfig();
            FillConnList();
        }

        private void btnNewConn_Click(object sender, EventArgs e)
        {
            // add new connection
            lbConn.SelectedIndex = lbConn.Items.Add(new ConnectionItem
            {
                ConnectionOptions = new DbConnectionOptions
                {
                    Name = "New Connection",
                    KnownDBMS = KnownDBMS.PostgreSQL
                }
            });

            txtName.Focus();
        }

        private void btnDeleteConn_Click(object sender, EventArgs e)
        {
            // delete selected connection
            int index = lbConn.SelectedIndex;

            if (index >= 0)
            {
                lbConn.Items.RemoveAt(index);

                if (lbConn.Items.Count > 0)
                    lbConn.SelectedIndex = Math.Min(index, lbConn.Items.Count - 1);
            }
        }

        private void lbConn_SelectedIndexChanged(object sender, EventArgs e)
        {
            // show selected connection options
            ConnectionItem item = lbConn.SelectedItem as ConnectionItem;
            selectedItem = null;
            ShowConnectionOptions(item);
            selectedItem = item;

            // enable or disable button
            btnDeleteConn.Enabled = lbConn.SelectedItem != null;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            // update selected connection name
            if (selectedItem != null)
            {
                selectedItem.ConnectionOptions.Name = txtName.Text;
                sortRequired = true;
                RefreshSelectedItem();
            }
        }

        private void txtName_Validated(object sender, EventArgs e)
        {
            // update sort
            if (sortRequired)
            {
                sortRequired = false;
                object item = selectedItem;

                if (item != null)
                {
                    lbConn.Items.Remove(item);
                    lbConn.SelectedIndex = lbConn.Items.Add(item);
                }
            }
        }

        private void txtServer_TextChanged(object sender, EventArgs e)
        {
            if (selectedItem != null)
                selectedItem.ConnectionOptions.Server = txtServer.Text;
        }

        private void txtDatabase_TextChanged(object sender, EventArgs e)
        {
            if (selectedItem != null)
                selectedItem.ConnectionOptions.Database = txtDatabase.Text;
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            if (selectedItem != null)
                selectedItem.ConnectionOptions.Username = txtUsername.Text;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (selectedItem != null)
                selectedItem.ConnectionOptions.Password = txtPassword.Text;
        }

        private void chkConnectionString_CheckedChanged(object sender, EventArgs e)
        {
            if (chkConnectionString.Checked)
            {
                txtServer.Text = "";
                txtDatabase.Text = "";
                txtUsername.Text = "";
                txtPassword.Text = "";
                txtConnectionString.ReadOnly = false;
            }
            else
            {
                txtConnectionString.Text = BuildConnectionString(selectedItem);
                txtConnectionString.ReadOnly = true;
            }
        }

        private void txtConnectionString_TextChanged(object sender, EventArgs e)
        {
            if (selectedItem != null)
                selectedItem.ConnectionOptions.ConnectionString = txtConnectionString.Text;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (SaveConfig())
                DialogResult = DialogResult.OK;
        }
    }
}
