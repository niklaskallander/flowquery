namespace NHibernate.FlowQuery.Revealing.Conventions
{
    /// <summary>
    ///     Adds "m_" to the beginning of the provided string.
    /// </summary>
    /// <seealso cref="IRevealConvention" />
    /// <seealso cref="CustomConvention" />
    /// <seealso cref="MConvention" />
    /// <seealso cref="UnderscoreConvention" />
    public class MUnderscoreConvention : IRevealConvention
    {
        /// <summary>
        ///     Returns the <paramref name="property" /> name prefixed with "m_".
        /// </summary>
        /// <inheritdoc />
        public virtual string RevealFrom(string property)
        {
            return string.Format("m_{0}", property);
        }
    }
}