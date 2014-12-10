namespace NHibernate.FlowQuery.ExtensionHelpers
{
    using System;
    using System.Collections;

    using NHibernate.FlowQuery.Core;

    /// <summary>
    ///     A helper class defining a set of extension methods to create filters used in
    ///     <see cref="NHibernate.FlowQuery" /> queries.
    /// </summary>
    public static class IsExtensions
    {
        /// <summary>
        ///     Creates a "is between" filter on the specified property.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <param name="lowValue">
        ///     The low value to match.
        /// </param>
        /// <param name="highValue">
        ///     The high value to match.
        /// </param>
        /// <typeparam name="TProperty">
        ///     The <see cref="System.Type" /> of the specified property.
        /// </typeparam>
        /// <returns>
        ///     Nothing, as this method is not to be used directly and will throw an exception in such a case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If this method is used directly.
        /// </exception>
        public static bool IsBetween<TProperty>(this TProperty property, TProperty lowValue, TProperty highValue)
        {
            throw Exception();
        }

        /// <summary>
        ///     Creates an "is equal to" filter on the specified property.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <param name="value">
        ///     The value to match.
        /// </param>
        /// <typeparam name="TProperty">
        ///     The <see cref="System.Type" /> of the specified property.
        /// </typeparam>
        /// <returns>
        ///     Nothing, as this method is not to be used directly and will throw an exception in such a case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If this method is used directly.
        /// </exception>
        public static bool IsEqualTo<TProperty>(this TProperty property, TProperty value)
        {
            throw Exception();
        }

        /// <summary>
        ///     Creates an "is greater than" filter on the specified property.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <param name="value">
        ///     The value to match.
        /// </param>
        /// <typeparam name="TProperty">
        ///     The <see cref="System.Type" /> of the specified property.
        /// </typeparam>
        /// <returns>
        ///     Nothing, as this method is not to be used directly and will throw an exception in such a case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If this method is used directly.
        /// </exception>
        public static bool IsGreaterThan<TProperty>(this TProperty property, TProperty value)
        {
            throw Exception();
        }

        /// <summary>
        ///     Creates an "is greater than or equal to" filter on the specified property.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <param name="value">
        ///     The value to match.
        /// </param>
        /// <typeparam name="TProperty">
        ///     The <see cref="System.Type" /> of the specified property.
        /// </typeparam>
        /// <returns>
        ///     Nothing, as this method is not to be used directly and will throw an exception in such a case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If this method is used directly.
        /// </exception>
        public static bool IsGreaterThanOrEqualTo<TProperty>(this TProperty property, TProperty value)
        {
            throw Exception();
        }

        /// <summary>
        ///     Creates an "is in" filter on the specified property.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <param name="query">
        ///     The subquery yielding the results to match.
        /// </param>
        /// <typeparam name="TProperty">
        ///     The <see cref="System.Type" /> of the specified property.
        /// </typeparam>
        /// <returns>
        ///     Nothing, as this method is not to be used directly and will throw an exception in such a case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If this method is used directly.
        /// </exception>
        public static bool IsIn<TProperty>(this TProperty property, IDetachedImmutableFlowQuery query)
        {
            throw Exception();
        }

        /// <summary>
        ///     Creates an "is in" filter on the specified property.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <param name="values">
        ///     The values to match.
        /// </param>
        /// <typeparam name="TProperty">
        ///     The <see cref="System.Type" /> of the specified property.
        /// </typeparam>
        /// <returns>
        ///     Nothing, as this method is not to be used directly and will throw an exception in such a case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If this method is used directly.
        /// </exception>
        public static bool IsIn<TProperty>(this TProperty property, params TProperty[] values)
        {
            throw Exception();
        }

        /// <summary>
        ///     Creates an "is in" filter on the specified property.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <param name="values">
        ///     The values to match.
        /// </param>
        /// <typeparam name="TProperty">
        ///     The <see cref="System.Type" /> of the specified property.
        /// </typeparam>
        /// <returns>
        ///     Nothing, as this method is not to be used directly and will throw an exception in such a case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If this method is used directly.
        /// </exception>
        public static bool IsIn<TProperty>(this TProperty property, IEnumerable values)
        {
            throw Exception();
        }

        /// <summary>
        ///     Creates an "is less than" filter on the specified property.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <param name="value">
        ///     The value to match.
        /// </param>
        /// <typeparam name="TProperty">
        ///     The <see cref="System.Type" /> of the specified property.
        /// </typeparam>
        /// <returns>
        ///     Nothing, as this method is not to be used directly and will throw an exception in such a case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If this method is used directly.
        /// </exception>
        public static bool IsLessThan<TProperty>(this TProperty property, TProperty value)
        {
            throw Exception();
        }

        /// <summary>
        ///     Creates an "is less than or equal to" filter on the specified property.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <param name="value">
        ///     The value to match.
        /// </param>
        /// <typeparam name="TProperty">
        ///     The <see cref="System.Type" /> of the specified property.
        /// </typeparam>
        /// <returns>
        ///     Nothing, as this method is not to be used directly and will throw an exception in such a case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If this method is used directly.
        /// </exception>
        public static bool IsLessThanOrEqualTo<TProperty>(this TProperty property, TProperty value)
        {
            throw Exception();
        }

        /// <summary>
        ///     Creates an "is like" filter on the specified property.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <param name="value">
        ///     The value to match.
        /// </param>
        /// <typeparam name="TProperty">
        ///     The <see cref="System.Type" /> of the specified property.
        /// </typeparam>
        /// <returns>
        ///     Nothing, as this method is not to be used directly and will throw an exception in such a case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If this method is used directly.
        /// </exception>
        public static bool IsLike<TProperty>(this TProperty property, TProperty value)
        {
            throw Exception();
        }

        /// <summary>
        ///     Creates an "is not null" filter on the specified property.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <typeparam name="TProperty">
        ///     The <see cref="System.Type" /> of the specified property.
        /// </typeparam>
        /// <returns>
        ///     Nothing, as this method is not to be used directly and will throw an exception in such a case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If this method is used directly.
        /// </exception>
        public static bool IsNotNull<TProperty>(this TProperty property)
        {
            throw Exception();
        }

        /// <summary>
        ///     Creates an "is null" filter on the specified property.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <typeparam name="TProperty">
        ///     The <see cref="System.Type" /> of the specified property.
        /// </typeparam>
        /// <returns>
        ///     Nothing, as this method is not to be used directly and will throw an exception in such a case.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     If this method is used directly.
        /// </exception>
        public static bool IsNull<TProperty>(this TProperty property)
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