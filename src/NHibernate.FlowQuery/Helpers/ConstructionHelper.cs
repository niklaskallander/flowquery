namespace NHibernate.FlowQuery.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using NHibernate.FlowQuery.Helpers.ExpressionHandlers;

    /// <summary>
    ///     A static utility class providing methods to build a <see cref="IEnumerable{T}" /> from a
    ///     <see cref="LambdaExpression" />.
    /// </summary>
    public static class ConstructionHelper
    {
        /// <summary>
        ///     Determines whether the provided <see cref="Expression" /> can be handled by
        ///     <see cref="GetListByExpression{TDestination}" />.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="Expression" /> instance to verify.
        /// </param>
        /// <returns>
        ///     True if the <see cref="Expression" /> can be handled; false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="expression" /> is null.
        /// </exception>
        public static bool CanHandle(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (expression.NodeType == ExpressionType.Lambda)
            {
                return CanHandle(((LambdaExpression)expression).Body);
            }

            return expression.NodeType == ExpressionType.New
                || expression.NodeType == ExpressionType.MemberInit;
        }

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

            if (expression.NodeType == ExpressionType.Lambda)
            {
                return GetListByExpression<TDestination>(((LambdaExpression)expression).Body, list);
            }

            if (list != null)
            {
                bool canHandle = CanHandle(expression);

                if (canHandle)
                {
                    if (expression.NodeType == ExpressionType.New)
                    {
                        return ForNewExpression<TDestination>(expression as NewExpression, list);
                    }

                    return ForMemberInitExpression<TDestination>(expression as MemberInitExpression, list);
                }
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

            if (expression.NodeType == ExpressionType.Lambda)
            {
                return GetObjectByExpressionConverter<TDestination>(((LambdaExpression)expression).Body);
            }

            bool canHandle = CanHandle(expression);

            if (canHandle)
            {
                if (expression.NodeType == ExpressionType.New)
                {
                    return x => ForNewExpression<TDestination>(expression as NewExpression, x);
                }

                return x => ForMemberInitExpression<TDestination>(expression as MemberInitExpression, x);
            }

            return null;
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

            switch (expression.NodeType)
            {
                case ExpressionType.New:
                    return Invoke(expression as NewExpression, arguments, out value);

                case ExpressionType.MemberInit:
                    return Invoke(expression as MemberInitExpression, arguments, out value);

                default:
                    value = arguments[0];

                    return 1;
            }
        }

        /// <summary>
        ///     Creates a <see cref="IEnumerable{TDestination}" /> from the provided <see cref="MemberInitExpression" />
        ///     and <see cref="IEnumerable" />.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="MemberInitExpression" /> constructor.
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
        private static IEnumerable<TDestination> ForMemberInitExpression<TDestination>
            (
            MemberInitExpression expression,
            IEnumerable list
            )
        {
            var temp = new List<TDestination>();

            foreach (object item in list)
            {
                var destinationItem = ForMemberInitExpression<TDestination>(expression, item);

                temp.Add(destinationItem);
            }

            return temp;
        }

        /// <summary>
        ///     Creates a <see cref="T:TDestination" /> from the provided <see cref="MemberInitExpression" />
        ///     and <see cref="object" />.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="MemberInitExpression" /> constructor.
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
        private static TDestination ForMemberInitExpression<TDestination>
            (
            MemberInitExpression expression,
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

        /// <summary>
        ///     Creates a <see cref="IEnumerable{TDestination}" /> from the provided <see cref="NewExpression" />
        ///     and <see cref="IEnumerable" />.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="NewExpression" /> constructor.
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
        private static IEnumerable<TDestination> ForNewExpression<TDestination>
            (
            NewExpression expression,
            IEnumerable list
            )
        {
            var temp = new List<TDestination>();

            foreach (object item in list)
            {
                var destinationItem = ForNewExpression<TDestination>(expression, item);

                temp.Add(destinationItem);
            }

            return temp;
        }

        /// <summary>
        ///     Creates a <see cref="T:TDestination" /> from the provided <see cref="NewExpression" />
        ///     and <see cref="object" />.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="NewExpression" /> constructor.
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
        private static TDestination ForNewExpression<TDestination>
            (
            NewExpression expression,
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
                if (handler.CanHandleConstructionOf(expression))
                {
                    bool wasHandled;

                    int i = handler.Construct(expression, arguments, out value, out wasHandled);

                    if (wasHandled)
                    {
                        return i;
                    }
                }
            }

            value = arguments[0];

            return 1;
        }

        /// <summary>
        ///     Invokes the provided <see cref="NewExpression" /> with the provided arguments.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="NewExpression" /> constructor.
        /// </param>
        /// <param name="arguments">
        ///     The constructor arguments.
        /// </param>
        /// <param name="instance">
        ///     The generated instance.
        /// </param>
        /// <returns>
        ///     The number of arguments used.
        /// </returns>
        private static int Invoke
            (
            NewExpression expression,
            object[] arguments,
            out object instance
            )
        {
            int i = 0;

            var list = new List<object>();

            foreach (Expression argument in expression.Arguments)
            {
                object value;

                i += Invoke(argument, arguments.Skip(i).ToArray(), out value);

                list.Add(value);
            }

            instance = expression.Constructor.Invoke(list.ToArray());

            return i;
        }

        /// <summary>
        ///     Invokes the provided <see cref="MemberInitExpression" /> with the provided arguments.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="MemberInitExpression" /> constructor.
        /// </param>
        /// <param name="arguments">
        ///     The constructor arguments.
        /// </param>
        /// <param name="instance">
        ///     The generated instance.
        /// </param>
        /// <returns>
        ///     The number of arguments used.
        /// </returns>
        private static int Invoke
            (
            MemberInitExpression expression,
            object[] arguments,
            out object instance
            )
        {
            int i = Invoke(expression.NewExpression, arguments, out instance);

            foreach (MemberBinding binding in expression.Bindings)
            {
                var memberAssignment = binding as MemberAssignment;

                if (memberAssignment != null)
                {
                    object value;

                    i += Invoke(memberAssignment.Expression, arguments.Skip(i).ToArray(), out value);

                    SetValue(binding.Member, instance, value);
                }
            }

            return i;
        }

        /// <summary>
        ///     Sets the value of the <see cref="MemberInfo" /> member on <paramref name="instance" /> to
        ///     <paramref name="value" />.
        /// </summary>
        /// <param name="memberInfo">
        ///     The <see cref="MemberInfo" /> member to set.
        /// </param>
        /// <param name="instance">
        ///     The instance to update.
        /// </param>
        /// <param name="value">
        ///     The value to use.
        /// </param>
        /// <remarks>
        ///     Borrowed from Linq to NHibernate.
        /// </remarks>
        private static void SetValue
            (
            MemberInfo memberInfo,
            object instance,
            object value
            )
        {
            var field = memberInfo as FieldInfo;

            if (field != null)
            {
                field.SetValue(instance, value);
            }
            else
            {
                var prop = memberInfo as PropertyInfo;

                if (prop != null)
                {
                    prop.SetValue(instance, value, null);
                }
            }
        }
    }
}