using Scada.Comm.Drivers.DrvGoogle.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Comm.Drivers.DrvGoogle.Config
{
    public interface IGoogleCloudChannel
    {
        /// <summary>
        /// Cloud配置选项
        /// </summary>
        GoogleCloudOptions GoogleCloudOptions { get; }
    }
}
