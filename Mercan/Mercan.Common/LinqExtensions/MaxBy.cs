using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class MaxByExtension
    {
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            var comparer = Comparer<TKey>.Default;
            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }

                TSource max = sourceIterator.Current;
                var maxKey = selector(max);
                while (sourceIterator.MoveNext())
                {
                    TSource candicate = sourceIterator.Current;
                    TKey candicateProjected = selector(candicate);
                    if (comparer.Compare(candicateProjected, maxKey) > 0)
                    {
                        max = candicate;
                        maxKey = candicateProjected;

                    }
                }
                return max;
            }
        }


    }
}