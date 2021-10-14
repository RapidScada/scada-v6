// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Lang;
using Scada.Storages;

namespace Scada.Server.Modules.ModArcPostgreSql.Logic
{
    /// <summary>
    /// Implements the server module logic.
    /// <para>Реализует логику серверного модуля.</para>
    /// </summary>
    public class ModArcPostgreSqlLogic : ModuleLogic
    {
        private readonly ModuleConfig moduleConfig; // the module configuration


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModArcPostgreSqlLogic(IServerContext serverContext)
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
                    return new PostgreCAL(archiveContext, archiveConfig, cnlNums, moduleConfig);
                case ArchiveKind.Historical:
                    return new PostgreHAL(archiveContext, archiveConfig, cnlNums, moduleConfig);
                case ArchiveKind.Events:
                    return new PostgreEAL(archiveContext, archiveConfig, cnlNums, moduleConfig);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Performs actions when starting the service.
        /// </summary>
        public override void OnServiceStart()
        {
            if (ServerContext.Storage.GetFileInfo(DataCategory.Config, ModuleConfig.DefaultFileName).Exists &&
                !moduleConfig.Load(ServerContext.Storage, ModuleConfig.DefaultFileName, out string errMsg))
            {
                Log.WriteError(ServerPhrases.ModuleMessage, Code, errMsg);
            }
        }
    }
}
