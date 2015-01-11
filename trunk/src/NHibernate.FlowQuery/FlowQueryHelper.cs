namespace NHibernate.FlowQuery
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers.ExpressionHandlers;
    using NHibernate.FlowQuery.Helpers.ExpressionHandlers.MethodCalls;

    /// <summary>
    ///     The <see cref="FlowQueryHelper" /> class is intended to replace <see cref="Aggregate" /> and other static
    ///     utilities currently existing in <see cref="FlowQuery" />. This to make it possible to override the default
    ///     functionality in user code.
    /// </summary>
    public class FlowQueryHelper
    {
        /// <summary>
        ///     All custom method call handlers.
        /// </summary>
        private static readonly Dictionary<string, IMethodCallExpressionHandler> CustomMethodCallHandlers;

        /// <summary>
        ///     All default method call handlers.
        /// </summary>
        private static readonly Dictionary<string, IMethodCallExpressionHandler> DefaultMethodCallHandlers;

        /// <summary>
        ///     The method call handler lock.
        /// </summary>
        private static readonly object MethodHandlerLock;

        /// <summary>
        ///     Initializes static members of the <see cref="FlowQueryHelper" /> class.
        /// </summary>
        static FlowQueryHelper()
        {
            MethodHandlerLock = new object();

            CustomMethodCallHandlers = new Dictionary<string, IMethodCallExpressionHandler>();

            DefaultMethodCallHandlers = new Dictionary<string, IMethodCallExpressionHandler>();

            var likeHandler = new LikeHandler();

            AddMethodCallHandlerInternal("As", new AsHandler());
            AddMethodCallHandlerInternal("Average", new SimpleMethodCallHandler(Projections.Avg));
            AddMethodCallHandlerInternal("Contains", likeHandler);
            AddMethodCallHandlerInternal("CountDistinct", new CountDistinctHandler());
            AddMethodCallHandlerInternal("Count", new SimpleMethodCallHandler(Projections.Count));
            AddMethodCallHandlerInternal("EndsWith", likeHandler);
            AddMethodCallHandlerInternal("GroupBy", new SimpleMethodCallHandler(Projections.GroupProperty));
            AddMethodCallHandlerInternal("Max", new SimpleMethodCallHandler(Projections.Max));
            AddMethodCallHandlerInternal("Min", new SimpleMethodCallHandler(Projections.Min));
            AddMethodCallHandlerInternal("Project", new ProjectHandler());
            AddMethodCallHandlerInternal("Round", new RoundHandler());
            AddMethodCallHandlerInternal("StartsWith", likeHandler);
            AddMethodCallHandlerInternal("Subquery", new SubqueryHandler());
            AddMethodCallHandlerInternal("Substring", new SubstringHandler());
            AddMethodCallHandlerInternal("Sum", new SimpleMethodCallHandler(Projections.Sum));
            AddMethodCallHandlerInternal("Trim", new TrimHandler());
            AddMethodCallHandlerInternal("TrimEnd", new TrimEndHandler());
            AddMethodCallHandlerInternal("TrimStart", new TrimStartHandler());
        }

        /// <summary>
        ///     Adds a <see cref="IMethodCallExpressionHandler" /> to be used when handling method calls.
        /// </summary>
        /// <param name="methodName">
        ///     The name of the method.
        /// </param>
        /// <param name="handler">
        ///     The <see cref="IMethodCallExpressionHandler" />.
        /// </param>
        /// <param name="withForce">
        ///     Determines whether to add the handler with force. In other words: if <paramref name="withForce" /> is
        ///     <c>true</c> and a handler already exists for the given method name (<paramref name="methodName" />), it
        ///     will be replaced. If <paramref name="withForce" /> is <c>false</c> and a handler already exists for the
        ///     given method name (<paramref name="methodName" />), this call will simply be ignored.
        /// </param>
        /// <returns>
        ///     A value indicating whether the <see cref="IMethodCallExpressionHandler" /> was added (true) or not
        ///     (false). If another <see cref="IMethodCallExpressionHandler" /> with the same method name is already
        ///     added, false will be returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="handler" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="methodName" /> is null or <see cref="string.Empty" />.
        /// </exception>
        public static bool AddMethodCallHandler
            (
            string methodName,
            IMethodCallExpressionHandler handler,
            bool withForce = false
            )
        {
            return AddMethodCallHandlerInternal(methodName, handler, false, withForce);
        }

        /// <summary>
        ///     Clears all custom defined <see cref="IMethodCallExpressionHandler" /> instances.
        /// </summary>
        public static void ClearMethodCallHandlers()
        {
            CustomMethodCallHandlers.Clear();
        }

        /// <summary>
        ///     Gets all defined <see cref="IMethodCallExpressionHandler" /> instances registered for the given
        ///     <paramref name="methodName" /> (any user-defined handler will come first in the list).
        /// </summary>
        /// <param name="methodName">
        ///     The method name.
        /// </param>
        /// <returns>
        ///     All defined <see cref="IMethodCallExpressionHandler" /> instances registered for the given
        ///     <paramref name="methodName" /> (any user-defined handler will come first in the list).
        /// </returns>
        public static IEnumerable<IMethodCallExpressionHandler> GetMethodCallHandlers(string methodName)
        {
            methodName = (methodName ?? string.Empty).Trim().ToLower();

            IMethodCallExpressionHandler handler;

            if (CustomMethodCallHandlers.TryGetValue(methodName, out handler))
            {
                yield return handler;
            }

            if (DefaultMethodCallHandlers.TryGetValue(methodName, out handler))
            {
                yield return handler;
            }
        }

        // TODO: Add usage scenario documentation for FlowQueryHelper.Project(..)

        /// <summary>
        ///     Provides a means to split up your projections into several expressions for re-use and testability.
        /// </summary>
        /// <typeparam name="TIn">
        ///     The type of the alias.
        /// </typeparam>
        /// <typeparam name="TOut">
        ///     The type of the mapping result.
        /// </typeparam>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="mapper">
        ///     The mapper expression.
        /// </param>
        /// <returns>
        ///     When overridden in a derived class, the mapped result, otherwise nothing
        ///     (throws <see cref="NotImplementedException" />).
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     Unless overridden in a derived class, always thrown.
        /// </exception>
        /// <remarks>
        ///     This is a query resolution helper and should not be executed directly (unless overridden in a derived
        ///     class).
        /// </remarks>
        public virtual TOut Project<TIn, TOut>(TIn alias, Expression<Func<TIn, TOut>> mapper)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Adds a <see cref="IMethodCallExpressionHandler" /> to be used when handling method calls.
        /// </summary>
        /// <param name="methodName">
        ///     The name of the method.
        /// </param>
        /// <param name="handler">
        ///     The <see cref="IMethodCallExpressionHandler" />.
        /// </param>
        /// <param name="isDefaultHandler">
        ///     A value indicating whether the handler is added internally (true) or externally using
        ///     <see cref="FlowQueryHelper.AddMethodCallHandler" /> (false).
        /// </param>
        /// <param name="withForce">
        ///     Determines whether to add the handler with force. In other words: if <paramref name="withForce" /> is
        ///     <c>true</c> and a handler already exists for the given method name (<paramref name="methodName" />), it
        ///     will be replaced. If <paramref name="withForce" /> is <c>false</c> and a handler already exists for the
        ///     given method name (<paramref name="methodName" />), this call will simply be ignored.
        /// </param>
        /// <returns>
        ///     A value indicating whether the <see cref="IMethodCallExpressionHandler" /> was added (true) or not
        ///     (false). If another <see cref="IMethodCallExpressionHandler" /> with the same method name is already
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
            IMethodCallExpressionHandler handler,
            bool isDefaultHandler = true,
            bool withForce = false
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

                Dictionary<string, IMethodCallExpressionHandler> collection = isDefaultHandler
                    ? DefaultMethodCallHandlers
                    : CustomMethodCallHandlers;

                if (collection.ContainsKey(methodName))
                {
                    if (withForce)
                    {
                        collection[methodName] = handler;
                    }

                    return withForce;
                }

                collection.Add(methodName, handler);

                return true;
            }
        }
    }
}