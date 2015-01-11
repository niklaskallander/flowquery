namespace NHibernate.FlowQuery.Expressions
{
    using NHibernate.Criterion;

    /// <summary>
    ///     Provides the basic functionality for a query filter.
    /// </summary>
    public abstract class IsExpression
    {
        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="IsExpression" /> is negated.
        /// </summary>
        /// <value>
        ///     A value indicating whether this <see cref="IsExpression" /> is negated.
        /// </value>
        public virtual bool Negated { get; set; }

        /// <summary>
        ///     Compiles this <see cref="IsExpression" /> into a <see cref="ICriterion" /> instance.
        /// </summary>
        /// <param name="property">
        ///     The property to filter.
        /// </param>
        /// <returns>
        ///     The compiled <see cref="ICriterion" /> instance.
        /// </returns>
        public virtual ICriterion Compile(string property)
        {
            ICriterion compiled = CompileCore(property);

            if (Negated)
            {
                return Restrictions.Not(compiled);
            }

            return compiled;
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
        protected abstract ICriterion CompileCore(string property);
    }
}