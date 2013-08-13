using System;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Expressions;

namespace NHibernate.FlowQuery.Core
{
    public partial interface ISubFlowQuery<TSource>
    {
        ISubFlowQuery<TSource> And(params ICriterion[] criterions);

        ISubFlowQuery<TSource> And(string property, IsExpression expression);

        ISubFlowQuery<TSource> And(Expression<Func<TSource, bool>> expression);

        ISubFlowQuery<TSource> And(Expression<Func<TSource, object>> property, IsExpression expression);

        ISubFlowQuery<TSource> Count();

        ISubFlowQuery<TSource> Count(string property);

        ISubFlowQuery<TSource> Count(IProjection projection);

        ISubFlowQuery<TSource> Count(Expression<Func<TSource, object>> property);

        ISubFlowQuery<TSource> CountDistinct(string property);

        ISubFlowQuery<TSource> CountDistinct(Expression<Func<TSource, object>> property);

        ISubFlowQuery<TSource> CountLong();

        ISubFlowQuery<TSource> Limit(int limit);

        ISubFlowQuery<TSource> Limit(int limit, int offset);

        ISubFlowQuery<TSource> OrderBy(string property);

        ISubFlowQuery<TSource> OrderBy(IProjection projection);

        ISubFlowQuery<TSource> OrderBy(Expression<Func<TSource, object>> property);

        ISubFlowQuery<TSource> OrderByDescending(string property);

        ISubFlowQuery<TSource> OrderByDescending(IProjection projection);

        ISubFlowQuery<TSource> OrderByDescending(Expression<Func<TSource, object>> property);

        ISubFlowQuery<TSource> Select(params string[] properties);

        ISubFlowQuery<TSource> Select(IProjection projection);

        ISubFlowQuery<TSource> Select(params Expression<Func<TSource, object>>[] expressions);

        ISubFlowQuery<TSource> SelectDistinct(IProjection projection);

        ISubFlowQuery<TSource> SelectDistinct(params string[] properties);

        ISubFlowQuery<TSource> SelectDistinct(params Expression<Func<TSource, object>>[] expressions);

        ISubFlowQuery<TSource> SetRootAlias<TRoot>(Expression<Func<TRoot>> rootAlias);

        ISubFlowQuery<TSource> Skip(int skip);

        ISubFlowQuery<TSource> Take(int take);

        ISubFlowQuery<TSource> Where(params ICriterion[] criterions);

        ISubFlowQuery<TSource> Where(string property, IsExpression expression);

        ISubFlowQuery<TSource> Where(Expression<Func<TSource, bool>> expression);

        ISubFlowQuery<TSource> Where(Expression<Func<TSource, object>> property, IsExpression expression);
    }
}