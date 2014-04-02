using System;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery
{
    public static class Aggregate
    {
        public static double Average<TDestination>(TDestination property)
        {
            throw Exception();
        }

        public static double Average<TDestination>(TDestination? property)
            where TDestination : struct
        {
            throw Exception();
        }

        public static int Count<TDestination>(TDestination property)
        {
            throw Exception();
        }

        public static int Count<TDestination>(TDestination? property)
            where TDestination : struct
        {
            throw Exception();
        }

        public static int CountDistinct<TDestination>(TDestination property)
        {
            throw Exception();
        }

        public static int CountDistinct<TDestination>(TDestination? property)
            where TDestination : struct
        {
            throw Exception();
        }

        private static InvalidOperationException Exception()
        {
            return new InvalidOperationException("This is only a helper method for the NHibernate.FlowQuery API and should not be executed directly");
        }

        public static TDestination GroupBy<TDestination>(TDestination property)
        {
            throw Exception();
        }

        public static TDestination GroupBy<TDestination>(TDestination? property)
            where TDestination : struct
        {
            throw Exception();
        }

        public static TDestination Max<TDestination>(TDestination property)
        {
            throw Exception();
        }

        public static TDestination Max<TDestination>(TDestination? property)
            where TDestination : struct
        {
            throw Exception();
        }

        public static TDestination Min<TDestination>(TDestination property)
        {
            throw Exception();
        }

        public static TDestination Min<TDestination>(TDestination? property)
            where TDestination : struct
        {
            throw Exception();
        }

        public static TDestination Sum<TDestination>(TDestination property)
        {
            throw Exception();
        }

        public static TDestination Sum<TDestination>(TDestination? property)
            where TDestination : struct
        {
            throw Exception();
        }

        public static T Subquery<T>(IDetachedImmutableFlowQuery subquery)
        {
            throw Exception();
        }

        public static T Subquery<T>(DetachedCriteria subquery)
        {
            throw Exception();
        }
    }
}