using System;
using System.Linq.Expressions;
using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Core
{
    public interface IMorphableFlowQuery<TSource, TFlowQuery> : IFlowQuery<TSource, TFlowQuery>
        where TSource : class
        where TFlowQuery : class, IFlowQuery<TSource, TFlowQuery>
    {
        #region Projection

        TFlowQuery Distinct();

        TFlowQuery Indistinct();

        TFlowQuery Project(params string[] properties);

        TFlowQuery Project(IProjection projection);

        TFlowQuery Project(params Expression<Func<TSource, object>>[] properties);

        #endregion

        #region Alternations

        IDelayedFlowQuery<TSource> Delayed();

        IDetachedFlowQuery<TSource> Detached();

        IImmediateFlowQuery<TSource> Immediate();

        #endregion
    }
}