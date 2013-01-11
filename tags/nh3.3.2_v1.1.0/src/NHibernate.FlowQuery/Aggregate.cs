using System;

namespace NHibernate.FlowQuery
{
    public static class Aggregate
    {
        #region Methods (15)

        public static decimal Average<TReturn>(TReturn property)
        {
            throw Exception();
        }

        public static decimal Average<TReturn>(TReturn? property)
            where TReturn : struct
        {
            throw Exception();
        }

        public static int Count<TReturn>(TReturn property)
        {
            throw Exception();
        }

        public static int Count<TReturn>(TReturn? property)
            where TReturn : struct
        {
            throw Exception();
        }

        public static int CountDistinct<TReturn>(TReturn property)
        {
            throw Exception();
        }

        public static int CountDistinct<TReturn>(TReturn? property)
            where TReturn : struct
        {
            throw Exception();
        }

        private static InvalidOperationException Exception()
        {
            return new InvalidOperationException("This is only a helper method for the NHibernate.FlowQuery API and should not be executed directly");
        }

        public static TReturn GroupBy<TReturn>(TReturn property)
        {
            throw Exception();
        }

        public static TReturn GroupBy<TReturn>(TReturn? property)
            where TReturn : struct
        {
            throw Exception();
        }

        public static TReturn Max<TReturn>(TReturn property)
        {
            throw Exception();
        }

        public static TReturn Max<TReturn>(TReturn? property)
            where TReturn : struct
        {
            throw Exception();
        }

        public static TReturn Min<TReturn>(TReturn property)
        {
            throw Exception();
        }

        public static TReturn Min<TReturn>(TReturn? property)
            where TReturn : struct
        {
            throw Exception();
        }

        public static TReturn Sum<TReturn>(TReturn property)
        {
            throw Exception();
        }

        public static TReturn Sum<TReturn>(TReturn? property)
            where TReturn : struct
        {
            throw Exception();
        }

        #endregion Methods
    }
}