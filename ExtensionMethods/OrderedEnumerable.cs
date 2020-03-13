using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtensionMethods
{
    public class OrderedEnumerable<TElement> : IOrderedEnumerable<TElement>
    {
        private readonly IEnumerable<TElement> source;
        private readonly IComparer<TElement> currentComparer;

        internal OrderedEnumerable(IEnumerable<TElement> source,
            IComparer<TElement> comparer)
        {
            this.source = source;
            this.currentComparer = comparer;
        }

        public IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, 
            IComparer<TKey> comparer,
            bool descending)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector is null");
            }

            IComparer<TElement> secondaryComparer =
                       new CompareByKey<TElement, TKey>(keySelector, comparer);

            return new OrderedEnumerable<TElement>(source,
                new CompareObjects<TElement>(currentComparer, secondaryComparer));
        }

        public IEnumerator<TElement> GetEnumerator()
        {
           var list = source.ToList();
            while (list.Count > 0)
            {
                TElement minElement = list[0];
                int minIndex = 0;
                for (int i = 1; i < list.Count; i++)
                {
                    if (currentComparer.Compare(list[i], minElement) < 0)
                    {
                        minElement = list[i];
                        minIndex = i;
                    }
                }
                list.RemoveAt(minIndex);
                yield return minElement;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
