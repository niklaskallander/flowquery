namespace NHibernate.FlowQuery.Core.Implementations
{
    /// <summary>
    ///     A delegate used to create a <see cref="FlowQuerySelection{TDestination}" /> instance from a
    ///     <see cref="ISelectSetup{TSource, TDestination}" /> instance.
    /// </summary>
    /// <param name="selectSetup">
    ///     The <see cref="ISelectSetup{TSource, TDestination}" /> instance.
    /// </param>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the source entity.
    /// </typeparam>
    /// <typeparam name="TDestination">
    ///     The <see cref="System.Type" /> of the result.
    /// </typeparam>
    /// <returns>
    ///     The created <see cref="FlowQuerySelection{TDestination}" /> instance.
    /// </returns>
    public delegate FlowQuerySelection<TDestination> SelectionBuilder<TSource, TDestination>
        (
        ISelectSetup<TSource, TDestination> selectSetup
        )
        where TSource : class;
}
