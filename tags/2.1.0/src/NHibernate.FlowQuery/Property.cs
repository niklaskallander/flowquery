using System;

namespace NHibernate.FlowQuery
{
    public static class Property
    {
        public static TDestination As<TDestination>(string property)
        {
            throw new InvalidOperationException("This is only a helper method for the NHibernate.FlowQuery API and should not be executed directly");
        }
    }
}