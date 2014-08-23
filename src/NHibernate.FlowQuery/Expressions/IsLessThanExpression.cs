namespace NHibernate.FlowQuery.Expressions
{
    using NHibernate.Criterion;

    /// <summary>
    ///     Represents a "is (not) less than" filter.
    /// </summary>
    public class IsLessThanExpression : SimpleIsExpression
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsLessThanExpression" /> class.
        /// </summary>
        /// <param name="value">
        ///     The value to be used by the filter.
        /// </param>
        public IsLessThanExpression(object value)
            : base(value, Restrictions.Lt)
        {
        }
    }
}