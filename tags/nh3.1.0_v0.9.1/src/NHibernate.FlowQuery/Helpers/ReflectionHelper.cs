using System.Collections.Generic;

namespace NHibernate.FlowQuery.Helpers
{
    public static class ReflectionHelper
    {
        #region Methods (1)

        public static string[] GetNamesFromPublicToPublicTypeToTypeMappableProperties<TSource, TDestination>()
        {
            var propertyNames = new List<string>();

            var destinationType = typeof(TDestination);
            var sourceType = typeof(TSource);
            foreach (var destinationProperty in destinationType.GetProperties())
            {
                bool propertyWasFound = false;
                foreach (var sourceProperty in sourceType.GetProperties())
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

        #endregion Methods
    }
}