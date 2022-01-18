/*
 * Copyright 2022 Rapid Software LLC
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
 * Module   : Administrator
 * Summary  : The main entry point for the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

using Scada.Admin.App.Code;
using Scada.Admin.App.Forms;
using Scada.Forms;
using Scada.Lang;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Scada.Admin.App
{
    static class Program
    {
        private static AppData appData; // common data of the application

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            appData = new AppData();
            appData.Init(Path.GetDirectoryName(Application.ExecutablePath));

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;
            Application.Run(new FrmMain(appData));
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            appData.ErrLog.HandleError(e.Exception, CommonPhrases.UnhandledException);
        }
    }
}
