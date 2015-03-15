namespace NHibernate.FlowQuery
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;
    using NHibernate.FlowQuery.Helpers;

    /// <summary>
    ///     A helper class defining a set of extension methods on <see cref="ISession" /> and
    ///     <see cref="IStatelessSession" /> to create <see cref="NHibernate.FlowQuery" /> queries.
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        ///     The default root entity alias.
        /// </summary>
        internal const string RootAlias = "this";

        /// <summary>
        ///     Creates a new <see cref="IDelayedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="ISession" /> instance.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IDelayedFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Execution of delayed queries will be deferred until the results are required by user code. This makes it
        ///     possible for <see cref="NHibernate" /> to batch multiple queries in one round-trip to the database
        ///     instead of making one round-trip per query.
        /// </remarks>
        public static IDelayedFlowQuery<TSource> DelayedFlowQuery<TSource>(this ISession session)
            where TSource : class
        {
            return DelayedFlowQuery<TSource>(session, options: null);
        }

        /// <summary>
        ///     Creates a new <see cref="IDelayedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="ISession" /> instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IDelayedFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Execution of delayed queries will be deferred until the results are required by user code. This makes it
        ///     possible for <see cref="NHibernate" /> to batch multiple queries in one round-trip to the database
        ///     instead of making one round-trip per query.
        /// </remarks>
        public static IDelayedFlowQuery<TSource> DelayedFlowQuery<TSource>
            (
            this ISession session,
            Expression<Func<TSource>> alias
            )
            where TSource : class
        {
            return DelayedFlowQuery(session, alias, null);
        }

        /// <summary>
        ///     Creates a new <see cref="IDelayedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="ISession" /> instance.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IDelayedFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Execution of delayed queries will be deferred until the results are required by user code. This makes it
        ///     possible for <see cref="NHibernate" /> to batch multiple queries in one round-trip to the database
        ///     instead of making one round-trip per query.
        /// </remarks>
        public static IDelayedFlowQuery<TSource> DelayedFlowQuery<TSource>
            (
            this ISession session,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return DelayedFlowQuery<TSource>(session, null, options);
        }

        /// <summary>
        ///     Creates a new <see cref="IDelayedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="ISession" /> instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IDelayedFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Execution of delayed queries will be deferred until the results are required by user code. This makes it
        ///     possible for <see cref="NHibernate" /> to batch multiple queries in one round-trip to the database
        ///     instead of making one round-trip per query.
        /// </remarks>
        public static IDelayedFlowQuery<TSource> DelayedFlowQuery<TSource>
            (
            this ISession session,
            Expression<Func<TSource>> alias,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return DelayedFlowQuery(session.CreateCriteria, alias, options);
        }

        /// <summary>
        ///     Creates a new <see cref="IDelayedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="IStatelessSession" /> instance.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IDelayedFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Execution of delayed queries will be deferred until the results are required by user code. This makes it
        ///     possible for <see cref="NHibernate" /> to batch multiple queries in one round-trip to the database
        ///     instead of making one round-trip per query.
        /// </remarks>
        public static IDelayedFlowQuery<TSource> DelayedFlowQuery<TSource>(this IStatelessSession session)
            where TSource : class
        {
            return DelayedFlowQuery<TSource>(session, options: null);
        }

        /// <summary>
        ///     Creates a new <see cref="IDelayedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="IStatelessSession" /> instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IDelayedFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Execution of delayed queries will be deferred until the results are required by user code. This makes it
        ///     possible for <see cref="NHibernate" /> to batch multiple queries in one round-trip to the database
        ///     instead of making one round-trip per query.
        /// </remarks>
        public static IDelayedFlowQuery<TSource> DelayedFlowQuery<TSource>
            (
            this IStatelessSession session,
            Expression<Func<TSource>> alias
            )
            where TSource : class
        {
            return DelayedFlowQuery(session, alias, null);
        }

        /// <summary>
        ///     Creates a new <see cref="IDelayedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="IStatelessSession" /> instance.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IDelayedFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Execution of delayed queries will be deferred until the results are required by user code. This makes it
        ///     possible for <see cref="NHibernate" /> to batch multiple queries in one round-trip to the database
        ///     instead of making one round-trip per query.
        /// </remarks>
        public static IDelayedFlowQuery<TSource> DelayedFlowQuery<TSource>
            (
            this IStatelessSession session,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return DelayedFlowQuery<TSource>(session, null, options);
        }

        /// <summary>
        ///     Creates a new <see cref="IDelayedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="IStatelessSession" /> instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IDelayedFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Execution of delayed queries will be deferred until the results are required by user code. This makes it
        ///     possible for <see cref="NHibernate" /> to batch multiple queries in one round-trip to the database
        ///     instead of making one round-trip per query.
        /// </remarks>
        public static IDelayedFlowQuery<TSource> DelayedFlowQuery<TSource>
            (
            this IStatelessSession session,
            Expression<Func<TSource>> alias,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return DelayedFlowQuery(session.CreateCriteria, alias, options);
        }

        /// <summary>
        ///     Creates a new <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="ISession" /> instance.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        public static IDetachedFlowQuery<TSource> DetachedFlowQuery<TSource>(this ISession session)
            where TSource : class
        {
            return DetachedFlowQuery<TSource>(session, options: null);
        }

        /// <summary>
        ///     Creates a new <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="ISession" /> instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        public static IDetachedFlowQuery<TSource> DetachedFlowQuery<TSource>
            (
            this ISession session,
            Expression<Func<TSource>> alias
            )
            where TSource : class
        {
            return DetachedFlowQuery(session, alias, null);
        }

        /// <summary>
        ///     Creates a new <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="ISession" /> instance.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        public static IDetachedFlowQuery<TSource> DetachedFlowQuery<TSource>
            (
            this ISession session,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return DetachedFlowQuery<TSource>(session, null, options);
        }

        /// <summary>
        ///     Creates a new <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="ISession" /> instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        public static IDetachedFlowQuery<TSource> DetachedFlowQuery<TSource>
            (
            this ISession session,
            Expression<Func<TSource>> alias,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return DetachedFlowQuery(session.CreateCriteria, alias, options);
        }

        /// <summary>
        ///     Creates a new <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="IStatelessSession" /> instance.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        public static IDetachedFlowQuery<TSource> DetachedFlowQuery<TSource>(this IStatelessSession session)
            where TSource : class
        {
            return DetachedFlowQuery<TSource>(session, options: null);
        }

        /// <summary>
        ///     Creates a new <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="IStatelessSession" /> instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        public static IDetachedFlowQuery<TSource> DetachedFlowQuery<TSource>
            (
            this IStatelessSession session,
            Expression<Func<TSource>> alias
            )
            where TSource : class
        {
            return DetachedFlowQuery(session, alias, null);
        }

        /// <summary>
        ///     Creates a new <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="IStatelessSession" /> instance.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        public static IDetachedFlowQuery<TSource> DetachedFlowQuery<TSource>
            (
            this IStatelessSession session,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return DetachedFlowQuery<TSource>(session, null, options);
        }

        /// <summary>
        ///     Creates a new <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="IStatelessSession" /> instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        public static IDetachedFlowQuery<TSource> DetachedFlowQuery<TSource>
            (
            this IStatelessSession session,
            Expression<Func<TSource>> alias,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return DetachedFlowQuery(session.CreateCriteria, alias, options);
        }

        /// <summary>
        ///     Creates a new <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="ISession" /> instance.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Immediate queries are executed by <see cref="NHibernate" /> immediately (no pun intended), with a
        ///     separate round-trip to the database for each query. To reduce the number of round-trips to the database
        ///     you should use delayed queries instead (<see cref="IDelayedFlowQuery{TSource}" />).
        /// </remarks>
        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>(this ISession session)
            where TSource : class
        {
            return FlowQuery<TSource>(session, options: null);
        }

        /// <summary>
        ///     Creates a new <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="ISession" /> instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Immediate queries are executed by <see cref="NHibernate" /> immediately (no pun intended), with a
        ///     separate round-trip to the database for each query. To reduce the number of round-trips to the database
        ///     you should use delayed queries instead (<see cref="IDelayedFlowQuery{TSource}" />).
        /// </remarks>
        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>
            (
            this ISession session,
            Expression<Func<TSource>> alias
            )
            where TSource : class
        {
            return FlowQuery(session, alias, null);
        }

        /// <summary>
        ///     Creates a new <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="ISession" /> instance.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Immediate queries are executed by <see cref="NHibernate" /> immediately (no pun intended), with a
        ///     separate round-trip to the database for each query. To reduce the number of round-trips to the database
        ///     you should use delayed queries instead (<see cref="IDelayedFlowQuery{TSource}" />).
        /// </remarks>
        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>(this ISession session, FlowQueryOptions options)
            where TSource : class
        {
            return FlowQuery<TSource>(session, null, options);
        }

        /// <summary>
        ///     Creates a new <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="ISession" /> instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Immediate queries are executed by <see cref="NHibernate" /> immediately (no pun intended), with a
        ///     separate round-trip to the database for each query. To reduce the number of round-trips to the database
        ///     you should use delayed queries instead (<see cref="IDelayedFlowQuery{TSource}" />).
        /// </remarks>
        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>
            (
            this ISession session,
            Expression<Func<TSource>> alias,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return FlowQuery(session.CreateCriteria, alias, options);
        }

        /// <summary>
        ///     Creates a new <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="IStatelessSession" /> instance.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Immediate queries are executed by <see cref="NHibernate" /> immediately (no pun intended), with a
        ///     separate round-trip to the database for each query. To reduce the number of round-trips to the database
        ///     you should use delayed queries instead (<see cref="IDelayedFlowQuery{TSource}" />).
        /// </remarks>
        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>(this IStatelessSession session)
            where TSource : class
        {
            return FlowQuery<TSource>(session, options: null);
        }

        /// <summary>
        ///     Creates a new <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="IStatelessSession" /> instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Immediate queries are executed by <see cref="NHibernate" /> immediately (no pun intended), with a
        ///     separate round-trip to the database for each query. To reduce the number of round-trips to the database
        ///     you should use delayed queries instead (<see cref="IDelayedFlowQuery{TSource}" />).
        /// </remarks>
        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>
            (
            this IStatelessSession session,
            Expression<Func<TSource>> alias
            )
            where TSource : class
        {
            return FlowQuery(session, alias, null);
        }

        /// <summary>
        ///     Creates a new <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="IStatelessSession" /> instance.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Immediate queries are executed by <see cref="NHibernate" /> immediately (no pun intended), with a
        ///     separate round-trip to the database for each query. To reduce the number of round-trips to the database
        ///     you should use delayed queries instead (<see cref="IDelayedFlowQuery{TSource}" />).
        /// </remarks>
        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>
            (
            this IStatelessSession session,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return FlowQuery<TSource>(session, null, options);
        }

        /// <summary>
        ///     Creates a new <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="IStatelessSession" /> instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Immediate queries are executed by <see cref="NHibernate" /> immediately (no pun intended), with a
        ///     separate round-trip to the database for each query. To reduce the number of round-trips to the database
        ///     you should use delayed queries instead (<see cref="IDelayedFlowQuery{TSource}" />).
        /// </remarks>
        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>
            (
            this IStatelessSession session,
            Expression<Func<TSource>> alias,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return FlowQuery(session.CreateCriteria, alias, options);
        }

        /// <summary>
        ///     Creates a new <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="ISession" /> instance.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Immediate queries are executed by <see cref="NHibernate" /> immediately (no pun intended), with a
        ///     separate round-trip to the database for each query. To reduce the number of round-trips to the database
        ///     you should use delayed queries instead (<see cref="IDelayedFlowQuery{TSource}" />).
        /// </remarks>
        public static IImmediateFlowQuery<TSource> ImmediateFlowQuery<TSource>(this ISession session)
            where TSource : class
        {
            return ImmediateFlowQuery<TSource>(session, options: null);
        }

        /// <summary>
        ///     Creates a new <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="ISession" /> instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Immediate queries are executed by <see cref="NHibernate" /> immediately (no pun intended), with a
        ///     separate round-trip to the database for each query. To reduce the number of round-trips to the database
        ///     you should use delayed queries instead (<see cref="IDelayedFlowQuery{TSource}" />).
        /// </remarks>
        public static IImmediateFlowQuery<TSource> ImmediateFlowQuery<TSource>
            (
            this ISession session,
            Expression<Func<TSource>> alias
            )
            where TSource : class
        {
            return ImmediateFlowQuery(session, alias, null);
        }

        /// <summary>
        ///     Creates a new <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="ISession" /> instance.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Immediate queries are executed by <see cref="NHibernate" /> immediately (no pun intended), with a
        ///     separate round-trip to the database for each query. To reduce the number of round-trips to the database
        ///     you should use delayed queries instead (<see cref="IDelayedFlowQuery{TSource}" />).
        /// </remarks>
        public static IImmediateFlowQuery<TSource> ImmediateFlowQuery<TSource>
            (
            this ISession session,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return ImmediateFlowQuery<TSource>(session, null, options);
        }

        /// <summary>
        ///     Creates a new <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="ISession" /> instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Immediate queries are executed by <see cref="NHibernate" /> immediately (no pun intended), with a
        ///     separate round-trip to the database for each query. To reduce the number of round-trips to the database
        ///     you should use delayed queries instead (<see cref="IDelayedFlowQuery{TSource}" />).
        /// </remarks>
        public static IImmediateFlowQuery<TSource> ImmediateFlowQuery<TSource>
            (
            this ISession session,
            Expression<Func<TSource>> alias,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return ImmediateFlowQuery(session.CreateCriteria, alias, options);
        }

        /// <summary>
        ///     Creates a new <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="IStatelessSession" /> instance.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Immediate queries are executed by <see cref="NHibernate" /> immediately (no pun intended), with a
        ///     separate round-trip to the database for each query. To reduce the number of round-trips to the database
        ///     you should use delayed queries instead (<see cref="IDelayedFlowQuery{TSource}" />).
        /// </remarks>
        public static IImmediateFlowQuery<TSource> ImmediateFlowQuery<TSource>(this IStatelessSession session)
            where TSource : class
        {
            return ImmediateFlowQuery<TSource>(session, options: null);
        }

        /// <summary>
        ///     Creates a new <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="IStatelessSession" /> instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Immediate queries are executed by <see cref="NHibernate" /> immediately (no pun intended), with a
        ///     separate round-trip to the database for each query. To reduce the number of round-trips to the database
        ///     you should use delayed queries instead (<see cref="IDelayedFlowQuery{TSource}" />).
        /// </remarks>
        public static IImmediateFlowQuery<TSource> ImmediateFlowQuery<TSource>
            (
            this IStatelessSession session,
            Expression<Func<TSource>> alias
            )
            where TSource : class
        {
            return ImmediateFlowQuery(session, alias, null);
        }

        /// <summary>
        ///     Creates a new <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="IStatelessSession" /> instance.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Immediate queries are executed by <see cref="NHibernate" /> immediately (no pun intended), with a
        ///     separate round-trip to the database for each query. To reduce the number of round-trips to the database
        ///     you should use delayed queries instead (<see cref="IDelayedFlowQuery{TSource}" />).
        /// </remarks>
        public static IImmediateFlowQuery<TSource> ImmediateFlowQuery<TSource>
            (
            this IStatelessSession session,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return ImmediateFlowQuery<TSource>(session, null, options);
        }

        /// <summary>
        ///     Creates a new <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="IStatelessSession" /> instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Immediate queries are executed by <see cref="NHibernate" /> immediately (no pun intended), with a
        ///     separate round-trip to the database for each query. To reduce the number of round-trips to the database
        ///     you should use delayed queries instead (<see cref="IDelayedFlowQuery{TSource}" />).
        /// </remarks>
        public static IImmediateFlowQuery<TSource> ImmediateFlowQuery<TSource>
            (
            this IStatelessSession session,
            Expression<Func<TSource>> alias,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return ImmediateFlowQuery(session.CreateCriteria, alias, options);
        }

        /// <summary>
        ///     Creates a new <see cref="IStreamedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="ISession" /> instance.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IStreamedFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Streamed queries are executed by <see cref="NHibernate" /> immediately (just like 
        ///     <see cref="IImmediateFlowQuery{TSource}" />), with a separate round-trip to the database for each query.
        ///     To reduce the number of round-trips to the database you should use delayed queries instead 
        ///     (<see cref="IDelayedFlowQuery{TSource}" />). The difference between a 
        ///     <see cref="IStreamedFlowQuery{TSource}" /> and a <see cref="IImmediateFlowQuery{TSource}" /> is that the
        ///     entire result set for a <see cref="IStreamedFlowQuery{TSource}" /> never will be buffered into memory 
        ///     before it reaches user-code. It's entirely up to user-code to load it into memory, if it is deemed 
        ///     necessary for the particular situation.
        /// </remarks>
        public static IStreamedFlowQuery<TSource> StreamedFlowQuery<TSource>(this ISession session)
            where TSource : class
        {
            return StreamedFlowQuery<TSource>(session, options: null);
        }

        /// <summary>
        ///     Creates a new <see cref="IStreamedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="ISession" /> instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IStreamedFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Streamed queries are executed by <see cref="NHibernate" /> immediately (just like 
        ///     <see cref="IImmediateFlowQuery{TSource}" />), with a separate round-trip to the database for each query.
        ///     To reduce the number of round-trips to the database you should use delayed queries instead 
        ///     (<see cref="IDelayedFlowQuery{TSource}" />). The difference between a 
        ///     <see cref="IStreamedFlowQuery{TSource}" /> and a <see cref="IImmediateFlowQuery{TSource}" /> is that the
        ///     entire result set for a <see cref="IStreamedFlowQuery{TSource}" /> never will be buffered into memory 
        ///     before it reaches user-code. It's entirely up to user-code to load it into memory, if it is deemed 
        ///     necessary for the particular situation.
        /// </remarks>
        public static IStreamedFlowQuery<TSource> StreamedFlowQuery<TSource>
            (
            this ISession session,
            Expression<Func<TSource>> alias
            )
            where TSource : class
        {
            return StreamedFlowQuery(session, alias, null);
        }

        /// <summary>
        ///     Creates a new <see cref="IStreamedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="ISession" /> instance.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IStreamedFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Streamed queries are executed by <see cref="NHibernate" /> immediately (just like 
        ///     <see cref="IImmediateFlowQuery{TSource}" />), with a separate round-trip to the database for each query.
        ///     To reduce the number of round-trips to the database you should use delayed queries instead 
        ///     (<see cref="IDelayedFlowQuery{TSource}" />). The difference between a 
        ///     <see cref="IStreamedFlowQuery{TSource}" /> and a <see cref="IImmediateFlowQuery{TSource}" /> is that the
        ///     entire result set for a <see cref="IStreamedFlowQuery{TSource}" /> never will be buffered into memory 
        ///     before it reaches user-code. It's entirely up to user-code to load it into memory, if it is deemed 
        ///     necessary for the particular situation.
        /// </remarks>
        public static IStreamedFlowQuery<TSource> StreamedFlowQuery<TSource>
            (
            this ISession session,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return StreamedFlowQuery<TSource>(session, null, options);
        }

        /// <summary>
        ///     Creates a new <see cref="IStreamedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="ISession" /> instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IStreamedFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Streamed queries are executed by <see cref="NHibernate" /> immediately (just like 
        ///     <see cref="IImmediateFlowQuery{TSource}" />), with a separate round-trip to the database for each query.
        ///     To reduce the number of round-trips to the database you should use delayed queries instead 
        ///     (<see cref="IDelayedFlowQuery{TSource}" />). The difference between a 
        ///     <see cref="IStreamedFlowQuery{TSource}" /> and a <see cref="IImmediateFlowQuery{TSource}" /> is that the
        ///     entire result set for a <see cref="IStreamedFlowQuery{TSource}" /> never will be buffered into memory 
        ///     before it reaches user-code. It's entirely up to user-code to load it into memory, if it is deemed 
        ///     necessary for the particular situation.
        /// </remarks>
        public static IStreamedFlowQuery<TSource> StreamedFlowQuery<TSource>
            (
            this ISession session,
            Expression<Func<TSource>> alias,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return StreamedFlowQuery(session.CreateCriteria, alias, options);
        }

        /// <summary>
        ///     Creates a new <see cref="IStreamedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="IStatelessSession" /> instance.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IStreamedFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Streamed queries are executed by <see cref="NHibernate" /> immediately (just like 
        ///     <see cref="IImmediateFlowQuery{TSource}" />), with a separate round-trip to the database for each query.
        ///     To reduce the number of round-trips to the database you should use delayed queries instead 
        ///     (<see cref="IDelayedFlowQuery{TSource}" />). The difference between a 
        ///     <see cref="IStreamedFlowQuery{TSource}" /> and a <see cref="IImmediateFlowQuery{TSource}" /> is that the
        ///     entire result set for a <see cref="IStreamedFlowQuery{TSource}" /> never will be buffered into memory 
        ///     before it reaches user-code. It's entirely up to user-code to load it into memory, if it is deemed 
        ///     necessary for the particular situation.
        /// </remarks>
        public static IStreamedFlowQuery<TSource> StreamedFlowQuery<TSource>(this IStatelessSession session)
            where TSource : class
        {
            return StreamedFlowQuery<TSource>(session, options: null);
        }

        /// <summary>
        ///     Creates a new <see cref="IStreamedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="IStatelessSession" /> instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IStreamedFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Streamed queries are executed by <see cref="NHibernate" /> immediately (just like 
        ///     <see cref="IImmediateFlowQuery{TSource}" />), with a separate round-trip to the database for each query.
        ///     To reduce the number of round-trips to the database you should use delayed queries instead 
        ///     (<see cref="IDelayedFlowQuery{TSource}" />). The difference between a 
        ///     <see cref="IStreamedFlowQuery{TSource}" /> and a <see cref="IImmediateFlowQuery{TSource}" /> is that the
        ///     entire result set for a <see cref="IStreamedFlowQuery{TSource}" /> never will be buffered into memory 
        ///     before it reaches user-code. It's entirely up to user-code to load it into memory, if it is deemed 
        ///     necessary for the particular situation.
        /// </remarks>
        public static IStreamedFlowQuery<TSource> StreamedFlowQuery<TSource>
            (
            this IStatelessSession session,
            Expression<Func<TSource>> alias
            )
            where TSource : class
        {
            return StreamedFlowQuery(session, alias, null);
        }

        /// <summary>
        ///     Creates a new <see cref="IStreamedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="IStatelessSession" /> instance.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IStreamedFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Streamed queries are executed by <see cref="NHibernate" /> immediately (just like 
        ///     <see cref="IImmediateFlowQuery{TSource}" />), with a separate round-trip to the database for each query.
        ///     To reduce the number of round-trips to the database you should use delayed queries instead 
        ///     (<see cref="IDelayedFlowQuery{TSource}" />). The difference between a 
        ///     <see cref="IStreamedFlowQuery{TSource}" /> and a <see cref="IImmediateFlowQuery{TSource}" /> is that the
        ///     entire result set for a <see cref="IStreamedFlowQuery{TSource}" /> never will be buffered into memory 
        ///     before it reaches user-code. It's entirely up to user-code to load it into memory, if it is deemed 
        ///     necessary for the particular situation.
        /// </remarks>
        public static IStreamedFlowQuery<TSource> StreamedFlowQuery<TSource>
            (
            this IStatelessSession session,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return StreamedFlowQuery<TSource>(session, null, options);
        }

        /// <summary>
        ///     Creates a new <see cref="IStreamedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="session">
        ///     The <see cref="IStatelessSession" /> instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IStreamedFlowQuery{TSource}" /> query.
        /// </returns>
        /// <remarks>
        ///     Streamed queries are executed by <see cref="NHibernate" /> immediately (just like 
        ///     <see cref="IImmediateFlowQuery{TSource}" />), with a separate round-trip to the database for each query.
        ///     To reduce the number of round-trips to the database you should use delayed queries instead 
        ///     (<see cref="IDelayedFlowQuery{TSource}" />). The difference between a 
        ///     <see cref="IStreamedFlowQuery{TSource}" /> and a <see cref="IImmediateFlowQuery{TSource}" /> is that the
        ///     entire result set for a <see cref="IStreamedFlowQuery{TSource}" /> never will be buffered into memory 
        ///     before it reaches user-code. It's entirely up to user-code to load it into memory, if it is deemed 
        ///     necessary for the particular situation.
        /// </remarks>
        public static IStreamedFlowQuery<TSource> StreamedFlowQuery<TSource>
            (
            this IStatelessSession session,
            Expression<Func<TSource>> alias,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return StreamedFlowQuery(session.CreateCriteria, alias, options);
        }

        /// <summary>
        ///     Generates a <see cref="string" /> representation of the provided alias expression.
        /// </summary>
        /// <param name="alias">
        ///     The alias expression.
        /// </param>
        /// <typeparam name="TAlias">
        ///     The <see cref="System.Type" /> of the alias.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="string" /> representation of the alias.
        /// </returns>
        internal static string Alias<TAlias>(Expression<Func<TAlias>> alias)
        {
            string aliasName = null;

            if (alias != null)
            {
                aliasName = ExpressionHelper.GetPropertyName(alias);
            }

            return aliasName;
        }

        /// <summary>
        ///     Creates a <see cref="IDelayedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="criteriaFactory">
        ///     The criteria factory.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IDelayedFlowQuery{TSource}" /> query.
        /// </returns>
        private static IDelayedFlowQuery<TSource> DelayedFlowQuery<TSource>
            (
            Func<Type, string, ICriteria> criteriaFactory,
            Expression<Func<TSource>> alias,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return new DelayedFlowQuery<TSource>
            (
                criteriaFactory,
                Alias(alias) ?? RootAlias,
                options
            );
        }

        /// <summary>
        ///     Creates a <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="criteriaFactory">
        ///     The criteria factory.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        private static IDetachedFlowQuery<TSource> DetachedFlowQuery<TSource>
            (
            Func<Type, string, ICriteria> criteriaFactory,
            Expression<Func<TSource>> alias,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return new DetachedFlowQuery<TSource>
            (
                criteriaFactory,
                Alias(alias) ?? RootAlias,
                options
            );
        }

        /// <summary>
        ///     Creates a <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="criteriaFactory">
        ///     The criteria factory.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </returns>
        private static IImmediateFlowQuery<TSource> FlowQuery<TSource>
            (
            Func<Type, string, ICriteria> criteriaFactory,
            Expression<Func<TSource>> alias,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return new ImmediateFlowQuery<TSource>
            (
                criteriaFactory,
                Alias(alias) ?? RootAlias,
                options
            );
        }

        /// <summary>
        ///     Creates a <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="criteriaFactory">
        ///     The criteria factory.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IImmediateFlowQuery{TSource}" /> query.
        /// </returns>
        private static IImmediateFlowQuery<TSource> ImmediateFlowQuery<TSource>
            (
            Func<Type, string, ICriteria> criteriaFactory,
            Expression<Func<TSource>> alias,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return new ImmediateFlowQuery<TSource>
            (
                criteriaFactory,
                Alias(alias) ?? RootAlias,
                options
            );
        }

        /// <summary>
        ///     Creates a <see cref="IStreamedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="criteriaFactory">
        ///     The criteria factory.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IStreamedFlowQuery{TSource}" /> query.
        /// </returns>
        private static IStreamedFlowQuery<TSource> StreamedFlowQuery<TSource>
            (
            Func<Type, string, ICriteria> criteriaFactory,
            Expression<Func<TSource>> alias,
            FlowQueryOptions options
            )
            where TSource : class
        {
            return new StreamedFlowQuery<TSource>
            (
                criteriaFactory,
                Alias(alias) ?? RootAlias,
                options
            );
        }
    }
}