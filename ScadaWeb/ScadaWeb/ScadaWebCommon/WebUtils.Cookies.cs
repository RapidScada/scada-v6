using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace Scada.Web
{
	partial class WebUtils
    {
		/// <summary>
		/// The cookie lifetime.
		/// </summary>
		public static readonly TimeSpan CookieExpiration = TimeSpan.FromDays(365);


		/// <summary>
		/// Creates cookie options scoped to the specified page.
		/// </summary>
		private static CookieOptions CreateCookieOptions(PageModel page)
		{
			return new CookieOptions
			{
				Path = page.GetPageCookiePath(),
				MaxAge = CookieExpiration,
				HttpOnly = true, // accessible on server-side only
				Secure = page.Request.IsHttps,
				SameSite = SameSiteMode.Lax,
				IsEssential = true // bypass cookie consent policy
			};
		}

		/// <summary>
		/// Gets the cookie path equal to the current page URL including PathBase.
		/// </summary>
		/// <remarks>
		/// Do not use this method for pages with route parameters, because every distinct value of the parameter
		/// produces a distinct URL (e.g. "/Items/Details/1"), and therefore a distinct cookie path.
		/// </remarks>
		private static string GetPageCookiePath(this PageModel page)
		{
			string path = page.Request.PathBase.Add(page.Request.Path).Value ?? "/";
			return path.Length > 1 ? path.TrimEnd('/') : path;
		}

		/// <summary>
		/// Converts the provided cookie value into a string.
		/// </summary>
		private static string CookieValueToString(object value)
		{
			return value switch
			{
				IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
				bool boolValue => boolValue.ToLowerString(),
				_ => value?.ToString()
			};
		}


		/// <summary>
		/// Tries to get the raw value of the page cookie.
		/// </summary>
		public static bool TryGetPageCookie(this PageModel page, string key, out string value)
		{
			return page.Request.Cookies.TryGetValue(key, out value);
		}

		/// <summary>
		/// Tries to get the value of the page cookie and parse it using the invariant culture.
		/// Returns false if the cookie is missing or cannot be parsed.
		/// </summary>
		public static bool TryGetPageCookie<T>(this PageModel page, string key, out T value)
			where T : IParsable<T>
		{
			value = default;
			return page.Request.Cookies.TryGetValue(key, out string s) &&
				T.TryParse(s, CultureInfo.InvariantCulture, out value);
		}

		/// <summary>
		/// Gets the value of the page cookie, or the default value if the cookie is missing or cannot be parsed.
		/// </summary>
		public static T GetPageCookie<T>(this PageModel page, string key, T defaultValue = default)
			where T : IParsable<T>
		{
			return page.TryGetPageCookie(key, out T value) ? value : defaultValue;
		}

		/// <summary>
		/// Sets the page cookie. The cookie is sent by the browser only to the current page.
		/// </summary>
		public static void SetPageCookie(this PageModel page, string key, string value)
		{
			page.Response.Cookies.Append(key, value, CreateCookieOptions(page));
		}

		/// <summary>
		/// Sets the page cookie, formatting the value using the invariant culture.
		/// </summary>
		public static void SetPageCookie<T>(this PageModel page, string key, T value)
			where T : struct
		{
			page.SetPageCookie(key, CookieValueToString(value));
		}

		/// <summary>
		/// Sets the page cookie if the specified value differs from the stored one.
		/// </summary>
		public static void SetPageCookieIfChanged<T>(this PageModel page, string key, T value)
			where T : struct
		{
			string stringValue = CookieValueToString(value);

			if (!page.TryGetPageCookie(key, out string stored) || stored != stringValue)
			{
				page.SetPageCookie(key, stringValue);
			}
		}

		/// <summary>
		/// Deletes the page cookie.
		/// </summary>
		public static void DeletePageCookie(this PageModel page, string key)
		{
			// path must match the one used when the cookie was set
			page.Response.Cookies.Delete(key, new CookieOptions { Path = page.GetPageCookiePath() });
		}
	}
}
