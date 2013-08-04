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
            TimeSpan timeDifference = DateTime.UtcNow -
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long unixEpochTime = Convert.ToInt64(timeDifference.TotalSeconds);

            return unixEpochTime;
        }
    }
}