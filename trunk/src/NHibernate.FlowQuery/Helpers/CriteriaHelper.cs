using System;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Core.CustomProjections;
using NHibernate.FlowQuery.Core.Joins;
using NHibernate.FlowQuery.Core.Locks;

namespace NHibernate.FlowQuery.Helpers
{
    public static class CriteriaHelper
    {
        private static IProjection GetProjection(IMorphableFlowQuery query)
        {
            IProjection queryProjection = query.Projection;

            var projectionList = queryProjection as ProjectionList;

            bool isAggregate = query.GroupBys.Count > 0;

            if (projectionList != null || isAggregate)
            {
                var newList = new FqProjectionList();

                if (projectionList != null)
                {
                    for (int i = 0; i < projectionList.Length; i++)
                    {
                        IProjection projection = projectionList[i];

                        isAggregate = isAggregate || projection.IsAggregate || projection.IsGrouped;
                    }

                    for (int i = 0; i < projectionList.Length; i++)
                    {
                        IProjection projection = projectionList[i];

                        if (isAggregate)
                        {
                            if (!projection.IsAggregate && !projection.IsGrouped)
                            {
                                var alias = projection as FqAliasProjection;

                                if (alias == null || !(alias.Projection is SubqueryProjection))
                                {
                                    projection = Projections.GroupProperty(projection);
                                }
                            }
                        }

                        newList.Add(projection);
                    }
                }
                else
                {
                    newList.Add(query.Projection);
                }

                foreach (var projection in query.GroupBys)
                {
                    newList.Add(projection);
                }

                queryProjection = newList;
            }

            return query.IsDistinct
                ? Projections.Distinct(queryProjection)
                : queryProjection;
        }

        public static DetachedCriteria BuildDetachedCriteria<TSource>(IDetachedFlowQuery<TSource> query)
            where TSource : class
        {
            var options = query as IMorphableFlowQuery;

            if (options == null)
            {
                return null;
            }

            DetachedCriteria criteria = DetachedCriteria.For<TSource>(options.Alias);

            foreach (Join join in options.Joins)
            {
                criteria.CreateAlias(join.Property, join.Alias, join.JoinType, join.WithClause);
            }

            foreach (ICriterion criterion in options.Criterions)
            {
                criteria.Add(criterion);
            }

            foreach (Lock fqLock in options.Locks)
            {
                if (fqLock.Alias == null)
                {
                    criteria.SetLockMode(fqLock.LockMode);
                }
                else
                {
                    criteria.SetLockMode(fqLock.Alias, fqLock.LockMode);
                }
            }

            bool skips = options.ResultsToSkip.HasValue && options.ResultsToSkip.Value > 0;

            if (skips)
            {
                criteria.SetFirstResult(options.ResultsToSkip.Value);
            }

            bool takes = options.ResultsToTake.HasValue && options.ResultsToTake.Value > 0;

            if (takes)
            {
                criteria.SetMaxResults(options.ResultsToTake.Value);
            }

            if (options.IsCacheable)
            {
                criteria.SetCacheable(true);

                if (options.CacheMode.HasValue)
                {
                    criteria.SetCacheMode(options.CacheMode.Value);
                }

                if (!string.IsNullOrEmpty(options.CacheRegion))
                {
                    criteria.SetCacheRegion(options.CacheRegion);
                }
            }

            criteria
                .SetProjection
                (
                    GetProjection(options)
                );

            if (options.ResultTransformer != null)
            {
                criteria.SetResultTransformer(options.ResultTransformer);
            }

            if (options.Orders.Count > 0 && (skips || takes))
            {
                foreach (var statement in options.Orders)
                {
                    if (statement.IsBasedOnSource)
                    {
                        criteria.AddOrder(statement.Order);
                    }
                }
            }

            return criteria;
        }

        public static ICriteria BuildCriteria<TSource, TDestination>(QuerySelection query)
            where TSource : class
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            ICriteria criteria = query.CriteriaFactory(typeof(TSource), query.Alias);

            if (query.Options != null)
            {
                query.Options.Use(criteria);
            }

            // remove fetch duplicates created by aliasing
            var fetches = query.Fetches
                .Select
                (
                    x => new
                    {
                        x.Path,
                        x.FetchMode
                    }
                )
                .Distinct();

            foreach (var fetch in fetches)
            {
                criteria.SetFetchMode(fetch.Path, fetch.FetchMode);
            }

            foreach (Join join in query.Joins)
            {
                criteria.CreateAlias(join.Property, join.Alias, join.JoinType, join.WithClause);
            }

            foreach (ICriterion criterion in query.Criterions)
            {
                criteria.Add(criterion);
            }

            foreach (Lock fqLock in query.Locks)
            {
                if (fqLock.Alias == null)
                {
                    criteria.SetLockMode(fqLock.LockMode);
                }
                else
                {
                    criteria.SetLockMode(fqLock.Alias, fqLock.LockMode);
                }
            }

            if (query.ResultsToSkip.HasValue)
            {
                criteria.SetFirstResult(query.ResultsToSkip.Value);
            }

            if (query.ResultsToTake.HasValue)
            {
                criteria.SetMaxResults(query.ResultsToTake.Value);
            }

            if (query.IsCacheable)
            {
                criteria.SetCacheable(true);

                if (query.CacheMode.HasValue)
                {
                    criteria.SetCacheMode(query.CacheMode.Value);
                }

                if (!string.IsNullOrEmpty(query.CacheRegion))
                {
                    criteria.SetCacheRegion(query.CacheRegion);
                }
            }

            if (query.IsReadOnly.HasValue)
            {
                criteria.SetReadOnly(query.IsReadOnly.Value);
            }

            if (query.CommentValue != null)
            {
                criteria.SetComment(query.CommentValue);
            }

            if (query.FetchSizeValue > 0)
            {
                criteria.SetFetchSize(query.FetchSizeValue);
            }

            criteria
                .SetProjection
                (
                    GetProjection(query)
                );

            if (query.TimeoutValue.HasValue)
            {
                criteria.SetTimeout(query.TimeoutValue.Value);
            }

            if (query.ResultTransformer != null)
            {
                criteria.SetResultTransformer(query.ResultTransformer);
            }

            if (query.Orders.Count > 0)
            {
                foreach (var statement in query.Orders)
                {
                    if (statement.IsBasedOnSource)
                    {
                        criteria.AddOrder(statement.Order);
                    }
                    else if (query.Mappings != null)
                    {
                        if (statement.ProjectionSourceType != typeof(TDestination))
                        {
                            throw new InvalidOperationException("unable to order by a projection property on a projection of a type other than the returned one.");
                        }

                        if (!query.Mappings.ContainsKey(statement.Property))
                        {
                            throw new InvalidOperationException("unable to order by a projection property that is not projected into by the query");
                        }

                        criteria.AddOrder(new Order(statement.Property, statement.OrderAscending));
                    }
                }
            }

            return criteria;
        }
    }
}