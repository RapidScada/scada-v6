/*
 * Copyright 2021 Rapid Software LLC
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
 * Module   : ScadaAdminCommon
 * Summary  : Represents a Rapid SCADA project
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Admin.Project
{
    /// <summary>
    /// Represents a Rapid SCADA project.
    /// <para>Представляет проект Rapid SCADA.</para>
    /// </summary>
    public class ScadaProject
    {
        /// <summary>
        /// The default project name.
        /// </summary>
        public const string DefaultName = "NewProject";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ScadaProject()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets the configuration database.
        /// </summary>
        public ConfigBase ConfigBase { get; protected set; }

        /// <summary>
        /// Gets the metadata of the views.
        /// </summary>
        public ProjectViews Views { get; protected set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            ConfigBase = new ConfigBase();
            Views = new ProjectViews();
        }

        /// <summary>
        /// Loads the project from the specified file.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            try
            {
                //Load(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ScadaUtils.BuildErrorMessage(ex, AdminPhrases.LoadProjectError);
                return false;
            }
        }

        /// <summary>
        /// Saves the project to the specified file.
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                //Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ScadaUtils.BuildErrorMessage(ex, AdminPhrases.SaveProjectError);
                return false;
            }
        }
    }
}
