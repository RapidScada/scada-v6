// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Server.Lang;
using Scada.Server.Modules.ModActiveDirectory.Config;

namespace Scada.Server.Modules.ModActiveDirectory.Logic
{
    /// <summary>
    /// Implements the server module logic.
    /// <para>Реализует логику серверного модуля.</para>
    /// </summary>
    public class ModActiveDirectoryLogic : ModuleLogic
    {
        private readonly ModuleConfig moduleConfig; // the module configuration


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModActiveDirectoryLogic(IServerContext serverContext)
            : base(serverContext)
        {
            moduleConfig = new ModuleConfig();
        }


        /// <summary>
        /// Gets the module code.
        /// </summary>
        public override string Code
        {
            get
            {
                return ModuleUtils.ModuleCode;
            }
        }

        /// <summary>
        /// Performs actions when starting the service.
        /// </summary>
        public override void OnServiceStart()
        {
            if (!moduleConfig.Load(ServerContext.Storage, ModuleConfig.DefaultFileName, out string errMsg))
                Log.WriteError(ServerPhrases.ModuleMessage, Code, errMsg);
        }
    }
}
