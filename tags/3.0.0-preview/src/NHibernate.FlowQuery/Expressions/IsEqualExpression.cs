namespace NHibernate.FlowQuery.Expressions
{
    using NHibernate.Criterion;

    /// <summary>
    ///     Represents a "is (not) equal to" filter.
    /// </summary>
    public class IsEqualExpression : SimpleIsExpression
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsEqualExpression"/> class.
        /// </summary>
        /// <param name="value">
        ///     The value to be used by the filter.
        /// </param>
        public IsEqualExpression(object value)
            : base(value, Restrictions.Eq)
        {
        }
    }
}