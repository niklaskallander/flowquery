using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery
{
    public static class SubQuery
    {
        #region Methods (1)

        public static ISubFlowQuery<TSource> For<TSource>()
        {
            return new SubFlowQueryImpl<TSource>(DetachedCriteria.For<TSource>());
        }

        #endregion Methods
    }
}