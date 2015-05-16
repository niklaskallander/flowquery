namespace NHibernate.FlowQuery.Test.FlowQuery.Features.NhFq7
{
    using System;

    using NHibernate.FlowQuery.Core;

    public class NhFq7TestBase : BaseTest
    {
        protected static IQueryFilter<T> For<T>
            (
            Action<IFilterableQuery<T>, int> action,
            int value
            )
        {
            return new QueryFilter<T>(query => action(query, value));
        }
    }
}