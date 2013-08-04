using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcUtilities
{
    public static class OperationTools
    {
        /// <summary>
        /// ForEach alternative for IEnumerables. Safe to use with null values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elem"></param>
        /// <param name="action"></param>
        public static void Each<T>(this IEnumerable<T> elem, Action<T> action)
        {
            if (elem == null)
                return;

            foreach (var e in elem)
            {
                var localE = e;
                action(localE);
            }
        }

        /// <summary>
        /// ForEach alternative for IEnumerables. Safe to use with null values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elem"></param>
        /// <param name="action">second int parameter points out the order of currently processed element by the lambda</param>
        public static void Each<T>(this IEnumerable<T> elem, Action<T, int> action)
        {
            if (elem == null)
                return;

            int i = 0;
            foreach (var e in elem)
            {
                var localE = e;
                action(localE, i++);
            }
        }
    }
}
