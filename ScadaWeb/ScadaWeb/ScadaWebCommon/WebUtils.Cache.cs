using System;

namespace Scada.Web
{
    partial class WebUtils
    {
        /// <summary>
        /// The prefix for cache entry keys.
        /// </summary>
        private const string CachePrefix = "ScadaWeb_";
        /// <summary>
        /// Specifies how long a cache entry can be inactive before it will be removed.
        /// </summary>
        public static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Gets the cache key corresponding to the user.
        /// </summary>
        public static string GetUserKey(int userID)
        {
            return CachePrefix + "User_" + userID;
        }
    }
}
