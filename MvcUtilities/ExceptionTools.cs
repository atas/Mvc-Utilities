using System;

namespace MvcUtilities
{
    public static class ExceptionTools
    {
        /// <summary>
        /// Throws a NullReferenceException if the given object is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="variableName"></param>
        /// <returns></returns>
        public static T ThrowIfNull<T>(this T value, string variableName)
        {
            if (Equals(value, null))
            {
                throw new NullReferenceException(string.Format("Value is Null: {0}", variableName));
            }

            return value;
        }
    }
}
