// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Npgsql;
using Scada.Config;
using Scada.Forms;
using Scada.Lang;
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
        private readonly string configFileName;      // the module configuration file name
        private readonly ModuleConfig moduleConfig;  // the module configuration
        private DbConnectionOptions selectedOptions; // the currently selected connection options
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
            selectedOptions = null;
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
        /// Fills the connection list according to the configuration.
        /// </summary>
        private void FillConnList()
        {
            lvConn.BeginUpdate();
            lvConn.Items.Clear();

            foreach (KeyValuePair<string, DbConnectionOptions> pair in moduleConfig.Connections)
            {
                lvConn.Items.Add(CreateConnectionItem(pair.Value));
            }

            lvConn.EndUpdate();

            if (lvConn.Items.Count > 0)
                lvConn.Items[0].Selected = true;
        }

        /// <summary>
        /// Retrieves the connections from the connection list to the module configuration.
        /// </summary>
        private void RetrieveConnections()
        {
            moduleConfig.Connections.Clear();

            foreach (ListViewItem item in lvConn.Items)
            {
                DbConnectionOptions options = (DbConnectionOptions)item.Tag;
                moduleConfig.Connections[options.Name] = options;
            }
        }

        /// <summary>
        /// Shows the connection options.
        /// </summary>
        private void ShowConnectionOptions(DbConnectionOptions options)
        {
            if (options == null)
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
                gbConnOptions.Enabled = true;
                txtName.Text = options.Name;
                txtServer.Text = options.Server;
                txtDatabase.Text = options.Database;
                txtUsername.Text = options.Username;
                txtPassword.Text = options.Password;

                if (string.IsNullOrEmpty(options.ConnectionString))
                {
                    SetFieldsReadOnly(false);
                    chkConnectionString.Checked = false;
                    txtConnectionString.Text = BuildConnectionString(options);
                }
                else
                {
                    SetFieldsReadOnly(true);
                    chkConnectionString.Checked = true;
                    txtConnectionString.Text = options.ConnectionString;
                }
            }
        }

        /// <summary>
        /// Sets the ReadOnly property of the textboxes depending on connection string usage.
        /// </summary>
        private void SetFieldsReadOnly(bool useConnectionString)
        {
            txtServer.ReadOnly = useConnectionString;
            txtDatabase.ReadOnly = useConnectionString;
            txtUsername.ReadOnly = useConnectionString;
            txtPassword.ReadOnly = useConnectionString;
            txtConnectionString.ReadOnly = !useConnectionString;
        }

        /// <summary>
        /// Creates a new list view item that represents the specified connection.
        /// </summary>
        private static ListViewItem CreateConnectionItem(DbConnectionOptions options)
        {
            return new ListViewItem
            {
                Text = string.IsNullOrEmpty(options.Name) ? ModulePhrases.UnnamedConnection : options.Name,
                Tag = options
            };
        }

        /// <summary>
        /// Builds the connection string.
        /// </summary>
        private static string BuildConnectionString(DbConnectionOptions options)
        {
            if (options == null)
                return "";

            ScadaUtils.RetrieveHostAndPort(options.Server, NpgsqlConnection.DefaultPort, 
                out string host, out int port);

            return new NpgsqlConnectionStringBuilder
            {
                Host = host,
                Port = port,
                Database = options.Database,
                Username = options.Username,
                Password = CommonPhrases.HiddenPassword
            }
            .ToString();
        }


        private void FrmConnManager_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            LoadConfig();
            FillConnList();
            ActiveControl = lvConn;
        }

        private void btnNewConn_Click(object sender, EventArgs e)
        {
            // add new connection
            lvConn.Items
                .Add(CreateConnectionItem(new DbConnectionOptions
                {
                    Name = ModulePhrases.NewConnection,
                    KnownDBMS = KnownDBMS.PostgreSQL
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
            DbConnectionOptions options = lvConn.GetSelectedItem()?.Tag as DbConnectionOptions;
            selectedOptions = null;
            ShowConnectionOptions(options);
            selectedOptions = options;

            // enable or disable button
            btnDeleteConn.Enabled = options != null;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            // update selected connection name
            if (selectedOptions != null)
            {
                selectedOptions.Name = txtName.Text;
                sortRequired = true;

                if (lvConn.GetSelectedItem() is ListViewItem listViewItem)
                {
                    listViewItem.Text = string.IsNullOrEmpty(selectedOptions.Name) ?
                        ModulePhrases.UnnamedConnection : selectedOptions.Name;
                }
            }
        }

        private void txtName_Validated(object sender, EventArgs e)
        {
            // update sort
            if (sortRequired)
            {
                sortRequired = false;
                lvConn.Sort();
            }
        }

        private void txtServer_TextChanged(object sender, EventArgs e)
        {
            if (selectedOptions != null)
            {
                selectedOptions.Server = txtServer.Text;
                txtConnectionString.Text = BuildConnectionString(selectedOptions);
            }
        }

        private void txtDatabase_TextChanged(object sender, EventArgs e)
        {
            if (selectedOptions != null)
            {
                selectedOptions.Database = txtDatabase.Text;
                txtConnectionString.Text = BuildConnectionString(selectedOptions);
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            if (selectedOptions != null)
            {
                selectedOptions.Username = txtUsername.Text;
                txtConnectionString.Text = BuildConnectionString(selectedOptions);
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (selectedOptions != null)
            {
                selectedOptions.Password = txtPassword.Text;
                txtConnectionString.Text = BuildConnectionString(selectedOptions);
            }
        }

        private void chkConnectionString_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedOptions != null)
            {
                if (chkConnectionString.Checked)
                {
                    SetFieldsReadOnly(true);
                    selectedOptions.ConnectionString = txtConnectionString.Text;
                }
                else
                {
                    SetFieldsReadOnly(false);
                    selectedOptions.ConnectionString = "";
                }
            }
        }

        private void txtConnectionString_TextChanged(object sender, EventArgs e)
        {
            if (selectedOptions != null)
                selectedOptions.ConnectionString = chkConnectionString.Checked ? txtConnectionString.Text : "";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            RetrieveConnections();

            if (SaveConfig())
                DialogResult = DialogResult.OK;
        }
    }
}
