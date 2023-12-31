﻿using System;
using System.Linq;
using System.Reflection;

namespace Colosoft.Mapping.Expressions
{
    public static class PropertyInfoExtensions
    {
        public static bool IsSameAs(this PropertyInfo propertyInfo, PropertyInfo otherPropertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }
            else if (otherPropertyInfo == null)
            {
                throw new ArgumentNullException(nameof(otherPropertyInfo));
            }

            return (propertyInfo == otherPropertyInfo)
                || (propertyInfo.Name == otherPropertyInfo.Name
                    && (propertyInfo.DeclaringType == otherPropertyInfo.DeclaringType
                        || propertyInfo.DeclaringType.IsSubclassOf(otherPropertyInfo.DeclaringType)
                        || otherPropertyInfo.DeclaringType.IsSubclassOf(propertyInfo.DeclaringType)
                        || propertyInfo.DeclaringType.GetInterfaces().Contains(otherPropertyInfo.DeclaringType)
                        || otherPropertyInfo.DeclaringType.GetInterfaces().Contains(propertyInfo.DeclaringType)));
        }
    }
}
