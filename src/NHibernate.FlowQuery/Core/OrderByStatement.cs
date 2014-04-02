using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Core
{
    public class OrderByStatement
    {
        public string Property { get; set; }

        public bool OrderAscending { get; set; }

        public bool IsBasedOnSource { get; set; }

        public System.Type ProjectionSourceType { get; set; }

        public Order Order { get; set; }
    }
}