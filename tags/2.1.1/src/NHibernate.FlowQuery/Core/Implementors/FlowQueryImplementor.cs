using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core.CustomProjections;
using NHibernate.FlowQuery.Core.Fetches;
using NHibernate.FlowQuery.Core.Joins;
using NHibernate.FlowQuery.Core.Locks;
using NHibernate.FlowQuery.Expressions;
using NHibernate.FlowQuery.Helpers;
using NHibernate.FlowQuery.Revealing.Conventions;
using NHibernate.Metadata;
using NHibernate.SqlCommand;

namespace NHibernate.FlowQuery.Core.Implementors
{
    public abstract class FlowQueryImplementor<TSource, TQuery> : IFlowQuery<TSource, TQuery>, IFlowQuery
        where TSource : class
        where TQuery : class, IFlowQuery<TSource, TQuery>
    {
        protected internal FlowQueryImplementor(Func<System.Type, string, ICriteria> criteriaFactory, Func<System.Type, IClassMetadata> metaDataFactory, string alias = null, FlowQueryOptions options = null, IFlowQuery query = null)
        {
            Query = this as TQuery;

            if (Query == null)
            {
                throw new ArgumentException("The provided TFlowQuery must the type of this instance");
            }

            if (criteriaFactory == null)
            {
                throw new ArgumentNullException("criteriaFactory");
            }

            if (metaDataFactory == null)
            {
                throw new ArgumentNullException("metaDataFactory");
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
            MetaDataFactory = metaDataFactory;

            Data = new QueryHelperData(Aliases, Joins, MetaDataFactory);

            Options = options;

            Inner = new JoinBuilder<TSource, TQuery>(this, Query, JoinType.InnerJoin);
            LeftOuter = new JoinBuilder<TSource, TQuery>(this, Query, JoinType.LeftOuterJoin);
            RightOuter = new JoinBuilder<TSource, TQuery>(this, Query, JoinType.RightOuterJoin);
            Full = new JoinBuilder<TSource, TQuery>(this, Query, JoinType.FullJoin);
        }

        public string Alias { get; private set; }

        public bool IsCacheable { get; private set; }

        public string CacheRegion { get; private set; }

        public CacheMode? CacheMode { get; private set; }

        public QueryHelperData Data { get; private set; }

        public FlowQueryOptions Options { get; private set; }

        public Func<System.Type, string, ICriteria> CriteriaFactory { get; private set; }

        public Func<System.Type, IClassMetadata> MetaDataFactory { get; private set; }

        protected internal TQuery Query { get; private set; }

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

        public virtual IFetchBuilder<TSource, TQuery> Fetch(string path)
        {
            return FetchCore(path, path);
        }

        public virtual IFetchBuilder<TSource, TQuery> Fetch(Expression<Func<TSource, object>> expression, Expression<Func<object>> aliasProjection = null, IRevealConvention revealConvention = null)
        {
            if (revealConvention == null)
            {
                revealConvention = Reveal.DefaultConvention ?? new CustomConvention(x => x);
            }

            string path = Reveal.ByConvention(expression, revealConvention);

            string alias = aliasProjection != null
                ? ExpressionHelper.GetPropertyName(aliasProjection)
                : path;

            return FetchCore(path, alias);
        }

        public virtual TQuery GroupBy(Expression<Func<TSource, object>> property)
        {
            if (property != null)
            {
                IProjection projection = ProjectionHelper.GetProjection(
                    property.Body,
                    property.Parameters[0].Name,
                    Data);

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

        public virtual ILockBuilder<TSource, TQuery> Lock()
        {
            return Lock(null as string);
        }

        public virtual ILockBuilder<TSource, TQuery> Lock(Expression<Func<object>> alias)
        {
            return Lock(ExpressionHelper.GetPropertyName(alias));
        }

        public virtual ILockBuilder<TSource, TQuery> Lock(string alias)
        {
            return new LockBuilder<TSource, TQuery>(this, Query, alias);
        }

        public virtual TQuery Where(params ICriterion[] criterions)
        {
            if (criterions == null)
            {
                throw new ArgumentNullException("criterions");
            }

            foreach (var criterion in criterions)
            {
                if (criterion != null)
                {
                    Criterions.Add(criterion);
                }
            }

            return Query;
        }

        public virtual TQuery Where(string property, IsExpression expression)
        {
            ICriterion criterion = expression.Compile(property);

            return Where(criterion);
        }

        public virtual TQuery Where(Expression<Func<TSource, bool>> expression)
        {
            return Where(RestrictionHelper.GetCriterion(expression, expression.Parameters[0].Name, Data));
        }

        public virtual TQuery Where(Expression<Func<TSource, object>> property, IsExpression expression)
        {
            return Where(ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name), expression);
        }

        public virtual TQuery Where(Expression<Func<TSource, WhereDelegate, bool>> expression)
        {
            return Where(RestrictionHelper.GetCriterion(expression, expression.Parameters[0].Name, Data));
        }

        public virtual TQuery Where(IDetachedImmutableFlowQuery subquery, Expressions.IsEmptyExpression expression)
        {
            return Where(subquery.Criteria, expression);
        }

        public virtual TQuery Where(DetachedCriteria subquery, Expressions.IsEmptyExpression expression)
        {
            ICriterion criterion = expression.Compile(subquery);

            return Where(criterion);
        }

        public virtual TQuery And(IDetachedImmutableFlowQuery subquery, Expressions.IsEmptyExpression expression)
        {
            return Where(subquery, expression);
        }

        public virtual TQuery And(DetachedCriteria subquery, Expressions.IsEmptyExpression expression)
        {
            return Where(subquery, expression);
        }

        protected virtual TQuery Cacheable(bool isCacheable, CacheMode? cacheMode, string cacheRegion)
        {
            IsCacheable = isCacheable;

            CacheMode = cacheMode;
            CacheRegion = cacheRegion;

            return Query;
        }

        public virtual TQuery Cacheable(bool isCacheable = true)
        {
            return Cacheable(isCacheable, null, null);
        }

        public virtual TQuery Cacheable(string cacheRegion)
        {
            return Cacheable(true, CacheMode, cacheRegion);
        }

        public virtual TQuery Cacheable(string cacheRegion, CacheMode cacheMode)
        {
            return Cacheable(true, cacheMode, cacheRegion);
        }

        public virtual TQuery Cacheable(CacheMode cacheMode)
        {
            return Cacheable(true, cacheMode, CacheRegion);
        }

        public virtual TQuery And(params ICriterion[] criterions)
        {
            return Where(criterions);
        }

        public virtual TQuery And(string property, IsExpression expression)
        {
            return Where(property, expression);
        }

        public virtual TQuery And(Expression<Func<TSource, bool>> expression)
        {
            return Where(expression);
        }

        public virtual TQuery And(Expression<Func<TSource, object>> property, IsExpression expression)
        {
            return Where(property, expression);
        }

        public virtual TQuery And(Expression<Func<TSource, WhereDelegate, bool>> expression)
        {
            return Where(expression);
        }

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

        public List<ICriterion> Criterions { get; private set; }

        public virtual IJoinBuilder<TSource, TQuery> Inner { get; private set; }

        public virtual IJoinBuilder<TSource, TQuery> LeftOuter { get; private set; }

        public virtual IJoinBuilder<TSource, TQuery> RightOuter { get; private set; }

        public virtual IJoinBuilder<TSource, TQuery> Full { get; private set; }

        public Dictionary<string, string> Aliases { get; private set; }

        public List<Fetch> Fetches { get; set; }

        public List<FqGroupByProjection> GroupBys { get; private set; }

        public List<Join> Joins { get; private set; }

        public List<Lock> Locks { get; private set; }

        public List<OrderByStatement> Orders { get; private set; }

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

        public virtual TQuery OrderByDescending(string property)
        {
            return OrderBy(property, false);
        }

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

        public virtual TQuery OrderByDescending(IProjection projection)
        {
            return OrderBy(projection, false);
        }

        public virtual TQuery OrderBy(Expression<Func<TSource, object>> property, bool ascending = true)
        {
            IProjection projection = ProjectionHelper.GetProjection(property.Body, property.Parameters[0].Name, Data);

            return OrderBy(projection, ascending);
        }

        public virtual TQuery OrderByDescending(Expression<Func<TSource, object>> property)
        {
            return OrderBy(property, false);
        }

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

        public virtual TQuery OrderByDescending<TProjection>(string property)
        {
            return OrderBy<TProjection>(property, false);
        }

        public virtual TQuery OrderBy<TProjection>(Expression<Func<TProjection, object>> projection, bool ascending = true)
        {
            return OrderBy<TProjection>(ExpressionHelper.GetPropertyName(projection.Body, projection.Parameters[0].Name), ascending);
        }

        public virtual TQuery OrderByDescending<TProjection>(Expression<Func<TProjection, object>> projection)
        {
            return OrderBy(projection, false);
        }

        public virtual TQuery Limit(int limit)
        {
            return Take(limit);
        }

        public virtual TQuery Limit(int limit, int offset)
        {
            return Take(limit).Skip(offset);
        }

        public virtual TQuery Skip(int skip)
        {
            ResultsToSkip = skip;

            return Query;
        }

        public virtual TQuery Take(int take)
        {
            ResultsToTake = take;

            return Query;
        }

        public virtual int? ResultsToSkip { get; private set; }

        public virtual int? ResultsToTake { get; private set; }

        public virtual TQuery ClearFetches()
        {
            Fetches.Clear();

            return Query;
        }

        public virtual TQuery ClearGroupBys()
        {
            GroupBys.Clear();

            return Query;
        }

        public virtual TQuery ClearJoins()
        {
            Joins.Clear();

            return Query;
        }

        public virtual TQuery ClearLimit()
        {
            ResultsToSkip = null;
            ResultsToTake = null;

            return Query;
        }

        public virtual TQuery ClearLocks()
        {
            Locks.Clear();

            return Query;
        }

        public virtual TQuery ClearOrders()
        {
            Orders.Clear();

            return Query;
        }

        public virtual TQuery ClearRestrictions()
        {
            Criterions.Clear();

            return Query;
        }
    }
}