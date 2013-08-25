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

        public override ICriterion Compile(string property)
        {
            if (!(Value is ICollection))
            {
                List<object> objs = new List<object>();
                foreach (var obj in Value as IEnumerable)
                {
                    objs.Add(obj);
                }
                return Restrictions.In(Projections.Property(property), objs.ToArray());
            }
            return Restrictions.In(Projections.Property(property), Value as ICollection);
        }
    }
}