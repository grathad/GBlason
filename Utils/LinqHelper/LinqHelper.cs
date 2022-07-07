using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("Utils.Test")]
[assembly: InternalsVisibleTo("Blazon.Test")]
namespace Utils.LinqHelper
{
    /// <summary>
    /// Helper methods to extend linq
    /// </summary>
    public static class LinqHelper
    {
        /// <summary>
        /// Return the element that get the maximum value as defined in <paramref name="comparison"/>
        /// </summary>
        /// <remarks>The comparison is only applied to the items in the list that are <b>NOT</b> null</remarks>
        /// <typeparam name="T">The type of the elements in the list</typeparam>
        /// <param name="source">The list containing the elements to test</param>
        /// <param name="comparison">The comparison that will return the value to determine the maximum</param>
        /// <returns>The first element in the collection that get the best score from <paramref name="comparison"/></returns>
        /// <exception cref="ArgumentNullException">If <paramref name="source"/> or <paramref name="comparison"/> are null</exception>
        public static T GetMaxElement<T>(this IEnumerable<T> source, Func<T, int> comparison)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (comparison == null)
            {
                throw new ArgumentNullException(nameof(comparison));
            }
            var max = int.MinValue;
            var toReturn = default(T);
            foreach (var value in source.Where(v => v != null))
            {
                var val = comparison.Invoke(value);
                if (val <= max)
                {
                    continue;
                }
                max = val;
                toReturn = value;
            }
            return toReturn;
        }

        /// <summary>
        /// Add an element to the list ONLY if not already present
        /// </summary>
        /// <typeparam name="T">The type of element in the list (and to add)</typeparam>
        /// <param name="source">The list to add the element in</param>
        /// <param name="elementToAdd">The element to add</param>
        /// <returns>The element if added, otherwise null </returns>
        public static T AddDistinct<T>(this IList<T> source, T elementToAdd)
        {
            if(source == null)
            {
                return default(T);
            }
            if(elementToAdd == null)
            {
                return default(T);
            }

            if (source.Contains(elementToAdd))
            {
                return default(T);
            }

            source.Add(elementToAdd);
            return elementToAdd;
        }
    }
}