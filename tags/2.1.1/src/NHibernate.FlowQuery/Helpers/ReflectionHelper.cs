using System.Collections.Generic;
using System.Reflection;

namespace NHibernate.FlowQuery.Helpers
{
    public static class ReflectionHelper
    {
        public static string[] GetNamesFromPublicToPublicTypeToTypeMappableProperties<TSource, TDestination>()
        {
            var propertyNames = new List<string>();

            var destinationType = typeof(TDestination);
            var sourceType = typeof(TSource);

            foreach (PropertyInfo destinationProperty in destinationType.GetProperties())
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
    }
}