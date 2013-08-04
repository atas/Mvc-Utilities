using System;
using System.Web;

namespace MvcUtilities
{
    public static class CookieTools
    {
        /// <summary>
        /// Deletes a given cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cookies"></param>
        public static void DeleteCookie(string name, HttpCookieCollection cookies)
        {
            var cookie = new HttpCookie(name)
            {
                Path = "/",
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(-1D)
            };

            cookies.Add(cookie);
        }
    }
}