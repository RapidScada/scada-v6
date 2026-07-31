using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Drivers.DrvSms.Logic.Messaging
{
    public interface IMessageBag
    {
        IEnumerable<IMessageItem> GetMessageItems(string address);
    }
}
