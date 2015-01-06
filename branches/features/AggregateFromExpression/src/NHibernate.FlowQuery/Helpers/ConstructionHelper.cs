namespace NHibernate.FlowQuery.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using NHibernate.FlowQuery.Helpers.ConstructionHandlers.MethodCalls;

    /// <summary>
    ///     A static utility class providing methods to build a <see cref="IEnumerable{T}" /> from a
    ///     <see cref="LambdaExpression" />.
    /// </summary>
    public static class ConstructionHelper
    {
        /// <summary>
        ///     All custom method call handlers.
        /// </summary>
        private static readonly Dictionary<string, IMethodCallConstructionHandler> CustomMethodCallHandlers;

        /// <summary>
        ///     All default method call handlers.
        /// </summary>
        private static readonly Dictionary<string, IMethodCallConstructionHandler> DefaultMethodCallHandlers;

        /// <summary>
        ///     The method call handler lock.
        /// </summary>
        private static readonly object MethodHandlerLock;

        /// <summary>
        ///     Initializes static members of the <see cref="ConstructionHelper" /> class.
        /// </summary>
        static ConstructionHelper()
        {
            MethodHandlerLock = new object();

            CustomMethodCallHandlers = new Dictionary<string, IMethodCallConstructionHandler>();

            DefaultMethodCallHandlers = new Dictionary<string, IMethodCallConstructionHandler>();

            AddMethodCallHandlerInternal("FromExpression", new FromExpressionHandler());
        }

        /// <summary>
        ///     Adds a <see cref="IMethodCallConstructionHandler" /> to be used when handling method calls.
        /// </summary>
        /// <param name="methodName">
        ///     The name of the method.
        /// </param>
        /// <param name="handler">
        ///     The <see cref="IMethodCallConstructionHandler" />.
        /// </param>
        /// <returns>
        ///     A value indicating whether the <see cref="IMethodCallConstructionHandler" /> was added (true) or not
        ///     (false). If another <see cref="IMethodCallConstructionHandler" /> with the same method name is already
        ///     added, false will be returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="handler" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="methodName" /> is null or <see cref="string.Empty" />.
        /// </exception>
        public static bool AddMethodCallHandler(string methodName, IMethodCallConstructionHandler handler)
        {
            return AddMethodCallHandlerInternal(methodName, handler, false);
        }

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
        public static int Invoke(Expression expression, object[] arguments, out object value)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Lambda:
                    return Invoke(((LambdaExpression)expression).Body, arguments, out value);

                case ExpressionType.New:
                    return Invoke(expression as NewExpression, arguments, out value);

                case ExpressionType.MemberInit:
                    return Invoke(expression as MemberInitExpression, arguments, out value);

                case ExpressionType.Call:
                    return Invoke(expression as MethodCallExpression, arguments, out value);

                default:
                    value = arguments[0];

                    return 1;
            }
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
        ///     Adds a <see cref="IMethodCallConstructionHandler" /> to be used when handling method calls.
        /// </summary>
        /// <param name="methodName">
        ///     The name of the method.
        /// </param>
        /// <param name="handler">
        ///     The <see cref="IMethodCallConstructionHandler" />.
        /// </param>
        /// <param name="isDefaultHandler">
        ///     A value indicating whether the handler is added internally (true) or externally using
        ///     <see cref="AddMethodCallHandler(string,IMethodCallConstructionHandler)" /> (false).
        /// </param>
        /// <returns>
        ///     A value indicating whether the <see cref="IMethodCallConstructionHandler" /> was added (true) or not
        ///     (false). If another <see cref="IMethodCallConstructionHandler" /> with the same method name is already
        ///     added, false will be returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="handler" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="methodName" /> is null or <see cref="string.Empty" />.
        /// </exception>
        internal static bool AddMethodCallHandlerInternal
            (
            string methodName,
            IMethodCallConstructionHandler handler,
            bool isDefaultHandler = true
            )
        {
            lock (MethodHandlerLock)
            {
                if (handler == null)
                {
                    throw new ArgumentNullException("handler");
                }

                methodName = (methodName ?? string.Empty).Trim().ToLower();

                if (methodName == string.Empty)
                {
                    throw new ArgumentException("key");
                }

                Dictionary<string, IMethodCallConstructionHandler> collection = isDefaultHandler
                    ? DefaultMethodCallHandlers
                    : CustomMethodCallHandlers;

                if (collection.ContainsKey(methodName))
                {
                    return false;
                }

                collection.Add(methodName, handler);

                return true;
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

            foreach (object o in list)
            {
                object[] args = o as object[] ?? new[]
                {
                    o
                };

                object instance;

                Invoke(expression, args, out instance);

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
                object[] args = o as object[] ?? new[]
                {
                    o
                };

                object instance;

                Invoke(expression, args, out instance);

                temp.Add((TDestination)instance);
            }

            return temp;
        }

        /// <summary>
        ///     Invokes the provided <see cref="MethodCallExpression" /> with the provided arguments.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="MethodCallExpression" /> expression.
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
        private static int Invoke(MethodCallExpression expression, object[] arguments, out object value)
        {
            string key = expression.Method.Name.ToLower();

            IMethodCallConstructionHandler handler;

            bool wasHandled;

            int i;

            // first, attempt with any found custom handler
            bool found = CustomMethodCallHandlers.TryGetValue(key, out handler);

            if (found)
            {
                i = handler.Handle(expression, arguments, out value, out wasHandled);

                if (wasHandled)
                {
                    return i;
                }
            }

            // if no luck with a custom handler, attempt with a default handler (if found)
            found = DefaultMethodCallHandlers.TryGetValue(key, out handler);

            if (found)
            {
                i = handler.Handle(expression, arguments, out value, out wasHandled);

                if (wasHandled)
                {
                    return i;
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
        private static int Invoke(NewExpression expression, object[] arguments, out object instance)
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
        private static int Invoke(MemberInitExpression expression, object[] arguments, out object instance)
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