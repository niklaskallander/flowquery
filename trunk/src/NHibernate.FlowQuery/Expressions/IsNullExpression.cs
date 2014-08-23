namespace NHibernate.FlowQuery.Expressions
{
    using NHibernate.Criterion;

    /// <summary>
    ///     Represents a "is (not) null" filter.
    /// </summary>
    public class IsNullExpression : IsExpression
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
                return Restrictions.IsNotNull(Projections.Property(property));
            }

            return CompileCore(property);
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
            return Restrictions.IsNull(Projections.Property(property));
        }
    }
}