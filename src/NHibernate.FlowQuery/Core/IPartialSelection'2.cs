namespace NHibernate.FlowQuery.Core
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Core.Implementations;

    /// <summary>
    ///     An interface defining the functionality and structure of a class used to combine multiple partial 
    ///     selections.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the source entity.
    /// </typeparam>
    /// <typeparam name="TDestination">
    ///     The <see cref="System.Type" /> of the selection.
    /// </typeparam>
    public interface IPartialSelection<TSource, TDestination>
    {
        /// <summary>
        ///     Gets the number of partial selections.
        /// </summary>
        /// <value>
        ///     The number of partial selections.
        /// </value>
        int Count { get; }

        /// <summary>
        ///     Adds a new partial selection.
        /// </summary>
        /// <param name="expression">
        ///     The partial selection.
        /// </param>
        /// <returns>
        ///     The <see cref="PartialSelection{TSource, TDestination}" /> instance.
        /// </returns>
        PartialSelection<TSource, TDestination> Add(Expression<Func<TSource, TDestination>> expression);

        /// <summary>
        ///     The compile all the partial selections into one expression.
        /// </summary>
        /// <returns>
        ///     The compiled expression instance.
        /// </returns>
        Expression<Func<TSource, TDestination>> Compile();

        /// <summary>
        ///     Creates a selection from this instance.
        /// </summary>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TDestination}" /> instance.
        /// </returns>
        FlowQuerySelection<TDestination> Select();
    }
}