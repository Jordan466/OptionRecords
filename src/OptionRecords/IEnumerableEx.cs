using System;
using System.Collections.Generic;

namespace OptionRecords
{
    /// <summary>
    /// Extensions for IEnumerable
    /// </summary>
    public static class IEnumerableEx
    {
        /// <summary>
        /// Returns the first matching value in the enumerable, or none
        /// </summary>
        public static Option<T> FirstOrNone<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            foreach (var item in enumerable)
            {
                if (predicate(item))
                    return new Some<T>(item);
            }
            return new None<T>();
        }

        /// <summary>
        /// Maps the items in the enumerable, filters out the None cases
        /// </summary>
        public static IEnumerable<U> Choose<T, U>(this IEnumerable<T> enumerable, Func<T, Option<U>> map)
        {
            foreach (var item in enumerable)
            {
                if (map(item) is Some<U> s)
                    yield return s.Value;
            }
        }
    }
}