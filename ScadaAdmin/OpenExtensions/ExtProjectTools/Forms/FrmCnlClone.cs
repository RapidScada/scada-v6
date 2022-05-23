// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtProjectTools.Code;
using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtProjectTools.Forms
{
    /// <summary>
    /// Represents a form for cloning channels.
    /// <para>Представляет форму для клонирования каналов.</para>
    /// </summary>
    public partial class FrmCnlClone : Form
    {
        /// <summary>
        /// The known functions that contain a channel number argument.
        /// </summary>
        private static readonly string[] KnownFunctions = { "N(", "Val(", "Stat(", "Data(", "Time(", "Deriv(", 
            "PrevVal(", "PrevStat(", "PrevData(", "PrevTime(", "SetVal(", "SetStat(", "SetData(" };
        /// <summary>
        /// The possible end symbols of an argument.
        /// </summary>
        private static readonly char[] ArgumentEnds = { ')', ',' };

        private readonly IAdminContext adminContext;    // the Administrator context
        private readonly ConfigDatabase configDatabase; // the configuration database


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmCnlClone()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmCnlClone(IAdminContext adminContext, ConfigDatabase configDatabase)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.configDatabase = configDatabase ?? throw new ArgumentNullException(nameof(configDatabase));
            ChannelsCloned = false;
        }


        /// <summary>
        /// Gets a value indicating whether channels have been cloned.
        /// </summary>
        public bool ChannelsCloned { get; private set; }


        /// <summary>
        /// Fills the combo box with the objects.
        /// </summary>
        private void FillObjList()
        {
            List<Obj> objList = new(configDatabase.ObjTable.ItemCount + 2);
            objList.Add(new Obj { ObjNum = -1, Name = ExtensionPhrases.KeepUnchanged });
            objList.Add(new Obj { ObjNum = 0, Name = " " });
            objList.AddRange(configDatabase.ObjTable.Enumerate().OrderBy(obj => obj.Name));

            cbReplaceObj.ValueMember = "ObjNum";
            cbReplaceObj.DisplayMember = "Name";
            cbReplaceObj.DataSource = objList;
            cbReplaceObj.SelectedValue = -1;
        }

        /// <summary>
        /// Fills the combo box with the devices.
        /// </summary>
        private void FillDeviceList()
        {
            List<Device> deviceList = new(configDatabase.DeviceTable.ItemCount + 2);
            deviceList.Add(new Device { DeviceNum = -1, Name = ExtensionPhrases.KeepUnchanged });
            deviceList.Add(new Device { DeviceNum = 0, Name = " " });
            deviceList.AddRange(configDatabase.DeviceTable.Enumerate().OrderBy(obj => obj.Name));

            cbReplaceDevice.ValueMember = "DeviceNum";
            cbReplaceDevice.DisplayMember = "Name";
            cbReplaceDevice.DataSource = deviceList;
            cbReplaceDevice.SelectedValue = -1;
        }

        /// <summary>
        /// Calculates the end destination channel number.
        /// </summary>
        private void CalcDestEndNum()
        {
            numDestEndNum.SetValue(numSrcEndNum.Value - numSrcStartNum.Value + numDestStartNum.Value);
        }

        /// <summary>
        /// Clones channels with the specified parameters.
        /// </summary>
        private bool CloneChannels(int srcStartNum, int srcEndNum, int destStartNum, 
            int replaceObjNum, int replaceDeviceNum, bool updateFormulas)
        {
            try
            {
                BaseTable<Cnl> cnlTable = configDatabase.CnlTable;
                int affectedRows = 0;

                if (srcStartNum <= srcEndNum)
                {
                    // create new channels
                    ExtensionUtils.NormalizeIdRange(ConfigDatabase.MinID, ConfigDatabase.MaxID, 
                        ref srcStartNum, ref srcEndNum, destStartNum, out int numOffset);
                    List<Cnl> cnlsToAdd = new(srcEndNum - srcStartNum + 1);

                    foreach (Cnl cnl in cnlTable.EnumerateItems())
                    {
                        if (cnl.CnlNum < srcStartNum)
                        {
                            continue;
                        }
                        else if (cnl.CnlNum > srcEndNum)
                        {
                            break;
                        }
                        else if (!cnlTable.PkExists(cnl.CnlNum + numOffset))
                        {
                            Cnl newCnl = ScadaUtils.DeepClone(cnl);
                            newCnl.CnlNum = cnl.CnlNum + numOffset;

                            if (replaceObjNum >= 0)
                                newCnl.ObjNum = replaceObjNum > 0 ? replaceObjNum : null;

                            if (replaceDeviceNum >= 0)
                                newCnl.DeviceNum = replaceDeviceNum > 0 ? replaceDeviceNum : null;

                            if (updateFormulas)
                            {
                                newCnl.InFormula = UpdateFormula(newCnl.InFormula, numOffset);
                                newCnl.OutFormula = UpdateFormula(newCnl.OutFormula, numOffset);
                            }

                            cnlsToAdd.Add(newCnl);
                        }
                    }

                    // add created channels
                    cnlsToAdd.ForEach(cnl => cnlTable.AddItem(cnl));
                    affectedRows = cnlsToAdd.Count;
                }

                if (affectedRows > 0)
                    cnlTable.Modified = true;

                ScadaUiUtils.ShowInfo(string.Format(ExtensionPhrases.CloneChannelsCompleted, affectedRows));
                return true;
            }
            catch (Exception ex)
            {
                adminContext.ErrLog.HandleError(ex, ExtensionPhrases.CloneChannelsError);
                return false;
            }
        }

        /// <summary>
        /// Updates channel numbers in the specified formula.
        /// </summary>
        private static string UpdateFormula(string formula, int shiftNum)
        {
            if (!string.IsNullOrEmpty(formula))
            {
                foreach (string knownFunc in KnownFunctions)
                {
                    bool funcFound;
                    int searchInd = 0;
                    int formulaEndInd = formula.Length - 1;

                    do
                    {
                        funcFound = false;
                        int funcStart = formula.IndexOf(knownFunc, searchInd);

                        if (funcStart == 0 || funcStart > 0 && !char.IsLetter(formula, funcStart - 1))
                        {
                            int argStart = funcStart + knownFunc.Length;
                            int argEnd = formula.IndexOfAny(ArgumentEnds, argStart);

                            if (argEnd >= 0)
                            {
                                string cnlNumStr = formula[argStart..argEnd];

                                if (int.TryParse(cnlNumStr, out int cnlNum))
                                {
                                    funcFound = true;
                                    searchInd = argEnd;
                                    formula = formula.Substring(0, argStart) + (cnlNum + shiftNum) + formula[argEnd..];
                                }
                            }
                        }
                    }
                    while (funcFound && searchInd < formulaEndInd);
                }
            }

            return formula;
        }


        private void FrmCnlClone_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            FillObjList();
            FillDeviceList();
            CalcDestEndNum();
        }

        private void num_ValueChanged(object sender, EventArgs e)
        {
            CalcDestEndNum();
        }

        private void btnClone_Click(object sender, EventArgs e)
        {
            if (CloneChannels(
                Convert.ToInt32(numSrcStartNum.Value),
                Convert.ToInt32(numSrcEndNum.Value),
                Convert.ToInt32(numDestStartNum.Value),
                (int)cbReplaceObj.SelectedValue, 
                (int)cbReplaceDevice.SelectedValue,
                chkUpdateFormulas.Checked))
            {
                ChannelsCloned = true;
            }
        }
    }
}
