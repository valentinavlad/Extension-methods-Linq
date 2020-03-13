using System;
using System.Collections.Generic;
using System.Text;

namespace ExtensionMethods
{
    public class CompareObjects<T> : IComparer<T>
    {
        private readonly IComparer<T> primary;
        private readonly IComparer<T> secondary;
        public CompareObjects(IComparer<T> primary,
            IComparer<T> secondary)
        {
            this.primary = primary;
            this.secondary = secondary;
        }

        public int Compare(T x, T y)
        {
            int primaryResult = primary.Compare(x, y);
            if (primaryResult != 0)
            {
                return primaryResult;
            }
            return secondary.Compare(x, y);
        }
    }
}
