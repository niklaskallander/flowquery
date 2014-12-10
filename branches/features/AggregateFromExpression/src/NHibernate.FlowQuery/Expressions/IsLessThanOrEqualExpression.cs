namespace NHibernate.FlowQuery.Expressions
{
    using NHibernate.Criterion;

    /// <summary>
    ///     Represents a "is (not) less than or equal to" filter.
    /// </summary>
    public class IsLessThanOrEqualExpression : SimpleIsExpression
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsLessThanOrEqualExpression" /> class.
        /// </summary>
        /// <param name="value">
        ///     The value to be used by the filter.
        /// </param>
        public IsLessThanOrEqualExpression(object value)
            : base(value, Restrictions.Le)
        {
        }
    }
}