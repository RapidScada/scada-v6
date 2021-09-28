// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Lang;
using System.IO;

namespace Scada.Server.Modules.ModArcPostgreSql.Logic
{
    /// <summary>
    /// Implements the server module logic.
    /// <para>Реализует логику серверного модуля.</para>
    /// </summary>
    public class ModArcPostgreSqlLogic : ModuleLogic
    {
        private readonly InstanceConfig instanceConfig; // the instance configuration
        private readonly ModuleConfig moduleConfig;     // the module configuration


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModArcPostgreSqlLogic(IServerContext serverContext)
            : base(serverContext)
        {
            instanceConfig = new InstanceConfig();
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
        /// Gets the module purposes.
        /// </summary>
        public override ModulePurposes ModulePurposes
        {
            get
            {
                return ModulePurposes.Archive;
            }
        }


        /// <summary>
        /// Creates a new archive logic.
        /// </summary>
        public override ArchiveLogic CreateArchive(IArchiveContext archiveContext, ArchiveConfig archiveConfig,
            int[] cnlNums)
        {
            switch (archiveConfig.Kind)
            {
                case ArchiveKind.Current:
                    return new PostgreCAL(archiveContext, archiveConfig, cnlNums, instanceConfig, moduleConfig);
                case ArchiveKind.Historical:
                    return new PostgreHAL(archiveContext, archiveConfig, cnlNums, instanceConfig, moduleConfig);
                case ArchiveKind.Events:
                    return new PostgreEAL(archiveContext, archiveConfig, cnlNums, instanceConfig, moduleConfig);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Performs actions when starting the service.
        /// </summary>
        public override void OnServiceStart()
        {
            // load instance configuration
            if (!instanceConfig.Load(
                Path.Combine(ServerContext.AppDirs.InstanceDir, "Config", InstanceConfig.DefaultFileName),
                out string errMsg))
            {
                Log.WriteError(ServerPhrases.ModuleMessage, Code, errMsg);
            }

            // load module configuration
            if (!moduleConfig.Load(ServerContext.Storage, ModuleConfig.DefaultFileName, out errMsg))
                Log.WriteError(ServerPhrases.ModuleMessage, Code, errMsg);
        }
    }
}
