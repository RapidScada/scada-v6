using Scada.Admin.Extensions.ExtImport.Controls;
using Scada.Admin.Extensions.ExtImport.Forms;
using Scada.Admin.Project;
using Scada.Comm.Config;
using Scada.Forms;
using System;
using System.Collections;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtImport.Code
{
	internal class TreeViewBuilder
	{
		private readonly IAdminContext adminContext;    // the Administrator context
		private readonly CtrlExtensionMenu menuControl; // contains the menus


		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		public TreeViewBuilder(IAdminContext adminContext, CtrlExtensionMenu menuControl)
		{
			this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
			this.menuControl = menuControl ?? throw new ArgumentNullException(nameof(menuControl));
		}

		/// <summary>
		/// Creates a tree node that represents communication lines.
		/// </summary>
		private TreeNode CreateLinesNode(CommApp commApp)
		{
			TreeNode linesNode = TreeViewExtensions.CreateNode(ExtensionPhrases.LinesNode, ImageKey.Lines);
			//linesNode.ContextMenuStrip = menuControl.LineMenu;
			linesNode.Tag = new CommNodeTag(commApp, commApp.AppConfig, CommNodeType.Lines);

			foreach (LineConfig lineConfig in commApp.AppConfig.Lines)
			{
				linesNode.Nodes.Add(CreateLineNode(commApp, lineConfig));
			}

			return linesNode;
		}

		/// <summary>
		/// Updates the tree node that represents communication lines and its child nodes.
		/// </summary>
		public void UpdateLinesNode(TreeNode linesNode)
		{
			ArgumentNullException.ThrowIfNull(linesNode, nameof(linesNode));

			try
			{
				linesNode.TreeView?.BeginUpdate();

				// remove existing line nodes
				foreach (TreeNode lineNode in new ArrayList(linesNode.Nodes))
				{
					lineNode.Remove();
				}

				// add new line nodes
				CommNodeTag nodeTag = (CommNodeTag)linesNode.Tag;
				CommApp commApp = nodeTag.CommApp;

				foreach (LineConfig lineConfig in commApp.AppConfig.Lines)
				{
					linesNode.Nodes.Add(CreateLineNode(commApp, lineConfig));
				}
			}
			finally
			{
				linesNode.TreeView?.EndUpdate();
			}
		}

		/// <summary>
		/// Creates a tree node that represents the specified communication line.
		/// </summary>
		public TreeNode CreateLineNode(CommApp commApp, LineConfig lineConfig)
		{
			TreeNode lineNode = TreeViewExtensions.CreateNode(lineConfig.Title,
				lineConfig.Active ? ImageKey.Line : ImageKey.LineInactive);
			//lineNode.ContextMenuStrip = menuControl.LineMenu;
			lineNode.Tag = new CommNodeTag(commApp, lineConfig, CommNodeType.Line);

			lineNode.Nodes.AddRange(new TreeNode[]
			{
				new TreeNode(ExtensionPhrases.LineOptionsNode)
				{
					ImageKey = ImageKey.Options,
					SelectedImageKey = ImageKey.Options,
					Tag = new CommNodeTag(commApp, null, CommNodeType.LineOptions)
					{
						//FormType = typeof(FrmLineConfig),
						FormArgs = new object[] { adminContext, commApp, lineConfig }
					}
				},
				new TreeNode(ExtensionPhrases.LineStatsNode)
				{
					ImageKey = ImageKey.Stats,
					SelectedImageKey = ImageKey.Stats,
					Tag = new CommNodeTag(commApp, null, CommNodeType.LineStats)
					{
						//FormType = typeof(FrmLineStats),
						FormArgs = new object[] { adminContext, lineConfig }
					}
				}
			});

			foreach (DeviceConfig deviceConfig in lineConfig.DevicePolling)
			{
				lineNode.Nodes.Add(CreateDeviceNode(commApp, deviceConfig));
			}

			return lineNode;
		}

		/// <summary>
		/// Updates the tree node that represents a communication line and its child nodes.
		/// </summary>
		public void UpdateLineNode(TreeNode lineNode)
		{
			ArgumentNullException.ThrowIfNull(lineNode, nameof(lineNode));

			try
			{
				lineNode.TreeView?.BeginUpdate();

				// update line text and image
				CommNodeTag nodeTag = (CommNodeTag)lineNode.Tag;
				LineConfig lineConfig = (LineConfig)nodeTag.RelatedObject;
				lineNode.Text = lineConfig.Title;
				lineNode.SetImageKey(lineConfig.Active ? ImageKey.Line : ImageKey.LineInactive);

				// remove existing device nodes
				foreach (TreeNode lineSubnode in new ArrayList(lineNode.Nodes))
				{
					if (lineSubnode.TagIs(CommNodeType.Device))
						lineSubnode.Remove();
				}

				// add new device nodes
				foreach (DeviceConfig deviceConfig in lineConfig.DevicePolling)
				{
					lineNode.Nodes.Add(CreateDeviceNode(nodeTag.CommApp, deviceConfig));
				}
			}
			finally
			{
				lineNode.TreeView?.EndUpdate();
			}
		}

		/// <summary>
		/// Updates text of the tree node that represents a communication line.
		/// </summary>
		public static void UpdateLineNodeText(TreeNode lineNode)
		{
			ArgumentNullException.ThrowIfNull(lineNode, nameof(lineNode));
			CommNodeTag nodeTag = (CommNodeTag)lineNode.Tag;
			LineConfig lineConfig = (LineConfig)nodeTag.RelatedObject;
			lineNode.Text = lineConfig.Title;
		}

		/// <summary>
		/// Creates a tree node that represents the device.
		/// </summary>
		public TreeNode CreateDeviceNode(CommApp commApp, DeviceConfig deviceConfig)
		{
			return new TreeNode(deviceConfig.Title)
			{
				ImageKey = ImageKey.Device,
				SelectedImageKey = ImageKey.Device,
				//ContextMenuStrip = menuControl.DeviceMenu,
				Tag = new CommNodeTag(commApp, deviceConfig, CommNodeType.Device)
				{
					//FormType = typeof(FrmDeviceData),
					FormArgs = new object[] { adminContext, commApp, deviceConfig }
				}
			};
		}

		/// <summary>
		/// Updates text of the tree node that represents a device.
		/// </summary>
		public static void UpdateDeviceNodeText(TreeNode deviceNode)
		{
			ArgumentNullException.ThrowIfNull(deviceNode, nameof(deviceNode));
			CommNodeTag nodeTag = (CommNodeTag)deviceNode.Tag;
			DeviceConfig deviceConfig = (DeviceConfig)nodeTag.RelatedObject;
			deviceNode.Text = deviceConfig.Title;
		}

		/// <summary>
		/// Creates tree nodes for the explorer tree.
		/// </summary>
		public TreeNode[] CreateTreeNodes(CommApp commApp)
		{
			ArgumentNullException.ThrowIfNull(commApp, nameof(commApp));

			return new TreeNode[]
			{
				new TreeNode(ExtensionPhrases.GeneralOptionsNode)
				{
					ImageKey = ImageKey.Options,
					SelectedImageKey = ImageKey.Options,
					Tag = new CommNodeTag(commApp, null, CommNodeType.GeneralOptions)
					{
						//FormType = typeof(FrmGeneralOptions),
						FormArgs = new object[] { adminContext.ErrLog, commApp }
					}
				},
				new TreeNode(ExtensionPhrases.DriversNode)
				{
					ImageKey = ImageKey.Driver,
					SelectedImageKey = ImageKey.Driver,
					Tag = new CommNodeTag(commApp, null, CommNodeType.Drivers)
					{
						//FormType = typeof(FrmDrivers),
						FormArgs = new object[] { adminContext, commApp }
					}
				},
				new TreeNode(ExtensionPhrases.DataSourcesNode)
				{
					ImageKey = ImageKey.DataSource,
					SelectedImageKey = ImageKey.DataSource,
					Tag = new CommNodeTag(commApp, null, CommNodeType.DataSources)
					{
						//FormType = typeof(FrmDataSources),
						FormArgs = new object[] { adminContext, commApp }
					}
				},
				CreateLinesNode(commApp),
				new TreeNode(ExtensionPhrases.LogsNode)
				{
					ImageKey = ImageKey.Stats,
					SelectedImageKey = ImageKey.Stats,
					Tag = new CommNodeTag(commApp, null, CommNodeType.Logs)
					{
						//FormType = typeof(FrmCommLogs),
						FormArgs = new object[] { adminContext }
					}
				}
			};
		}
	}
}