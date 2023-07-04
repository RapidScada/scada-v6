using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Scada.Web.Services;
using System.Text;

namespace Scada.Web
{
    partial class WebUtils
    {
        /// <summary>
        /// The prefix for cache entry keys.
        /// </summary>
        private const string CachePrefix = "ScadaWeb";
        /// <summary>
        /// Specifies how long a cache entry can be inactive before it will be removed.
        /// </summary>
        public static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(1);
        /// <summary>
        /// The cache expiration for views.
        /// </summary>
        public static readonly TimeSpan ViewCacheExpiration = TimeSpan.FromMinutes(10);
        /// <summary>
        /// The cache expiration on data retrieval error.
        /// </summary>
        public static readonly TimeSpan ErrorCacheExpiration = TimeSpan.FromSeconds(1);


        /// <summary>
        /// Sets the default cache entry options.
        /// </summary>
        public static ICacheEntry SetDefaultOptions(this ICacheEntry cacheEntry, IWebContext webContext)
        {
            cacheEntry.SetSlidingExpiration(CacheExpiration);
            cacheEntry.AddExpirationToken(webContext);
            return cacheEntry;
        }

        /// <summary>
        /// Adds an expiration token, stored in the web context, to the cache entry.
        /// </summary>
        public static ICacheEntry AddExpirationToken(this ICacheEntry cacheEntry, IWebContext webContext)
        {
            cacheEntry.AddExpirationToken(new CancellationChangeToken(webContext.CacheExpirationTokenSource.Token));
            return cacheEntry;
        }

        /// <summary>
        /// Gets the cache key.
        /// </summary>
        public static string GetCacheKey(string prefix, string typeName, object objectID)
        {
            return prefix + "_" + typeName + "_" + objectID;
        }

        /// <summary>
        /// Gets the cache key.
        /// </summary>
        public static string GetCacheKey(string prefix, string typeName, params object[] args)
        {
            StringBuilder sbKey = new();
            sbKey.Append(prefix).Append('_').Append(typeName);

            if (args != null)
            {
                foreach (object arg in args)
                {
                    sbKey.Append('_').Append(arg == null ? "null" : arg.ToString());
                }
            }

            return sbKey.ToString();
        }

        /// <summary>
        /// Gets the cache key corresponding to the user.
        /// </summary>
        public static string GetUserCacheKey(int userID)
        {
            return GetCacheKey(CachePrefix, "User", userID);
        }

        /// <summary>
        /// Gets the cache key corresponding to the view specification.
        /// </summary>
        public static string GetViewSpecCacheKey(int viewID)
        {
            return GetCacheKey(CachePrefix, "ViewSpec", viewID);
        }

        /// <summary>
        /// Gets the cache key corresponding to the view.
        /// </summary>
        public static string GetViewCacheKey(int viewID)
        {
            return GetCacheKey(CachePrefix, "View", viewID);
        }
    }
}
