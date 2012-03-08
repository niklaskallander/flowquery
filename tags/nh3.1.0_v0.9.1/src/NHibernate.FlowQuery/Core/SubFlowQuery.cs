using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Core
{
    public class SubFlowQuery
    {
        #region Properties (1)

        public virtual DetachedCriteria Criteria { get; set; }

        #endregion Properties
    }
}