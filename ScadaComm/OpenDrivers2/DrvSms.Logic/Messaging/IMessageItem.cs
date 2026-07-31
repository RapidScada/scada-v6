using System;

namespace Scada.Comm.Drivers.DrvSms.Logic.Messaging
{
    public interface IMessageItem
    {
        DateTime Timestamp { get; }

        string Address { get; }

        string Text { get; }

        bool IsProcessed { get; set; }
    }
}
