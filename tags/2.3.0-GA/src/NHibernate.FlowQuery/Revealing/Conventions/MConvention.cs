namespace NHibernate.FlowQuery.Revealing.Conventions
{
    /// <summary>
    ///     Adds a "m" to the beginning of the provided string.
    /// </summary>
    /// <seealso cref="IRevealConvention" />
    /// <seealso cref="CustomConvention" />
    /// <seealso cref="MUnderscoreConvention" />
    /// <seealso cref="UnderscoreConvention" />
    public class MConvention : IRevealConvention
    {
        /// <summary>
        ///     Returns the <paramref name="property" /> name prefixed with "m".
        /// </summary>
        /// <inheritdoc />
        public virtual string RevealFrom(string property)
        {
            return string.Format("m{0}", property);
        }
    }
}