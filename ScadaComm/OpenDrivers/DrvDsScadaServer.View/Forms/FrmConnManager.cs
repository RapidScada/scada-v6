// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Client;
using Scada.Comm.Drivers.DrvDsScadaServer.Config;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvDsScadaServer.View.Forms
{
    /// <summary>
    /// Represents a connection manager form.
    /// <para>Представляет форму менеджера соединений.</para>
    /// </summary>
    public partial class FrmConnManager : Form
    {
        private readonly string configFileName;     // the driver configuration file name
        private readonly DriverConfig driverConfig; // the driver configuration
        private bool sortRequired;                  // indicated whether to sort the list


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
            configFileName = Path.Combine(configDir, DriverConfig.DefaultFileName);
            driverConfig = new DriverConfig();
            sortRequired = false;
        }


        /// <summary>
        /// Gets the connection names.
        /// </summary>
        public string[] ConnectionNames => driverConfig.Connections.Keys.ToArray();


        /// <summary>
        /// Loads the driver configuration.
        /// </summary>
        private void LoadConfig()
        {
            if (File.Exists(configFileName) && !driverConfig.Load(configFileName, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);
        }

        /// <summary>
        /// Saves the driver configuration.
        /// </summary>
        private bool SaveConfig()
        {
            if (driverConfig.Save(configFileName, out string errMsg))
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

            foreach (KeyValuePair<string, ConnectionOptions> pair in driverConfig.Connections)
            {
                lvConn.Items.Add(CreateConnectionItem(pair.Value));
            }

            lvConn.EndUpdate();

            if (lvConn.Items.Count > 0)
                lvConn.Items[0].Selected = true;
        }

        /// <summary>
        /// Retrieves the connections from the connection list to the driver configuration.
        /// </summary>
        private void RetrieveConnections()
        {
            driverConfig.Connections.Clear();

            foreach (ListViewItem item in lvConn.Items)
            {
                ConnectionOptions options = (ConnectionOptions)item.Tag;
                driverConfig.Connections[options.Name] = options;
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
            FormTranslator.Translate(ctrlClientConnection, ctrlClientConnection.GetType().FullName);
            ActiveControl = lvConn;

            LoadConfig();
            FillConnList();
        }

        private void btnNewConn_Click(object sender, EventArgs e)
        {
            // add new connection
            lvConn.Items
                .Add(CreateConnectionItem(new ConnectionOptions { Name = CommonPhrases.NewConnection }))
                .Selected = true;

            ctrlClientConnection.Focus();
        }

        private void btnDeleteConn_Click(object sender, EventArgs e)
        {
            // delete selected connection
            lvConn.RemoveSelectedItem();
        }

        private void lvConn_SelectedIndexChanged(object sender, EventArgs e)
        {
            // show selected connection options
            ConnectionOptions options = lvConn.GetSelectedItem()?.Tag as ConnectionOptions;
            ctrlClientConnection.ConnectionOptions = options;

            // enable or disable button
            btnDeleteConn.Enabled = options != null;
        }

        private void ctrlClientConnection_NameChanged(object sender, EventArgs e)
        {
            // update selected connection name
            sortRequired = true;

            if (lvConn.GetSelectedItem() is ListViewItem listViewItem)
            {
                listViewItem.Text = string.IsNullOrEmpty(ctrlClientConnection.ConnectionOptions.Name)
                    ? CommonPhrases.UnnamedConnection
                    : ctrlClientConnection.ConnectionOptions.Name;
            }
        }

        private void ctrlClientConnection_NameValidated(object sender, EventArgs e)
        {
            // update sort
            if (sortRequired)
            {
                sortRequired = false;
                lvConn.Sort();
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
