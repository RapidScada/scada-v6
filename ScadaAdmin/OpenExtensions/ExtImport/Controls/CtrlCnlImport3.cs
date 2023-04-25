using Scada.Admin.Config;
using Scada.Admin.Project;
using Scada.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Controls
{
	public partial class CtrlCnlImport3 : UserControl
	{
		private IAdminContext adminContext; // the Administrator context
		private ScadaProject project;       // the project under development
		private int lastStartCnlNum;        // the last calculated start channel number
		private int lastCnlCnt;             // the last specified number of channels

		public CtrlCnlImport3()
		{
			InitializeComponent();
		}

		public string DeviceName
		{
			get
			{
				return txtDevice.Text;
			}
			set
			{
				txtDevice.Text = value ?? "";
			}
		}

		public int StartCnlNum => Convert.ToInt32(numStartCnlNum.Value);

		private bool CalcStartCnlNum(int cnlCnt, out int startCnlNum)
		{
			ChannelNumberingOptions options = adminContext.AppConfig.ChannelNumberingOptions;
			startCnlNum = options.Multiplicity + options.Shift;
			int prevCnlNum = 0;

			foreach (int cnlNum in project.ConfigDatabase.CnlTable.EnumerateKeys())
			{
				if (prevCnlNum < cnlNum - options.Multiplicity && startCnlNum <= prevCnlNum)
				{
					break;
				}
				prevCnlNum = cnlNum;
				startCnlNum = Math.Max(startCnlNum, cnlNum + options.Multiplicity);
			}

			return startCnlNum + (cnlCnt - 1) * options.Multiplicity <= options.MaxCnlNum;
		}

		public void Init(IAdminContext adminContext, ScadaProject project)
		{
			this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
			this.project = project ?? throw new ArgumentNullException(nameof(project));
		}

		public bool ValidateChannels(List<int> errors)
		{
			int cnlCnt = Convert.ToInt32(numCnlCnt.Value);

			if (cnlCnt > 0)
			{
				if (lastCnlCnt != cnlCnt)
				{
					CalcStartCnlNum(cnlCnt, out lastStartCnlNum);
					lastCnlCnt = cnlCnt;
				}

				numStartCnlNum.Value = lastStartCnlNum;
			}
			else
			{
				errors.Add(0);
			}

			return errors.Count == 0;
		}

		private void numCnlCnt_ValueChanged(object sender, EventArgs e)
		{
			CalcStartCnlNum(Convert.ToInt32(numCnlCnt.Value), out lastStartCnlNum);
			numStartCnlNum.Value = lastStartCnlNum;
		}
	}
}
