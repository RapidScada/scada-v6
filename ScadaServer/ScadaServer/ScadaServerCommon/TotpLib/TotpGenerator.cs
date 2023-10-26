using System;
using System.Collections.Generic;
using Scada.Server.TotpLib.Helper;
using Scada.Server.TotpLib.Interface;

namespace Scada.Server.TotpLib
{
    public class TotpGenerator : ITotpGenerator
    {
        private readonly DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        ///     Generates a valid TOTP.
        /// </summary>
        /// <param name="accountSecretKey">User's secret key. Same as used to create the setup.</param>
        /// <returns>Creates a 6 digit one time password.</returns>
        public int Generate(string accountSecretKey)
        {
            return TotpHasher.Hash(accountSecretKey, GetCurrentCounter());
        }

        /// <summary>
        ///     Gets valid valid TOTPs.
        /// </summary>
        /// <param name="accountSecretKey">User's secret key. Same as used to create the setup.</param>
        /// <param name="timeTolerance">Time tolerance in seconds to acceppt before and after now.</param>
        /// <returns>List of valid totps.</returns>
        public IEnumerable<int> GetValidTotps(string accountSecretKey, TimeSpan timeTolerance)
        {
            var codes = new List<int>();
            var iterationCounter = GetCurrentCounter();
            var iterationOffset = 0;

            if (timeTolerance.TotalSeconds > 30) iterationOffset = Convert.ToInt32(timeTolerance.TotalSeconds / 30.00);

            var iterationStart = iterationCounter - iterationOffset;
            var iterationEnd = iterationCounter + iterationOffset;

            for (var counter = iterationStart; counter <= iterationEnd; counter++)
                codes.Add(Generate(accountSecretKey, counter));

            return codes.ToArray();
        }

        private int Generate(string accountSecretKey, long counter, int digits = 6)
        {
            return TotpHasher.Hash(accountSecretKey, counter, digits);
        }

        private long GetCurrentCounter()
        {
            return (long)(DateTime.UtcNow - unixEpoch).TotalSeconds / 30;
        }
    }
}