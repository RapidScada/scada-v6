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
 * Module   : ScadaCommon.Svc
 * Summary  : The base class for Windows service installer
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2022
 */

using Scada.Lang;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Scada.Svc
{
    /// <summary>
    /// The base class for Windows service installer.
    /// <para>Базовый класс установщика службы Windows.</para>
    /// </summary>
    public abstract class SvcInstallerBase : Installer
    {
        /// <summary>
        /// Initializes the service installer.
        /// </summary>
        protected void Init(string defaultServiceName, string defaultDescription)
        {
            // load and validate service properties
            SvcProps svcProps = new SvcProps();

            if (!svcProps.LoadFromFile())
            {
                svcProps.ServiceName = defaultServiceName;
                svcProps.Description = defaultDescription;
            }

            if (string.IsNullOrEmpty(svcProps.ServiceName))
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Имя службы не должно быть пустым." :
                    "Service name must not be empty.");
            }

            // configure installer
            ServiceInstaller serviceInstaller = new ServiceInstaller
            {
                ServiceName = svcProps.ServiceName,
                DisplayName = svcProps.ServiceName,
                Description = svcProps.Description ?? "",
                StartType = ServiceStartMode.Automatic
            };

            ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem,
                Password = null,
                Username = null
            };

            Installers.AddRange(new Installer[] { serviceInstaller, serviceProcessInstaller });
        }
    }
}
