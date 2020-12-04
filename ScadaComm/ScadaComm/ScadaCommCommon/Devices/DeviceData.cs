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
 * Summary  : Represents device data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Const;
using Scada.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Represents device data.
    /// <para>Представляет данные КП.</para>
    /// </summary>
    /// <remarks>The class is thread safe.</remarks>
    public class DeviceData
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceData()
        {

        }


        /// <summary>
        /// Gets or sets the data for the device tag at the specified index.
        /// </summary>
        public CnlData this[int index]
        {
            get
            {
                return CnlData.Empty;
            }
            set
            {

            }
        }

        /// <summary>
        /// Gets or sets the data for the device tag at the specified code.
        /// </summary>
        public CnlData this[string code]
        {
            get
            {
                return CnlData.Empty;
            }
            set
            {

            }
        }


        /// <summary>
        /// Initializes the device data to maintain the specified device tags.
        /// </summary>
        public void Init(DeviceTags deviceTags)
        {

        }

        /// <summary>
        /// Gets the floating point value of the tag.
        /// </summary>
        public double Get(int tagIndex)
        {
            CnlData cnlData = this[tagIndex];
            return cnlData.Stat > CnlStatusID.Undefined ? cnlData.Val : 0.0;
        }

        /// <summary>
        /// Gets the floating point value of the tag.
        /// </summary>
        public double Get(string tagCode)
        {
            CnlData cnlData = this[tagCode];
            return cnlData.Stat > CnlStatusID.Undefined ? cnlData.Val : 0.0;
        }

        /// <summary>
        /// Gets the integer value of the tag.
        /// </summary>
        public long GetInt64(int tagIndex)
        {
            return 0;
        }

        /// <summary>
        /// Gets the integer value of the tag.
        /// </summary>
        public long GetInt64(string tagCode)
        {
            return 0;
        }

        /// <summary>
        /// Gets the ASCII string value of the tag.
        /// </summary>
        public string GetAscii(int tagIndex)
        {
            return "";
        }

        /// <summary>
        /// Gets the ASCII string value of the tag.
        /// </summary>
        public string GetAscii(string tagCode)
        {
            return "";
        }

        /// <summary>
        /// Gets the Unicode string value of the tag.
        /// </summary>
        public string GetUnicode(int tagIndex)
        {
            return "";
        }

        /// <summary>
        /// Gets the Unicode string value of the tag.
        /// </summary>
        public string GetUnicode(string tagCode)
        {
            return "";
        }

        /// <summary>
        /// Sets the floating point value and status of the tag.
        /// </summary>
        public void Set(int tagIndex, double val, int stat)
        {
            this[tagIndex] = new CnlData(val, stat);
        }

        /// <summary>
        /// Sets the floating point value of the tag.
        /// </summary>
        public void Set(int tagIndex, double val)
        {
            this[tagIndex] = double.IsNaN(val) ?
                CnlData.Empty :
                new CnlData(val, CnlStatusID.Defined);
        }

        /// <summary>
        /// Sets the floating point value and status of the tag.
        /// </summary>
        public void Set(string tagCode, double val, int stat)
        {
            this[tagCode] = new CnlData(val, stat);
        }

        /// <summary>
        /// Sets the floating point value of the tag.
        /// </summary>
        public void Set(string tagCode, double val)
        {
            this[tagCode] = double.IsNaN(val) ?
                CnlData.Empty :
                new CnlData(val, CnlStatusID.Defined);
        }

        /// <summary>
        /// Sets the integer value and status of the tag.
        /// </summary>
        public void SetInt64(int tagIndex, long val, int stat)
        {

        }

        /// <summary>
        /// Sets the integer value and status of the tag.
        /// </summary>
        public void SetInt64(string tagCode, long val, int stat)
        {

        }

        /// <summary>
        /// Sets the archive of floating point values and status of the tag.
        /// </summary>
        public void SetArchive(int tagIndex, double[] vals, int stat)
        {

        }

        /// <summary>
        /// Sets the archive of floating point values and status of the tag.
        /// </summary>
        public void SetArchive(string tagCode, double[] vals, int stat)
        {

        }

        /// <summary>
        /// Sets the ASCII string value and status of the tag.
        /// </summary>
        public void SetAscii(int tagIndex, string s, int stat)
        {

        }

        /// <summary>
        /// Sets the ASCII string value and status of the tag.
        /// </summary>
        public void SetAscii(string tagCode, string s, int stat)
        {

        }

        /// <summary>
        /// Sets the Unicode string value and status of the tag.
        /// </summary>
        public void SetUnicode(int tagIndex, string s, int stat)
        {

        }

        /// <summary>
        /// Sets the Unicode string value and status of the tag.
        /// </summary>
        public void SetUnicode(string tagCode, string s, int stat)
        {

        }

        /// <summary>
        /// Adds the specified term to the tag value.
        /// </summary>
        public void Add(int tagIndex, double val)
        {

        }

        /// <summary>
        /// Adds the specified term to the tag value.
        /// </summary>
        public void Add(string tagCode, double val)
        {

        }

        /// <summary>
        /// Sets all tags to undefined.
        /// </summary>
        public void Invalidate()
        {

        }

        /// <summary>
        /// Sets the specified tag to undefined.
        /// </summary>
        public void Invalidate(int tagIndex)
        {

        }

        /// <summary>
        /// Sets the specified tag to undefined.
        /// </summary>
        public void Invalidate(string tagCode)
        {

        }

        /// <summary>
        /// Sets the specified tag range to undefined.
        /// </summary>
        public void Invalidate(int tagIndex, int tagCount)
        {

        }

        /// <summary>
        /// Sets the specified tag range to undefined.
        /// </summary>
        public void Invalidate(string tagCode, int tagCount)
        {

        }

        /// <summary>
        /// Adds the device archive slice.
        /// </summary>
        public void AddSlice(DeviceSlice deviceSlice)
        {
        }

        /// <summary>
        /// Adds the device event.
        /// </summary>
        public void AddEvent(DeviceEvent deviceEvent)
        { 
        }
    }
}
