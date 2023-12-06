using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Colosoft.Mapping.Expressions
{
    public static class ExpressionExtensions
    {
        public static Action<T, TResult> GetSetter<T, TResult>(Expression<Func<T, TResult>> parentProperty)
        {
            var property = parentProperty.GetMember() as PropertyInfo;

            if (property.CanWrite)
            {
                return (parent, child) =>
                {
                    if (parent == null)
                    {
                        throw new ArgumentNullException(nameof(parent));
                    }

                    property.SetValue(parent, child, null);
                };
            }
            else
            {
                return null;
            }
        }

        public static MemberInfo GetMember<T, TResult>(this Expression<Func<T, TResult>> expression)
        {
            var memberExp = RemoveUnary(expression.Body);

            if (memberExp == null)
            {
                return null;
            }

            return memberExp.Member;
        }

        private static MemberExpression RemoveUnary(Expression toUnwrap)
        {
            var unaryExpression = toUnwrap as UnaryExpression;

            if (unaryExpression != null)
            {
                if (unaryExpression.Operand is UnaryExpression level2)
                {
                    return (MemberExpression)level2.Operand;
                }

                return (MemberExpression)unaryExpression.Operand;
            }

            return toUnwrap as MemberExpression;
        }

        public static Expression RemoveConvert(this Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            while (expression.NodeType == ExpressionType.Convert
                   || expression.NodeType == ExpressionType.ConvertChecked)
            {
                expression = ((UnaryExpression)expression).Operand;
            }

            return expression;
        }

        private static PropertyPath MatchPropertyAccess(
            this Expression parameterExpression, Expression propertyAccessExpression)
        {
            if (parameterExpression == null)
            {
                throw new ArgumentNullException(nameof(parameterExpression));
            }
            else if (propertyAccessExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyAccessExpression));
            }

            var propertyInfos = new List<PropertyInfo>();

            MemberExpression memberExpression;

            do
            {
                memberExpression = RemoveConvert(propertyAccessExpression) as MemberExpression;

                if (memberExpression == null)
                {
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
                    return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
                }

                var propertyInfo = memberExpression.Member as PropertyInfo;

                if (propertyInfo == null)
                {
                    return null;
                }

                propertyInfos.Insert(0, propertyInfo);

                propertyAccessExpression = memberExpression.Expression;
            }
            while (memberExpression.Expression != parameterExpression);

            return new PropertyPath(propertyInfos);
        }

        private static PropertyPath MatchSimplePropertyAccess(
            this Expression parameterExpression, Expression propertyAccessExpression)
        {
            if (propertyAccessExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyAccessExpression));
            }

            var propertyPath = MatchPropertyAccess(parameterExpression, propertyAccessExpression);

#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
            return propertyPath != null && propertyPath.Count == 1 ? propertyPath : null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
        }

        public static PropertyPath GetSimplePropertyAccess(this LambdaExpression propertyAccessExpression)
        {
            if (propertyAccessExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyAccessExpression));
            }

            var propertyPath
                = propertyAccessExpression
                    .Parameters
                    .Single()
                    .MatchSimplePropertyAccess(propertyAccessExpression.Body);

            if (propertyPath == null)
            {
                throw new InvalidOperationException($"The properties expression '{propertyAccessExpression}' is not valid.");
            }

            return propertyPath;
        }

        private static PropertyPath MatchComplexPropertyAccess(
            this Expression parameterExpression, Expression propertyAccessExpression)
        {
            if (propertyAccessExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyAccessExpression));
            }

            var propertyPath = MatchPropertyAccess(parameterExpression, propertyAccessExpression);

            return propertyPath;
        }

        public static PropertyPath GetComplexPropertyAccess(this LambdaExpression propertyAccessExpression)
        {
            if (propertyAccessExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyAccessExpression));
            }

            var propertyPath
                = propertyAccessExpression
                    .Parameters
                    .Single()
                    .MatchComplexPropertyAccess(propertyAccessExpression.Body);

            if (propertyPath == null)
            {
                throw new InvalidOperationException($"The expression '{propertyAccessExpression}' is not a valid property expression.");
            }

            return propertyPath;
        }
    }
}
