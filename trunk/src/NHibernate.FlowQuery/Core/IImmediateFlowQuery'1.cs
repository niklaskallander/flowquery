using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Expressions;

namespace NHibernate.FlowQuery.Core
{
    public interface IImmediateFlowQuery<TSource> : IFlowQuery<TSource, IImmediateFlowQuery<TSource>>, IQueryableFlowQuery<TSource>
        where TSource : class
    {
        bool Any();

        bool Any(params ICriterion[] criterions);

        bool Any(string property, IsExpression expression);

        bool Any(Expression<Func<TSource, bool>> expression);

        bool Any(Expression<Func<TSource, object>> property, IsExpression expression);

        bool Any(Expression<Func<TSource, WhereDelegate, bool>> expression);

        int Count();

        int Count(string property);

        int Count(IProjection projection);

        int Count(Expression<Func<TSource, object>> property);

        long CountLong();

        IDelayedFlowQuery<TSource> Delayed();

        IDetachedFlowQuery<TSource> Detached();

        IImmediateFlowQuery<TSource> Distinct();

        IImmediateFlowQuery<TSource> Indistinct();

        Dictionary<TKey, TValue> SelectDictionary<TKey, TValue>(Expression<Func<TSource, TKey>> key, Expression<Func<TSource, TValue>> value);
    }
}