using System.Collections;
using System.Collections.Generic;
using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsInValuesExpression : SimpleIsExpression
    {
        public IsInValuesExpression(IEnumerable value)
            : base(value)
        { }

        protected override ICriterion CompileCore(string property)
        {
            var collection = Value as ICollection;

            if (collection == null)
            {
                var enumerable = (IEnumerable)Value;

                var items = new List<object>();

                foreach (var item in enumerable)
                {
                    items.Add(item);
                }

                return Restrictions.In(Projections.Property(property), items.ToArray());
            }

            return Restrictions.In(Projections.Property(property), collection);
        }
    }
}