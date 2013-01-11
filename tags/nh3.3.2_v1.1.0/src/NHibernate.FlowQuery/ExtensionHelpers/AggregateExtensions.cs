using System;
namespace NHibernate.FlowQuery.ExtensionHelpers
{
    public static class AggregateExtensions
    {
		#region Methods (8) 

        public static decimal Average<TReturn>(this TReturn property)
        {
            throw Exception();
        }

        public static int Count<TReturn>(this TReturn property)
        {
            throw Exception();
        }

        public static int CountDistinct<TReturn>(this TReturn property)
        {
            throw Exception();
        }

        private static InvalidOperationException Exception()
        {
            return new InvalidOperationException("This is only a helper method for the NHibernate.FlowQuery API and should not be executed directly");
        }

        public static TReturn GroupBy<TReturn>(this TReturn property)
        {
            throw Exception();
        }

        public static TReturn Max<TReturn>(this TReturn property)
        {
            throw Exception();
        }

        public static TReturn Min<TReturn>(this TReturn property)
        {
            throw Exception();
        }

        public static TReturn Sum<TReturn>(this TReturn property)
        {
            throw Exception();
        }

		#endregion Methods 
    }
}