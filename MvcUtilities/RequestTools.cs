using System.Globalization;
using System.Linq;

namespace MvcUtilities
{
    public class RequestTools
    {
        /// <summary>
        /// Gets the CultureInfo by the localization of the user's browser's
        /// </summary>
        /// <param name="userLanguages">Get it from HttpContext.Request.UserLanguages</param>
        /// <returns></returns>
        public static CultureInfo GetBrowserCultureInfo(string[] userLanguages)
        {
            CultureInfo ci;

            if (userLanguages != null && userLanguages.Any())
            {
                try
                {
                    ci = new CultureInfo(userLanguages[0]);
                }
                catch (CultureNotFoundException)
                {
                    ci = CultureInfo.InvariantCulture;
                }
            }
            else
            {
                ci = CultureInfo.InvariantCulture;
            }

            return ci;
        }
    }
}