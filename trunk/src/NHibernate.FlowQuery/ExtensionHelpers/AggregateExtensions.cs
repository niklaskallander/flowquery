using System;
namespace NHibernate.FlowQuery.ExtensionHelpers
{
    public static class AggregateExtensions
    {
        #region Methods (8)

        public static decimal Average<TDestination>(this TDestination property)
        {
            throw Exception();
        }

        public static int Count<TDestination>(this TDestination property)
        {
            throw Exception();
        }

        public static int CountDistinct<TDestination>(this TDestination property)
        {
            throw Exception();
        }

        private static InvalidOperationException Exception()
        {
            return new InvalidOperationException("This is only a helper method for the NHibernate.FlowQuery API and should not be executed directly");
        }

        public static TDestination GroupBy<TDestination>(this TDestination property)
        {
            throw Exception();
        }

        public static TDestination Max<TDestination>(this TDestination property)
        {
            throw Exception();
        }

        public static TDestination Min<TDestination>(this TDestination property)
        {
            throw Exception();
        }

        public static TDestination Sum<TDestination>(this TDestination property)
        {
            throw Exception();
        }

        #endregion Methods
    }
}