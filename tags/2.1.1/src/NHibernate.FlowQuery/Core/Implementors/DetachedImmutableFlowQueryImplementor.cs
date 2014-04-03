using System;
using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Core.Implementors
{
    public class DetachedImmutableFlowQueryImplementor : IDetachedImmutableFlowQuery
    {
        protected internal DetachedImmutableFlowQueryImplementor(DetachedCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException("criteria");
            }

            Criteria = criteria;
        }

        public virtual DetachedCriteria Criteria { get; private set; }
    }
}