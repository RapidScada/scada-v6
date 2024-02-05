﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Server.Modules.ModDiffCalculator.Logic
{
    /// <summary>
    /// Implements the server module logic.
    /// <para>Реализует логику серверного модуля.</para>
    /// </summary>
    public class ModDiffCalculatorLogic : ModuleLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModDiffCalculatorLogic(IServerContext serverContext)
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
    }
}
