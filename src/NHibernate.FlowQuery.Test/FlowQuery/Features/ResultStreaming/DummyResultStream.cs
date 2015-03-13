namespace NHibernate.FlowQuery.Test.FlowQuery.Features.ResultStreaming
{
    using System;
    using System.Linq.Expressions;

    public static class DummyResultStream<TSource>
    {
        public static DummyResultStream<TSource, TDestination> CreateFrom<TDestination>
            (
            Expression<Func<TSource, TDestination>> expression
            )
        {
            return new DummyResultStream<TSource, TDestination>(expression);
        }
    }
}