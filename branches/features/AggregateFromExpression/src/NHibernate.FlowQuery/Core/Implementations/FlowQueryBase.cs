namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core.CustomProjections;
    using NHibernate.FlowQuery.Core.Structures;
    using NHibernate.FlowQuery.Expressions;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Revealing.Conventions;
    using NHibernate.SqlCommand;

    using IsEmptyExpression = NHibernate.FlowQuery.Expressions.IsEmptyExpression;

    /// <summary>
    ///     A class implementing the basic functionality of a <see cref="NHibernate.FlowQuery" /> query.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity to be queried.
    /// </typeparam>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the class or interface implementing or extending this interface.
    /// </typeparam>
    public abstract class FlowQueryBase<TSource, TQuery> : IFlowQuery<TSource, TQuery>, IFlowQuery
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
        {
            Query = this as TQuery;

            if (Query == null)
            {
                throw new ArgumentException("The provided TQuery must the type of this instance");
            }

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
                Aliases = query.Aliases.ToDictionary(x => x.Key, x => x.Value);
                CacheMode = query.CacheMode;
                CacheRegion = query.CacheRegion;
                Criterions = query.Criterions.ToList();
                IsCacheable = query.IsCacheable;
                Fetches = query.Fetches.ToList();
                GroupBys = query.GroupBys.ToList();
                Joins = query.Joins.ToList();
                Locks = query.Locks.ToList();
                Orders = query.Orders.ToList();

                ResultsToSkip = query.ResultsToSkip;
                ResultsToTake = query.ResultsToTake;
            }
            else
            {
                Aliases = new Dictionary<string, string>();
                Criterions = new List<ICriterion>();
                Fetches = new List<Fetch>();
                GroupBys = new List<FqGroupByProjection>();
                Joins = new List<Join>();
                Locks = new List<Lock>();
                Orders = new List<OrderByStatement>();

                if (alias != null)
                {
                    Aliases.Add("entity.root.alias", alias);
                }
            }

            Alias = alias;

            CriteriaFactory = criteriaFactory;

            Data = new QueryHelperData(Aliases, Joins);

            Options = options;

            Inner = new JoinBuilder<TSource, TQuery>(this, Query, JoinType.InnerJoin);
            LeftOuter = new JoinBuilder<TSource, TQuery>(this, Query, JoinType.LeftOuterJoin);
            RightOuter = new JoinBuilder<TSource, TQuery>(this, Query, JoinType.RightOuterJoin);
            Full = new JoinBuilder<TSource, TQuery>(this, Query, JoinType.FullJoin);
        }

        /// <inheritdoc />
        public string Alias { get; private set; }

        /// <inheritdoc />
        public Dictionary<string, string> Aliases { get; private set; }

        /// <inheritdoc />
        public CacheMode? CacheMode { get; private set; }

        /// <inheritdoc />
        public string CacheRegion { get; private set; }

        /// <inheritdoc />
        public Func<Type, string, ICriteria> CriteriaFactory { get; private set; }

        /// <inheritdoc />
        public List<ICriterion> Criterions { get; private set; }

        /// <inheritdoc />
        public QueryHelperData Data { get; private set; }

        /// <inheritdoc />
        public List<Fetch> Fetches { get; private set; }

        /// <inheritdoc />
        public virtual IJoinBuilder<TSource, TQuery> Full { get; private set; }

        /// <inheritdoc />
        public List<FqGroupByProjection> GroupBys { get; private set; }

        /// <inheritdoc />
        public virtual IJoinBuilder<TSource, TQuery> Inner { get; private set; }

        /// <inheritdoc />
        public bool IsCacheable { get; private set; }

        /// <inheritdoc />
        public List<Join> Joins { get; private set; }

        /// <inheritdoc />
        public virtual IJoinBuilder<TSource, TQuery> LeftOuter { get; private set; }

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
        public virtual IJoinBuilder<TSource, TQuery> RightOuter { get; private set; }

        /// <summary>
        ///     Gets the query.
        /// </summary>
        /// <value>
        ///     The query.
        /// </value>
        protected internal TQuery Query { get; private set; }

        /// <inheritdoc />
        public virtual TQuery And(IDetachedImmutableFlowQuery subquery, IsEmptyExpression expression)
        {
            return Where(subquery, expression);
        }

        /// <inheritdoc />
        public virtual TQuery And(DetachedCriteria subquery, IsEmptyExpression expression)
        {
            return Where(subquery, expression);
        }

        /// <inheritdoc />
        public virtual TQuery And(params ICriterion[] criterions)
        {
            return Where(criterions);
        }

        /// <inheritdoc />
        public virtual TQuery And(string property, IsExpression expression)
        {
            return Where(property, expression);
        }

        /// <inheritdoc />
        public virtual TQuery And(Expression<Func<TSource, bool>> expression)
        {
            return Where(expression);
        }

        /// <inheritdoc />
        public virtual TQuery And(Expression<Func<TSource, object>> property, IsExpression expression)
        {
            return Where(property, expression);
        }

        /// <inheritdoc />
        public virtual TQuery And(Expression<Func<TSource, WhereDelegate, bool>> expression)
        {
            return Where(expression);
        }

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
        public virtual TQuery Cacheable(string cacheRegion, CacheMode cacheMode)
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
        public virtual IFetchBuilder<TSource, TQuery> Fetch(string path)
        {
            return FetchCore(path, path);
        }

        /// <inheritdoc />
        public virtual IFetchBuilder<TSource, TQuery> Fetch
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
                        property.Parameters[0].Name,
                        Data
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
        public virtual TQuery Limit(int limit, int offset)
        {
            return Take(limit).Skip(offset);
        }

        /// <inheritdoc />
        public virtual ILockBuilder<TSource, TQuery> Lock()
        {
            return Lock(null as string);
        }

        /// <inheritdoc />
        public virtual ILockBuilder<TSource, TQuery> Lock(Expression<Func<object>> alias)
        {
            return Lock(ExpressionHelper.GetPropertyName(alias));
        }

        /// <inheritdoc />
        public virtual ILockBuilder<TSource, TQuery> Lock(string alias)
        {
            return new LockBuilder<TSource, TQuery>(this, Query, alias);
        }

        /// <inheritdoc />
        public virtual TQuery OrderBy(string property, bool ascending = true)
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
        public virtual TQuery OrderBy(IProjection projection, bool ascending = true)
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
        public virtual TQuery OrderBy(Expression<Func<TSource, object>> property, bool ascending = true)
        {
            IProjection projection = ProjectionHelper.GetProjection(property.Body, property.Parameters[0].Name, Data);

            return OrderBy(projection, ascending);
        }

        /// <inheritdoc />
        /// <typeparam name="TProjection">
        ///     The <see cref="System.Type" /> of the projection.
        /// </typeparam>
        public virtual TQuery OrderBy<TProjection>(string property, bool ascending = true)
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
                ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name),
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
        public virtual TQuery RestrictByExample(TSource exampleInstance, Action<IExampleWrapper<TSource>> example)
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

        /// <inheritdoc />
        public virtual TQuery Where(params ICriterion[] criterions)
        {
            if (criterions == null)
            {
                throw new ArgumentNullException("criterions");
            }

            foreach (ICriterion criterion in criterions.Where(x => x != null))
            {
                Criterions.Add(criterion);
            }

            return Query;
        }

        /// <inheritdoc />
        public virtual TQuery Where(string property, IsExpression expression)
        {
            ICriterion criterion = expression.Compile(property);

            return Where(criterion);
        }

        /// <inheritdoc />
        public virtual TQuery Where(Expression<Func<TSource, bool>> expression)
        {
            return Where(RestrictionHelper.GetCriterion(expression, expression.Parameters[0].Name, Data));
        }

        /// <inheritdoc />
        public virtual TQuery Where(Expression<Func<TSource, object>> property, IsExpression expression)
        {
            return Where(ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name), expression);
        }

        /// <inheritdoc />
        public virtual TQuery Where(Expression<Func<TSource, WhereDelegate, bool>> expression)
        {
            return Where(RestrictionHelper.GetCriterion(expression, expression.Parameters[0].Name, Data));
        }

        /// <inheritdoc />
        public virtual TQuery Where(IDetachedImmutableFlowQuery subquery, IsEmptyExpression expression)
        {
            return Where(subquery.Criteria, expression);
        }

        /// <inheritdoc />
        public virtual TQuery Where(DetachedCriteria subquery, IsEmptyExpression expression)
        {
            ICriterion criterion = expression.Compile(subquery);

            return Where(criterion);
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
        protected virtual TQuery Cacheable(bool isCacheable, CacheMode? cacheMode, string cacheRegion)
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
        ///     A <see cref="IFetchBuilder{TSource, TQuery}" /> instance.
        /// </returns>
        protected virtual IFetchBuilder<TSource, TQuery> FetchCore(string path, string alias)
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

            return new FetchBuilder<TSource, TQuery>(this, Query, path, alias);
        }
    }
}