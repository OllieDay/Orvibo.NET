using System.Collections.Generic;
using System.Linq;

namespace Orvibo.Extensions
{
    /// <summary>
    ///     Enumerable extensions.
    /// </summary>
    internal static class EnumerableExtensions
    {
        /// <summary>
        ///     Finds all indexes of value in source.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source and value.</typeparam>
        /// <param name="source">Source to search.</param>
        /// <param name="value">Value to find.</param>
        /// <param name="comparer">Comparer used to test equality.</param>
        /// <returns>Indexes of value in source.</returns>
        public static IEnumerable<int> IndexOfSequence<T>(this IEnumerable<T> source, IEnumerable<T> value, IEqualityComparer<T> comparer)
        {
            if (value == null)
            {
                yield break;
            }

            var s = source as T[] ?? source.ToArray();
            var v = value as T[] ?? value.ToArray();

            if (s.Length == 0 || v.Length == 0 || s.Length < v.Length)
            {
                yield break;
            }

            if (ReferenceEquals(source, value))
            {
                yield return 0;
                yield break;
            }

            if (s.Length == v.Length)
            {
                if (s.SequenceEqual(v))
                {
                    yield return 0;
                }

                yield break;
            }

            for (var i = 0; i < s.Length - v.Length + 1; i++)
            {
                var start = i;

                if (v.All(t => comparer.Equals(s[start++], t)))
                {
                    yield return i;
                }
            }
        }

        /// <summary>
        ///     Finds all indexes of value in source using the default equality comparer.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source and value.</typeparam>
        /// <param name="source">Source to search.</param>
        /// <param name="value">Value to find.</param>
        /// <returns>Indexes of value in source.</returns>
        public static IEnumerable<int> IndexOfSequence<T>(this IEnumerable<T> source, IEnumerable<T> value)
        {
            return source.IndexOfSequence(value, EqualityComparer<T>.Default);
        }
    }
}
