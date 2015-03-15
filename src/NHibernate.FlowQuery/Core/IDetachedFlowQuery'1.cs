namespace NHibernate.FlowQuery.Core
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    /// <summary>
    ///     An interface defining the functionality required of a detached query (a.k.a. subquery).
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying data for this query.
    /// </typeparam>
    /// <seealso cref="IDelayedFlowQuery{TSource}" />
    /// <seealso cref="IDetachedImmutableFlowQuery" />
    /// <seealso cref="IImmediateFlowQuery{TSource}" />
    public interface IDetachedFlowQuery<TSource>
        : IFlowQuery<TSource, IDetachedFlowQuery<TSource>>, IDetachedImmutableFlowQuery
        where TSource : class
    {
        /// <summary>
        ///     Returns a copy of this <see cref="IDetachedFlowQuery{TSource}" /> instance.
        /// </summary>
        /// <returns>
        ///     A copy of this <see cref="IDetachedFlowQuery{TSource}" /> instance.
        /// </returns>
        IDetachedFlowQuery<TSource> Copy();

        /// <summary>
        ///     Specifies that this query should project the number of objects matching the filters specified for the
        ///     query.
        /// </summary>
        /// <returns>
        ///     This <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        IDetachedFlowQuery<TSource> Count();

        /// <summary>
        ///     Specifies that this query should project the number of objects matching the filters specified for the
        ///     query.
        /// </summary>
        /// <param name="property">
        ///     The property to count.
        /// </param>
        /// <returns>
        ///     This <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        IDetachedFlowQuery<TSource> Count(string property);

        /// <summary>
        ///     Specifies that this query should project the number of objects matching the filters specified for the
        ///     query.
        /// </summary>
        /// <param name="projection">
        ///     The projection to count.
        /// </param>
        /// <returns>
        ///     This <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        IDetachedFlowQuery<TSource> Count(IProjection projection);

        /// <summary>
        ///     Specifies that this query should project the number of objects matching the filters specified for the
        ///     query.
        /// </summary>
        /// <param name="property">
        ///     The property to count.
        /// </param>
        /// <returns>
        ///     This <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        IDetachedFlowQuery<TSource> Count(Expression<Func<TSource, object>> property);

        /// <summary>
        ///     Specifies that this query should project the number of objects matching the filters specified for the
        ///     query.
        /// </summary>
        /// <returns>
        ///     This <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        IDetachedFlowQuery<TSource> CountLong();

        /// <summary>
        ///     Returns a copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
        ///     <see cref="IDelayedFlowQuery{TSource}" /> instead.
        /// </summary>
        /// <returns>
        ///     A copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
        ///     <see cref="IDelayedFlowQuery{TSource}" /> instead.
        /// </returns>
        IDelayedFlowQuery<TSource> Delayed();

        /// <summary>
        ///     Returns a copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
        ///     <see cref="IDelayedFlowQuery{TSource}" /> instead.
        /// </summary>
        /// <param name="session">
        ///     The session.
        /// </param>
        /// <returns>
        ///     A copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
        ///     <see cref="IDelayedFlowQuery{TSource}" /> instead.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="session" /> is null.
        /// </exception>
        IDelayedFlowQuery<TSource> Delayed(ISession session);

        /// <summary>
        ///     Returns a copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
        ///     <see cref="IDelayedFlowQuery{TSource}" /> instead.
        /// </summary>
        /// <param name="session">
        ///     The session.
        /// </param>
        /// <returns>
        ///     A copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
        ///     <see cref="IDelayedFlowQuery{TSource}" /> instead.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="session" /> is null.
        /// </exception>
        IDelayedFlowQuery<TSource> Delayed(IStatelessSession session);

        /// <summary>
        ///     Specifies that any projections/selections on this query should be performed distinctly.
        /// </summary>
        /// <returns>
        ///     This <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        IDetachedFlowQuery<TSource> Distinct();

        /// <summary>
        ///     Returns a copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
        ///     <see cref="IImmediateFlowQuery{TSource}" /> instead.
        /// </summary>
        /// <returns>
        ///     A copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
        ///     <see cref="IImmediateFlowQuery{TSource}" /> instead.
        /// </returns>
        IImmediateFlowQuery<TSource> Immediate();

        /// <summary>
        ///     Returns a copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
        ///     <see cref="IImmediateFlowQuery{TSource}" /> instead.
        /// </summary>
        /// <param name="session">
        ///     The session.
        /// </param>
        /// <returns>
        ///     A copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
        ///     <see cref="IImmediateFlowQuery{TSource}" /> instead.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="session" /> is null.
        /// </exception>
        IImmediateFlowQuery<TSource> Immediate(ISession session);

        /// <summary>
        ///     Returns a copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
        ///     <see cref="IImmediateFlowQuery{TSource}" /> instead.
        /// </summary>
        /// <param name="session">
        ///     The session.
        /// </param>
        /// <returns>
        ///     A copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
        ///     <see cref="IImmediateFlowQuery{TSource}" /> instead.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="session" /> is null.
        /// </exception>
        IImmediateFlowQuery<TSource> Immediate(IStatelessSession session);

        /// <summary>
        ///     Specifies that any projections/selections on this query should be performed indistinctly.
        /// </summary>
        /// <returns>
        ///     This <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        IDetachedFlowQuery<TSource> Indistinct();

        /// <summary>
        ///     Specifies the property this query should project.
        /// </summary>
        /// <param name="property">
        ///     The property to project.
        /// </param>
        /// <returns>
        ///     This <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        IDetachedFlowQuery<TSource> Select(string property);

        /// <summary>
        ///     Specifies the projection this query should use.
        /// </summary>
        /// <param name="projection">
        ///     The projection to use.
        /// </param>
        /// <returns>
        ///     This <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        IDetachedFlowQuery<TSource> Select(IProjection projection);

        /// <summary>
        ///     Specifies the projection this query should use.
        /// </summary>
        /// <param name="expression">
        ///     The projection to use.
        /// </param>
        /// <returns>
        ///     This <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        IDetachedFlowQuery<TSource> Select(Expression<Func<TSource, object>> expression);

        /// <summary>
        ///     Sets the alias of the root entity of the main query for this detached query (a.k.a. subquery).
        /// </summary>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <typeparam name="TAlias">
        ///     The <see cref="System.Type" /> of the alias.
        /// </typeparam>
        /// <returns>
        ///     This <see cref="IDetachedFlowQuery{TSource}" /> query.
        /// </returns>
        IDetachedFlowQuery<TSource> SetRootAlias<TAlias>(Expression<Func<TAlias>> alias)
            where TAlias : class;

        /// <summary>
        ///     Returns a copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
        ///     <see cref="IStreamedFlowQuery{TSource}" /> instead.
        /// </summary>
        /// <returns>
        ///     A copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
        ///     <see cref="IStreamedFlowQuery{TSource}" /> instead.
        /// </returns>
        IStreamedFlowQuery<TSource> Streamed();

        /// <summary>
        ///     Returns a copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
        ///     <see cref="IStreamedFlowQuery{TSource}" /> instead.
        /// </summary>
        /// <param name="session">
        ///     The session.
        /// </param>
        /// <returns>
        ///     A copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
        ///     <see cref="IStreamedFlowQuery{TSource}" /> instead.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="session" /> is null.
        /// </exception>
        IStreamedFlowQuery<TSource> Streamed(ISession session);

        /// <summary>
        ///     Returns a copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
        ///     <see cref="IStreamedFlowQuery{TSource}" /> instead.
        /// </summary>
        /// <param name="session">
        ///     The session.
        /// </param>
        /// <returns>
        ///     A copy of this <see cref="IDetachedFlowQuery{TSource}" /> but in the form of a
        ///     <see cref="IStreamedFlowQuery{TSource}" /> instead.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="session" /> is null.
        /// </exception>
        IStreamedFlowQuery<TSource> Streamed(IStatelessSession session);
    }
}