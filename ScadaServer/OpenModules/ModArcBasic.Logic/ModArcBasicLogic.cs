// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Server.Archives;
using Scada.Server.Config;

namespace Scada.Server.Modules.ModArcBasic.Logic
{
    /// <summary>
    /// Implements the server module logic.
    /// <para>Реализует логику серверного модуля.</para>
    /// </summary>
    public class ModArcBasicLogic : ModuleLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModArcBasicLogic(IServerContext serverContext)
            : base(serverContext)
        {
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
                    return new BasicCAL(archiveContext, archiveConfig, cnlNums);
                case ArchiveKind.Historical:
                    return new BasicHAL(archiveContext, archiveConfig, cnlNums);
                case ArchiveKind.Events:
                    return new BasicEAL(archiveContext, archiveConfig, cnlNums);
                default:
                    return null;
            }
        }
    }
}
