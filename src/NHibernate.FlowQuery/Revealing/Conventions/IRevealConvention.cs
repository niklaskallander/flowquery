namespace NHibernate.FlowQuery.Revealing.Conventions
{
    /// <summary>
    ///     An interface defining a utility to convert the name of a public member into the name of a private or
    ///     protected member.
    /// </summary>
    /// <seealso cref="CustomConvention" />
    /// <seealso cref="MConvention" />
    /// <seealso cref="MUnderscoreConvention" />
    /// <seealso cref="UnderscoreConvention" />
    public interface IRevealConvention
    {
        /// <summary>
        ///     Converts the provided, supposedly public member name into the name of a private or protected member
        ///     using the defined convention for this <see cref="IRevealConvention" /> instance.
        /// </summary>
        /// <param name="property">
        ///     The name of the public member to convert.
        /// </param>
        /// <returns>
        ///     The converted name.
        /// </returns>
        string RevealFrom(string property);
    }
}