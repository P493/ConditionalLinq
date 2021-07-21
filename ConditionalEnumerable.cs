using System;
using System.Collections.Generic;
using System.Linq;

namespace ConditionalLinq
{
    /// <summary>
    /// make sure to use ConditionalQueryable style when it's linq to sql
    /// </summary>
    public static class ConditionalEnumerable
    {
        public static IEnumerable<T> MayWhereInt<T>(this IEnumerable<T> source, string value, Func<int, T, bool> predicate)
        {
            if (!int.TryParse(value, out int typed_value))
            {
                return source;
            }
            return source.Where(t => predicate(typed_value, t));
        }

        public static IEnumerable<T> MayWhereString<T>(this IEnumerable<T> source, string value, Func<string, T, bool> predicate)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return source;
            }
            return source.Where(t => predicate(value, t));
        }
    }
}
