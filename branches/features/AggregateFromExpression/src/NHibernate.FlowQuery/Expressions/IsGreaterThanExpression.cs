namespace NHibernate.FlowQuery.Expressions
{
    using NHibernate.Criterion;

    /// <summary>
    ///     Represents a "is (not) greater than" filter.
    /// </summary>
    public class IsGreaterThanExpression : SimpleIsExpression
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsGreaterThanExpression"/> class.
        /// </summary>
        /// <param name="value">
        ///     The value to be used by the filter.
        /// </param>
        public IsGreaterThanExpression(object value)
            : base(value, Restrictions.Gt)
        {
        }
    }
}