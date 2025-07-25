// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using System.Xml;

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Represents data bindings of a component.
    /// <para>Представляет привязки данных компонента.</para>
    /// </summary>
    public class ComponentBindings
    {
        /// <summary>
        /// Gets or sets the input channel number.
        /// Specifies the default data source for the component.
        /// </summary>
        public int InCnlNum { get; set; } = 0;

        /// <summary>
        /// Gets or sets the output channel number.
        /// By default it is used for sending commands.
        /// </summary>
        public int OutCnlNum { get; set; } = 0;

        /// <summary>
        /// Gets or sets the object number. It is used for binding channels by code.
        /// </summary>
        public int ObjNum { get; set; } = 0;

        /// <summary>
        /// Gets or sets the device number. It is used for binding channels by tag code.
        /// </summary>
        public int DeviceNum { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating whether to check user rights to display the component.
        /// </summary>
        public bool CheckRights { get; set; } = false;

        /// <summary>
        /// Gets or sets the properties of the input channel.
        /// </summary>
        public CnlProps InCnlProps { get; set; } = null;

        /// <summary>
        /// Gets or sets the properties of the output channel.
        /// </summary>
        public CnlProps OutCnlProps { get; set; } = null;

        /// <summary>
        /// Gets the property bindings of the compoment.
        /// </summary>
        public List<PropertyBinding> PropertyBindings { get; } = [];


        /// <summary>
        /// Binds channels based on the object.
        /// </summary>
        private void BindChannelsByObj(ConfigDataset configDataset)
        {
            if (configDataset.ObjTable.Items.TryGetValue(ObjNum, out Obj obj))
            {
                IEnumerable<Cnl> cnls = configDataset.CnlTable.Select(new TableFilter("ObjNum", ObjNum), true);
                Dictionary<string, Cnl> cnlByCode = [];

                foreach (Cnl cnl in cnls)
                {
                    if (!string.IsNullOrEmpty(cnl.Code))
                        cnlByCode.TryAdd(cnl.Code, cnl);
                }

                foreach (PropertyBinding propertyBinding in PropertyBindings)
                {
                    if (cnlByCode.TryGetValue(obj.Code + propertyBinding.DataSource, out Cnl cnl))
                    {
                        propertyBinding.CnlNum = cnl.CnlNum;
                        propertyBinding.CnlProps = new CnlProps(cnl, configDataset);
                    }
                }
            }
        }

        /// <summary>
        /// Binds channels based on the device.
        /// </summary>
        private void BindChannelsByDevice(ConfigDataset configDataset)
        {
            if (configDataset.DeviceTable.PkExists(DeviceNum))
            {
                IEnumerable<Cnl> cnls = configDataset.CnlTable.Select(new TableFilter("DeviceNum", DeviceNum), true);
                Dictionary<string, Cnl> cnlByTagCode = [];

                foreach (Cnl cnl in cnls)
                {
                    if (!string.IsNullOrEmpty(cnl.TagCode))
                        cnlByTagCode.TryAdd(cnl.TagCode, cnl);
                }

                foreach (PropertyBinding propertyBinding in PropertyBindings)
                {
                    if (cnlByTagCode.TryGetValue(propertyBinding.DataSource, out Cnl cnl))
                    {
                        propertyBinding.CnlNum = cnl.CnlNum;
                        propertyBinding.CnlProps = new CnlProps(cnl, configDataset);
                    }
                }
            }
        }

        /// <summary>
        /// Binds channels by direct copying.
        /// </summary>
        private void BindChannelsDirectly(ConfigDataset configDataset)
        {
            foreach (PropertyBinding propertyBinding in PropertyBindings)
            {
                if (int.TryParse(propertyBinding.DataSource, out int cnlNum))
                {
                    propertyBinding.CnlNum = cnlNum;
                    propertyBinding.CnlProps = new CnlProps(cnlNum, configDataset);
                }
            }
        }


        /// <summary>
        /// Loads the object from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            InCnlNum = xmlNode.GetChildAsInt("InCnlNum");
            OutCnlNum = xmlNode.GetChildAsInt("OutCnlNum");
            ObjNum = xmlNode.GetChildAsInt("ObjNum");
            DeviceNum = xmlNode.GetChildAsInt("DeviceNum");
            CheckRights = xmlNode.GetChildAsBool("CheckRights");

            if (xmlNode.SelectSingleNode("PropertyBindings") is XmlNode propertyBindingsNode)
            {
                foreach (XmlNode itemNode in propertyBindingsNode.SelectNodes("Item"))
                {
                    PropertyBinding propertyBinding = new();
                    propertyBinding.LoadFromXml(itemNode);
                    PropertyBindings.Add(propertyBinding);
                }
            }
        }

        /// <summary>
        /// Binds channels to the component properties and fills in channel information.
        /// </summary>
        public void BindChannels(ConfigDataset configDataset)
        {
            ArgumentNullException.ThrowIfNull(configDataset, nameof(configDataset));

            if (InCnlNum > 0)
                InCnlProps = new CnlProps(InCnlNum, configDataset);

            if (OutCnlNum > 0)
                OutCnlProps = new CnlProps(OutCnlNum, configDataset);

            if (ObjNum > 0)
                BindChannelsByObj(configDataset);
            else if (DeviceNum > 0)
                BindChannelsByDevice(configDataset);
            else
                BindChannelsDirectly(configDataset);
        }

        /// <summary>
        /// Gets all channel numbers from the component bindings.
        /// </summary>
        public List<int> GetAllCnlNums()
        {
            HashSet<int> cnlNums = [];

            if (InCnlNum > 0)
                cnlNums.Add(InCnlNum);

            if (OutCnlNum > 0)
                cnlNums.Add(OutCnlNum);

            foreach (int cnlNum in PropertyBindings.Select(b => b.CnlNum))
            {
                if (cnlNum > 0)
                    cnlNums.Add(cnlNum);
            }

            return cnlNums.ToList();
        }

        /// <summary>
        /// Offsets the channel numbers by the specified value.
        /// </summary>
        public void OffsetCnlNums(int offset)
        {
            if (offset > 0)
            {
                if (InCnlNum > 0)
                    InCnlNum += offset;

                if (OutCnlNum > 0)
                    OutCnlNum += offset;

                foreach (PropertyBinding propertyBinding in PropertyBindings)
                {
                    if (propertyBinding.CnlNum > 0)
                        propertyBinding.CnlNum += offset;
                }
            }
        }
    }
}
