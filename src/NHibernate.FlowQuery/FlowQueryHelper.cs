namespace NHibernate.FlowQuery
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers.ExpressionHandlers;
    using NHibernate.FlowQuery.Helpers.ExpressionHandlers.MethodCalls;
    using NHibernate.FlowQuery.Helpers.ExpressionHandlers.Misc;
    using NHibernate.Util;

    /// <summary>
    ///     The <see cref="FlowQueryHelper" /> class is intended to replace <see cref="Aggregate" /> and other static
    ///     utilities currently existing in <see cref="FlowQuery" />. This to make it possible to override the default
    ///     functionality in user code.
    /// </summary>
    public class FlowQueryHelper
    {
        /// <summary>
        ///     All custom expression handlers.
        /// </summary>
        private static readonly Dictionary<ExpressionType, HashSet<IExpressionHandler>> CustomExpressionHandlers;

        /// <summary>
        ///     All default method call handlers.
        /// </summary>
        private static readonly Dictionary<ExpressionType, HashSet<IExpressionHandler>> DefaultExpressionHandlers;

        /// <summary>
        ///     The expression handler lock.
        /// </summary>
        private static readonly object ExpressionHandlerLock;

        /// <summary>
        ///     Initializes static members of the <see cref="FlowQueryHelper" /> class.
        /// </summary>
        static FlowQueryHelper()
        {
            ExpressionHandlerLock = new object();

            CustomExpressionHandlers = new Dictionary<ExpressionType, HashSet<IExpressionHandler>>();

            DefaultExpressionHandlers = new Dictionary<ExpressionType, HashSet<IExpressionHandler>>();

            AddCallHandler(new AsHandler());
            AddCallHandler(new SimpleMethodCallHandler(Projections.Avg, "Average"));
            AddCallHandler(new LikeHandler());
            AddCallHandler(new CountDistinctHandler());
            AddCallHandler(new SimpleMethodCallHandler(Projections.Count, "Count"));
            AddCallHandler(new SimpleMethodCallHandler(Projections.GroupProperty, "GroupBy"));
            AddCallHandler(new SimpleMethodCallHandler(Projections.Max, "Max"));
            AddCallHandler(new SimpleMethodCallHandler(Projections.Min, "Min"));
            AddCallHandler(new ProjectHandler());
            AddCallHandler(new RoundHandler());
            AddCallHandler(new SubqueryHandler());
            AddCallHandler(new SubstringHandler());
            AddCallHandler(new SimpleMethodCallHandler(Projections.Sum, "Sum"));
            AddCallHandler(new TrimHandler());
            AddCallHandler(new TrimEndHandler());
            AddCallHandler(new TrimStartHandler());

            var conditionHandler = new ConditionHandler();

            ConditionHandler.SupportedExpressionTypes.ForEach(x => AddHandler(x, conditionHandler));

            var arithmeticHandler = new ArithmeticHandler();

            ArithmeticHandler.SupportExpressionTypes.ForEach(x => AddHandler(x, arithmeticHandler));

            AddHandler(ExpressionType.Add, new ConcatenationHandler());
            AddHandler(ExpressionType.Coalesce, new CoalesceHandler());
            AddHandler(ExpressionType.Conditional, new ConditionalHandler());
            AddHandler(ExpressionType.Convert, new ConvertHandler());
            AddHandler(ExpressionType.Lambda, new LambdaHandler());
        }

        /// <summary>
        ///     Adds a <see cref="IMethodCallExpressionHandler" /> to be used when handling method calls.
        /// </summary>
        /// <param name="expressionType">
        ///     The <see cref="ExpressionType" /> handled by the given <see cref="IExpressionHandler" />
        /// </param>
        /// <param name="handler">
        ///     The <see cref="IExpressionHandler" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="handler" /> is null.
        /// </exception>
        public static void AddExpressionHandler
            (
            ExpressionType expressionType,
            IExpressionHandler handler
            )
        {
            AddHandler(expressionType, handler, false);
        }

        /// <summary>
        ///     Clears all custom defined <see cref="IExpressionHandler" /> instances.
        /// </summary>
        public static void ClearExpressionHandlers()
        {
            CustomExpressionHandlers.Clear();
        }

        /// <summary>
        ///     Gets all defined <see cref="IExpressionHandler" /> instances registered for the given
        ///     <paramref name="expressionType" /> (any user-defined handler will come first in the list).
        /// </summary>
        /// <param name="expressionType">
        ///     The <see cref="ExpressionType" /> handled by the desired <see cref="IExpressionHandler" />.
        /// </param>
        /// <returns>
        ///     All defined <see cref="IExpressionHandler" /> instances registered for the given
        ///     <paramref name="expressionType" /> (any user-defined handler will come first in the list).
        /// </returns>
        public static IEnumerable<IExpressionHandler> GetExpressionHandlers(ExpressionType expressionType)
        {
            HashSet<IExpressionHandler> handlers;

            if (CustomExpressionHandlers.TryGetValue(expressionType, out handlers))
            {
                return handlers;
            }

            if (DefaultExpressionHandlers.TryGetValue(expressionType, out handlers))
            {
                return handlers;
            }

            return new HashSet<IExpressionHandler>();
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
        ///     When overridden in a derived class, the mapped result, otherwise nothing (throws
        ///     <see cref="NotImplementedException" />).
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     Unless overridden in a derived class, always thrown.
        /// </exception>
        /// <remarks>
        ///     This is a query resolution helper and should not be executed directly (unless overridden in a derived
        ///     class).
        /// </remarks>
        public virtual TOut Project<TIn, TOut>
            (
            TIn alias,
            Expression<Func<TIn, TOut>> mapper
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Adds a <see cref="IExpressionHandler" /> to be used when handling method call expressions.
        /// </summary>
        /// <param name="handler">
        ///     The <see cref="IExpressionHandler" />.
        /// </param>
        /// <param name="isDefaultHandler">
        ///     A value indicating whether the handler is added internally (true) or externally using
        ///     <see cref="FlowQueryHelper.AddExpressionHandler" /> (false).
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="handler" /> is null.
        /// </exception>
        private static void AddCallHandler
            (
            IExpressionHandler handler,
            bool isDefaultHandler = true
            )
        {
            AddHandler(ExpressionType.Call, handler, isDefaultHandler);
        }

        /// <summary>
        ///     Adds a <see cref="IExpressionHandler" /> to be used when handling expressions.
        /// </summary>
        /// <param name="expressionType">
        ///     The <see cref="ExpressionType" /> handled by the given <see cref="IExpressionHandler" />
        /// </param>
        /// <param name="handler">
        ///     The <see cref="IExpressionHandler" />.
        /// </param>
        /// <param name="isDefaultHandler">
        ///     A value indicating whether the handler is added internally (true) or externally using
        ///     <see cref="FlowQueryHelper.AddExpressionHandler" /> (false).
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="handler" /> is null.
        /// </exception>
        private static void AddHandler
            (
            ExpressionType expressionType,
            IExpressionHandler handler,
            bool isDefaultHandler = true
            )
        {
            lock (ExpressionHandlerLock)
            {
                if (handler == null)
                {
                    throw new ArgumentNullException("handler");
                }

                Dictionary<ExpressionType, HashSet<IExpressionHandler>> collection = isDefaultHandler
                    ? DefaultExpressionHandlers
                    : CustomExpressionHandlers;

                HashSet<IExpressionHandler> handlers;

                bool found = collection.TryGetValue(expressionType, out handlers);

                if (found)
                {
                    handlers.Add(handler);
                }
                else
                {
                    collection.Add(expressionType, new HashSet<IExpressionHandler> { handler });
                }
            }
        }
    }
}