using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace NHibernate.FlowQuery.Core.Joins
{
    public class Join
    {
        public virtual JoinType JoinType { get; set; }

        public virtual string Property { get; set; }

        public virtual string Alias { get; set; }

        public virtual ICriterion WithClause { get; set; }
    }
}