using System.Collections.Generic;
using System.Linq;

namespace RewriteMe.Business.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this T[] array, int size)
        {
            for (var i = 0; i < (float)array.Length / size; i++)
            {
                yield return array.Skip(i * size).Take(size);
            }
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this IList<T> array, int size)
        {
            for (var i = 0; i < (float)array.Count / size; i++)
            {
                yield return array.Skip(i * size).Take(size);
            }
        }
    }
}
