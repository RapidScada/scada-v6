// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.ComponentModel;
using Scada.Data.Models;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Xml;
using NCM = System.ComponentModel;

#if WINFORMS
using Scada.Forms.ComponentModel;
using System.Drawing.Design;
#endif

namespace Scada.Server.Modules.ModDiffCalculator.Config
{
    /// <summary>
    /// Represents a configuration of a calculated item.
    /// <para>Представляет конфигурацию вычисляемого элемента.</para>
    /// </summary>
    /// <remarks>
    /// An implementation of INotifyPropertyChanged is required to update a tree node text when an item changes.
    /// </remarks>
    [Serializable]
    internal class ItemConfig : ITreeNode, IConfigDatasetAccessor, NCM.INotifyPropertyChanged
    {
        private int srcCnlNum = 0;
        private int destCnlNum = 0;


        /// <summary>
        /// Gets or sets the source channel number from which data for calculation is taken.
        /// </summary>
        #region Attributes
        [DisplayName, Category, Description]
#if WINFORMS
        [NCM.Editor(typeof(CnlNumEditor), typeof(UITypeEditor))]
#endif
        #endregion
        public int SrcCnlNum
        {
            get
            { 
                return srcCnlNum; 
            }
            set
            { 
                srcCnlNum = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the destination channel number into which a calculation result is written.
        /// </summary>
        #region Attributes
        [DisplayName, Category, Description]
#if WINFORMS
        [NCM.Editor(typeof(CnlNumEditor), typeof(UITypeEditor))]
#endif
        #endregion
        public int DestCnlNum
        {
            get
            {
                return destCnlNum;
            }
            set
            {
                destCnlNum = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        [NCM.Browsable(false)]
        [field: NonSerialized]
        public ITreeNode Parent { get; set; }

        /// <summary>
        /// Get a list of child nodes.
        /// </summary>
        [NCM.Browsable(false)]
        public IList Children => null;

        /// <summary>
        /// Gets the configuration database.
        /// </summary>
        [NCM.Browsable(false)]
        public ConfigDataset ConfigDataset => ModuleUtils.ConfigDataset;


        /// <summary>
        /// Raises a PropertyChanged event.
        /// </summary>
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new NCM.PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            SrcCnlNum = xmlElem.GetAttrAsInt("srcCnlNum");
            DestCnlNum = xmlElem.GetAttrAsInt("destCnlNum");
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.SetAttribute("srcCnlNum", SrcCnlNum);
            xmlElem.SetAttribute("destCnlNum", DestCnlNum);
        }


        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        [field: NonSerialized]
        public event NCM.PropertyChangedEventHandler PropertyChanged;
    }
}
