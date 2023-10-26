using System;

namespace Scada.Server.TotpLib.Helper
{
    internal static class Guard
    {
        internal static void NotNull(object testee)
        {
            if (testee == null) throw new NullReferenceException();
        }
    }
}