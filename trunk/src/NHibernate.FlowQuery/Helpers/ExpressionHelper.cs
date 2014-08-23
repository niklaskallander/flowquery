namespace NHibernate.FlowQuery.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    ///     A static utility class providing methods to work with <see cref="System.Linq.Expressions.Expression" />
    ///     objects.
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        ///     Combines (merges) the provided expressions into one.
        /// </summary>
        /// <param name="expressions">
        ///     The expressions to merge.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the result.
        /// </typeparam>
        /// <returns>
        ///     The resulting expression.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="expressions" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     If one or more of the provided expressions is neither of <see cref="MemberInitExpression" /> or
        ///     <see cref="NewExpression" />, or if any but the first expression is a <see cref="NewExpression" />.
        /// </exception>
        public static Expression<Func<TSource, TDestination>> Combine<TSource, TDestination>
            (
            params Expression<Func<TSource, TDestination>>[] expressions
            )
        {
            if (expressions == null)
            {
                throw new ArgumentNullException("expressions");
            }

            bool allAreMemberInit = expressions
                .Where(x => x != null)
                .Skip(1) // skip first which may also be a NewExpression
                .All(x => x.Body is MemberInitExpression);

            if (!allAreMemberInit)
            {
                throw new ArgumentException
                (
                    "All expressions must be MemberInitExpression except the first which can also be a NewExpression",
                    "expressions"
                );
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
                throw new ArgumentException
                (
                    "The first expression is not a MemberInitExpression, nor is it a NewExpression",
                    "expressions"
                );
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

        /// <summary>
        ///     Gets the constant root <see cref="string" /> value of a <see cref="Expression" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <returns>
        ///     The constant root <see cref="string" /> value.
        /// </returns>
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

        /// <summary>
        ///     Gets the property name for the provided <see cref="MemberExpression" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <returns>
        ///     The property name.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="expression" /> is null.
        /// </exception>
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

        /// <summary>
        ///     Gets the property name for the provided <see cref="Expression" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <returns>
        ///     The property name.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="expression" /> is null.
        /// </exception>
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

        /// <summary>
        ///     Gets the property name for the provided <see cref="Expression" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="expectedRoot">
        ///     The expected root.
        /// </param>
        /// <returns>
        ///     The property name.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="expression" /> is null.
        /// </exception>
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

        /// <summary>
        ///     Gets the root <see cref="string" /> value for the provided <see cref="Expression" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <returns>
        ///     The root <see cref="string" /> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="expression" /> is null.
        /// </exception>
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

        /// <summary>
        ///     Gets the value from the provided <see cref="Expression" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <returns>
        ///     The <see cref="object" /> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="expression" /> is null.
        /// </exception>
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

        /// <summary>
        ///     Gets the value for the provided <see cref="Expression" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <typeparam name="T">
        ///     The <see cref="System.Type" /> of the value.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="T:T" /> value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="expression" /> is null.
        /// </exception>
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

        /// <summary>
        ///     Determines whether the <see cref="Expression" /> has a constant root.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <returns>
        ///     True if the <see cref="Expression" /> has a constant root; false otherwise.
        /// </returns>
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

        /// <summary>
        ///     Determines whether the <see cref="Expression" /> is rooted.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="expectedRoot">
        ///     The expected root.
        /// </param>
        /// <param name="data">
        ///     The <see cref="QueryHelperData" /> info.
        /// </param>
        /// <returns>
        ///     True if the <see cref="Expression" /> is rooted; false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="expression" /> is null.
        /// </exception>
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
    }
}