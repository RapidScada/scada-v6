using Scada.Admin.Extensions.ExtImport.Code;
using Scada.Admin.Extensions.ExtImport.Forms;
using Scada.Admin.Lang;
using Scada.Admin.Project;
using Scada.Agent;
using Scada.Comm;
using Scada.Comm.Config;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Forms;
using Scada.Lang;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using WinControl;
using System.IO;


namespace Scada.Admin.Extensions.ExtImport.Controls
{
	public partial class CtrlExtensionMenu : UserControl
	{
		private readonly IAdminContext adminContext;      // the Administrator context
		private readonly RecentSelection recentSelection; // the recently selected objects
		private CtrlExtensionMenu()
		{
			InitializeComponent();
		}

		public CtrlExtensionMenu(IAdminContext adminContext) : this()
		{
		

			this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
			recentSelection = new RecentSelection();

			SetMenuItemsEnabled();
			adminContext.CurrentProjectChanged += AdminContext_CurrentProjectChanged;
			adminContext.MessageToExtension += AdminContext_MessageToExtension;
		}
		/// <summary>
		/// Gets the explorer tree.
		/// </summary>
		private TreeView ExplorerTree => adminContext.MainForm.ExplorerTree;

		/// <summary>
		/// Gets the selected node of the explorer tree.
		/// </summary>
		private TreeNode SelectedNode => adminContext.MainForm.SelectedNode;
		/// <summary>
		/// Gets the Communicator application from the selected node, and validates the node type.
		/// </summary>
		private bool GetCommApp(out CommApp commApp, params string[] allowedNodeTypes)
		{
			if (SelectedNode?.Tag is CommNodeTag commNodeTag &&
				(allowedNodeTypes == null || allowedNodeTypes.Contains(commNodeTag.NodeType)))
			{
				commApp = commNodeTag.CommApp;
				return true;
			}
			else
			{
				commApp = null;
				return false;
			}
		}
		/// <summary>
		/// Saves the Communicator configuration.
		/// </summary>
		private void SaveCommConfig(CommApp commApp)
		{
			if (!commApp.SaveConfig(out string errMsg))
				adminContext.ErrLog.HandleError(errMsg);
		}
		/// <summary>
		/// Refreshes an open child form that shows the communication line configuration.
		/// </summary>
		private void RefreshLineConfigForm(TreeNode lineNode)
		{
			if (lineNode.FindFirst(CommNodeType.LineOptions) is TreeNode lineOptionsNode &&
				lineOptionsNode.Tag is TreeNodeTag tag && tag.ExistingForm is IChildForm childForm)
			{
				childForm.ChildFormTag.SendMessage(this, AdminMessage.RefreshData);
			}
		}
		/// <summary>
		/// Updates the specified communication line node.
		/// </summary>
		private void UpdateLineNode(string instanceName, int commLineNum)
		{
			if (adminContext.MainForm.FindInstanceNode(instanceName, out bool justPrepared) is TreeNode instanceNode &&
				!justPrepared && instanceNode.FindFirst(CommNodeType.Lines) is TreeNode linesNode)
			{
				foreach (TreeNode lineNode in linesNode.Nodes)
				{
					if (lineNode.GetRelatedObject() is LineConfig lineConfig &&
						lineConfig.CommLineNum == commLineNum)
					{
						adminContext.MainForm.CloseChildForms(lineNode, false);
						new TreeViewBuilder(adminContext, this).UpdateLineNode(lineNode);
						break;
					}
				}
			}
		}
		private void AdminContext_CurrentProjectChanged(object sender, EventArgs e)
		{
			SetMenuItemsEnabled();
		}
		private void AdminContext_MessageToExtension(object sender, MessageEventArgs e)
		{
			if (e.Message == KnownExtensionMessage.UpdateLineNode)
			{
				UpdateLineNode(
					(string)e.Arguments["InstanceName"],
					(int)e.Arguments["CommLineNum"]);
			}
		}

		private void SetMenuItemsEnabled()
		{
			bool projectIsOpen = adminContext.CurrentProject != null;
			miImport.Enabled = btnImport.Enabled = projectIsOpen;
		}

		public ToolStripItem[] GetMainMenuItems()
		{
			return new ToolStripItem[] { miImport };
		}

		public ToolStripItem[] GetToobarButtons()
		{
			return new ToolStripItem[] { btnImport };
		}
		/// <summary>
		/// Finds a tree node that contains the specified related object.
		/// </summary>
		private static TreeNode FindNode(TreeNode startNode, object relatedObject)
		{
			foreach (TreeNode node in startNode.IterateNodes())
			{
				if (node.GetRelatedObject() == relatedObject)
					return node;
			}

			return null;
		}

		private void btnImport_Click(object sender, EventArgs e)
		{
			if (adminContext.CurrentProject != null)
			{
				FrmImport frmCnlImport = new(adminContext, adminContext.CurrentProject, recentSelection);
				if (frmCnlImport.ShowDialog() == DialogResult.OK)
					adminContext.MainForm.RefreshBaseTables(typeof(Cnl), true);

			}
		}

		
	}
}