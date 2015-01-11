namespace NHibernate.FlowQuery.Core
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core.Implementations;

    /// <summary>
    ///     An interface defining the basic functionality required of a projectable
    ///     <see cref="NHibernate.FlowQuery" /> query.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity to be queried.
    /// </typeparam>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the class or interface implementing or extending this interface.
    /// </typeparam>
    /// <seealso cref="IDelayedFlowQuery{TSource}" />
    /// <seealso cref="IImmediateFlowQuery{TSource}" />
    public interface IQueryableFlowQuery<TSource, out TQuery> : IFlowQuery<TSource, TQuery>
        where TSource : class
        where TQuery : class, IQueryableFlowQuery<TSource, TQuery>
    {
        /// <summary>
        ///     Removes any previously specified timeout value.
        /// </summary>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery ClearTimeout();

        /// <summary>
        ///     Specifies a comment to include in the query when executed. Specify null for <paramref name="comment" />
        ///     to clear any previously specified value.
        /// </summary>
        /// <param name="comment">
        ///     The comment (or null to clear previously specified value).
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Comment(string comment);

        /// <summary>
        ///     Specifies fetch size (not to be confused with <see cref="IFlowQuery{TSource,TQuery}.Take" /> or
        ///     <see cref="IFlowQuery{TSource,TQuery}.Limit(int)" />).
        /// </summary>
        /// <param name="size">
        ///     The fetch size.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery FetchSize(int size);

        /// <summary>
        ///     Creates a partial projection and returns a utility to extend it when appropriate.
        /// </summary>
        /// <param name="projection">
        ///     The partial projection.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the projection.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="PartialSelection{TSource,TDestination}" /> instance.
        /// </returns>
        IPartialSelection<TSource, TDestination> PartialSelect<TDestination>
            (
            Expression<Func<TSource, TDestination>> projection = null
            );

        /// <summary>
        ///     Specifies the read only flag.
        /// </summary>
        /// <param name="isReadOnly">
        ///     The read only flag.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery ReadOnly(bool isReadOnly = true);

        /// <summary>
        ///     Creates a selection from the query.
        /// </summary>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TSource}" /> instance.
        /// </returns>
        FlowQuerySelection<TSource> Select();

        /// <summary>
        ///     Creates a utility to make a per-property-mapped selection.
        /// </summary>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the projection.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="ISelectSetup{TSource, TDestination}" /> instance.
        /// </returns>
        ISelectSetup<TSource, TDestination> Select<TDestination>();

        /// <summary>
        ///     Creates a selection from the query using the specified properties.
        /// </summary>
        /// <param name="properties">
        ///     The properties to select.
        /// </param>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TSource}" /> instance.
        /// </returns>
        FlowQuerySelection<TSource> Select(params string[] properties);

        /// <summary>
        ///     Creates a selection from the query using the specified projection.
        /// </summary>
        /// <param name="projection">
        ///     The projection to select.
        /// </param>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TSource}" /> instance.
        /// </returns>
        FlowQuerySelection<TSource> Select(IProjection projection);

        /// <summary>
        ///     Creates a selection from the query using the specified properties.
        /// </summary>
        /// <param name="properties">
        ///     The properties to select.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TDestination}" /> instance.
        /// </returns>
        FlowQuerySelection<TDestination> Select<TDestination>(params string[] properties);

        /// <summary>
        ///     Creates a selection from the query using the specified projection.
        /// </summary>
        /// <param name="projection">
        ///     The projection to select.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TDestination}" /> instance.
        /// </returns>
        FlowQuerySelection<TDestination> Select<TDestination>(IProjection projection);

        /// <summary>
        ///     Creates a selection from the query using the specified properties.
        /// </summary>
        /// <param name="properties">
        ///     The properties to select.
        /// </param>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TSource}" /> instance.
        /// </returns>
        FlowQuerySelection<TSource> Select(params Expression<Func<TSource, object>>[] properties);

        /// <summary>
        ///     Creates a selection from the query using the specified projection.
        /// </summary>
        /// <param name="projection">
        ///     The projection to select.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TDestination}" /> instance.
        /// </returns>
        FlowQuerySelection<TDestination> Select<TDestination>(Expression<Func<TSource, TDestination>> projection);

        /// <summary>
        ///     Creates a selection from the specified <see cref="ISelectSetup{TSource, TDestination}" /> instance.
        /// </summary>
        /// <param name="setup">
        ///     The <see cref="ISelectSetup{TSource, TDestination}" /> instance from which to create a selection.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TDestination}" /> instance.
        /// </returns>
        FlowQuerySelection<TDestination> Select<TDestination>(ISelectSetup<TSource, TDestination> setup);

        /// <summary>
        ///     Creates a selection from the specified <see cref="PartialSelection{TSource, TDestination}" /> instance.
        /// </summary>
        /// <param name="combiner">
        ///     The <see cref="PartialSelection{TSource, TDestination}" /> instance from which to create a selection.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TDestination}" /> instance.
        /// </returns>
        FlowQuerySelection<TDestination> Select<TDestination>(IPartialSelection<TSource, TDestination> combiner);

        /// <summary>
        ///     Specifies a timeout for the query. You can use <see cref="ClearTimeout" /> to clear any previously set
        ///     timeout value.
        /// </summary>
        /// <param name="seconds">
        ///     The timeout in seconds (values of zero or less is simply ignored).
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Timeout(int seconds);
    }
}