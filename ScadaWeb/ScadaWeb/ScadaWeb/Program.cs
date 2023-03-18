/*
 * Copyright 2023 Rapid Software LLC
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
 * Summary  : The Webstation application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Scada.Lang;
using Scada.Web.Code;
using System;

namespace Scada.Web
{
    /// <summary>
    /// The Webstation application.
    /// <para>Приложение Вебстанция.</para>
    /// </summary>
    public class Program
    {
        private static WebContext webContext;


        /// <summary>
        /// Initializes the application context.
        /// </summary>
        private static void InitContext()
        {
            webContext = new WebContext();
            
            if (webContext.Init())
            {
                webContext.StartProcessing();
                webContext.WaitForPlugins();
            }
        }

        /// <summary>
        /// Runs the web application.
        /// </summary>
        private static void RunWebHost(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                webContext.Log.WriteError(ex, Locale.IsRussian ?
                    "Не удалось запустить веб-узел" :
                    "Web host failed to start");
            }
        }

        /// <summary>
        /// Initializes a new instance of the host builder.
        /// </summary>
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup(context => new Startup(context.Configuration, webContext));
                });


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif

            InitContext();
            RunWebHost(args);
            webContext.FinalizeContext();
        }
    }
}
