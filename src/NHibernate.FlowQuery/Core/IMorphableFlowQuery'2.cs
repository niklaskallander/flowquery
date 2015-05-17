namespace NHibernate.FlowQuery.Core
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    /// <summary>
    ///     An interface defining the basic functionality required of a transformable 
    ///     <see cref="NHibernate.FlowQuery" /> query.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity to be queried.
    /// </typeparam>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the class or interface implementing or extending this interface.
    /// </typeparam>
    /// <seealso cref="IDelayedFlowQuery{TSource}" />
    /// <seealso cref="IDetachedFlowQuery{TSource}" />
    /// <seealso cref="IDetachedImmutableFlowQuery" />
    /// <seealso cref="IImmediateFlowQuery{TSource}" />
    public interface IMorphableFlowQuery<TSource, out TQuery> : IFlowQuery<TSource, TQuery>
    {
        /// <summary>
        ///     Transform this query into a <see cref="IDelayedFlowQuery{TSource}" /> query instance.
        /// </summary>
        /// <returns>
        ///     A new <see cref="IDelayedFlowQuery{TSource}" /> instance created from this query.
        /// </returns>
        /// <remarks>
        ///     Execution of delayed queries will be deferred until the results are required by user code. This makes it
        ///     possible for <see cref="NHibernate" /> to batch multiple queries in one round-trip to the database 
        ///     instead of making one round-trip per query.
        /// </remarks>
        IDelayedFlowQuery<TSource> Delayed();

        /// <summary>
        ///     Transform this query into a <see cref="IDetachedFlowQuery{TSource}" /> query instance.
        /// </summary>
        /// <returns>
        ///     A new <see cref="IDetachedFlowQuery{TSource}" /> instance created from this query.
        /// </returns>
        IDetachedFlowQuery<TSource> Detached();

        /// <summary>
        ///     Specifies that any projections/selections on this query should be performed distinctly.
        /// </summary>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Distinct();

        /// <summary>
        ///     Transform this query into a <see cref="IImmediateFlowQuery{TSource}" /> query instance.
        /// </summary>
        /// <returns>
        ///     A new <see cref="IImmediateFlowQuery{TSource}" /> instance created from this query.
        /// </returns>
        /// <remarks>
        ///     Immediate queries are executed by <see cref="NHibernate" /> immediately (no pun intended), with a 
        ///     separate round-trip to the database for each query. To reduce the number of round-trips to the database 
        ///     you should use delayed queries instead (<see cref="Delayed()" />, 
        ///     <see cref="IDelayedFlowQuery{TSource}" />).
        /// </remarks>
        IImmediateFlowQuery<TSource> Immediate();

        /// <summary>
        ///     Specifies that any projections/selections on this query should be performed indistinctly.
        /// </summary>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Indistinct();

        /// <summary>
        ///     Specifies a list of properties to project.
        /// </summary>
        /// <param name="property">
        ///     The property to select.
        /// </param>
        /// <param name="properties">
        ///     Additional properties to select.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Project(string property, params string[] properties);

        /// <summary>
        ///     Specifies a projection.
        /// </summary>
        /// <param name="projection">
        ///     The projection.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Project(IProjection projection);

        /// <summary>
        ///     Specifies a list of properties to project.
        /// </summary>
        /// <param name="properties">
        ///     The properties to project.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Project(params Expression<Func<TSource, object>>[] properties);

        /// <summary>
        ///     Transform this query into a <see cref="IStreamedFlowQuery{TSource}" /> query instance.
        /// </summary>
        /// <returns>
        ///     A new <see cref="IStreamedFlowQuery{TSource}" /> instance created from this query.
        /// </returns>
        /// <remarks>
        ///     Streamed queries are executed by <see cref="NHibernate" /> immediately (just like 
        ///     <see cref="IImmediateFlowQuery{TSource}" />), with a separate round-trip to 
        ///     the database for each query. To reduce the number of round-trips to the database you should use delayed
        ///     queries instead (<see cref="Delayed()" />, <see cref="IDelayedFlowQuery{TSource}" />). The difference 
        ///     between a <see cref="IStreamedFlowQuery{TSource}" /> and a <see cref="IImmediateFlowQuery{TSource}" />
        ///     is that the entire result set for a <see cref="IStreamedFlowQuery{TSource}" /> never will be buffered 
        ///     into memory before it reaches user-code. It's entirely up to user-code to load it into memory, if it is
        ///     deemed necessary for the particular situation.
        /// </remarks>
        IStreamedFlowQuery<TSource> Streamed();
    }
}