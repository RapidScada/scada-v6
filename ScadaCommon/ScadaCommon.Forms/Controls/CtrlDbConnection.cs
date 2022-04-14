// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Scada.Forms.Controls
{
    /// <summary>
    /// Represents a control for editing database connection options.
    /// <para>Представляет элемент управления для редактирования параметров соединения с БД.</para>
    /// </summary>
    public partial class CtrlDbConnection : UserControl
    {
        private DbConnectionOptions connectionOptions;
        private bool nameEnabled;
        private bool dbmsEnabled;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlDbConnection()
        {
            InitializeComponent();

            connectionOptions = null;
            nameEnabled = txtName.Enabled;
            dbmsEnabled = cbDbms.Enabled;
        }


        /// <summary>
        /// Gets or sets the database connection options being edited.
        /// </summary>
        [Browsable(false)]
        public DbConnectionOptions ConnectionOptions
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
                    txtServer.Text = "";
                    txtDatabase.Text = "";
                    txtUsername.Text = "";
                    txtPassword.Text = "";
                    chkConnectionString.Checked = false;
                    txtConnectionString.Text = "";
                }
                else
                {
                    gbConnectionOptions.Enabled = true;
                    txtName.Text = value.Name;
                    cbDbms.SelectedIndex = (int)value.KnownDBMS;
                    txtServer.Text = value.Server;
                    txtDatabase.Text = value.Database;
                    txtUsername.Text = value.Username;
                    txtPassword.Text = value.Password;

                    if (string.IsNullOrEmpty(value.ConnectionString))
                    {
                        SetFieldsReadOnly(false);
                        chkConnectionString.Checked = false;
                        txtConnectionString.Text = BuildConnectionString(value);
                    }
                    else
                    {
                        SetFieldsReadOnly(true);
                        chkConnectionString.Checked = true;
                        txtConnectionString.Text = value.ConnectionString;
                    }
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
        /// Gets or sets a value indicating whether the DBMS control can respond to user interaction.
        /// </summary>
        public bool DbmsEnabled
        {
            get
            {
                return dbmsEnabled;
            }
            set
            {
                dbmsEnabled = value;
                cbDbms.Enabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the function that builds a connection string.
        /// </summary>
        [Browsable(false)]
        public Func<DbConnectionOptions, string> BuildConnectionStringFunc { get; set; }


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
        /// Builds the connection string.
        /// </summary>
        private string BuildConnectionString(DbConnectionOptions options)
        {
            return options == null || BuildConnectionStringFunc == null
                ? ""
                : BuildConnectionStringFunc(options);
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

        private void cbDbms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                connectionOptions.KnownDBMS = (KnownDBMS)cbDbms.SelectedIndex;
                txtConnectionString.Text = BuildConnectionString(connectionOptions);
                OnConnectionOptionsChanged();
            }
        }

        private void txtServer_TextChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                connectionOptions.Server = txtServer.Text;
                txtConnectionString.Text = BuildConnectionString(connectionOptions);
                OnConnectionOptionsChanged();
            }
        }

        private void txtDatabase_TextChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                connectionOptions.Database = txtDatabase.Text;
                txtConnectionString.Text = BuildConnectionString(connectionOptions);
                OnConnectionOptionsChanged();
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                connectionOptions.Username = txtUsername.Text;
                txtConnectionString.Text = BuildConnectionString(connectionOptions);
                OnConnectionOptionsChanged();
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                connectionOptions.Password = txtPassword.Text;
                txtConnectionString.Text = BuildConnectionString(connectionOptions);
                OnConnectionOptionsChanged();
            }
        }

        private void chkConnectionString_CheckedChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                if (chkConnectionString.Checked)
                {
                    SetFieldsReadOnly(true);
                    connectionOptions.ConnectionString = txtConnectionString.Text;
                }
                else
                {
                    SetFieldsReadOnly(false);
                    connectionOptions.ConnectionString = "";
                }

                OnConnectionOptionsChanged();
            }
        }

        private void txtConnectionString_TextChanged(object sender, EventArgs e)
        {
            if (connectionOptions != null)
            {
                connectionOptions.ConnectionString = chkConnectionString.Checked ? txtConnectionString.Text : "";
                OnConnectionOptionsChanged();
            }
        }
    }
}
