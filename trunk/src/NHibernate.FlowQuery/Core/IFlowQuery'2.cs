using System;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core.Fetches;
using NHibernate.FlowQuery.Core.Joins;
using NHibernate.FlowQuery.Core.Locks;
using NHibernate.FlowQuery.Expressions;
using NHibernate.FlowQuery.Revealing.Conventions;

namespace NHibernate.FlowQuery.Core
{
    public interface IFlowQuery<TSource, TQuery>
        where TSource : class
        where TQuery : class, IFlowQuery<TSource, TQuery>
    {
        TQuery And(params ICriterion[] criterions);

        TQuery And(string property, IsExpression expression);

        TQuery And(Expression<Func<TSource, bool>> expression);

        TQuery And(Expression<Func<TSource, object>> property, IsExpression expression);

        TQuery And(Expression<Func<TSource, WhereDelegate, bool>> expression);

        TQuery And(IDetachedImmutableFlowQuery subquery, Expressions.IsEmptyExpression expresson);

        TQuery And(DetachedCriteria subquery, Expressions.IsEmptyExpression expresson);

        /// <summary>
        ///     Determines whether this query should take advantage of second-level caching.
        /// </summary>
        /// <param name="isCacheable">
        ///     Optional switch (mostly to be able to turn cacheability off after it's been switched on. If set to false 
        ///     the values for |cacheRegion| and |cacheMode| will be reset as well.
        /// </param>
        TQuery Cacheable(bool isCacheable = true);

        TQuery Cacheable(string cacheRegion);

        TQuery Cacheable(string cacheRegion, CacheMode cacheMode);

        TQuery Cacheable(CacheMode cacheMode);

        TQuery ClearFetches();

        TQuery ClearGroupBys();

        TQuery ClearJoins();

        TQuery ClearLimit();

        TQuery ClearLocks();

        TQuery ClearOrders();

        TQuery ClearRestrictions();

        IFetchBuilder<TSource, TQuery> Fetch(string path);

        IFetchBuilder<TSource, TQuery> Fetch(Expression<Func<TSource, object>> expression, Expression<Func<object>> alias = null, IRevealConvention revealConvention = null);

        TQuery GroupBy(Expression<Func<TSource, object>> property);

        ILockBuilder<TSource, TQuery> Lock(Expression<Func<object>> alias);

        ILockBuilder<TSource, TQuery> Lock(string alias);

        ILockBuilder<TSource, TQuery> Lock();

        TQuery Where(params ICriterion[] criterions);

        TQuery Where(string property, IsExpression expression);

        TQuery Where(Expression<Func<TSource, bool>> expression);

        TQuery Where(Expression<Func<TSource, object>> property, IsExpression expression);

        TQuery Where(Expression<Func<TSource, WhereDelegate, bool>> expression);

        TQuery Where(IDetachedImmutableFlowQuery subquery, Expressions.IsEmptyExpression expresson);

        TQuery Where(DetachedCriteria subquery, Expressions.IsEmptyExpression expresson);

        /// <summary>
        /// If you haven't set any values to non-nullable datatypes or non-zero-based datatypes you
        /// must exclude these properties manually, e.g. for DateTime, Enums, bool etc.
        /// </summary>
        TQuery RestrictByExample(TSource exampleInstance, Action<IExampleWrapper<TSource>> example);

        IJoinBuilder<TSource, TQuery> Inner { get; }

        IJoinBuilder<TSource, TQuery> LeftOuter { get; }

        IJoinBuilder<TSource, TQuery> RightOuter { get; }

        IJoinBuilder<TSource, TQuery> Full { get; }

        TQuery OrderBy(string property, bool ascending = true);

        TQuery OrderBy(IProjection projection, bool ascending = true);

        TQuery OrderBy(Expression<Func<TSource, object>> property, bool ascending = true);

        /// <summary>
        /// OrderBy an alias in your projections. NOTE: Only works when projecting with MemberInitializers ( standalone and in new expressions ) and SelectSetup.
        /// </summary>
        TQuery OrderBy<TProjection>(Expression<Func<TProjection, object>> projectionProperty, bool ascending = true);

        /// <summary>
        /// OrderBy an alias in your projections. NOTE: Only works when projecting with MemberInitializers ( standalone and in new expressions ) and SelectSetup.
        /// </summary>
        TQuery OrderBy<TProjection>(string property, bool ascending = true);

        TQuery OrderByDescending(string property);

        TQuery OrderByDescending(IProjection projection);

        TQuery OrderByDescending(Expression<Func<TSource, object>> property);

        /// <summary>
        /// OrderBy an alias in your projections. NOTE: Only works when projecting with MemberInitializers ( standalone and in new exressions ) and SelectSetup.
        /// </summary>
        TQuery OrderByDescending<TProjection>(Expression<Func<TProjection, object>> projectionProperty);

        /// <summary>
        /// OrderBy an alias in your projections. NOTE: Only works when projecting with MemberInitializers ( standalone and in new exressions ) and SelectSetup.
        /// </summary>
        TQuery OrderByDescending<TProjection>(string property);

        TQuery Limit(int limit);

        TQuery Limit(int limit, int offset);

        TQuery Skip(int skip);

        TQuery Take(int take);
    }
}