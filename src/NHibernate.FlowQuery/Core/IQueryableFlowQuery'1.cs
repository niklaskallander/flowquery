namespace NHibernate.FlowQuery.Core
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core.Implementations;

    /// <summary>
    ///     An interface defining the basic functionality required of a projectable
    ///     <see cref="NHibernate.FlowQuery" /> query.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity to be queried.
    /// </typeparam>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the class or interface implementing or extending this interface.
    /// </typeparam>
    /// <seealso cref="IDelayedFlowQuery{TSource}" />
    /// <seealso cref="IImmediateFlowQuery{TSource}" />
    /// <seealso cref="IStreamedFlowQuery{TSource}" />
    public interface IQueryableFlowQuery<TSource, out TQuery> : IQueryableFlowQueryBase<TSource, TQuery>
        where TSource : class
        where TQuery : class, IQueryableFlowQueryBase<TSource, TQuery>
    {
        /// <summary>
        ///     Creates a selection from the query.
        /// </summary>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TSource}" /> instance.
        /// </returns>
        FlowQuerySelection<TSource> Select();

        /// <summary>
        ///     Creates a selection from the query using the specified properties.
        /// </summary>
        /// <param name="property">
        ///     The property to select.
        /// </param>
        /// <param name="properties">
        ///     Additional properties to select.
        /// </param>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TSource}" /> instance.
        /// </returns>
        FlowQuerySelection<TSource> Select
            (
            string property,
            params string[] properties
            );

        /// <summary>
        ///     Creates a selection from the query using the specified projection.
        /// </summary>
        /// <param name="projection">
        ///     The projection to select.
        /// </param>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TSource}" /> instance.
        /// </returns>
        FlowQuerySelection<TSource> Select(IProjection projection);

        /// <summary>
        ///     Creates a selection from the query using the specified properties.
        /// </summary>
        /// <param name="property">
        ///     The property to select.
        /// </param>
        /// <param name="properties">
        ///     Additional properties to select.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TDestination}" /> instance.
        /// </returns>
        FlowQuerySelection<TDestination> Select<TDestination>
            (
            string property,
            params string[] properties
            );

        /// <summary>
        ///     Creates a selection from the query using the specified projection.
        /// </summary>
        /// <param name="projection">
        ///     The projection to select.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TDestination}" /> instance.
        /// </returns>
        FlowQuerySelection<TDestination> Select<TDestination>(IProjection projection);

        /// <summary>
        ///     Creates a selection from the query using the specified properties.
        /// </summary>
        /// <param name="properties">
        ///     The properties to select.
        /// </param>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TSource}" /> instance.
        /// </returns>
        FlowQuerySelection<TSource> Select(params Expression<Func<TSource, object>>[] properties);

        /// <summary>
        ///     Creates a selection from the query using the specified projection.
        /// </summary>
        /// <param name="projection">
        ///     The projection to select.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TDestination}" /> instance.
        /// </returns>
        FlowQuerySelection<TDestination> Select<TDestination>(Expression<Func<TSource, TDestination>> projection);

        /// <summary>
        ///     Creates a selection from the specified <see cref="ISelectSetup{TSource, TDestination}" /> instance.
        /// </summary>
        /// <param name="setup">
        ///     The <see cref="ISelectSetup{TSource, TDestination}" /> instance from which to create a selection.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TDestination}" /> instance.
        /// </returns>
        FlowQuerySelection<TDestination> Select<TDestination>(ISelectSetup<TSource, TDestination> setup);

        /// <summary>
        ///     Creates a selection from the specified <see cref="PartialSelection{TSource, TDestination}" /> instance.
        /// </summary>
        /// <param name="combiner">
        ///     The <see cref="PartialSelection{TSource, TDestination}" /> instance from which to create a selection.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TDestination}" /> instance.
        /// </returns>
        FlowQuerySelection<TDestination> Select<TDestination>(IPartialSelection<TSource, TDestination> combiner);
    }
}