using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public static class CommonUtility
    {
        public static bool IsNullOrEmpty(this IEnumerable coll)
        {
            if (coll != null)
            {
                return !coll.GetEnumerator().MoveNext();
            }
            return true;
        }
        public static Collection<T> ToCollection<T>(this IEnumerable<T> coll)
        {
            Collection<T> final = new Collection<T>();
            if (coll != null)
            {
                foreach (var item in coll)
                {
                    final.Add(item);
                }
            }
            return final;
        }

        public static Collection<T> Safely<T>(this IEnumerable<T> coll)
        {
            if (coll == null)
            {
                return new Collection<T>();
            }
            return coll.ToCollection();
        }

        public static void AddRange<T>(this Collection<T> coll, Collection<T> coll2)
        {
            if (coll == null)
            {
                coll = new Collection<T>();
            }
            if (!coll2.IsNullOrEmpty())
            {
                foreach (var item in coll2)
                {
                    coll.Add(item);
                }
            }
        }
    }
}
