using System;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Core.Joins;

namespace NHibernate.FlowQuery.Helpers
{
    public static class CriteriaHelper
    {
        public static DetachedCriteria BuildDetachedCriteria<TSource>(IDetachedFlowQuery<TSource> query)
            where TSource : class
        {
            IMorphableFlowQuery options = query as IMorphableFlowQuery;

            if (options == null)
            {
                return null;
            }

            DetachedCriteria criteria = DetachedCriteria.For<TSource>();

            foreach (Join join in options.Joins)
            {
                criteria.CreateAlias(join.Property, join.Alias, join.JoinType, join.WithClause);
            }

            foreach (ICriterion criterion in options.Criterions)
            {
                criteria.Add(criterion);
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

            criteria
                .SetProjection
                (
                    options.IsDistinct
                        ? Projections.Distinct(options.Projection)
                        : options.Projection
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

            foreach (Join join in query.Joins)
            {
                criteria.CreateAlias(join.Property, join.Alias, join.JoinType, join.WithClause);
            }

            foreach (ICriterion criterion in query.Criterions)
            {
                criteria.Add(criterion);
            }

            if (query.ResultsToSkip.HasValue)
            {
                criteria.SetFirstResult(query.ResultsToSkip.Value);
            }

            if (query.ResultsToTake.HasValue)
            {
                criteria.SetMaxResults(query.ResultsToTake.Value);
            }

            criteria
                .SetProjection
                (
                    query.IsDistinct
                        ? Projections.Distinct(query.Projection)
                        : query.Projection
                );

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

                        criteria.AddOrder(new Order(query.Mappings[statement.Property], statement.OrderAscending));
                    }
                }
            }

            return criteria;
        }
    }
}
