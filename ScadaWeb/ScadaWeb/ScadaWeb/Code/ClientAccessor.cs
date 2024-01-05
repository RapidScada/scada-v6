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
 * Module   : Webstation Application
 * Summary  : Provides access to a client that interacts with the Server service
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Client;
using Scada.Web.Services;
using System;

namespace Scada.Web.Code
{
    /// <summary>
    /// Provides access to a client that interacts with the Server service.
    /// <para>Предоставляет доступ к клиенту, который взаимодействует со службой Сервера.</para>
    /// </summary>
    internal sealed class ClientAccessor : IClientAccessor, IDisposable
    {
        private readonly IWebContext webContext;
        private ScadaClient scadaClient;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ClientAccessor(IWebContext webContext)
        {
            this.webContext = webContext ?? throw new ArgumentNullException(nameof(webContext));
            scadaClient = null;
        }


        /// <summary>
        /// Gets the client.
        /// </summary>
        public ScadaClient ScadaClient
        {
            get
            {
                scadaClient ??= webContext.ClientPool.GetClient(webContext.AppConfig.ConnectionOptions);
                return scadaClient;
            }
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            webContext.ClientPool.ReturnClient(scadaClient);
        }
    }
}
