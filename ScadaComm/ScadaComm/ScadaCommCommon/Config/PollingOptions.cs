/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Summary  : Represents device polling options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Config;
using System;
using System.Xml;

namespace Scada.Comm.Config
{
    /// <summary>
    /// Represents device polling options.
    /// <para>Представляет параметры опроса КП.</para>
    /// </summary>
    public class PollingOptions
    {
        /// <summary>
        /// The default request timeout, ms.
        /// </summary>
        public const int DefaultTimeout = 1000;
        /// <summary>
        /// The default delay after request, ms.
        /// </summary>
        public const int DefaultDelay = 200;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PollingOptions()
        {
            Timeout = 0;
            Delay = 0;
            Time = DateTime.MinValue;
            Period = TimeSpan.Zero;
            CmdLine = "";
            CustomOptions = new CustomOptions();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PollingOptions(int timeout, int delay)
        {
            Timeout = timeout;
            Delay = delay;
            Time = DateTime.MinValue;
            Period = TimeSpan.Zero;
            CmdLine = "";
            CustomOptions = new CustomOptions();
        }


        /// <summary>
        /// Gets or sets the request timeout, ms.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Gets or sets the delay after request, ms.
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// Gets or sets the polling time.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Gets or sets the polling period.
        /// </summary>
        public TimeSpan Period { get; set; }

        /// <summary>
        /// Gets or sets the command line containing additional request options.
        /// </summary>
        public string CmdLine { get; set; }

        /// <summary>
        /// Gets the custom options.
        /// </summary>
        public CustomOptions CustomOptions { get; private set; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            Timeout = xmlElem.GetAttrAsInt("timeout", DefaultTimeout);
            Delay = xmlElem.GetAttrAsInt("delay", DefaultDelay);
            Time = xmlElem.GetAttrAsDateTime("time");
            Period = xmlElem.GetAttrAsTimeSpan("period");
            CmdLine = xmlElem.GetAttrAsString("cmdLine");
            CustomOptions.LoadFromXml(xmlElem);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException("xmlElem");

            xmlElem.SetAttribute("timeout", Timeout);
            xmlElem.SetAttribute("delay", Delay);
            xmlElem.SetAttribute("time", Time);
            xmlElem.SetAttribute("period", Period);
            xmlElem.SetAttribute("cmdLine", CmdLine);
            CustomOptions.SaveToXml(xmlElem);
        }

        /// <summary>
        /// Creates default polling options.
        /// </summary>
        public static PollingOptions CreateDefault()
        {
            return new PollingOptions(DefaultTimeout, DefaultDelay);
        }
    }
}
