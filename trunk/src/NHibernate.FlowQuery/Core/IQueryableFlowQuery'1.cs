using System;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core.Selection;

namespace NHibernate.FlowQuery.Core
{
    public interface IQueryableFlowQuery<TSource>
        where TSource : class
    {
        FlowQuerySelection<TSource> Select();

        ISelectSetup<TSource, TDestination> Select<TDestination>();

        FlowQuerySelection<TSource> Select(params string[] properties);

        FlowQuerySelection<TSource> Select(IProjection projection);

        FlowQuerySelection<object> Select(PropertyProjection projection);

        FlowQuerySelection<TDestination> Select<TDestination>(IProjection projection);

        FlowQuerySelection<TDestination> Select<TDestination>(PropertyProjection projection);

        FlowQuerySelection<TSource> Select(params Expression<Func<TSource, object>>[] properties);

        FlowQuerySelection<TDestination> Select<TDestination>(Expression<Func<TSource, TDestination>> expression);

        FlowQuerySelection<TDestination> Select<TDestination>(ISelectSetup<TSource, TDestination> setup);

        FlowQuerySelection<TDestination> Select<TDestination>(PartialSelection<TSource, TDestination> combiner);

        PartialSelection<TSource, TDestination> PartialSelect<TDestination>(Expression<Func<TSource, TDestination>> expression = null);
    }
}