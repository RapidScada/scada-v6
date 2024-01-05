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
 * Summary  : Represents a group of channel prototypes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2023
 */

using Scada.Data.Const;
using System.Collections.Generic;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Represents a group of channel prototypes.
    /// <para>Представляет группу тегов прототипов каналов.</para>
    /// </summary>
    public class CnlPrototypeGroup
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlPrototypeGroup()
            : this("")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlPrototypeGroup(string name)
        {
            Name = name;
            Hidden = false;
            CnlPrototypes = new List<CnlPrototype>();
        }


        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the channels of the group should be hidden or inactive.
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// Gets the channel prototypes.
        /// </summary>
        public List<CnlPrototype> CnlPrototypes { get; }


        /// <summary>
        /// Adds a new channel prototype to the group.
        /// </summary>
        public CnlPrototype AddCnlPrototype(string code, string name)
        {
            CnlPrototype cnlPrototype = new CnlPrototype
            {
                Active = !Hidden,
                TagCode = code,
                Name = name
            };

            CnlPrototypes.Add(cnlPrototype);
            return cnlPrototype;
        }

        /// <summary>
        /// Converts the group of channel prototypes to a group of device tags.
        /// </summary>
        public TagGroup ToTagGroup()
        {
            TagGroup tagGroup = new TagGroup(Name) { Hidden = Hidden };

            foreach (CnlPrototype cnlPrototype in CnlPrototypes)
            {
                if (CnlTypeID.IsInput(cnlPrototype.CnlTypeID))
                    tagGroup.DeviceTags.Add(cnlPrototype.ToDeviceTag());
            }

            return tagGroup;
        }
    }
}
