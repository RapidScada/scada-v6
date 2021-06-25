using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Scada.Web.Services;
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
        /// Specifies the cache expiration for views.
        /// </summary>
        public static readonly TimeSpan ViewCacheExpiration = TimeSpan.FromMinutes(10);


        /// <summary>
        /// Sets the default cache entry options.
        /// </summary>
        public static void SetDefaultOptions(this ICacheEntry cacheEntry, IWebContext webContext)
        {
            cacheEntry.SetSlidingExpiration(CacheExpiration);
            cacheEntry.AddExpirationToken(webContext);
        }

        /// <summary>
        /// Adds an expiration token, stored in the web context, to the cache entry.
        /// </summary>
        public static void AddExpirationToken(this ICacheEntry cacheEntry, IWebContext webContext)
        {
            cacheEntry.AddExpirationToken(new CancellationChangeToken(webContext.CacheExpirationTokenSource.Token));
        }

        /// <summary>
        /// Gets the cache key corresponding to the user.
        /// </summary>
        public static string GetUserCacheKey(int userID)
        {
            return CachePrefix + "User_" + userID;
        }

        /// <summary>
        /// Gets the cache key corresponding to the view specification.
        /// </summary>
        public static string GetViewSpecCacheKey(int viewID)
        {
            return CachePrefix + "ViewSpec_" + viewID;
        }

        /// <summary>
        /// Gets the cache key corresponding to the view.
        /// </summary>
        public static string GetViewCacheKey(int viewID)
        {
            return CachePrefix + "View_" + viewID;
        }
    }
}
