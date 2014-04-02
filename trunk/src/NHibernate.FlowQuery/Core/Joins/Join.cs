using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace NHibernate.FlowQuery.Core.Joins
{
    public class Join
    {
        public JoinType JoinType { get; set; }

        public string Property { get; set; }

        public string Alias { get; set; }

        public ICriterion WithClause { get; set; }

        public bool IsCollection { get; set; }
    }
}