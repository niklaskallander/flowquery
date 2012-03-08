using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery
{
    public static class DetachedCriteriaExtensions
    {
		#region Methods (1) 

        public static ISubFlowQuery<TSource> SubQuery<TSource>(this DetachedCriteria criteria)
        {
            return new SubFlowQueryImpl<TSource>(criteria);
        }

		#endregion Methods 
    }
}