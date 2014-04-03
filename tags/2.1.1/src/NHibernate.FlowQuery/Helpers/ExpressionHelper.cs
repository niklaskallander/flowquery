using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.FlowQuery.Core.Joins;

namespace NHibernate.FlowQuery.Helpers
{
    public static class ExpressionHelper
    {
        public static Expression<Func<TSource, TDestination>> Combine<TSource, TDestination>(params Expression<Func<TSource, TDestination>>[] expressions)
        {
            if (expressions == null)
            {
                throw new ArgumentNullException("expressions");
            }

            if (!expressions.Where(x => x != null).Skip(1).All(x => x.Body is MemberInitExpression))
            {
                throw new ArgumentException("All expressions must be MemberInitExpression except the first which can also be a NewExpression", "expressions");
            }

            Expression first = expressions[0].Body;

            NewExpression constructor;

            var bindings = new List<MemberBinding>();

            if (first.NodeType == ExpressionType.MemberInit)
            {
                var initializer = (MemberInitExpression)first;

                constructor = initializer.NewExpression;

                bindings.AddRange(initializer.Bindings.OfType<MemberAssignment>());
            }
            else if (first.NodeType == ExpressionType.New)
            {
                constructor = (NewExpression)first;
            }
            else
            {
                throw new ArgumentException("The first expression is not a MemberInitExpression, nor is it a NewExpression", "expressions");
            }

            ParameterExpression parameter = expressions[0].Parameters[0];

            for (int i = 1; i < expressions.Length; i++)
            {
                if (expressions[i] == null)
                {
                    continue;
                }

                var initializer = (MemberInitExpression)expressions[i].Body;

                var replace = new ParameterReplaceVisitor(expressions[i].Parameters[0], parameter);

                foreach (MemberAssignment binding in initializer.Bindings.OfType<MemberAssignment>())
                {
                    Expression converted = replace.VisitAndConvert(binding.Expression, "Combine");

                    if (converted != null)
                    {
                        MemberAssignment assignment = Expression.Bind(binding.Member, converted);

                        bindings.Add(assignment);
                    }
                }
            }

            MemberInitExpression combined = Expression.MemberInit(constructor, bindings);

            return Expression.Lambda<Func<TSource, TDestination>>(combined, parameter);
        }

        public static string GetConstantRootString(Expression expression)
        {
            if (expression != null)
            {
                switch (expression.NodeType)
                {
                    case ExpressionType.Call:

                        var methodCall = (MethodCallExpression)expression;

                        return GetConstantRootString(methodCall.Object ?? methodCall.Arguments[0]);

                    case ExpressionType.Convert:
                        return GetConstantRootString(((UnaryExpression)expression).Operand);

                    case ExpressionType.Lambda:
                        return GetConstantRootString(((LambdaExpression)expression).Body);

                    case ExpressionType.MemberAccess:
                        return GetConstantRootString(((MemberExpression)expression).Expression);

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
                    var methodCall = (MethodCallExpression)expression;

                    return GetPropertyName(methodCall.Object ?? methodCall.Arguments[0]);

                case ExpressionType.Convert:
                    return GetPropertyName(((UnaryExpression)expression).Operand);

                case ExpressionType.Lambda:
                    return GetPropertyName(((LambdaExpression)expression).Body);

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

        public static bool IsRooted(Expression expression, string expectedRoot, QueryHelperData data)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            string property = GetPropertyName(expression);

            string[] splits = property.Split('.');

            if (splits.Length <= 1)
            {
                return false;
            }

            return splits[0] == expectedRoot 
                || data.Aliases
                    .ContainsValue(splits[0]);
        }

        public static string GetPropertyName(Expression expression, string expectedRoot)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            string property = GetPropertyName(expression);

            string[] splits = property.Split('.');

            if (splits[0] == expectedRoot)
            {
                var parts = new string[splits.Length - 1];

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
                    var methodCall = (MethodCallExpression)expression;

                    return GetRoot(methodCall.Object ?? methodCall.Arguments[0]); // original method ?? extension method

                case ExpressionType.Convert:
                    return GetRoot(((UnaryExpression)expression).Operand);

                case ExpressionType.Lambda:
                    return GetRoot(((LambdaExpression)expression).Body);

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
                        var methodCall = (MethodCallExpression)expression;

                        return HasConstantRoot(methodCall.Object ?? methodCall.Arguments[0]);

                    case ExpressionType.Convert:
                        return HasConstantRoot(((UnaryExpression)expression).Operand);

                    case ExpressionType.Lambda:
                        return HasConstantRoot(((LambdaExpression)expression).Body);

                    case ExpressionType.MemberAccess:
                        return HasConstantRoot(((MemberExpression)expression).Expression);

                    case ExpressionType.Constant:
                        return true;
                }
            }

            return false;
        }
    }
}