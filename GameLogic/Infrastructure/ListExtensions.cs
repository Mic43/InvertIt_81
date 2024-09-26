using System.Collections.Generic;
using System.Linq;

namespace GameLogic.Infrastructure
{
    public static class ListExtensions
    {
        public static IList<T> Optimize2<T>(this IList<T> collection)
        {
            var dict = collection.ToLookup(x => x);
            return dict.Where(x => x.Count()%2 !=0).Select(x => x.Key).ToList();
        }

        public static IList<T> Optimize<T>(this IList<T> collection)
        {
            var resylt = new List<T>();
            int i = 0;
            while ( i < collection.Count)
            {
                int j = i;
                while (j < collection.Count && collection[j].Equals(collection[i]))
                {
                    j++;
                }
                int subSequenceLen = j - i ;
                if (subSequenceLen%2 != 0)
                {                 
                    if(resylt.Count == 0 || !resylt.Last().Equals(collection[i]))
                        resylt.Add(collection[i]);
                    else
                    {
                        resylt.RemoveAt(resylt.Count - 1);
                    }
                }

                i += subSequenceLen;
            }
            return resylt;
        }
    }
}