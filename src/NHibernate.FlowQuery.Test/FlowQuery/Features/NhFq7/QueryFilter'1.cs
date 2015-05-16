namespace NHibernate.FlowQuery.Test.FlowQuery.Features.NhFq7
{
    using System;

    using NHibernate.FlowQuery.Core;

    public class QueryFilter<T> : IQueryFilter<T>
    {
        private readonly Action<IFilterableQuery<T>> _filter;

        public QueryFilter(Action<IFilterableQuery<T>> filter)
        {
            _filter = filter;
        }

        public void Apply(IFilterableQuery<T> query)
        {
            _filter(query);
        }
    }
}