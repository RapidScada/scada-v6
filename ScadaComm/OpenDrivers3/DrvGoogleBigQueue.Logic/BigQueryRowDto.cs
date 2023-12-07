using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Comm.Drivers.DrvGoogleBigQueue.Logic
{
    internal class BigQueryRowDto
    {
        public DateTime Timestamp { get; set; }

        public string SerieName { get; set; }

        public double Value { get; set; }
    }
}
