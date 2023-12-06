using System;
using System.Collections.Generic;

namespace Colosoft.Mapping.Expressions
{
    internal sealed class DynamicEqualityComparer<T> : IEqualityComparer<T>
        where T : class
    {
        private readonly Func<T, T, bool> func;

        public DynamicEqualityComparer(Func<T, T, bool> func)
        {
            this.func = func ?? throw new ArgumentNullException(nameof(func));
        }

        public bool Equals(T x, T y)
        {
            return this.func(x, y);
        }

        public int GetHashCode(T obj)
        {
            return 0;
        }
    }
}