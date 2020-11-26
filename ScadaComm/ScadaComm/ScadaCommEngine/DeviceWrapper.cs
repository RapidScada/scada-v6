using Scada.Comm.Drivers;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Engine
{
    internal class DeviceWrapper
    {
        private readonly ILog log; // the communication line log

        public DeviceWrapper(DeviceLogic deviceLogic, ILog log)
        {
            DeviceLogic = deviceLogic ?? throw new ArgumentNullException(nameof(deviceLogic));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public DeviceLogic DeviceLogic { get; }

        public void OnCommLineStart()
        {
            try
            {
                DeviceLogic.OnCommLineStart();
            }
            catch (Exception ex)
            {
                //log.WriteException(ex, CommPhrases.ErrorInDevice, nameof(OnCommLineStart), DeviceLogic.Title);
            }
        }

        public void OnCommLineTerminate()
        {
        }
    }
}
