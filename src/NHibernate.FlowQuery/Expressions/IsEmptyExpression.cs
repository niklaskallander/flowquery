namespace NHibernate.FlowQuery.Expressions
{
    using NHibernate.Criterion;

    /// <summary>
    ///     Represents a "is (not) empty" filter.
    /// </summary>
    public class IsEmptyExpression : IsExpression
    {
        /// <summary>
        ///     Compiles this <see cref="IsExpression" /> into a <see cref="ICriterion" /> instance.
        /// </summary>
        /// <param name="property">
        ///     The property to filter.
        /// </param>
        /// <returns>
        ///     The compiled <see cref="ICriterion" /> instance.
        /// </returns>
        public override ICriterion Compile(string property)
        {
            if (Negated)
            {
                return Restrictions.IsNotEmpty(property);
            }

            return CompileCore(property);
        }

        /// <summary>
        ///     Compiles this <see cref="IsExpression" /> into a <see cref="ICriterion" /> instance.
        /// </summary>
        /// <param name="subquery">
        ///     The subquery to filter.
        /// </param>
        /// <returns>
        ///     The compiled <see cref="ICriterion" /> instance.
        /// </returns>
        public virtual ICriterion Compile(DetachedCriteria subquery)
        {
            if (Negated)
            {
                return Subqueries.Exists(subquery);
            }

            return Subqueries.NotExists(subquery);
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
            return Restrictions.IsEmpty(property);
        }
    }
}