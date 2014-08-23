namespace NHibernate.FlowQuery.Revealing.Conventions
{
    using System;

    /// <summary>
    ///     Lets you provide a custom convention delegate.
    /// </summary>
    /// <seealso cref="IRevealConvention" />
    /// <seealso cref="MConvention" />
    /// <seealso cref="MUnderscoreConvention" />
    /// <seealso cref="UnderscoreConvention" />
    public class CustomConvention : IRevealConvention
    {
        /// <summary>
        ///     The custom convention delegate.
        /// </summary>
        private readonly Func<string, string> _customConvention;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CustomConvention"/> class.
        /// </summary>
        /// <param name="customConvention">
        ///     The custom convention delegate.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="customConvention" /> is null.
        /// </exception>
        public CustomConvention(Func<string, string> customConvention)
        {
            if (customConvention == null)
            {
                throw new ArgumentNullException("customConvention");
            }

            _customConvention = customConvention;
        }

        /// <summary>
        ///     Returns the <paramref name="property" /> name after passing it through the given delegate.
        /// </summary>
        /// <inheritdoc />
        public virtual string RevealFrom(string property)
        {
            return _customConvention(property);
        }
    }
}