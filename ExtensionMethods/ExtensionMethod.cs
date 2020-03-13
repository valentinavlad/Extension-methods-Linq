using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtensionMethods
{
    public static class ExtensionMethod
    {
        public static TAccumulate Aggregate<TSource, TAccumulate>(
           this IEnumerable<TSource> source,
           TAccumulate seed,
           Func<TAccumulate, TSource, TAccumulate> func)
        {
            if (source == null || func == null)
            {
                throw new ArgumentNullException("null collection!");
            }
            foreach (var item in source)
            {
                seed = func(seed, item);
            }

            return seed;
        }

        public static bool All<TSource>(this IEnumerable<TSource> source,
                     Func<TSource, bool> predicate)
        {
            CheckNullability(source, predicate);

            foreach (var item in source)
            {
                if (!predicate(item))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Any<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            CheckNullability(source, predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<TSource> Distinct<TSource>(
            this IEnumerable<TSource> source,
            IEqualityComparer<TSource> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source is null!");
            }

            return new HashSet<TSource>(source, comparer);
        }

        public static IEnumerable<TSource> Except<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer)
        {
            if (first == null || second == null)
            {
                throw new ArgumentNullException("first or second collection is null!");
            }

            var duplicateElements = new HashSet<TSource>(second, comparer);
            
            foreach (var item in first)
            {
                if (!duplicateElements.Contains(item))
                {
                    yield return item;
                }
            }
        }

        public static TSource First<TSource>(this IEnumerable<TSource> source,
                    Func<TSource, bool> predicate)
        {
            if (source == null || predicate == null)
            {
                throw new ArgumentNullException(nameof(source), "List is null.");
            }

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    return item;
                }
            }
            throw new InvalidOperationException("No items matched the predicate");
        }

        public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            Func<TKey, IEnumerable<TElement>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            if (source == null || elementSelector == null || keySelector == null)
            {
                throw new ArgumentNullException("outer or inner or outerKeySelector or innerKeySelector or resultSelector is null.");
            }
            var dictionary = new Dictionary<TKey, List<TElement>>();
            foreach (var item in source)
            {
                var key = keySelector(item);
                var value = elementSelector(item);
                if (!dictionary.TryAdd(key,new List<TElement>(){value }))
                {
                    dictionary[key].Add(value);
                }
            }

            foreach (var key in dictionary.Keys)
            {
                yield return resultSelector(key, dictionary[key]);
            }

        }

        //public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(
        //    this IEnumerable<TSource> source,
        //    Func<TSource, TKey> keySelector,
        //    Func<TSource, TElement> elementSelector,
        //    Func<TKey, IEnumerable<TElement>, TResult> resultSelector,
        //    IEqualityComparer<TKey> comparer)
        //{
        //    if (source == null || elementSelector == null || keySelector == null)
        //    {
        //        throw new ArgumentNullException("outer or inner or outerKeySelector or innerKeySelector or resultSelector is null.");
        //    }
        //    var lookup = source.ToLookup(keySelector, elementSelector, comparer);
        //    foreach (var item in lookup)
        //    {
        //        yield return resultSelector(item.Key, item);
        //    }
        //}

        public static IEnumerable<TSource> Intersect<TSource>(
                     this IEnumerable<TSource> first,
             IEnumerable<TSource> second,
             IEqualityComparer<TSource> comparer)
        {
            if (first == null || second == null)
            {
                throw new ArgumentNullException("first or second collection is null!");
            }

            foreach (var firstItem in first)
            {
                foreach (var secondItem in second)
                {
                    if (comparer.Equals(firstItem, secondItem))
                    {
                        yield return firstItem;
                    }
                }
            }
        }
        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
                    this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
        {
            if (outer == null || inner == null
                || outerKeySelector == null || innerKeySelector == null
                || resultSelector == null)
            {
                throw new ArgumentNullException("outer or inner or outerKeySelector or innerKeySelector or resultSelector is null.");
            }

            var lookup = inner.ToLookup(innerKeySelector);
            foreach (var outerElement in outer)
            {
                var key = outerKeySelector(outerElement);
                foreach (var innerElement in lookup[key])
                {
                    yield return resultSelector(outerElement, innerElement);
                }
            }
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(
                 this IEnumerable<TSource> source,
                 Func<TSource, TKey> keySelector,
                 IComparer<TKey> comparer)
        {
            if (source == null || keySelector == null)
            {
                throw new ArgumentNullException("source or selector is null");
            }

            IComparer<TSource> sourceComparer = new CompareByKey<TSource, TKey>(keySelector, comparer);
            return new OrderedEnumerable<TSource>(source, sourceComparer);
        }

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(
            this IOrderedEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            return source.CreateOrderedEnumerable(keySelector, comparer, false);
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, TResult> selector)
        {
            if (source == null || selector == null)
            {
                throw new ArgumentNullException("source or selector is null");
            }
            foreach (var item in source)
            {
                yield return selector(item);
            }
        }

        public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TResult>> selector)
        {
            if (source == null || selector == null)
            {
                throw new ArgumentNullException("source or selector is null");
            }

            foreach (var item in source)
            {
                selector(item);
                foreach (var item1 in selector(item))
                {
                    yield return item1;
                }
            }
        }

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(
          this IEnumerable<TSource> source,
          Func<TSource, TKey> keySelector,
          Func<TSource, TElement> elementSelector)
        {
            if (source == null || keySelector == null)
            {
                throw new ArgumentNullException("source or keySelector is null!");
            }

            Dictionary<TKey, TElement> dictionary = new Dictionary<TKey, TElement>();
            foreach (var item in source)
            {

                TKey key = keySelector(item);

                if (dictionary.ContainsKey(key) || key == null)
                {
                    throw new ArgumentException("duplicate key or null key!");
                }

                TElement element = elementSelector(item);
                dictionary.Add(key, element);
            }

            return dictionary;
        }

        public static IEnumerable<TSource> Union<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer)
        {
            if (first == null || second == null)
            {
                throw new ArgumentNullException("first or second collection is null!");
            }
            HashSet<TSource> seenElements = new HashSet<TSource>(comparer);
            foreach (var item in first)
            {
                if (seenElements.Add(item))
                {
                    yield return item;
                }
            }
            foreach (var item in second)
            {
                if (seenElements.Add(item))
                {
                    yield return item;
                }
            }

        }

        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source,
                    Func<TSource, bool> predicate)
        {
            if (source == null || predicate == null)
            {
                throw new ArgumentNullException("source or predicate is null!");
            }

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null || second == null)
            {
                throw new ArgumentNullException("null collection!");
            }

            var firstEnumerator = first.GetEnumerator();
            var secondEnumerator = second.GetEnumerator();

            while (firstEnumerator.MoveNext() && secondEnumerator.MoveNext())
            {
                yield return resultSelector(firstEnumerator.Current, secondEnumerator.Current);
            }
        }

        private static void CheckNullability<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null || predicate == null)
            {
                throw new ArgumentNullException("your source is null!");
            }
        }

    }
}
