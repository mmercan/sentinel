using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class ModeByExtension
    {
        public static TKey ModeBy<T, TKey>(this IQueryable<T> sequence, Expression<Func<T, TKey>> selector)
        {
            var item = sequence.GroupBy(selector).Select(p => new { key = p.Key, count = p.Count() }).FirstOrDefault();
            if (item == null) return default(TKey);
            else return item.key;
        }

    }
}
