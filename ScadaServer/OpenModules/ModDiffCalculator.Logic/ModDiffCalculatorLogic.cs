// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using Scada.Log;
using Scada.Server.Lang;
using Scada.Server.Modules.ModDiffCalculator.Config;

namespace Scada.Server.Modules.ModDiffCalculator.Logic
{
    /// <summary>
    /// Implements the server module logic.
    /// <para>Реализует логику серверного модуля.</para>
    /// </summary>
    public class ModDiffCalculatorLogic : ModuleLogic
    {
        /// <summary>
        /// The module log file name.
        /// </summary>
        private const string LogFileName = ModuleUtils.ModuleCode + ".log";

        private readonly LogFile moduleLog;         // the module log
        private readonly ModuleConfig moduleConfig; // the module configuration

        private Thread moduleThread;      // the working thread of the module
        private volatile bool terminated; // necessary to stop the thread


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModDiffCalculatorLogic(IServerContext serverContext)
            : base(serverContext)
        {
            moduleLog = new LogFile(LogFormat.Simple)
            {
                FileName = Path.Combine(serverContext.AppDirs.LogDir, LogFileName),
                CapacityMB = serverContext.AppConfig.GeneralOptions.MaxLogSize
            };
            moduleConfig = new ModuleConfig();
        
            moduleThread = null;
            terminated = false;
        }


        /// <summary>
        /// Gets the module code.
        /// </summary>
        public override string Code => ModuleUtils.ModuleCode;


        /// <summary>
        /// Performs calculations in a separate thread.
        /// </summary>
        private void Execute()
        {
            while (!terminated)
            {


                Thread.Sleep(ScadaUtils.ThreadDelay);
            }
        }


        /// <summary>
        /// Performs actions when starting the service.
        /// </summary>
        public override void OnServiceStart()
        {
            // write to log
            moduleLog.WriteBreak();
            moduleLog.WriteAction(ServerPhrases.StartModule, Code, Version);

            // load configuration
            if (moduleConfig.Load(Storage, ModuleConfig.DefaultFileName, out string errMsg))
            {
                // start module thread
                moduleThread = new Thread(Execute);
                moduleThread.Start();
            }
            else
            {
                Log.WriteError(ServerPhrases.ModuleMessage, Code, errMsg);
                moduleLog.WriteError(errMsg);
                moduleLog.WriteError(CommonPhrases.ExecutionImpossible);
            }
        }

        /// <summary>
        /// Performs actions when the service stops.
        /// </summary>
        public override void OnServiceStop()
        {
            // stop module thread
            if (moduleThread != null)
            {
                terminated = true;
                moduleThread.Join();
                moduleThread = null;
            }

            // write to log
            moduleLog.WriteAction(ServerPhrases.StopModule, Code);
            moduleLog.WriteBreak();
        }
    }
}
