namespace NHibernate.FlowQuery.Expressions
{
    using NHibernate.Criterion;

    /// <summary>
    ///     Represents a "is (not) between" filter.
    /// </summary>
    public class IsBetweenExpression : IsExpression
    {
        /// <summary>
        ///     The high value.
        /// </summary>
        private readonly object _high;

        /// <summary>
        ///     The low value.
        /// </summary>
        private readonly object _low;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IsBetweenExpression" /> class.
        /// </summary>
        /// <param name="low">
        ///     The low value.
        /// </param>
        /// <param name="high">
        ///     The high value.
        /// </param>
        public IsBetweenExpression(object low, object high)
        {
            _low = low;
            _high = high;
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
            return Restrictions.Between(Projections.Property(property), _low, _high);
        }
    }
}