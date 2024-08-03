// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtProjectTools.Code;
using Scada.Admin.Extensions.ExtProjectTools.Properties;
using Scada.Admin.Project;
using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Forms;
using Scada.Lang;
using WinControls;

namespace Scada.Admin.Extensions.ExtProjectTools.Forms
{
    /// <summary>
    /// Represents a right matrix form.
    /// <para>Представляет форму матрицы прав.</para>
    /// </summary>
    public partial class FrmRightMatrix : Form, IChildForm
    {
        private readonly IAdminContext adminContext;    // the application context
        private readonly ConfigDatabase configDatabase; // the configuration database


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmRightMatrix()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmRightMatrix(IAdminContext adminContext, ConfigDatabase configDatabase)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.configDatabase = configDatabase ?? throw new ArgumentNullException(nameof(configDatabase));

            ChildFormTag = new ChildFormTag(new ChildFormOptions { Image = Resources.matrix });
            ChildFormTag.MessageToChildForm += ChildFormTag_MessageToChildForm;
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Shows the right matrix.
        /// </summary>
        private void ShowData()
        {
            try
            {
                lvMatrix.BeginUpdate();

                // add roles to the header
                List<Role> customRoles = configDatabase.RoleTable.Where(r => !RoleID.IsBuiltIn(r.RoleID)).ToList();

                foreach (Role role in customRoles)
                {
                    string roleCaption = string.Format(CommonPhrases.EntityCaption, role.RoleID, role.Name);
                    ColumnHeader column = lvMatrix.Columns.Add(roleCaption);
                    column.TextAlign = HorizontalAlignment.Center;
                }

                // add objects and rights
                RightMatrix rightMatrix = new(configDatabase);

                foreach (Obj obj in configDatabase.ObjTable)
                {
                    List<string> cells = new(customRoles.Count + 1)
                    {
                        string.Format(CommonPhrases.EntityCaption, obj.ObjNum, obj.Name)
                    };

                    foreach (Role role in customRoles)
                    {
                        Right right = rightMatrix.GetRight(role.RoleID, obj.ObjNum);
                        cells.Add(GetRightText(right));
                    }

                    ListViewItem item = new([.. cells]);
                    lvMatrix.Items.Add(item);
                }

                lvMatrix.Items.Add("");
                lvMatrix.Items.Add(ExtensionPhrases.ViewRightLegend);
                lvMatrix.Items.Add(ExtensionPhrases.ControlRightLegend);

                ColumnHeader tempColumn = lvMatrix.Columns.Add(""); // for correct sizing
                lvMatrix.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                lvMatrix.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
                lvMatrix.Columns.Remove(tempColumn);
            }
            catch (Exception ex)
            {
                adminContext.ErrLog.HandleError(ex, ExtensionPhrases.DisplayMatrixError);
            }
            finally
            {
                lvMatrix.EndUpdate();
            }
        }

        /// <summary>
        /// Gets a text description of the specified right.
        /// </summary>
        private static string GetRightText(Right right)
        {
            if (right.View && right.Control)
                return ExtensionPhrases.ViewRightSymbol + ", " + ExtensionPhrases.ControlRightSymbol;
            else if (right.View)
                return ExtensionPhrases.ViewRightSymbol;
            else if (right.Control)
                return ExtensionPhrases.ControlRightSymbol;
            else
                return "";
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        public void Save()
        {
            // do nothing
        }


        private void ChildFormTag_MessageToChildForm(object sender, FormMessageEventArgs e)
        {
            if (e.Message == AdminMessage.BaseReload)
                ShowData();
        }

        private void FrmRightMatrix_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            ShowData();
        }
    }
}
