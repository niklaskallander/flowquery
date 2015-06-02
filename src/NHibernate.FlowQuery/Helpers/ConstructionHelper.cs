namespace NHibernate.FlowQuery.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Helpers.ExpressionHandlers;

    /// <summary>
    ///     A static utility class providing methods to build a <see cref="IEnumerable{T}" /> from a
    ///     <see cref="LambdaExpression" />.
    /// </summary>
    public static class ConstructionHelper
    {
        /// <summary>
        ///     Creates a <see cref="IEnumerable{TDestination}" /> from the provided <see cref="Expression" />
        ///     and <see cref="IEnumerable" />.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="Expression" /> constructor.
        /// </param>
        /// <param name="list">
        ///     The <see cref="IEnumerable" /> data list.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the result.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IEnumerable{TDestination}" /> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="expression" /> is null.
        /// </exception>
        public static IEnumerable<TDestination> GetListByExpression<TDestination>
            (
            Expression expression,
            IEnumerable list
            )
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (list != null)
            {
                return GetList<TDestination>(expression, list);
            }

            return null;
        }

        /// <summary>
        ///     Creates a <see cref="System.Func{T,TResult}" /> conversion delegate for the
        ///     provided <see cref="Expression" />
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="Expression" /> constructor.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the result.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="System.Func{T,TResult}" /> delegate.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="expression" /> is null.
        /// </exception>
        public static Func<object, TDestination> GetObjectByExpressionConverter<TDestination>
            (
            Expression expression
            )
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return x => GetItem<TDestination>(expression, x);
        }

        /// <summary>
        ///     Invokes the provided <see cref="Expression" /> with the provided arguments.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="Expression" /> expression.
        /// </param>
        /// <param name="arguments">
        ///     The constructor arguments.
        /// </param>
        /// <param name="value">
        ///     The generated instance.
        /// </param>
        /// <returns>
        ///     The number of arguments used.
        /// </returns>
        public static int Invoke
            (
            Expression expression,
            object[] arguments,
            out object value
            )
        {
            IEnumerable<IExpressionHandler> handlers = FlowQueryHelper
                .GetExpressionHandlers(expression.NodeType)
                .Where(x => x.CanHandleConstructionOf(expression))
                .ToArray();

            if (handlers.Any())
            {
                return ConstructUsing(handlers, expression, arguments, out value);
            }

            value = arguments[0];

            return 1;
        }

        /// <summary>
        ///     Invokes the provided <see cref="Expression" /> with the provided arguments.
        /// </summary>
        /// <param name="handlers">
        ///     The set of <see cref="IExpressionHandler" /> instances to use when constructing a value for the given
        ///     <see cref="Expression" />.
        /// </param>
        /// <param name="expression">
        ///     The <see cref="Expression" /> expression.
        /// </param>
        /// <param name="arguments">
        ///     The constructor arguments.
        /// </param>
        /// <param name="value">
        ///     The generated instance.
        /// </param>
        /// <returns>
        ///     The number of arguments used.
        /// </returns>
        private static int ConstructUsing
            (
            IEnumerable<IExpressionHandler> handlers,
            Expression expression,
            object[] arguments,
            out object value
            )
        {
            foreach (IExpressionHandler handler in handlers)
            {
                bool wasHandled;

                int i = handler.Construct(expression, arguments, out value, out wasHandled);

                if (wasHandled)
                {
                    return i;
                }
            }

            value = arguments[0];

            return 1;
        }

        /// <summary>
        ///     Creates a <see cref="IEnumerable{TDestination}" /> from the provided <see cref="Expression" />
        ///     and <see cref="IEnumerable" />.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="Expression" /> expression.
        /// </param>
        /// <param name="list">
        ///     The <see cref="IEnumerable" /> data list.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the result.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IEnumerable{TDestination}" /> instance.
        /// </returns>
        private static IEnumerable<TDestination> GetList<TDestination>
            (
            Expression expression,
            IEnumerable list
            )
        {
            var temp = new List<TDestination>();

            foreach (object item in list)
            {
                var destinationItem = GetItem<TDestination>(expression, item);

                temp.Add(destinationItem);
            }

            return temp;
        }

        /// <summary>
        ///     Creates a <see cref="T:TDestination" /> from the provided <see cref="Expression" />
        ///     and <see cref="object" />.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="Expression" /> expression.
        /// </param>
        /// <param name="item">
        ///     The <see cref="object" /> data item.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the result.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="T:TDestination" /> instance.
        /// </returns>
        private static TDestination GetItem<TDestination>
            (
            Expression expression,
            object item
            )
        {
            object[] args = item as object[] ?? new[]
            {
                item
            };

            object instance;

            Invoke(expression, args, out instance);

            return (TDestination)instance;
        }
    }
}