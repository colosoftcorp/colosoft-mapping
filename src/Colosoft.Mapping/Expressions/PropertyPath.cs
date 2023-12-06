using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Colosoft.Mapping.Expressions
{
    public sealed class PropertyPath : IEnumerable<PropertyInfo>, IEquatable<PropertyPath>
    {
        private static readonly PropertyPath EmptyValue = new PropertyPath();

        private readonly List<PropertyInfo> components = new List<PropertyInfo>();

        public PropertyPath(IEnumerable<PropertyInfo> components)
        {
            if (components == null)
            {
                throw new ArgumentNullException(nameof(components));
            }

            this.components.AddRange(components);
        }

        public PropertyPath(PropertyInfo component)
        {
            if (component == null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            this.components.Add(component);
        }

        private PropertyPath()
        {
        }

        public static PropertyPath Empty => EmptyValue;

        public int Count => this.components.Count;

        public PropertyInfo this[int index] => this.components[index];

        public override string ToString()
        {
            var propertyPathName = new StringBuilder();

            foreach (var pi in this.components)
            {
                propertyPathName.Append(pi.Name);
                propertyPathName.Append('.');
            }

            return propertyPathName.ToString(0, propertyPathName.Length - 1);
        }

        public bool Equals(PropertyPath other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.components.SequenceEqual(
                other.components,
                new DynamicEqualityComparer<PropertyInfo>((p1, p2) => p1.IsSameAs(p2)));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (!(obj is PropertyPath))
            {
                return false;
            }

            return this.Equals((PropertyPath)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return this.components.Aggregate(
                    0,
                    (t, n) => t ^ (n.DeclaringType.GetHashCode() * n.Name.GetHashCode() * 397));
            }
        }

        IEnumerator<PropertyInfo> IEnumerable<PropertyInfo>.GetEnumerator()
        {
            return this.components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.components.GetEnumerator();
        }
    }
}
