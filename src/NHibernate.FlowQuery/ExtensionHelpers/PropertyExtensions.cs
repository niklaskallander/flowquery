namespace NHibernate.FlowQuery.ExtensionHelpers
{
    using System;

    /// <summary>
    ///     A helper class defining a set of extension methods useful for using <see cref="string" /> property names
    ///     of non-public members in query projections and filters.
    /// </summary>
    public static class PropertyExtensions
    {
        /// <summary>
        ///     Used to use a <see cref="string" /> property name of  non-public member in a query projection or filter.
        /// </summary>
        /// <param name="property">
        ///     The property name of the non-public member.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the member.
        /// </typeparam>
        /// <returns>
        ///     Nothing, as this method is not to be used directly and will throw an exception in such a case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If this method is used directly.
        /// </exception>
        public static TDestination As<TDestination>(this string property)
        {
            throw new InvalidOperationException
            (
                "This is only a helper method for the NHibernate.FlowQuery API and should not be executed directly"
            );
        }
    }
}