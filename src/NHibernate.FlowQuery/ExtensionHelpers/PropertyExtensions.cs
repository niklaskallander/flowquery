using System;

namespace NHibernate.FlowQuery.ExtensionHelpers
{
    public static class PropertyExtensions
    {
        public static TDestination As<TDestination>(this string property)
        {
            throw new InvalidOperationException("This is only a helper method for the NHibernate.FlowQuery API and should not be executed directly");
        }
    }
}