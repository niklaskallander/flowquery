namespace NHibernate.FlowQuery
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;

    /// <summary>
    ///     A static helper class defining a set of utilities to create truly detached (e.g. no <see cref="ISession" />
    ///     references) <see cref="NHibernate.FlowQuery" /> queries.
    /// </summary>
    public static class DetachedFlowQuery
    {
        /// <summary>
        ///     Creates a new <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </summary>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        public static IDetachedFlowQuery<TSource> For<TSource>
            (
            FlowQueryOptions options = null
            )
            where TSource : class
        {
            return For<TSource>(null, options);
        }

        /// <summary>
        ///     Creates a new <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </summary>
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
        public static IDetachedFlowQuery<TSource> For<TSource>
            (
            Expression<Func<TSource>> alias,
            FlowQueryOptions options = null
            )
            where TSource : class
        {
            return new DetachedFlowQuery<TSource>
            (
                null,
                SessionExtensions.Alias(alias) ?? SessionExtensions.RootAlias,
                options
            );
        }
    }
}