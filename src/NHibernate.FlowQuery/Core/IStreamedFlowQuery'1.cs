namespace NHibernate.FlowQuery.Core
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core.Implementations;

    /// <summary>
    ///     An interface defining the functionality required of a streamed query.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity for this query.
    /// </typeparam>
    /// <remarks>
    ///     Execution of delayed queries will be deferred until the results are required by user code. This makes it
    ///     possible for <see cref="NHibernate" /> to batch multiple queries in one round-trip to the database instead
    ///     of making one round-trip per query.
    /// </remarks>
    /// <seealso cref="IDelayedFlowQuery{TSource}" />
    /// <seealso cref="IDetachedFlowQuery{TSource}" />
    /// <seealso cref="IDetachedImmutableFlowQuery" />
    /// <seealso cref="IImmediateFlowQuery{TSource}" />
    public interface IStreamedFlowQuery<TSource>
        : IImmediateFlowQueryBase<TSource, IStreamedFlowQuery<TSource>>
    {
        /// <summary>
        ///     Returns a copy of this <see cref="IStreamedFlowQuery{TSource}" /> but in the form of a
        ///     <see cref="IImmediateFlowQuery{TSource}" /> instead.
        /// </summary>
        /// <returns>
        ///     A copy of this <see cref="IStreamedFlowQuery{TSource}" /> but in the form of a
        ///     <see cref="IImmediateFlowQuery{TSource}" /> instead.
        /// </returns>
        IImmediateFlowQuery<TSource> Immediate();

        /// <summary>
        ///     Creates a selection from the query and streams it into the specified
        ///     <see cref="IResultStream{TSource}" /> stream.
        /// </summary>
        /// <param name="stream">
        ///     The <see cref="IResultStream{TSource}" /> stream.
        /// </param>
        /// <seealso cref="IResultStream{T}" />
        void Select(IResultStream<TSource> stream);

        /// <summary>
        ///     Creates a selection from the query using the specified properties and streams it into the specified
        ///     <see cref="IResultStream{TSource}" /> stream.
        /// </summary>
        /// <param name="stream">
        ///     The <see cref="IResultStream{TSource}" /> stream.
        /// </param>
        /// <param name="property">
        ///     The property to select.
        /// </param>
        /// <param name="properties">
        ///     Additional properties to select.
        /// </param>
        /// <seealso cref="IResultStream{T}" />
        void Select
            (
            IResultStream<TSource> stream,
            string property,
            params string[] properties
            );

        /// <summary>
        ///     Creates a selection from the query using the specified projection and streams it into the specified
        ///     <see cref="IResultStream{TSource}" /> stream.
        /// </summary>
        /// <param name="stream">
        ///     The <see cref="IResultStream{TSource}" /> stream.
        /// </param>
        /// <param name="projection">
        ///     The projection to select.
        /// </param>
        /// <seealso cref="IResultStream{T}" />
        void Select
            (
            IResultStream<TSource> stream,
            IProjection projection
            );

        /// <summary>
        ///     Creates a selection from the query using the specified properties and streams it into the specified
        ///     <see cref="IResultStream{TDestination}" /> stream.
        /// </summary>
        /// <param name="stream">
        ///     The <see cref="IResultStream{TDestination}" /> stream.
        /// </param>
        /// <param name="property">
        ///     The property to select.
        /// </param>
        /// <param name="properties">
        ///     Additional properties to select.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        /// <seealso cref="IResultStream{T}" />
        void Select<TDestination>
            (
            IResultStream<TDestination> stream,
            string property,
            params string[] properties
            );

        /// <summary>
        ///     Creates a selection from the query using the specified projection and streams it into the specified
        ///     <see cref="IResultStream{TDestination}" /> stream.
        /// </summary>
        /// <param name="stream">
        ///     The <see cref="IResultStream{TDestination}" /> stream.
        /// </param>
        /// <param name="projection">
        ///     The projection to select.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        /// <seealso cref="IResultStream{T}" />
        void Select<TDestination>
            (
            IResultStream<TDestination> stream,
            IProjection projection
            );

        /// <summary>
        ///     Creates a selection from the query using the specified properties and streams it into the specified
        ///     <see cref="IResultStream{TSource}" /> stream.
        /// </summary>
        /// <param name="stream">
        ///     The <see cref="IResultStream{TSource}" /> stream.
        /// </param>
        /// <param name="properties">
        ///     The properties to select.
        /// </param>
        /// <seealso cref="IResultStream{T}" />
        void Select
            (
            IResultStream<TSource> stream,
            params Expression<Func<TSource, object>>[] properties
            );

        /// <summary>
        ///     Creates a selection from the query using the specified projection and streams it into the specified
        ///     <see cref="IResultStream{TDestination}" /> stream.
        /// </summary>
        /// <param name="stream">
        ///     The <see cref="IResultStream{TDestination}" /> stream.
        /// </param>
        /// <param name="projection">
        ///     The projection to select.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        /// <seealso cref="IResultStream{T}" />
        void Select<TDestination>
            (
            IResultStream<TDestination> stream,
            Expression<Func<TSource, TDestination>> projection
            );

        /// <summary>
        ///     Creates a selection from the specified <see cref="ISelectSetup{TSource, TDestination}" /> instance and
        ///     streams it into the specified <see cref="IResultStream{TDestination}" /> stream.
        /// </summary>
        /// <param name="stream">
        ///     The <see cref="IResultStream{TDestination}" /> stream.
        /// </param>
        /// <param name="setup">
        ///     The <see cref="ISelectSetup{TSource, TDestination}" /> instance from which to create a selection.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        /// <seealso cref="IResultStream{T}" />
        void Select<TDestination>
            (
            IResultStream<TDestination> stream,
            ISelectSetup<TSource, TDestination> setup
            );

        /// <summary>
        ///     Creates a selection from the specified <see cref="PartialSelection{TSource,TDestination}" /> instance
        ///     and streams it into the specified <see cref="IResultStream{TDestination}" /> stream.
        /// </summary>
        /// <param name="stream">
        ///     The <see cref="IResultStream{TDestination}" /> stream.
        /// </param>
        /// <param name="combiner">
        ///     The <see cref="PartialSelection{TSource, TDestination}" /> instance from which to create a selection.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        /// <seealso cref="IResultStream{T}" />
        void Select<TDestination>
            (
            IResultStream<TDestination> stream,
            IPartialSelection<TSource, TDestination> combiner
            );
    }
}