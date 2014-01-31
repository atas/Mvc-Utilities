using System;

namespace MvcUtilities
{
    public static class DateTools
    {
        /// <summary>
        /// Returns current Unix Epoch time
        /// </summary>
        /// <returns></returns>
        public static long EpochTimeNow()
        {
            TimeSpan timeDifference = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long unixEpochTime = Convert.ToInt64(timeDifference.TotalSeconds);

            return unixEpochTime;
        }

        /// <summary>
        /// Converts and epoch time to DateTime
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime FromUnixTime(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        /// <summary>
        /// Converts a DateTime to Epoch time
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long ToUnixTime(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }


        /// <summary>
        /// Returns a human-readable string for the passed time until now
        /// </summary>
        /// <param name="time"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        public static string PassedTime(this DateTime time, PassedTimeWords words)
        {
            TimeSpan span = DateTime.Now.Subtract(time);

            if (span.Days > 365 * 1.5)
                return span.Days / 30 + " " + words.YearsAgo;
            if (span.Days > 60)
                return span.Days / 30 + " " + words.MonthsAgo;
            if (span.Days > 30)
                return span.Days / 30 + " " + words.MonthAgo;
            if (span.Days > 1)
                return span.Days + " " + words.DaysAgo;
            if (span.Days > 0)
                return span.Days + " " + words.DayAgo;
            if (span.Hours > 1)
                return span.Hours + " " + words.HoursAgo;
            if (span.Hours > 0)
                return span.Hours + " " + words.HourAgo;
            if (span.Minutes > 1)
                return span.Hours + " " + words.MinutesAgo;

            return span.Minutes + " " + words.MinutesAgo;
        }

        public class PassedTimeWords
        {
            public string MonthsAgo { get; set; }
            public string MonthAgo { get; set; }
            public string DaysAgo { get; set; }
            public string DayAgo { get; set; }
            public string HoursAgo { get; set; }
            public string HourAgo { get; set; }
            public string MinutesAgo { get; set; }
            public string MinuteAgo { get; set; }
            public string YearsAgo { get; set; }
        }
    }
}