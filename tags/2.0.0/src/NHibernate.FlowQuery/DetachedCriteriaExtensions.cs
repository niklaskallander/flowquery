using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Core.Implementors;

namespace NHibernate.FlowQuery
{
    public static class DetachedCriteriaExtensions
    {
        public static IDetachedImmutableFlowQuery DetachedFlowQuery(this DetachedCriteria criteria)
        {
            return new DetachedImmutableFlowQueryImplementor(criteria);
        }
    }
}