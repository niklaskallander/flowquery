using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Core
{
    public interface IDetachedImmutableFlowQuery
    {
        DetachedCriteria Criteria { get; }
    }
}