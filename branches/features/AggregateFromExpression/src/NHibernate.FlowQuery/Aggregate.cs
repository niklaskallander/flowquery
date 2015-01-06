namespace NHibernate.FlowQuery
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;

    /// <summary>
    ///     This helper class is used to make aggregated projections.
    /// </summary>
    public static class Aggregate
    {
        /// <summary>
        ///     Specifies an Average aggregation on the given property.
        /// </summary>
        /// <typeparam name="TDestination">
        ///     The type of the given property.
        /// </typeparam>
        /// <param name="property">
        ///     The property for which the aggregation should apply.
        /// </param>
        /// <returns>
        ///     The aggregated value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Will always throw an <see cref="InvalidOperationException" /> when called outside a
        ///     <see cref="System.Linq.Expressions.LambdaExpression" />.
        /// </exception>
        public static double Average<TDestination>(TDestination property)
        {
            throw Exception();
        }

        /// <summary>
        ///     Specifies an Average aggregation on the given property.
        /// </summary>
        /// <typeparam name="TDestination">
        ///     The type of the given property.
        /// </typeparam>
        /// <param name="property">
        ///     The property for which the aggregation should apply.
        /// </param>
        /// <returns>
        ///     The aggregated value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Will always throw a <see cref="InvalidOperationException" /> if called outside a
        ///     <see cref="System.Linq.Expressions.Expression{Func}" />.
        /// </exception>
        public static double Average<TDestination>(TDestination? property)
            where TDestination : struct
        {
            throw Exception();
        }

        /// <summary>
        ///     Specifies a Count aggregation on the given property.
        /// </summary>
        /// <typeparam name="TDestination">
        ///     The type of the given property.
        /// </typeparam>
        /// <param name="property">
        ///     The property for which the aggregation should apply.
        /// </param>
        /// <returns>
        ///     The aggregated value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Will always throw a <see cref="InvalidOperationException" /> if called outside a
        ///     <see cref="System.Linq.Expressions.Expression{Func}" />.
        /// </exception>
        public static int Count<TDestination>(TDestination property)
        {
            throw Exception();
        }

        /// <summary>
        ///     Specifies a Count aggregation on the given property.
        /// </summary>
        /// <typeparam name="TDestination">
        ///     The type of the given property.
        /// </typeparam>
        /// <param name="property">
        ///     The property for which the aggregation should apply.
        /// </param>
        /// <returns>
        ///     The aggregated value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Will always throw a <see cref="InvalidOperationException" /> if called outside a
        ///     <see cref="System.Linq.Expressions.Expression{Func}" />.
        /// </exception>
        public static int Count<TDestination>(TDestination? property)
            where TDestination : struct
        {
            throw Exception();
        }

        /// <summary>
        ///     Specifies a Distinct Count aggregation on the given property.
        /// </summary>
        /// <typeparam name="TDestination">
        ///     The type of the given property.
        /// </typeparam>
        /// <param name="property">
        ///     The property for which the aggregation should apply.
        /// </param>
        /// <returns>
        ///     The aggregated value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Will always throw a <see cref="InvalidOperationException" /> if called outside a
        ///     <see cref="System.Linq.Expressions.Expression{Func}" />.
        /// </exception>
        public static int CountDistinct<TDestination>(TDestination property)
        {
            throw Exception();
        }

        /// <summary>
        ///     Specifies a Distinct Count aggregation on the given property.
        /// </summary>
        /// <typeparam name="TDestination">
        ///     The type of the given property.
        /// </typeparam>
        /// <param name="property">
        ///     The property for which the aggregation should apply.
        /// </param>
        /// <returns>
        ///     The aggregated value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Will always throw a <see cref="InvalidOperationException" /> if called outside a
        ///     <see cref="System.Linq.Expressions.Expression{Func}" />.
        /// </exception>
        public static int CountDistinct<TDestination>(TDestination? property)
            where TDestination : struct
        {
            throw Exception();
        }

        /// <summary>
        ///     Provides a means to split up your projections into several expressions for re-use
        ///     and testability.
        /// </summary>
        /// <typeparam name="TIn">
        ///     The type of the root entity.
        /// </typeparam>
        /// <typeparam name="TOut">
        ///     The type of the returned object.
        /// </typeparam>
        /// <param name="expression">
        ///     The expression containing the projection.
        /// </param>
        /// <returns>
        ///     Nothing. Will always throw a <see cref="InvalidOperationException" /> if called
        ///     outside a <see cref="System.Linq.Expressions.Expression{Func}" />.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Will always throw a <see cref="InvalidOperationException" /> if called outside a
        ///     <see cref="System.Linq.Expressions.Expression{Func}" />.
        /// </exception>
        public static TOut FromExpression<TIn, TOut>(Expression<Func<TIn, TOut>> expression)
        {
            throw new Exception();
        }

        /// <summary>
        ///     Specifies a Group By aggregation on the given property.
        /// </summary>
        /// <typeparam name="TDestination">
        ///     The type of the given property.
        /// </typeparam>
        /// <param name="property">
        ///     The property for which the aggregation should apply.
        /// </param>
        /// <returns>
        ///     The aggregated value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Will always throw a <see cref="InvalidOperationException" /> if called outside a
        ///     <see cref="System.Linq.Expressions.Expression{Func}" />.
        /// </exception>
        public static TDestination GroupBy<TDestination>(TDestination property)
        {
            throw Exception();
        }

        /// <summary>
        ///     Specifies a Group By aggregation on the given property.
        /// </summary>
        /// <typeparam name="TDestination">
        ///     The type of the given property.
        /// </typeparam>
        /// <param name="property">
        ///     The property for which the aggregation should apply.
        /// </param>
        /// <returns>
        ///     The aggregated value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Will always throw a <see cref="InvalidOperationException" /> if called outside a
        ///     <see cref="System.Linq.Expressions.Expression{Func}" />.
        /// </exception>
        public static TDestination GroupBy<TDestination>(TDestination? property)
            where TDestination : struct
        {
            throw Exception();
        }

        /// <summary>
        ///     Specifies a Max aggregation on the given property.
        /// </summary>
        /// <typeparam name="TDestination">
        ///     The type of the given property.
        /// </typeparam>
        /// <param name="property">
        ///     The property for which the aggregation should apply.
        /// </param>
        /// <returns>
        ///     The aggregated value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Will always throw a <see cref="InvalidOperationException" /> if called outside a
        ///     <see cref="System.Linq.Expressions.Expression{Func}" />.
        /// </exception>
        public static TDestination Max<TDestination>(TDestination property)
        {
            throw Exception();
        }

        /// <summary>
        ///     Specifies a Max aggregation on the given property.
        /// </summary>
        /// <typeparam name="TDestination">
        ///     The type of the given property.
        /// </typeparam>
        /// <param name="property">
        ///     The property for which the aggregation should apply.
        /// </param>
        /// <returns>
        ///     The aggregated value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Will always throw a <see cref="InvalidOperationException" /> if called outside a
        ///     <see cref="System.Linq.Expressions.Expression{Func}" />.
        /// </exception>
        public static TDestination Max<TDestination>(TDestination? property)
            where TDestination : struct
        {
            throw Exception();
        }

        /// <summary>
        ///     Specifies a Min aggregation on the given property.
        /// </summary>
        /// <typeparam name="TDestination">
        ///     The type of the given property.
        /// </typeparam>
        /// <param name="property">
        ///     The property for which the aggregation should apply.
        /// </param>
        /// <returns>
        ///     The aggregated value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Will always throw a <see cref="InvalidOperationException" /> if called outside a
        ///     <see cref="System.Linq.Expressions.Expression{Func}" />.
        /// </exception>
        public static TDestination Min<TDestination>(TDestination property)
        {
            throw Exception();
        }

        /// <summary>
        ///     Specifies a Min aggregation on the given property.
        /// </summary>
        /// <typeparam name="TDestination">
        ///     The type of the given property.
        /// </typeparam>
        /// <param name="property">
        ///     The property for which the aggregation should apply.
        /// </param>
        /// <returns>
        ///     The aggregated value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Will always throw a <see cref="InvalidOperationException" /> if called outside a
        ///     <see cref="System.Linq.Expressions.Expression{Func}" />.
        /// </exception>
        public static TDestination Min<TDestination>(TDestination? property)
            where TDestination : struct
        {
            throw Exception();
        }

        /// <summary>
        ///     Specifies a Subquery aggregation.
        /// </summary>
        /// <typeparam name="T">
        ///     The return type of the given subquery.
        /// </typeparam>
        /// <param name="subquery">
        ///     The subquery for which the aggregation should apply.
        /// </param>
        /// <returns>
        ///     The aggregated value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Will always throw a <see cref="InvalidOperationException" /> if called outside a
        ///     <see cref="System.Linq.Expressions.Expression{Func}" />.
        /// </exception>
        public static T Subquery<T>(IDetachedImmutableFlowQuery subquery)
        {
            throw Exception();
        }

        /// <summary>
        ///     Specifies a Subquery aggregation.
        /// </summary>
        /// <typeparam name="T">
        ///     The return type of the given subquery.
        /// </typeparam>
        /// <param name="subquery">
        ///     The subquery for which the aggregation should apply.
        /// </param>
        /// <returns>
        ///     The aggregated value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Will always throw a <see cref="InvalidOperationException" /> if called outside a
        ///     <see cref="System.Linq.Expressions.Expression{Func}" />.
        /// </exception>
        public static T Subquery<T>(DetachedCriteria subquery)
        {
            throw Exception();
        }

        /// <summary>
        ///     Specifies a Sum aggregation on the given property.
        /// </summary>
        /// <typeparam name="TDestination">
        ///     The type of the given property.
        /// </typeparam>
        /// <param name="property">
        ///     The property for which the aggregation should apply.
        /// </param>
        /// <returns>
        ///     The aggregated value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Will always throw a <see cref="InvalidOperationException" /> if called outside a
        ///     <see cref="System.Linq.Expressions.Expression{Func}" />.
        /// </exception>
        public static TDestination Sum<TDestination>(TDestination property)
        {
            throw Exception();
        }

        /// <summary>
        ///     Specifies a Sum aggregation on the given property.
        /// </summary>
        /// <typeparam name="TDestination">
        ///     The type of the given property.
        /// </typeparam>
        /// <param name="property">
        ///     The property for which the aggregation should apply.
        /// </param>
        /// <returns>
        ///     The aggregated value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Will always throw a <see cref="InvalidOperationException" /> if called outside a
        ///     <see cref="System.Linq.Expressions.Expression{Func}" />.
        /// </exception>
        public static TDestination Sum<TDestination>(TDestination? property)
            where TDestination : struct
        {
            throw Exception();
        }

        /// <summary>
        ///     Creates a new <see cref="InvalidOperationException" />.
        /// </summary>
        /// <returns>The created <see cref="InvalidOperationException" />.</returns>
        private static InvalidOperationException Exception()
        {
            return new InvalidOperationException
                (
                "This is only a helper method for the NHibernate.FlowQuery API and should not be executed directly"
                );
        }
    }
}