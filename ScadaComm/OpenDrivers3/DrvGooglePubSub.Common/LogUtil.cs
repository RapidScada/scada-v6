using Scada.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Comm.Drivers.DrvGooglePubSub
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public static class LogUtil
    {
        public static ILog Logger { get; set; }
        public static void Info(string message)
        {
            Logger?.WriteInfo(message);
        }
    }
}
