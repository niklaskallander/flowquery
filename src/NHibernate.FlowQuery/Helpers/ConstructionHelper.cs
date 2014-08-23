namespace NHibernate.FlowQuery.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

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

            if (list == null)
            {
                return null;
            }

            bool canHandle = CanHandle(expression);

            if (canHandle)
            {
                if (expression.NodeType == ExpressionType.New)
                {
                    return ForNewExpression<TDestination>(expression as NewExpression, list);
                }

                return ForMemberInitExpression<TDestination>(expression as MemberInitExpression, list);
            }

            return null;
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

            foreach (object o in list)
            {
                object instance;

                Invoke(expression, o as object[] ?? new[] { o }, out instance);

                temp.Add((TDestination)instance);
            }

            return temp;
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

            foreach (object o in list)
            {
                object instance;

                Invoke(expression, o as object[] ?? new[] { o }, out instance);

                temp.Add((TDestination)instance);
            }

            return temp;
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
        private static int Invoke(NewExpression expression, object[] arguments, out object instance)
        {
            int i = 0;

            var list = new List<object>();

            foreach (Expression argument in expression.Arguments)
            {
                object value;

                switch (argument.NodeType)
                {
                    case ExpressionType.New:
                        i += Invoke(argument as NewExpression, arguments.Skip(i).ToArray(), out value);
                        break;

                    case ExpressionType.MemberInit:
                        i += Invoke(argument as MemberInitExpression, arguments.Skip(i).ToArray(), out value);
                        break;

                    default:

                        value = arguments[i];

                        i++;

                        break;
                }

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
        private static int Invoke(MemberInitExpression expression, object[] arguments, out object instance)
        {
            int i = Invoke(expression.NewExpression, arguments, out instance);

            foreach (MemberBinding binding in expression.Bindings)
            {
                var memberAssignment = binding as MemberAssignment;

                if (memberAssignment != null)
                {
                    object value;

                    switch (memberAssignment.Expression.NodeType)
                    {
                        case ExpressionType.New:

                            i += Invoke
                            (
                                memberAssignment.Expression as NewExpression,
                                arguments.Skip(i).ToArray(),
                                out value
                            );

                            break;

                        case ExpressionType.MemberInit:

                            i += Invoke
                            (
                                memberAssignment.Expression as MemberInitExpression,
                                arguments.Skip(i).ToArray(),
                                out value
                            );

                            break;

                        default:

                            value = arguments[i];

                            i++;

                            break;
                    }

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
        private static void SetValue(MemberInfo memberInfo, object instance, object value)
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