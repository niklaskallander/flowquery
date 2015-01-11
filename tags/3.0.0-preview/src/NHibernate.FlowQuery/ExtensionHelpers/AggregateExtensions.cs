namespace NHibernate.FlowQuery.ExtensionHelpers
{
    using System;

    /// <summary>
    ///     A helper class defining a set of extension methods to create aggregation projections used in
    ///     <see cref="NHibernate.FlowQuery" /> queries.
    /// </summary>
    public static class AggregateExtensions
    {
        /// <summary>
        ///     Creates an average projection.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the property.
        /// </typeparam>
        /// <returns>
        ///     Nothing, as this method is not to be used directly and will throw an exception in such a case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If this method is used directly.
        /// </exception>
        public static double Average<TDestination>(this TDestination property)
        {
            throw Exception();
        }

        /// <summary>
        ///     Creates a count projection.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the property.
        /// </typeparam>
        /// <returns>
        ///     Nothing, as this method is not to be used directly and will throw an exception in such a case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If this method is used directly.
        /// </exception>
        public static int Count<TDestination>(this TDestination property)
        {
            throw Exception();
        }

        /// <summary>
        ///     Creates a distinct count projection.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the property.
        /// </typeparam>
        /// <returns>
        ///     Nothing, as this method is not to be used directly and will throw an exception in such a case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If this method is used directly.
        /// </exception>
        public static int CountDistinct<TDestination>(this TDestination property)
        {
            throw Exception();
        }

        /// <summary>
        ///     Creates a group by projection.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the property.
        /// </typeparam>
        /// <returns>
        ///     Nothing, as this method is not to be used directly and will throw an exception in such a case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If this method is used directly.
        /// </exception>
        public static TDestination GroupBy<TDestination>(this TDestination property)
        {
            throw Exception();
        }

        /// <summary>
        ///     Creates a max projection.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the property.
        /// </typeparam>
        /// <returns>
        ///     Nothing, as this method is not to be used directly and will throw an exception in such a case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If this method is used directly.
        /// </exception>
        public static TDestination Max<TDestination>(this TDestination property)
        {
            throw Exception();
        }

        /// <summary>
        ///     Creates a min projection.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the property.
        /// </typeparam>
        /// <returns>
        ///     Nothing, as this method is not to be used directly and will throw an exception in such a case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If this method is used directly.
        /// </exception>
        public static TDestination Min<TDestination>(this TDestination property)
        {
            throw Exception();
        }

        /// <summary>
        ///     Creates a sum projection.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the property.
        /// </typeparam>
        /// <returns>
        ///     Nothing, as this method is not to be used directly and will throw an exception in such a case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If this method is used directly.
        /// </exception>
        public static TDestination Sum<TDestination>(this TDestination property)
        {
            throw Exception();
        }

        /// <summary>
        ///     Generates an exception to throw in case any of the extension methods defined in this class are used
        ///     directly.
        /// </summary>
        /// <returns>
        ///     The generated <see cref="InvalidOperationException" /> instance.
        /// </returns>
        private static InvalidOperationException Exception()
        {
            return new InvalidOperationException
            (
                "This is only a helper method for the NHibernate.FlowQuery API and should not be executed directly"
            );
        }
    }
}