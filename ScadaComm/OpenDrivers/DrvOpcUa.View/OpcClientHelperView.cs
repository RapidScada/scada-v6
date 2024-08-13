// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvOpcUa.Config;
using Scada.Log;

namespace Scada.Comm.Drivers.DrvOpcUa.View
{
    /// <summary>
    /// Provides helper methods for using OPC client in the driver UI.
    /// <para>Предоставляет вспомогательные методы для клиента OPC в пользовательском интерфейсе драйвера.</para>
    /// </summary>
    internal class OpcClientHelperView : OpcClientHelperBase
    {
        /// <summary>
        /// The OPC UA configuration file name for the view runtime.
        /// </summary>
        private const string ViewOpcConfig = "DrvOpcUa.View.xml";

        private readonly AppDirs appDirs; // the application directories


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public OpcClientHelperView(OpcConnectionOptions connectionOptions, ILog log, AppDirs appDirs)
            : base(connectionOptions, log)
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
        }


        /// <summary>
        /// Writes the OPC configuration.
        /// </summary>
        private static void WriteConfiguration(string fileName, Stream stream)
        {
            BinaryReader reader = new(stream); // do not close reader
            byte[] bytes = reader.ReadBytes((int)stream.Length);
            File.WriteAllBytes(fileName, bytes);
        }

        /// <summary>
        /// Reads the OPC configuration.
        /// </summary>
        protected override Stream ReadConfiguration()
        {
            string fileName = Path.Combine(appDirs.ConfigDir, ViewOpcConfig);

            if (File.Exists(fileName))
            {
                byte[] bytes = File.ReadAllBytes(fileName);
                return new MemoryStream(bytes);
            }
            else
            {
                Stream resourceStream = GetConfigResourceStream();
                WriteConfiguration(fileName, resourceStream);
                resourceStream.Position = 0;
                return resourceStream;
            }
        }
    }
}
