namespace NHibernate.FlowQuery.Core
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Core.Implementations;

    /// <summary>
    ///     An interface defining the basic functionality required of an executable
    ///     <see cref="IFlowQuery" /> query.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity to be queried.
    /// </typeparam>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the class or interface implementing or extending this interface.
    /// </typeparam>
    /// <seealso cref="IDelayedFlowQuery{TSource}" />
    /// <seealso cref="IImmediateFlowQuery{TSource}" />
    /// <seealso cref="IStreamedFlowQuery{TSource}" />
    /// <seealso cref="IQueryableFlowQuery{TSource,TQuery}" />
    public interface IQueryableFlowQueryBase<TSource, out TQuery> : IFlowQuery<TSource, TQuery>
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
        ///     Specify a sort order on a property in your query projection.
        /// </summary>
        /// <param name="property">
        ///     The property to sort on.
        /// </param>
        /// <param name="ascending">
        ///     Indicates whether to sort ascending (true, default) or descending (false).
        /// </param>
        /// <typeparam name="TProjection">
        ///     The <see cref="System.Type" /> of the projection.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        /// <remarks>
        ///     Only works when projecting using <see cref="MemberInitExpression" />s or <see cref="NewExpression" />s,
        ///     or when projecting using <see cref="ISelectSetup{TSource,TDestination}" />.
        /// </remarks>
        TQuery OrderBy<TProjection>
            (
            Expression<Func<TProjection, object>> property,
            bool ascending = true
            );

        /// <summary>
        ///     Specify a sort order on a property in your query projection.
        /// </summary>
        /// <param name="property">
        ///     The property to sort on.
        /// </param>
        /// <param name="ascending">
        ///     Indicates whether to sort ascending (true, default) or descending (false).
        /// </param>
        /// <typeparam name="TProjection">
        ///     The <see cref="System.Type" /> of the projection.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        /// <remarks>
        ///     Only works when projecting using <see cref="MemberInitExpression" />s or <see cref="NewExpression" />s,
        ///     or when projecting using <see cref="ISelectSetup{TSource, TDestination}" />.
        /// </remarks>
        TQuery OrderBy<TProjection>
            (
            string property,
            bool ascending = true
            );

        /// <summary>
        ///     Specify a sort order on a property in your query projection (always sorting descending).
        /// </summary>
        /// <param name="property">
        ///     The property to sort on.
        /// </param>
        /// <typeparam name="TProjection">
        ///     The <see cref="System.Type" /> of the projection.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        /// <remarks>
        ///     Only works when projecting using <see cref="MemberInitExpression" />s or <see cref="NewExpression" />s,
        ///     or when projecting using <see cref="ISelectSetup{TSource, TDestination}" />.
        /// </remarks>
        TQuery OrderByDescending<TProjection>(Expression<Func<TProjection, object>> property);

        /// <summary>
        ///     Specify a sort order on a property in your query projection (always sorting descending).
        /// </summary>
        /// <param name="property">
        ///     The property to sort on.
        /// </param>
        /// <typeparam name="TProjection">
        ///     The <see cref="System.Type" /> of the projection.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        /// <remarks>
        ///     Only works when projecting using <see cref="MemberInitExpression" />s or <see cref="NewExpression" />s,
        ///     or when projecting using <see cref="ISelectSetup{TSource, TDestination}" />.
        /// </remarks>
        TQuery OrderByDescending<TProjection>(string property);

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