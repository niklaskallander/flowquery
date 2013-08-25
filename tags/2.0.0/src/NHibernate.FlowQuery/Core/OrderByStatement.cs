using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Core
{
    public class OrderByStatement
    {
        public virtual string Property { get; set; }

        public virtual bool OrderAscending { get; set; }

        public virtual bool IsBasedOnSource { get; set; }

        public virtual System.Type ProjectionSourceType { get; set; }

        public virtual Order Order { get; set; }
    }
}