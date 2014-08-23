namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Linq;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core.CustomProjections;
    using NHibernate.FlowQuery.Core.Structures;

    /// <summary>
    ///     A class providing utilities required to build <see cref="ICriteria" /> and <see cref="DetachedCriteria" />
    ///     instances from <see cref="NHibernate.FlowQuery" /> queries.
    /// </summary>
    public class CriteriaBuilder : ICriteriaBuilder
    {
        /// <summary>
        ///     Builds a <see cref="ICriteria" /> from the given <see cref="IQueryableFlowQuery" /> query.
        /// </summary>
        /// <param name="query">
        ///     The query from which to build a <see cref="ICriteria" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the underlying entity for the given query.
        /// </typeparam>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the result.
        /// </typeparam>
        /// <returns>
        ///     The built <see cref="ICriteria" />.
        /// </returns>
        public virtual ICriteria Build<TSource, TDestination>(IQueryableFlowQuery query)
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
                .Select(x => new
                {
                    x.Path,
                    x.FetchMode
                })
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

            foreach (Lock queryLock in query.Locks)
            {
                if (queryLock.Alias == null)
                {
                    criteria.SetLockMode(queryLock.LockMode);
                }
                else
                {
                    criteria.SetLockMode(queryLock.Alias, queryLock.LockMode);
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
                bool suppressErrors = FlowQueryOptions.GloballySuppressOrderByProjectionErrors
                    || (query.Options != null && query.Options.ShouldSuppressOrderByProjectionErrors);

                foreach (OrderByStatement statement in query.Orders)
                {
                    if (statement.IsBasedOnSource)
                    {
                        criteria.AddOrder(statement.Order);
                    }
                    else if (query.Mappings != null)
                    {
                        if (statement.ProjectionSourceType == typeof(TDestination))
                        {
                            string mappingKey = query.Mappings.Keys
                                .SingleOrDefault
                                (
                                    x => string
                                        .Equals(x, statement.Property, StringComparison.InvariantCultureIgnoreCase)
                                );

                            if (mappingKey != null)
                            {
                                criteria.AddOrder(new Order(mappingKey, statement.OrderAscending));
                            }
                            else if (!suppressErrors)
                            {
                                throw new InvalidOperationException
                                (
                                    "unable to order by a projection property that is not projected into by the query"
                                );
                            }
                        }
                        else if (!suppressErrors)
                        {
                            throw new InvalidOperationException
                            (
                                "unable to order by a projection property on a projection of a type other than the " +
                                    "returned one."
                            );
                        }
                    }
                }
            }

            return criteria;
        }

        /// <summary>
        ///     Builds a <see cref="DetachedCriteria" /> from the given <see cref="IMorphableFlowQuery" /> query.
        /// </summary>
        /// <param name="query">
        ///     The query from which to build a <see cref="ICriteria" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the underlying entity for the given query.
        /// </typeparam>
        /// <returns>
        ///     The built <see cref="DetachedCriteria" />.
        /// </returns>
        public virtual DetachedCriteria Build<TSource>(IMorphableFlowQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            DetachedCriteria criteria = DetachedCriteria.For<TSource>(query.Alias);

            foreach (Join join in query.Joins)
            {
                criteria.CreateAlias(join.Property, join.Alias, join.JoinType, join.WithClause);
            }

            foreach (ICriterion criterion in query.Criterions)
            {
                criteria.Add(criterion);
            }

            foreach (Lock queryLock in query.Locks)
            {
                if (queryLock.Alias == null)
                {
                    criteria.SetLockMode(queryLock.LockMode);
                }
                else
                {
                    criteria.SetLockMode(queryLock.Alias, queryLock.LockMode);
                }
            }

            bool skips = query.ResultsToSkip.HasValue && query.ResultsToSkip.Value > 0;

            if (skips)
            {
                criteria.SetFirstResult(query.ResultsToSkip.Value);
            }

            bool takes = query.ResultsToTake.HasValue && query.ResultsToTake.Value > 0;

            if (takes)
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

            criteria
                .SetProjection
                (
                    GetProjection(query)
                );

            if (query.ResultTransformer != null)
            {
                criteria.SetResultTransformer(query.ResultTransformer);
            }

            if (query.Orders.Count > 0 && (skips || takes))
            {
                foreach (OrderByStatement statement in query.Orders)
                {
                    if (statement.IsBasedOnSource)
                    {
                        criteria.AddOrder(statement.Order);
                    }
                }
            }

            return criteria;
        }

        /// <summary>
        ///     Attempts to resolve the <see cref="IProjection" /> representation for the provided
        ///     <see cref="IMorphableFlowQuery" /> instance.
        /// </summary>
        /// <param name="query">
        ///     The <see cref="IMorphableFlowQuery" /> instance.
        /// </param>
        /// <returns>
        ///     The resolved <see cref="IProjection" /> representation.
        /// </returns>
        protected virtual IProjection GetProjection(IMorphableFlowQuery query)
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

                foreach (FqGroupByProjection projection in query.GroupBys)
                {
                    newList.Add(projection);
                }

                queryProjection = newList;
            }

            return query.IsDistinct
                ? Projections.Distinct(queryProjection)
                : queryProjection;
        }
    }
}