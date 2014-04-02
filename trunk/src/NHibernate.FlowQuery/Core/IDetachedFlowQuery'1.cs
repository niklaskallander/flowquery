using System;
using System.Linq.Expressions;
using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Core
{
    public interface IDetachedFlowQuery<TSource> : IFlowQuery<TSource, IDetachedFlowQuery<TSource>>, IDetachedImmutableFlowQuery
        where TSource : class
    {
        IDetachedFlowQuery<TSource> Count();

        IDetachedFlowQuery<TSource> Count(string property);

        IDetachedFlowQuery<TSource> Count(IProjection projection);

        IDetachedFlowQuery<TSource> Count(Expression<Func<TSource, object>> property);

        IDetachedFlowQuery<TSource> CountLong();

        IDetachedFlowQuery<TSource> Copy();

        IDelayedFlowQuery<TSource> Delayed();

        IDetachedFlowQuery<TSource> Distinct();

        IImmediateFlowQuery<TSource> Immediate();

        IDetachedFlowQuery<TSource> Indistinct();

        IDetachedFlowQuery<TSource> Select(string[] properties);

        IDetachedFlowQuery<TSource> Select(IProjection projection);

        IDetachedFlowQuery<TSource> Select(params Expression<Func<TSource, object>>[] expressions);

        IDetachedFlowQuery<TSource> SetRootAlias<TAlias>(Expression<Func<TAlias>> alias)
            where TAlias : class;
    }
}