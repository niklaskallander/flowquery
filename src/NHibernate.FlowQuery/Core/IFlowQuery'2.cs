namespace NHibernate.FlowQuery.Core
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Revealing.Conventions;

    /// <summary>
    ///     An interface defining the basic functionality of a <see cref="NHibernate.FlowQuery" /> query.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity to be queried.
    /// </typeparam>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the class or interface implementing or extending this interface.
    /// </typeparam>
    public interface IFlowQuery<TSource, out TQuery> : IFilterableQuery<TSource, TQuery>
    {
        /// <summary>
        ///     Adds one or more filters to the query.
        /// </summary>
        /// <param name="criterions">
        ///     The filters.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        /// <remarks>
        ///     An alias for <see cref="Where(ICriterion[])" />.
        /// </remarks>
        TQuery And(params ICriterion[] criterions);

        /// <summary>
        ///     Determines whether this query should take advantage of second-level caching.
        /// </summary>
        /// <param name="isCacheable">
        ///     Optional switch (mostly to be able to turn cache-ability off after it's been switched on. If set to
        ///     false the values for cache region (<see cref="IFlowQuery.CacheRegion" />) and cache mode
        ///     (<see cref="IFlowQuery.CacheMode" />) will be reset as well.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Cacheable(bool isCacheable = true);

        /// <summary>
        ///     Determines whether this query should take advantage of second-level caching and within which cache
        ///     region.
        /// </summary>
        /// <param name="cacheRegion">
        ///     The cache region.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Cacheable(string cacheRegion);

        /// <summary>
        ///     Determines whether this query should take advantage of second-level caching and within which cache
        ///     region and with what cache mode.
        /// </summary>
        /// <param name="cacheRegion">
        ///     The cache region.
        /// </param>
        /// <param name="cacheMode">
        ///     The cache mode.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Cacheable
            (
            string cacheRegion,
            CacheMode cacheMode
            );

        /// <summary>
        ///     Determines whether this query should take advantage of second-level caching and with what cache mode.
        /// </summary>
        /// <param name="cacheMode">
        ///     The cache mode.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Cacheable(CacheMode cacheMode);

        /// <summary>
        ///     Removes all fetching strategies for this query.
        /// </summary>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery ClearFetches();

        /// <summary>
        ///     Removes all group by statements for this query.
        /// </summary>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery ClearGroupBys();

        /// <summary>
        ///     Removes all joins for this query.
        /// </summary>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery ClearJoins();

        /// <summary>
        ///     Removes the limit for this query.
        /// </summary>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery ClearLimit();

        /// <summary>
        ///     Removes all locking strategies for this query.
        /// </summary>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery ClearLocks();

        /// <summary>
        ///     Removes all order by statements for this query.
        /// </summary>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery ClearOrders();

        /// <summary>
        ///     Removes all filters for this query.
        /// </summary>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery ClearRestrictions();

        /// <summary>
        ///     Create a fetching strategy for the specified property or association path.
        /// </summary>
        /// <param name="path">
        ///     The property or association path.
        /// </param>
        /// <returns>
        ///     A <see cref="IFetchBuilder{TQuery}" /> instance.
        /// </returns>
        IFetchBuilder<TQuery> Fetch(string path);

        /// <summary>
        ///     Create a fetching strategy for the specified property or association path.
        /// </summary>
        /// <param name="expression">
        ///     The property or association path.
        /// </param>
        /// <param name="alias">
        ///     An optional alias to use for the fetching strategy path.
        /// </param>
        /// <param name="revealConvention">
        ///     An optional reveal convention.
        /// </param>
        /// <returns>
        ///     A <see cref="IFetchBuilder{TQuery}" /> instance.
        /// </returns>
        IFetchBuilder<TQuery> Fetch
            (
            Expression<Func<TSource, object>> expression,
            Expression<Func<object>> alias = null,
            IRevealConvention revealConvention = null
            );

        /// <summary>
        ///     Specify a property to group by.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery GroupBy(Expression<Func<TSource, object>> property);

        /// <summary>
        ///     Specifies a limit on the number of results to retrieve.
        /// </summary>
        /// <param name="limit">
        ///     The number of results to retrieve.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Limit(int limit);

        /// <summary>
        ///     Specifies a limit on the number of results to retrieve and an offset determining the number of results
        ///     to skip.
        /// </summary>
        /// <param name="limit">
        ///     The number of results to retrieve.
        /// </param>
        /// <param name="offset">
        ///     The number of results to skip.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Limit
            (
            int limit,
            int offset
            );

        /// <summary>
        ///     Specifies a locking strategy on the provided alias.
        /// </summary>
        /// <param name="alias">
        ///     The alias for the locking strategy.
        /// </param>
        /// <returns>
        ///     A <see cref="ILockBuilder{TQuery}" /> instance.
        /// </returns>
        ILockBuilder<TQuery> Lock(Expression<Func<object>> alias);

        /// <summary>
        ///     Specifies a locking strategy on the provided alias.
        /// </summary>
        /// <param name="alias">
        ///     The alias for the locking strategy.
        /// </param>
        /// <returns>
        ///     A <see cref="ILockBuilder{TQuery}" /> instance.
        /// </returns>
        ILockBuilder<TQuery> Lock(string alias);

        /// <summary>
        ///     Specifies a locking strategy on the query.
        /// </summary>
        /// <returns>
        ///     A <see cref="ILockBuilder{TQuery}" /> instance.
        /// </returns>
        ILockBuilder<TQuery> Lock();

        /// <summary>
        ///     Specify a sort order on a property.
        /// </summary>
        /// <param name="property">
        ///     The property to sort on.
        /// </param>
        /// <param name="ascending">
        ///     Indicates whether to sort ascending (true, default) or descending (false).
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery OrderBy
            (
            string property,
            bool ascending = true
            );

        /// <summary>
        ///     Specify a sort order on a projection.
        /// </summary>
        /// <param name="projection">
        ///     The projection to sort on.
        /// </param>
        /// <param name="ascending">
        ///     Indicates whether to sort ascending (true, default) or descending (false).
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery OrderBy
            (
            IProjection projection,
            bool ascending = true
            );

        /// <summary>
        ///     Specify a sort order on a property.
        /// </summary>
        /// <param name="property">
        ///     The property to sort on.
        /// </param>
        /// <param name="ascending">
        ///     Indicates whether to sort ascending (true, default) or descending (false).
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery OrderBy
            (
            Expression<Func<TSource, object>> property,
            bool ascending = true
            );

        /// <summary>
        ///     Specify a sort order on a property (always sorting descending).
        /// </summary>
        /// <param name="property">
        ///     The property to sort on.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery OrderByDescending(string property);

        /// <summary>
        ///     Specify a sort order on a projection (always sorting descending).
        /// </summary>
        /// <param name="projection">
        ///     The projection to sort on.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery OrderByDescending(IProjection projection);

        /// <summary>
        ///     Specify a sort order on a property (always sorting descending).
        /// </summary>
        /// <param name="property">
        ///     The property to sort on.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery OrderByDescending(Expression<Func<TSource, object>> property);

        /// <summary>
        ///     Filters the query by an example instance of <see cref="T:TQuery" />.
        /// </summary>
        /// <param name="exampleInstance">
        ///     The example instance.
        /// </param>
        /// <param name="example">
        ///     The filtering options.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        /// <remarks>
        ///     If you haven't set any values to value data types or non-zero-based data types you must exclude these
        ///     properties manually, e.g. for DateTime, Enumerators, Booleans etc.
        /// </remarks>
        TQuery RestrictByExample
            (
            TSource exampleInstance,
            Action<IExampleWrapper<TSource>> example
            );

        /// <summary>
        ///     Specifies an offset determining the number of results to skip.
        /// </summary>
        /// <param name="skip">
        ///     The number of results to skip.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Skip(int skip);

        /// <summary>
        ///     Specifies a limit on the number of results to retrieve.
        /// </summary>
        /// <param name="take">
        ///     The number of results to retrieve.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Take(int take);

        /// <summary>
        ///     Adds one or more filters to the query.
        /// </summary>
        /// <param name="criterions">
        ///     The filters.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Where(params ICriterion[] criterions);
    }
}