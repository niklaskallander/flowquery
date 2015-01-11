namespace NHibernate.FlowQuery.Expressions
{
    using NHibernate.Criterion;

    /// <summary>
    ///     Represents a "is (not) like" filter.
    /// </summary>
    public class IsLikeExpression : SimpleIsExpression
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsLikeExpression" /> class.
        /// </summary>
        /// <param name="value">
        ///     The value to be used by the filter.
        /// </param>
        public IsLikeExpression(object value)
            : base(value, Restrictions.Like)
        {
        }
    }
}