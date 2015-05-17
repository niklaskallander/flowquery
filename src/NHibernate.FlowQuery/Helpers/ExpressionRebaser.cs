namespace NHibernate.FlowQuery.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Core;

    /// <summary>
    ///     The predicate rewriter visitor.
    /// </summary>
    public class ExpressionRebaser
    {
        /// <summary>
        ///     The name of the alias.
        /// </summary>
        private readonly string _alias;

        /// <summary>
        ///     The type of the alias.
        /// </summary>
        private readonly Type _type;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExpressionRebaser" /> class.
        /// </summary>
        /// <param name="type">
        ///     The type of the alias.
        /// </param>
        /// <param name="alias">
        ///     The name of the alias.
        /// </param>
        public ExpressionRebaser
            (
            Type type,
            string alias
            )
        {
            _alias = alias;
            _type = type;
        }

        /// <summary>
        ///     Rewrites the given expression to use the specified type as root.
        /// </summary>
        /// <param name="expression">
        ///     The expression to rewrite.
        /// </param>
        /// <typeparam name="T1">
        ///     The type to use for parameter 1.
        /// </typeparam>
        /// <typeparam name="T">
        ///     The type to use as return value.
        /// </typeparam>
        /// <returns>
        ///     The rewritten expression.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="expression" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="expression" /> has zero or more than two parameters -or- <paramref name="expression" />
        ///     has two parameters but the second parameter is not a <see cref="WhereDelegate" />.
        /// </exception>
        public Expression<Func<T1, T>> RebaseTo<T1, T>(LambdaExpression expression)
        {
            return GetLambdaExpression(expression, typeof(Func<T1, T>), typeof(T1))
                as Expression<Func<T1, T>>;
        }

        /// <summary>
        ///     Rewrites the given expression to use the specified type as root.
        /// </summary>
        /// <param name="expression">
        ///     The expression to rewrite.
        /// </param>
        /// <typeparam name="T1">
        ///     The type to use for parameter 1.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     The type to use for parameter 2.
        /// </typeparam>
        /// <typeparam name="T">
        ///     The type to use as return value.
        /// </typeparam>
        /// <returns>
        ///     The rewritten expression.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="expression" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="expression" /> has zero or more than two parameters -or- <paramref name="expression" />
        ///     has two parameters but the second parameter is not a <see cref="WhereDelegate" />.
        /// </exception>
        public Expression<Func<T1, T2, T>> RebaseTo<T1, T2, T>(LambdaExpression expression)
        {
            return GetLambdaExpression(expression, typeof(Func<T1, T2, T>), typeof(T1), typeof(T2))
                as Expression<Func<T1, T2, T>>;
        }

        /// <summary>
        ///     Rewrites the given expression to use the specified type as root.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="delegateType">
        ///     The delegate type.
        /// </param>
        /// <param name="paramTypes">
        ///     The parameter types.
        /// </param>
        /// <returns>
        ///     The rewritten expression.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="expression" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="expression" /> has zero or more than two parameters -or- <paramref name="expression" />
        ///     has two parameters but the second parameter is not a <see cref="WhereDelegate" />.
        /// </exception>
        protected virtual LambdaExpression GetLambdaExpression
            (
            LambdaExpression expression,
            Type delegateType,
            params Type[] paramTypes
            )
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            int count = expression.Parameters.Count;

            if (count != 1 && !(count == 2 && expression.Parameters.Last().Type == typeof(WhereDelegate)))
            {
                throw new ArgumentException
                    (
                    "expression must have 1 param, or 2 param if last is WhereDelegate.",
                    "expression"
                    );
            }

            var visitor = new ParameterReplaceVisitor(expression.Parameters[0], Expression.Parameter(_type, _alias));

            Expression visited = visitor.Visit(expression.Body);

            var parameters = new List<ParameterExpression>
            {
                Expression.Parameter(paramTypes[0], "x")
            };

            if (paramTypes.Length == 2)
            {
                var param = expression.Parameters[1];

                var newParam = Expression.Parameter(param.Type, "where");

                visitor = new ParameterReplaceVisitor(param, newParam);

                visited = visitor.Visit(visited);

                parameters.Add(newParam);
            }

            return Expression.Lambda(delegateType, visited, parameters);
        }
    }
}