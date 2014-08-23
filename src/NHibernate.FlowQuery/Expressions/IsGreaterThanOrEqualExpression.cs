namespace NHibernate.FlowQuery.Expressions
{
    using NHibernate.Criterion;

    /// <summary>
    ///     Represents a "is (not) greater than or equal to" filter.
    /// </summary>
    public class IsGreaterThanOrEqualExpression : SimpleIsExpression
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsGreaterThanOrEqualExpression" /> class.
        /// </summary>
        /// <param name="value">
        ///     The value to be used by the filter.
        /// </param>
        public IsGreaterThanOrEqualExpression(object value)
            : base(value, Restrictions.Ge)
        {
        }
    }
}