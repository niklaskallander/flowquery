namespace NHibernate.FlowQuery.Expressions
{
    using System.Collections;
    using System.Collections.Generic;

    using NHibernate.Criterion;

    /// <summary>
    ///     Represents a "is (not) in" filter.
    /// </summary>
    public class IsInValuesExpression : SimpleIsExpression
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsInValuesExpression"/> class.
        /// </summary>
        /// <param name="value">
        ///     The value to be used by the filter.
        /// </param>
        public IsInValuesExpression(IEnumerable value)
            : base(value, null)
        {
        }

        /// <summary>
        ///     Compiles this <see cref="IsExpression" /> into a <see cref="ICriterion" /> instance.
        /// </summary>
        /// <param name="property">
        ///     The property to filter.
        /// </param>
        /// <returns>
        ///     The compiled <see cref="ICriterion" /> instance.
        /// </returns>
        protected override ICriterion CompileCore(string property)
        {
            var collection = Value as ICollection;

            if (collection == null)
            {
                var enumerable = (IEnumerable)Value;

                var items = new List<object>();

                foreach (object item in enumerable)
                {
                    items.Add(item);
                }

                return Restrictions.In(Projections.Property(property), items.ToArray());
            }

            return Restrictions.In(Projections.Property(property), collection);
        }
    }
}