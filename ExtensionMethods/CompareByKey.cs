using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ExtensionMethods
{
    public class CompareByKey<TElement, TKey> : IComparer<TElement>
    {

        private readonly Func<TElement, TKey> keySelector;
        private readonly IComparer<TKey> comparer;
        
        public CompareByKey(Func<TElement, TKey> keySelector,
            IComparer<TKey> comparer)
        {
            this.keySelector = keySelector;
            this.comparer = comparer;
        }

        public int Compare(TElement x, TElement y)
        {
            TKey keyX = keySelector(x);
            TKey keyY = keySelector(y);
            return comparer.Compare(keyX, keyY);
        }
    }
}
