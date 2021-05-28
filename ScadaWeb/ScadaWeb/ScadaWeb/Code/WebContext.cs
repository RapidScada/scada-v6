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
 * Summary  : Contains web application level data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Client;
using Scada.Data.Models;
using Scada.Log;
using Scada.Web.Config;
using System.IO;

namespace Scada.Web.Code
{
    /// <summary>
    /// Contains web application level data.
    /// <para>Содержит данные уровня веб-приложения.</para>
    /// </summary>
    internal class WebContext : IWebContext
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public WebContext()
        {
            Config = new WebConfig();
            AppDirs = new WebDirs();
            Log = LogStub.Instance;
            BaseDataSet = new BaseDataSet();
            ClientPool = new ScadaClientPool();
        }


        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public WebConfig Config { get; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public WebDirs AppDirs { get; }

        /// <summary>
        /// Gets the application log.
        /// </summary>
        public ILog Log { get; private set; }

        /// <summary>
        /// Gets the cached configuration database.
        /// </summary>
        public BaseDataSet BaseDataSet { get; private set; }

        /// <summary>
        /// Gets the client pool.
        /// </summary>
        public ScadaClientPool ClientPool { get; }



        /// <summary>
        /// Initializes the common application data.
        /// </summary>
        public void Init(string exeDir)
        {
            AppDirs.Init(exeDir);

            Log = new LogFile(LogFormat.Full)
            {
                FileName = Path.Combine(AppDirs.LogDir, WebUtils.LogFileName),
                Capacity = int.MaxValue
            };
        }

        /// <summary>
        /// Loads the application configuration.
        /// </summary>
        public void LoadConfig()
        {
            if (Config.Load(Path.Combine(AppDirs.ConfigDir, WebConfig.DefaultFileName), out string errMsg))
            {
                if (Log is LogFile logFile)
                    logFile.Capacity = Config.GeneralOptions.MaxLogSize;
            }
            else
            {
                Log.WriteError(errMsg);
            }
        }
    }
}
