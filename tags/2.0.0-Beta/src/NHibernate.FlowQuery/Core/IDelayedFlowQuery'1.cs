using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Expressions;

namespace NHibernate.FlowQuery.Core
{
    public interface IDelayedFlowQuery<TSource> : IFlowQuery<TSource, IDelayedFlowQuery<TSource>>, IQueryableFlowQuery<TSource>
        where TSource : class
    {
        Lazy<bool> Any();

        Lazy<bool> Any(params ICriterion[] criterions);

        Lazy<bool> Any(string property, IsExpression expression);

        Lazy<bool> Any(Expression<Func<TSource, bool>> expression);

        Lazy<bool> Any(Expression<Func<TSource, object>> property, IsExpression expression);

        Lazy<bool> Any(Expression<Func<TSource, WhereDelegate, bool>> expression);

        Lazy<int> Count();

        Lazy<int> Count(string property);

        Lazy<int> Count(IProjection projection);

        Lazy<int> Count(Expression<Func<TSource, object>> property);

        Lazy<long> CountLong();

        IDetachedFlowQuery<TSource> Detached();

        IDelayedFlowQuery<TSource> Distinct();

        IImmediateFlowQuery<TSource> Immediate();

        IDelayedFlowQuery<TSource> Indistinct();

        Lazy<Dictionary<TKey, TValue>> SelectDictionary<TKey, TValue>(Expression<Func<TSource, TKey>> key, Expression<Func<TSource, TValue>> value);
    }
}