namespace NHibernate.FlowQuery.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    ///     An interface defining the functionality required of a immediate query.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity for this query.
    /// </typeparam>
    /// <remarks>
    ///     Immediate queries are executed by <see cref="NHibernate" /> immediately (no pun intended), with a separate 
    ///     round-trip to the database for each query. To reduce the number of round-trips to the database you should
    ///     use delayed queries instead (<see cref="IDelayedFlowQuery{TSource}" />).
    /// </remarks>
    /// <seealso cref="IDelayedFlowQuery{TSource}" />
    /// <seealso cref="IDetachedFlowQuery{TSource}" />
    /// <seealso cref="IDetachedImmutableFlowQuery" />
    /// <seealso cref="IStreamedFlowQuery{TSource}" />
    public interface IImmediateFlowQuery<TSource>
        : IQueryableFlowQuery<TSource, IImmediateFlowQuery<TSource>>,
          IImmediateFlowQueryBase<TSource, IImmediateFlowQuery<TSource>>
        where TSource : class
    {
        /// <summary>
        ///     Returns a dictionary representing the objects in the underlying data source that matches the filters
        ///     specified for this query.
        /// </summary>
        /// <param name="key">
        ///     The projection to use for the keys in the returned dictionary.
        /// </param>
        /// <param name="value">
        ///     The projection to use for the values in the returned dictionary.
        /// </param>
        /// <typeparam name="TKey">
        ///     The <see cref="System.Type" /> of the keys.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The <see cref="System.Type" /> of the values.
        /// </typeparam>
        /// <returns>
        ///     The created dictionary.
        /// </returns>
        Dictionary<TKey, TValue> SelectDictionary<TKey, TValue>
            (
            Expression<Func<TSource, TKey>> key,
            Expression<Func<TSource, TValue>> value
            );

        /// <summary>
        ///     Transform this query into a <see cref="IStreamedFlowQuery{TSource}" /> query instance.
        /// </summary>
        /// <returns>
        ///     A new <see cref="IStreamedFlowQuery{TSource}" /> instance created from this query.
        /// </returns>
        /// <remarks>
        ///     Streamed queries are executed by <see cref="NHibernate" /> immediately (just like 
        ///     <see cref="IImmediateFlowQuery{TSource}" />), with a separate round-trip to the database for each query.
        ///     To reduce the number of round-trips to the database you should use delayed queries instead 
        ///     (<see cref="IDelayedFlowQuery{TSource}" />). The difference between a 
        ///     <see cref="IStreamedFlowQuery{TSource}" /> and a <see cref="IImmediateFlowQuery{TSource}" /> is that the
        ///     entire result set for a <see cref="IStreamedFlowQuery{TSource}" /> never will be buffered into memory 
        ///     before it reaches user-code. It's entirely up to user-code to load it into memory, if it is deemed 
        ///     necessary for the particular situation.
        /// </remarks>
        IStreamedFlowQuery<TSource> Streamed();
    }
}