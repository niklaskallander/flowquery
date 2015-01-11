namespace NHibernate.FlowQuery.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    ///     A static utility class providing methods to work with <see cref="System.Reflection" />.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        ///     Creates a list of all public properties of the same type with the same name that both 
        ///     <typeparamref name="TSource" /> and <typeparamref name="TDestination" /> have in common.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the result.
        /// </typeparam>
        /// <returns>
        ///     The list of properties.
        /// </returns>
        public static string[] GetNamesFromPublicToPublicTypeToTypeMappableProperties<TSource, TDestination>()
        {
            var propertyNames = new List<string>();

            Type destinationType = typeof(TDestination);
            Type sourceType = typeof(TSource);

            foreach (PropertyInfo destinationProperty in destinationType.GetProperties())
            {
                bool propertyWasFound = false;

                foreach (PropertyInfo sourceProperty in sourceType.GetProperties())
                {
                    if (sourceProperty.Name == destinationProperty.Name
                        && sourceProperty.PropertyType == destinationProperty.PropertyType)
                    {
                        propertyWasFound = true;

                        break;
                    }
                }

                if (propertyWasFound)
                {
                    if (!propertyNames.Contains(destinationProperty.Name))
                    {
                        propertyNames.Add(destinationProperty.Name);
                    }
                }
            }

            return propertyNames.ToArray();
        }
    }
}