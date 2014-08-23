namespace NHibernate.FlowQuery.Core
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    /// <summary>
    ///     An interface defining the functionality and structure of a class handling the projection part of
    ///     per-property-mapping of a <see cref="NHibernate.FlowQuery" /> query selection.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the source entity.
    /// </typeparam>
    /// <typeparam name="TDestination">
    ///     The <see cref="System.Type" /> of the selection.
    /// </typeparam>
    public interface ISelectSetupPart<TSource, TDestination>
        where TSource : class
    {
        /// <summary>
        ///     Specifies a property on the source type to use for the destination type's property.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <returns>
        ///     The <see cref="ISelectSetup{TSource, TDestination}" /> instance.
        /// </returns>
        ISelectSetup<TSource, TDestination> Use(string property);

        /// <summary>
        ///     Specifies a projection on the source type to use for the destination type's property.
        /// </summary>
        /// <param name="projection">
        ///     The projection.
        /// </param>
        /// <returns>
        ///     The <see cref="ISelectSetup{TSource, TDestination}" /> instance.
        /// </returns>
        ISelectSetup<TSource, TDestination> Use(IProjection projection);

        /// <summary>
        ///     Specifies a projection on the source type to use for the destination type's property.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <typeparam name="TProjection">
        ///     The <see cref="System.Type" /> of the expression.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="ISelectSetup{TSource, TDestination}" /> instance.
        /// </returns>
        ISelectSetup<TSource, TDestination> Use<TProjection>(Expression<Func<TSource, TProjection>> expression);
    }
}