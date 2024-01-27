/*
 * Copyright 2024 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaCommCommon
 * Summary  : Represents a communication line configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Scada.Comm.Config
{
    /// <summary>
    /// Represents a communication line configuration.
    /// <para>Представляет конфигурацию линии связи.</para>
    /// </summary>
    [Serializable]
    public class LineConfig : ITreeNode
    {
        /// <summary>
        /// The parent configuration.
        /// </summary>
        [NonSerialized]
        protected CommConfig parentConfig;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public LineConfig()
        {
            Active = true;
            IsBound = true;
            CommLineNum = 0;
            Name = "";
            LineOptions = new LineOptions();
            Channel = new ChannelConfig();
            CustomOptions = new OptionList();
            DevicePolling = new List<DeviceConfig>();
            Parent = null;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the line is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the line is bound to the configuration database.
        /// </summary>
        public bool IsBound { get; set; }

        /// <summary>
        /// Gets or sets the line number.
        /// </summary>
        public int CommLineNum { get; set; }

        /// <summary>
        /// Gets or sets the line name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the line title.
        /// </summary>
        public string Title
        {
            get
            {
                return CommUtils.GetLineTitle(CommLineNum, Name);
            }
        }

        /// <summary>
        /// Gets the line options.
        /// </summary>
        public LineOptions LineOptions { get; }

        /// <summary>
        /// Gets the channel configuration.
        /// </summary>
        public ChannelConfig Channel { get; }

        /// <summary>
        /// Gets the custom options.
        /// </summary>
        public OptionList CustomOptions { get; }

        /// <summary>
        /// Gets the polling sequence of the devices.
        /// </summary>
        public List<DeviceConfig> DevicePolling { get; }

        /// <summary>
        /// Gets or sets the parent tree node.
        /// </summary>
        public ITreeNode Parent
        {
            get
            {
                return parentConfig;
            }
            set
            {
                parentConfig = value == null ? null : (CommConfig)value;
            }
        }

        /// <summary>
        /// Gets the child tree nodes.
        /// </summary>
        public IList Children
        {
            get
            {
                return DevicePolling;
            }
        }


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            Active = xmlElem.GetAttrAsBool("active");
            IsBound = xmlElem.GetAttrAsBool("isBound");
            CommLineNum = xmlElem.GetAttrAsInt("number");
            Name = xmlElem.GetAttrAsString("name");

            if (xmlElem.SelectSingleNode("LineOptions") is XmlNode lineOptionsNode)
                LineOptions.LoadFromXml(lineOptionsNode);

            if (xmlElem.SelectSingleNode("Channel") is XmlElement channelElem)
                Channel.LoadFromXml(channelElem);

            if (xmlElem.SelectSingleNode("CustomOptions") is XmlNode customOptionsNode)
                CustomOptions.LoadFromXml(customOptionsNode);

            if (xmlElem.SelectSingleNode("DevicePolling") is XmlNode devicePollingNode)
            {
                foreach (XmlElement deviceElem in devicePollingNode.SelectNodes("Device"))
                {
                    DeviceConfig deviceConfig = new DeviceConfig { Parent = this };
                    deviceConfig.LoadFromXml(deviceElem);
                    DevicePolling.Add(deviceConfig);
                }
            }
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.SetAttribute("active", Active);
            xmlElem.SetAttribute("isBound", IsBound);
            xmlElem.SetAttribute("number", CommLineNum);
            xmlElem.SetAttribute("name", Name);
            LineOptions.SaveToXml(xmlElem.AppendElem("LineOptions"));
            Channel.SaveToXml(xmlElem.AppendElem("Channel"));
            CustomOptions.SaveToXml(xmlElem.AppendElem("CustomOptions"));

            XmlElement devicePollingElem = xmlElem.AppendElem("DevicePolling");
            foreach (DeviceConfig device in DevicePolling)
            {
                device.SaveToXml(devicePollingElem.AppendElem("Device"));
            }
        }

        /// <summary>
        /// Gets a list of driver codes used by the communication line.
        /// </summary>
        public List<string> GetDriverCodes()
        {
            List<string> driverCodes = new List<string>();
            HashSet<string> driverCodeSet = new HashSet<string>();

            void AddDriverCode(string driverCode)
            {
                if (!string.IsNullOrEmpty(driverCode) && driverCodeSet.Add(driverCode.ToLowerInvariant()))
                    driverCodes.Add(driverCode);
            }

            AddDriverCode(Channel.Driver);

            foreach (DeviceConfig deviceConfig in DevicePolling)
            {
                if (deviceConfig.Active)
                    AddDriverCode(deviceConfig.Driver);
            }

            driverCodes.Sort();
            return driverCodes;
        }
    }
}
