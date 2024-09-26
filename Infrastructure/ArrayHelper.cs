using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Infrastructure
{
    public static class ArrayHelper
    {
        public static void ForEach<T>(this T[,] array,Action<T> action)
        {
            ForEach(array, (obj, i, j) => action(obj));
        }
        public static void ForEach<T>(this T[,] array,Action<T,int,int> action)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    action(array[i, j], i, j);
                }
            }
        }
        public static ReadOnlyCollection<T> GetNeiborghood<T>(this T[,] array, int i, int j)
        {
            var result = new List<T>();
          
            if (i - 1 >= 0)
            {
                result.Add(array[i - 1, j]);
                if (j - 1 >= 0)
                {
                    result.Add(array[i - 1, j - 1]);
                }
                if (j < array.GetLength(1) - 1)
                {
                    result.Add(array[i - 1, j + 1]);
                }
            }
            if (i + 1 <= array.GetLength(0) - 1)
            {
                result.Add(array[i + 1, j]);
                if (j - 1 >= 0)
                {
                    result.Add(array[i + 1, j - 1]);
                }
                if (j + 1 <= array.GetLength(1) - 1)
                {
                    result.Add(array[i + 1, j + 1]);
                }
            }
            if (j - 1 >= 0)
            {
                result.Add(array[i, j - 1]);
            }
            if (j + 1 <= array.GetLength(1) - 1)
            {
                result.Add(array[i, j + 1]);
            }
            return new ReadOnlyCollection<T>(result);
        }

        public static T[,] ToDegenerated2DArray<T>(this IList<T> collection) where T:new()
        {
            var count = collection.Count;
            var result = new T[1, count];
                    
            for (int i = 0; i < count; i++)
            {
                result[0, i] = collection[i];
            }
            return result;
        }
     
    }
}
