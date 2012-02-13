using System.Collections.Generic;
using System.Linq;

namespace System.Collections.Generic
{
    public static class EnumerableExtensions
    {

        public static Boolean IsEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return true; // or throw an exception
            return !source.Any();
        }


        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection) action(item);
            return collection;
        }

    
    
    
    }
}
