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
 * Summary  : Represents device polling options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Config;
using System;
using System.Xml;

namespace Scada.Comm.Config
{
    /// <summary>
    /// Represents device polling options.
    /// <para>Представляет параметры опроса устройства.</para>
    /// </summary>
    [Serializable]
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
            : this(0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PollingOptions(int timeout, int delay)
        {
            PollOnCmd = false;
            Timeout = timeout;
            Delay = delay;
            Time = TimeSpan.Zero;
            Period = TimeSpan.Zero;
            CmdLine = "";
            CustomOptions = new OptionList();
        }


        /// <summary>
        /// Gets or sets a value indicating whether to poll the device only on command.
        /// </summary>
        public bool PollOnCmd { get; set; }

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
        public TimeSpan Time { get; set; }

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
        public OptionList CustomOptions { get; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            PollOnCmd = xmlElem.GetAttrAsBool("pollOnCmd", PollOnCmd);
            Timeout = xmlElem.GetAttrAsInt("timeout", Timeout);
            Delay = xmlElem.GetAttrAsInt("delay", Delay);
            Time = xmlElem.GetAttrAsTimeSpan("time", Time);
            Period = xmlElem.GetAttrAsTimeSpan("period", Period);
            CmdLine = xmlElem.GetAttrAsString("cmdLine", CmdLine);
            CustomOptions.LoadFromXml(xmlElem);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.SetAttribute("pollOnCmd", PollOnCmd);
            xmlElem.SetAttribute("timeout", Timeout);
            xmlElem.SetAttribute("delay", Delay);
            xmlElem.SetAttribute("time", Time);
            xmlElem.SetAttribute("period", Period);
            xmlElem.SetAttribute("cmdLine", CmdLine);
            CustomOptions.SaveToXml(xmlElem);
        }

        /// <summary>
        /// Copies the current object to the other.
        /// </summary>
        public void CopyTo(PollingOptions destination)
        {
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            destination.PollOnCmd = PollOnCmd;
            destination.Timeout = Timeout;
            destination.Delay = Delay;
            destination.Time = Time;
            destination.Period = Period;
            destination.CmdLine = CmdLine;
            destination.CustomOptions.Clear();
            CustomOptions.CopyTo(destination.CustomOptions);
        }

        /// <summary>
        /// Creates polling options with a default timeout and delay.
        /// </summary>
        public static PollingOptions CreateDefault()
        {
            return new PollingOptions(DefaultTimeout, DefaultDelay);
        }

        /// <summary>
        /// Creates polling options with zero timeout and default delay.
        /// </summary>
        public static PollingOptions CreateWithDefaultDelay()
        {
            return new PollingOptions(0, DefaultDelay);
        }
    }
}
