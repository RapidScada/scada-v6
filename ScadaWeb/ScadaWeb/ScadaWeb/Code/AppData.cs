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
 * Module   : Webstation Application
 * Summary  : Contains the common application data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2021
 */

using Scada.Log;
using System;
using System.Collections.Generic;
using System.IO;

namespace Scada.Web.Code
{
    /// <summary>
    /// Contains the common application data.
    /// <para>Содержит общие данные приложения.</para>
    /// </summary>
    public sealed class AppData
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public AppData()
        {
            AppDirs = new AppDirs();
            Log = LogStub.Instance;
        }


        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public AppDirs AppDirs { get; }

        /// <summary>
        /// Gets the application log.
        /// </summary>
        public ILog Log { get; private set; }


        /// <summary>
        /// Initializes the common application data.
        /// </summary>
        public void Init(string exeDir)
        {
            AppDirs.Init(exeDir);

            Log = new LogFile(LogFormat.Full)
            {
                FileName = Path.Combine(AppDirs.LogDir, WebUtils.LogFileName)
            };
        }
    }
}
