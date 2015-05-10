namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core.CustomProjections;
    using NHibernate.FlowQuery.Core.Structures;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Revealing.Conventions;

    /// <summary>
    ///     A class implementing the basic functionality of a <see cref="NHibernate.FlowQuery" /> query.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity to be queried.
    /// </typeparam>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the class or interface implementing or extending this interface.
    /// </typeparam>
    public abstract class FlowQueryBase<TSource, TQuery>
        : FilterableQuery<TSource, TQuery>,
          IFlowQuery<TSource, TQuery>,
          IFlowQuery
        where TSource : class
        where TQuery : class, IFlowQuery<TSource, TQuery>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FlowQueryBase{TSource,TQuery}" /> class.
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
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     The "this" reference is not of the type <see cref="T:TQuery" /> as specified by
        ///     <typeparamref name="TQuery" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="criteriaFactory" /> is null and the specific query alteration is not an implementation
        ///     of <see cref="T:IDetachedFlowQuery{TSource}" />.
        /// </exception>
        protected internal FlowQueryBase
            (
            Func<Type, string, ICriteria> criteriaFactory,
            string alias = null,
            FlowQueryOptions options = null,
            IFlowQuery query = null
            )
            : base(alias, query)
        {
            if (criteriaFactory == null)
            {
                // TODO: To allow for extensions the null-check should perhaps be pushed down the inheritance chain?
                // allow criteria factory to be null for detached queries.
                if (!(this is IDetachedFlowQuery<TSource>))
                {
                    throw new ArgumentNullException("criteriaFactory");
                }
            }

            if (query != null)
            {
                CacheMode = query.CacheMode;
                CacheRegion = query.CacheRegion;
                IsCacheable = query.IsCacheable;
                Fetches = query.Fetches.ToList();
                GroupBys = query.GroupBys.ToList();
                Locks = query.Locks.ToList();
                Orders = query.Orders.ToList();

                ResultsToSkip = query.ResultsToSkip;
                ResultsToTake = query.ResultsToTake;
            }
            else
            {
                Fetches = new List<Fetch>();
                GroupBys = new List<FqGroupByProjection>();
                Locks = new List<Lock>();
                Orders = new List<OrderByStatement>();
            }

            CriteriaFactory = criteriaFactory;

            Options = options;
        }

        /// <inheritdoc />
        public CacheMode? CacheMode { get; private set; }

        /// <inheritdoc />
        public string CacheRegion { get; private set; }

        /// <inheritdoc />
        public Func<Type, string, ICriteria> CriteriaFactory { get; private set; }

        /// <inheritdoc />
        public List<Fetch> Fetches { get; private set; }

        /// <inheritdoc />
        public List<FqGroupByProjection> GroupBys { get; private set; }

        /// <inheritdoc />
        public bool IsCacheable { get; private set; }

        /// <inheritdoc />
        public List<Lock> Locks { get; private set; }

        /// <inheritdoc />
        public FlowQueryOptions Options { get; private set; }

        /// <inheritdoc />
        public List<OrderByStatement> Orders { get; private set; }

        /// <inheritdoc />
        public virtual int? ResultsToSkip { get; private set; }

        /// <inheritdoc />
        public virtual int? ResultsToTake { get; private set; }

        /// <inheritdoc />
        public virtual TQuery Cacheable(bool isCacheable = true)
        {
            return Cacheable(isCacheable, null, null);
        }

        /// <inheritdoc />
        public virtual TQuery Cacheable(string cacheRegion)
        {
            return Cacheable(true, CacheMode, cacheRegion);
        }

        /// <inheritdoc />
        public virtual TQuery Cacheable
            (
            string cacheRegion,
            CacheMode cacheMode
            )
        {
            return Cacheable(true, cacheMode, cacheRegion);
        }

        /// <inheritdoc />
        public virtual TQuery Cacheable(CacheMode cacheMode)
        {
            return Cacheable(true, cacheMode, CacheRegion);
        }

        /// <inheritdoc />
        public virtual TQuery ClearFetches()
        {
            Fetches.Clear();

            return Query;
        }

        /// <inheritdoc />
        public virtual TQuery ClearGroupBys()
        {
            GroupBys.Clear();

            return Query;
        }

        /// <inheritdoc />
        public virtual TQuery ClearJoins()
        {
            Joins.Clear();

            return Query;
        }

        /// <inheritdoc />
        public virtual TQuery ClearLimit()
        {
            ResultsToSkip = null;
            ResultsToTake = null;

            return Query;
        }

        /// <inheritdoc />
        public virtual TQuery ClearLocks()
        {
            Locks.Clear();

            return Query;
        }

        /// <inheritdoc />
        public virtual TQuery ClearOrders()
        {
            Orders.Clear();

            return Query;
        }

        /// <inheritdoc />
        public virtual TQuery ClearRestrictions()
        {
            Criterions.Clear();

            return Query;
        }

        /// <inheritdoc />
        public virtual IFetchBuilder<TQuery> Fetch(string path)
        {
            return FetchCore(path, path);
        }

        /// <inheritdoc />
        public virtual IFetchBuilder<TQuery> Fetch
            (
            Expression<Func<TSource, object>> expression,
            Expression<Func<object>> alias = null,
            IRevealConvention revealConvention = null
            )
        {
            if (revealConvention == null)
            {
                revealConvention = Reveal.DefaultConvention ?? new CustomConvention(x => x);
            }

            string path = Reveal.ByConvention(expression, revealConvention);

            string aliasValue = alias != null
                ? ExpressionHelper.GetPropertyName(alias)
                : path;

            return FetchCore(path, aliasValue);
        }

        /// <inheritdoc />
        public virtual TQuery GroupBy(Expression<Func<TSource, object>> property)
        {
            if (property != null)
            {
                IProjection projection = ProjectionHelper
                    .GetProjection
                    (
                        property.Body,
                        new HelperContext(Data, property, HelperType.GroupBy)
                    );

                if (projection != null)
                {
                    if (projection.IsGrouped || projection.IsAggregate)
                    {
                        throw new InvalidOperationException(
                            "Cannot use an aggregate or grouped projection with GroupBy");
                    }

                    GroupBys.Add(new FqGroupByProjection(projection, false));
                }
            }

            return Query;
        }

        /// <inheritdoc />
        public virtual TQuery Limit(int limit)
        {
            return Take(limit);
        }

        /// <inheritdoc />
        public virtual TQuery Limit
            (
            int limit,
            int offset
            )
        {
            return Take(limit).Skip(offset);
        }

        /// <inheritdoc />
        public virtual ILockBuilder<TQuery> Lock()
        {
            return Lock(null as string);
        }

        /// <inheritdoc />
        public virtual ILockBuilder<TQuery> Lock(Expression<Func<object>> alias)
        {
            return Lock(ExpressionHelper.GetPropertyName(alias));
        }

        /// <inheritdoc />
        public virtual ILockBuilder<TQuery> Lock(string alias)
        {
            return new LockBuilder<TQuery>(this, Query, alias);
        }

        /// <inheritdoc />
        public virtual TQuery OrderBy
            (
            string property,
            bool ascending = true
            )
        {
            Orders.Add(new OrderByStatement
            {
                IsBasedOnSource = true,
                Order = ascending
                    ? Order.Asc(property)
                    : Order.Desc(property)
            });

            return Query;
        }

        /// <inheritdoc />
        public virtual TQuery OrderBy
            (
            IProjection projection,
            bool ascending = true
            )
        {
            Orders.Add(new OrderByStatement
            {
                IsBasedOnSource = true,
                Order = ascending
                    ? Order.Asc(projection)
                    : Order.Desc(projection)
            });

            return Query;
        }

        /// <inheritdoc />
        public virtual TQuery OrderBy
            (
            Expression<Func<TSource, object>> property,
            bool ascending = true
            )
        {
            IProjection projection = ProjectionHelper
                .GetProjection
                (
                    property.Body,
                    new HelperContext(Data, property, HelperType.Order)
                );

            return OrderBy(projection, ascending);
        }

        /// <inheritdoc />
        /// <typeparam name="TProjection">
        ///     The <see cref="System.Type" /> of the projection.
        /// </typeparam>
        public virtual TQuery OrderBy<TProjection>
            (
            string property,
            bool ascending = true
            )
        {
            Orders.Add(new OrderByStatement
            {
                IsBasedOnSource = false,
                OrderAscending = ascending,
                ProjectionSourceType = typeof(TProjection),
                Property = property
            });

            return Query;
        }

        /// <inheritdoc />
        /// <typeparam name="TProjection">
        ///     The <see cref="System.Type" /> of the projection.
        /// </typeparam>
        public virtual TQuery OrderBy<TProjection>
            (
            Expression<Func<TProjection, object>> property,
            bool ascending = true
            )
        {
            return OrderBy<TProjection>
                (
                    ExpressionHelper.GetPropertyName(property),
                    ascending
                );
        }

        /// <inheritdoc />
        public virtual TQuery OrderByDescending(string property)
        {
            return OrderBy(property, false);
        }

        /// <inheritdoc />
        public virtual TQuery OrderByDescending(IProjection projection)
        {
            return OrderBy(projection, false);
        }

        /// <inheritdoc />
        public virtual TQuery OrderByDescending(Expression<Func<TSource, object>> property)
        {
            return OrderBy(property, false);
        }

        /// <inheritdoc />
        /// <typeparam name="TProjection">
        ///     The <see cref="System.Type" /> of the projection.
        /// </typeparam>
        public virtual TQuery OrderByDescending<TProjection>(string property)
        {
            return OrderBy<TProjection>(property, false);
        }

        /// <inheritdoc />
        /// <typeparam name="TProjection">
        ///     The <see cref="System.Type" /> of the projection.
        /// </typeparam>
        public virtual TQuery OrderByDescending<TProjection>(Expression<Func<TProjection, object>> property)
        {
            return OrderBy(property, false);
        }

        /// <inheritdoc />
        public virtual TQuery RestrictByExample
            (
            TSource exampleInstance,
            Action<IExampleWrapper<TSource>> example
            )
        {
            if (example == null)
            {
                throw new ArgumentNullException("example");
            }

            if (exampleInstance == null)
            {
                throw new ArgumentNullException("exampleInstance");
            }

            IExampleWrapper<TSource> wrapper = new ExampleWrapper<TSource>(Example.Create(exampleInstance));

            example(wrapper);

            return Where(wrapper.Example);
        }

        /// <inheritdoc />
        public virtual TQuery Skip(int skip)
        {
            ResultsToSkip = skip;

            return Query;
        }

        /// <inheritdoc />
        public virtual TQuery Take(int take)
        {
            ResultsToTake = take;

            return Query;
        }

        /// <summary>
        ///     Determines whether this query should take advantage of second-level caching and within which cache
        ///     region and with what cache mode.
        /// </summary>
        /// <param name="isCacheable">
        ///     Indicates whether the query is cacheable.
        /// </param>
        /// <param name="cacheMode">
        ///     The cache mode.
        /// </param>
        /// <param name="cacheRegion">
        ///     The cache region.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        protected virtual TQuery Cacheable
            (
            bool isCacheable,
            CacheMode? cacheMode,
            string cacheRegion)
        {
            IsCacheable = isCacheable;

            CacheMode = cacheMode;
            CacheRegion = cacheRegion;

            return Query;
        }

        /// <summary>
        ///     Create a fetching strategy for the specified property or association path.
        /// </summary>
        /// <param name="path">
        ///     The association path.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <returns>
        ///     A <see cref="IFetchBuilder{TQuery}" /> instance.
        /// </returns>
        protected virtual IFetchBuilder<TQuery> FetchCore
            (
            string path,
            string alias)
        {
            if (Fetches.Count > 0 && Fetches.Any(x => x.HasAlias))
            {
                string[] steps = path.Split('.');

                if (steps.Length > 1)
                {
                    for (int i = 0; i < steps.Length; i++)
                    {
                        Fetch fetch = Fetches
                            .SingleOrDefault(x => x.HasAlias && x.Alias == steps[i]);

                        if (fetch != null)
                        {
                            steps[i] = fetch.Path;
                        }
                    }

                    path = string.Join(".", steps);
                }
            }

            return new FetchBuilder<TQuery>(this, Query, path, alias);
        }
    }
}