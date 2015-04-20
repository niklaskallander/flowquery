namespace NHibernate.FlowQuery.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    /// <summary>
    ///     An interface defining the functionality and structure of a class handling per-property-mapping of a 
    ///     <see cref="NHibernate.FlowQuery" /> query selection.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the source entity.
    /// </typeparam>
    /// <typeparam name="TDestination">
    ///     The <see cref="System.Type" /> of the selection.
    /// </typeparam>
    public interface ISelectSetup<TSource, TDestination>
    {
        /// <summary>
        ///     Gets a dictionary containing all the mapped properties with their corresponding projections.
        /// </summary>
        /// <value>
        ///     A dictionary containing all the mapped properties with their corresponding projections.
        /// </value>
        Dictionary<string, IProjection> Mappings { get; }

        /// <summary>
        ///     Gets the projection list.
        /// </summary>
        /// <value>
        ///     The projection list.
        /// </value>
        ProjectionList ProjectionList { get; }

        /// <summary>
        ///     Creates a new property mapping.
        /// </summary>
        /// <param name="property">
        ///     The property to map.
        /// </param>
        /// <returns>
        ///     A <see cref="ISelectSetupPart{TSource, TDestination}"/> instance.
        /// </returns>
        ISelectSetupPart<TSource, TDestination> For(string property);

        /// <summary>
        ///     Creates a new property mapping.
        /// </summary>
        /// <param name="property">
        ///     The property to map.
        /// </param>
        /// <returns>
        ///     A <see cref="ISelectSetupPart{TSource, TDestination}"/> instance.
        /// </returns>
        ISelectSetupPart<TSource, TDestination> For(Expression<Func<TDestination, object>> property);

        /// <summary>
        ///     Creates a selection from this <see cref="ISelectSetup{TSource, TDestination}" /> instance.
        /// </summary>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TDestination}"/> instance.
        /// </returns>
        FlowQuerySelection<TDestination> Select();
    }
}