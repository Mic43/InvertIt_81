using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    public static class LinqExt
    {
        public static Maybe<T> GetNext<T>(this IList<T> collection, T value)
        {
            int index = collection.IndexOf(value);
            if (index == -1)            
                throw new ArgumentException("value","provided value was nto founf in collection");
            
            int nextIndex = index + 1;
            if (nextIndex < collection.Count)
            {
                return new Maybe<T>(collection[nextIndex]);
            }
            return new Maybe<T>(); //Or throw an exception
        }

        public static IEnumerable<List<T>> Batch<T>(this IEnumerable<T> source, int max)
        {
            var toReturn = new List<T>(max);
            foreach (var item in source)
            {
                toReturn.Add(item);
                if (toReturn.Count == max)
                {
                    yield return toReturn;
                    toReturn = new List<T>(max);
                }
            }
            if (toReturn.Any())
            {
                yield return toReturn;
            }
        }
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
    Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, Comparer<TKey>.Default);
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            if (comparer == null) throw new ArgumentNullException("comparer");

            using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence was empty");
                }
                TSource min = sourceIterator.Current;
                TKey minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    TSource candidate = sourceIterator.Current;
                    TKey candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }
    }
}