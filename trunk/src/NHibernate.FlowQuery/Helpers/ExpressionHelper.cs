using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace NHibernate.FlowQuery.Helpers
{
    public static class ExpressionHelper
    {
        #region Methods (8)

        public static string GetConstantRootString(Expression expression)
        {
            if (expression != null)
            {
                switch (expression.NodeType)
                {
                    case ExpressionType.Call:
                        return GetConstantRootString((expression as MethodCallExpression).Object ?? (expression as MethodCallExpression).Arguments[0]);
                    case ExpressionType.Convert:
                        return GetConstantRootString((expression as UnaryExpression).Operand);
                    case ExpressionType.Lambda:
                        return GetConstantRootString((expression as LambdaExpression).Body);
                    case ExpressionType.MemberAccess:
                        return GetConstantRootString((expression as MemberExpression).Expression);
                    case ExpressionType.Constant:
                        return expression.ToString();
                }
            }

            return null;
        }

        public static string GetPropertyName(MemberExpression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            string fullExpression = expression.ToString();
            if (HasConstantRoot(expression))
            {
                string constantRoot = GetConstantRootString(expression);
                if (constantRoot.Length > 0)
                {
                    return fullExpression.Replace(constantRoot + ".", string.Empty);
                }
            }
            return fullExpression;
        }

        public static string GetPropertyName(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            switch (expression.NodeType)
            {
                case ExpressionType.Call:
                    return GetPropertyName((expression as MethodCallExpression).Object ?? (expression as MethodCallExpression).Arguments[0]);
                case ExpressionType.Convert:
                    return GetPropertyName((expression as UnaryExpression).Operand);
                case ExpressionType.Lambda:
                    return GetPropertyName((expression as LambdaExpression).Body);
                case ExpressionType.MemberAccess:
                    return GetPropertyName(expression as MemberExpression);
                case ExpressionType.Constant:
                    if (expression.Type == typeof(string))
                    {
                        return GetValue<string>(expression);
                    }
                    break;
            }

            throw new NotSupportedException("The expression contains unsupported features please revise your code");
        }

        public static bool IsRooted(Expression expression, string expectedRoot, Dictionary<string, string> aliases)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (string.IsNullOrEmpty(expectedRoot))
            {
                throw new ArgumentException("expectedRoot");
            }

            string property = GetPropertyName(expression);

            string[] splits = property.Split('.');

            return splits.Length != 1 && (splits[0] == expectedRoot || (aliases != null && aliases.ContainsValue(splits[0])));
        }

        public static string GetPropertyName(Expression expression, string expectedRoot)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (string.IsNullOrEmpty(expectedRoot))
            {
                throw new ArgumentException("expectedRoot");
            }

            string property = GetPropertyName(expression);

            string[] splits = property.Split('.');
            if (splits[0] == expectedRoot)
            {
                string[] parts = new string[splits.Length - 1];
                for (int i = 1; i < splits.Length; i++)
                {
                    parts[i - 1] = splits[i];
                }
                return string.Join(".", parts);
            }
            return property;
        }

        public static string GetRoot(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            switch (expression.NodeType)
            {
                case ExpressionType.Call:
                    return GetRoot((expression as MethodCallExpression).Object ?? (expression as MethodCallExpression).Arguments[0]); // original method ?? extension method
                case ExpressionType.Convert:
                    return GetRoot((expression as UnaryExpression).Operand);
                case ExpressionType.Lambda:
                    return GetRoot((expression as LambdaExpression).Body);
                case ExpressionType.MemberAccess:
                    return GetPropertyName(expression as MemberExpression).Split('.')[0];
                case ExpressionType.Constant:
                    if (expression.Type == typeof(string))
                    {
                        string[] splits = GetValue<string>(expression).Split('.');

                        return splits[0];
                    }
                    break;
            }

            return null;
        }

        public static object GetValue(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            try
            {
                return Expression
                        .Lambda(expression, null)
                            .Compile()
                                .DynamicInvoke(null);
            }
            catch (TargetInvocationException)
            {
                return null;
            }
        }

        public static T GetValue<T>(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return Expression
                    .Lambda<Func<T>>(expression, null)
                        .Compile()
                            .Invoke();
        }

        public static bool HasConstantRoot(Expression expression)
        {
            if (expression != null)
            {
                switch (expression.NodeType)
                {
                    case ExpressionType.Call:
                        return HasConstantRoot((expression as MethodCallExpression).Object ?? (expression as MethodCallExpression).Arguments[0]);
                    case ExpressionType.Convert:
                        return HasConstantRoot((expression as UnaryExpression).Operand);
                    case ExpressionType.Lambda:
                        return HasConstantRoot((expression as LambdaExpression).Body);
                    case ExpressionType.MemberAccess:
                        return HasConstantRoot((expression as MemberExpression).Expression);
                    case ExpressionType.Constant:
                        return true;
                }
            }
            return false;
        }

        #endregion Methods
    }
}