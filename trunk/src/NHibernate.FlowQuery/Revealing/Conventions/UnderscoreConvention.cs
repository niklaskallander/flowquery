namespace NHibernate.FlowQuery.Revealing.Conventions
{
    /// <summary>
    ///     Adds "_" to the beginning of the provided string.
    /// </summary>
    /// <seealso cref="IRevealConvention" />
    /// <seealso cref="CustomConvention" />
    /// <seealso cref="MConvention" />
    /// <seealso cref="MUnderscoreConvention" />
    public class UnderscoreConvention : IRevealConvention
    {
        /// <summary>
        ///     Returns the <paramref name="property" /> name prefixed with "_".
        /// </summary>
        /// <inheritdoc />
        public virtual string RevealFrom(string property)
        {
            return string.Format("_{0}", property);
        }
    }
}