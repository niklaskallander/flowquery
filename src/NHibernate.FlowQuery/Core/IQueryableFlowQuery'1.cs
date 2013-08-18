using System;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core.SelectSetup;

namespace NHibernate.FlowQuery.Core
{
    public interface IQueryableFlowQuery<TSource>
        where TSource : class
    {
        #region Select

        FlowQuerySelection<TSource> Select();

        ISelectSetup<TSource, TReturn> Select<TReturn>();

        FlowQuerySelection<TSource> Select(params string[] properties);

        FlowQuerySelection<TSource> Select(IProjection projection);

        FlowQuerySelection<object> Select(PropertyProjection projection);

        FlowQuerySelection<TReturn> Select<TReturn>(IProjection projection);

        FlowQuerySelection<TReturn> Select<TReturn>(PropertyProjection projection);

        FlowQuerySelection<TSource> Select(params Expression<Func<TSource, object>>[] properties);

        FlowQuerySelection<TReturn> Select<TReturn>(Expression<Func<TSource, TReturn>> expression);

        FlowQuerySelection<TReturn> Select<TReturn>(ISelectSetup<TSource, TReturn> setup);

        #endregion
    }
}