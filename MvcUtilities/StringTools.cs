using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace MvcUtilities
{
    public static class StringTools
    {

        /// <summary>
        /// Returns Sha1 hash of the given data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Sha1Hash(byte[] data)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                return sha1.ComputeHash(data);
            }
        }

        /// <summary>
        /// Splits the string with one separator string instead of an array separator.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string[] Split(this string str, string separator, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            return str.Split(new[] { separator }, options);
        }


        /// <see cref="BeautifyNumber(double)"/>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string BeautifyNumber(this string number)
        {
            return BeautifyNumber(Convert.ToInt32(number));
        }

        /// <see cref="BeautifyNumber(double)"/>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string BeautifyNumber(this int number)
        {
            return BeautifyNumber((double)number);
        }

        /// <summary>
        /// Returns 1M, 3M etc. short string of given numeric value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string BeautifyNumber(this double number)
        {
            if (number > 10000000)
                return Math.Round(number / 1000000) + "M";
            if (number > 1000000)
                return Math.Round(number / 1000000, 1).ToString() + "M";
            if (number > 10000)
                return Math.Round(number / 1000) + "K";
            if (number > 1000)
                return Math.Round(number / 1000, 1) + "K";

            return ((int)number).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Removes tags in a given html string
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string StripTags(this string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            return Regex.Replace(html, "<[^>]*>", string.Empty);
        }

        /// <summary>
        /// Returns the string between given starting and ending strings
        /// </summary>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string Between(this string source, string start, string end)
        {
            string[] startArr = { start };
            string[] endArr = { end };

            string[] splitted1 = source.Split(startArr, 2, StringSplitOptions.None);

            if (splitted1.Length < 2)
                return null;

            string str2 = splitted1[1].Split(endArr, 2, StringSplitOptions.None)[0];

            return str2;
        }

        /// <summary>
        /// Removes the string given starting an ending strings
        /// </summary>
        /// <param name="s"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string RemoveBetween(this string s, string begin, string end)
        {
            var regex = new Regex(string.Format("\\{0}.*?\\{1}", begin, end));
            return regex.Replace(s, string.Empty);
        }

        /// <summary>
        /// FormatWith 2.0 - String formatting with named variables
        /// http://james.newtonking.com/archive/2008/03/29/formatwith-2-0-string-formatting-with-named-variables.aspx
        /// </summary>
        /// <param name="format"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, object source)
        {
            return FormatWith(format, null, source);
        }

        /// <summary>
        /// <see cref="FormatWith(string,object)"/>
        /// </summary>
        /// <param name="format"></param>
        /// <param name="provider"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, IFormatProvider provider, object source)
        {
            if (format == null)
                throw new ArgumentNullException("format");

            Regex r = new Regex(@"(?<start>\{)+(?<property>[\w\.\[\]]+)(?<format>:[^}]+)?(?<end>\})+",
              RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

            var values = new List<object>();
            string rewrittenFormat = r.Replace(format, delegate(Match m)
            {
                Group startGroup = m.Groups["start"];
                Group propertyGroup = m.Groups["property"];
                Group formatGroup = m.Groups["format"];
                Group endGroup = m.Groups["end"];

                values.Add((propertyGroup.Value == "0")
                  ? source
                  : DataBinder.Eval(source, propertyGroup.Value));

                return new string('{', startGroup.Captures.Count) + (values.Count - 1) + formatGroup.Value
                  + new string('}', endGroup.Captures.Count);
            });

            return string.Format(provider, rewrittenFormat, values.ToArray());
        }

        /// <summary>
        /// Limits string length to given character, if string is longer, prepends '...'
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string LimitLength(this string str, int length)
        {
            if (str.Length > length - 3)
                str = str.Substring(0, length - 3) + "...";

            return str;
        }

        /// <summary>
        /// Converts given string to Integer
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            return Convert.ToInt32(str);
        }

        /// <summary>
        /// Converts given string to double
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int ToInt(this double d)
        {
            return Convert.ToInt32(d);
        }

        /// <summary>
        /// Makes the first character uppercase of given string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToUpperFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>
        /// Extension method instead of String.Format()
        /// Being used as string.StringFormat()
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string StringFormat(this string str, params object[] args)
        {
            return String.Format(str, args);
        }

        /// <summary>
        /// Puts html br elements instead of new lines
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isXhtml"></param>
        /// <returns></returns>
        public static string Nl2br(this string input, bool isXhtml = true)
        {
            return input.Replace("\r\n", isXhtml ? "<br />\r\n" : "<br>\r\n")
                .Replace("\n", isXhtml ? "<br />\r\n" : "<br>\r\n");
        }

        /// <summary>
        /// Indicates whether the specified string is null or an System.String.Empty string.
        /// </summary>
        /// <seealso cref="string.IsNullOrEmpty"/>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool NullOrEmpty(this string str)
        {
            return String.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Minimally converts a string to an HTML-encoded string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HtmlAttributeEncode(this string str)
        {
            if (str.NullOrEmpty())
                return str;

            return HttpUtility.HtmlAttributeEncode(str);
        }

        /// <summary>
        /// Returns a random string with a given size
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string RandomString(int size)
        {
            var random = new Random((int)DateTime.Now.Ticks);

            var builder = new StringBuilder();

            for (int i = 0; i < size; i++)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString().ToLowerInvariant();
        }

        /// <summary>
        /// Shortcut for HttpUtility.UrlEncode(string)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlEncode(this string url)
        {
            return HttpUtility.UrlEncode(url);
        }

        /// <summary>
        /// Shortcut for HttpUtility.UrlDecode(string)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlDecode(this string url)
        {
            return HttpUtility.UrlDecode(url);
        }

        public static object UpdateIf(this object obj, object equalityValue, object newValue)
        {
            if (obj.Equals(equalityValue))
                return newValue;

            return obj;
        }
    }
}