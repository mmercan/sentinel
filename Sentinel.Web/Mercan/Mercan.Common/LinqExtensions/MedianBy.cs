using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace System.Linq
{

    //TODO: this is not optimal for EntityFramework pulling all the data (tolist) and running them not the best way 
    //Do More
    public static class MedianByExtension
    {

        private static int Partition<T>(this IList<T> list, int start, int end, Random rnd = null) where T : IComparable<T>
        {
            if (rnd != null)
                list.Swap(end, rnd.Next(start, end));

            var pivot = list[end];
            var lastLow = start - 1;
            for (var i = start; i < end; i++)
            {
                if (list[i].CompareTo(pivot) <= 0)
                    list.Swap(i, ++lastLow);
            }
            list.Swap(end, ++lastLow);
            return lastLow;
        }

        /// <summary>
        /// Returns Nth smallest element from the list. Here n starts from 0 so that n=0 returns minimum, n=1 returns 2nd smallest element etc.
        /// Note: specified list would be mutated in the process.
        /// Reference: Introduction to Algorithms 3rd Edition, Corman et al, pp 216
        /// </summary>
        public static T NthOrderStatistic<T>(this IList<T> list, int n, Random rnd = null) where T : IComparable<T>
        {
            return NthOrderStatistic(list, n, 0, list.Count - 1, rnd);
        }
        private static T NthOrderStatistic<T>(this IList<T> list, int n, int start, int end, Random rnd) where T : IComparable<T>
        {
            while (true)
            {
                var pivotIndex = list.Partition(start, end, rnd);
                if (pivotIndex == n)
                    return list[pivotIndex];

                if (n < pivotIndex)
                    end = pivotIndex - 1;
                else
                    start = pivotIndex + 1;
            }
        }

        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            if (i == j)   //This check is not required but Partition function may make many calls so its for perf reason
                return;
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        /// <summary>
        /// Note: specified list would be mutated in the process.
        /// </summary>
        public static T Median<T>(this IList<T> list) where T : IComparable<T>
        {
            return list.NthOrderStatistic((list.Count - 1) / 2);
        }

        public static double Median<T>(this IEnumerable<T> sequence, Func<T, double> getValue)
        {
            var list = sequence.Select(getValue).ToList();
            var mid = (list.Count - 1) / 2;
            return list.NthOrderStatistic(mid);
        }


        //public static TResult MedianBy<T, TResult>(this IEnumerable<T> sequence, Func<T, TResult> getValue)
        //{
        //    var list = sequence.Select(getValue).OrderByDescending(p => p);
        //    var mid = (list.Count() - 1) / 2;
        //    if (mid == 0)
        //    {
        //        throw new InvalidOperationException("Empty collection");
        //    }
        //    if (mid % 2 == 0)
        //    {
        //        var elem1 = list.ElementAt(mid);
        //        var elem2 = list.ElementAt(mid - 1);
        //        return ((dynamic)elem1 + elem2) / 2.0;
        //    }
        //    else
        //    {
        //        return list.ElementAt(mid);
        //    }
        //}

        public static TResult MedianBy<T, TResult>(this IQueryable<T> sequence, Expression<Func<T, TResult>> getValue)
        {
            var count = sequence.Count();
            var list = sequence.OrderByDescending(getValue).Select(getValue);
            var mid = count / 2;
            if (mid == 0)
            {
                throw new InvalidOperationException("Empty collection");
            }
            if (count % 2 == 0)
            {
                var elem1 = list.Skip(mid - 1).FirstOrDefault();
                var elem2 = list.Skip(mid).FirstOrDefault();

                // return (Convert.ToDecimal(elem1) + Convert.ToDecimal(elem2)) / 2M;

                var expConstElem1 = Expression.Constant(elem1);
                var expConstElem2 = Expression.Constant(elem2);
                var addition = Expression.Add(expConstElem1, expConstElem2);

                var lamb = Expression.Lambda<Func<TResult>>(addition, null);
                var resAddition = lamb.Compile().Invoke();


                var ExpresAddition = Expression.Constant(resAddition);
                var exptwo = Expression.Constant(2);
                var expconvert = Expression.Convert(exptwo, typeof(TResult));

                var expDivide = Expression.Divide(ExpresAddition, expconvert);
                var lambDivide = Expression.Lambda<Func<TResult>>(expDivide, null);
                var endres = lambDivide.Compile().Invoke();
                return endres;
            }
            else
            {
                return list.Skip(mid).FirstOrDefault();
                //ElementAt Doesn't work with SQL
                //return list.ElementAt(mid);
            }
        }
    }
}
