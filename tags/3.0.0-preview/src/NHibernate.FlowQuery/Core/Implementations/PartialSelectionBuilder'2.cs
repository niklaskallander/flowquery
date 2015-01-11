namespace NHibernate.FlowQuery.Core.Implementations
{
    /// <summary>
    ///     A delegate used to create a <see cref="FlowQuerySelection{TDestination}" /> instance from a
    ///     <see cref="IPartialSelection{TSource, TDestination}" /> instance.
    /// </summary>
    /// <param name="partialSelection">
    ///     The <see cref="IPartialSelection{TSource, TDestination}" /> instance.
    /// </param>
    /// <typeparam name="TSource">
    ///     The <see cref="System" /> of the source entity.
    /// </typeparam>
    /// <typeparam name="TDestination">
    ///     The <see cref="System.Type" /> of the result.
    /// </typeparam>
    /// <returns>
    ///     The created <see cref="FlowQuerySelection{TDestination}" /> instance.
    /// </returns>
    public delegate FlowQuerySelection<TDestination> PartialSelectionBuilder<TSource, TDestination>
        (
        IPartialSelection<TSource, TDestination> partialSelection
        )
        where TSource : class;
}