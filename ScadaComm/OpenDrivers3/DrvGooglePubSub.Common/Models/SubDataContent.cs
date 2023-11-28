using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Comm.Drivers.DrvGooglePubSub.Models
{
    public class SubDataContent
    {
        /// <summary>
        /// 点位名称
        /// </summary>
        public string timeserie_name { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime timestamp { get; set; }

        /// <summary>
        /// 当前值
        /// </summary>
        public double value { get; set; }
    }
}
