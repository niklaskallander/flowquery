namespace NHibernate.FlowQuery.Core
{
    /// <summary>
    ///     Specifies a <see cref="IFilterableQuery{TSource,TQuery}" /> used to filter a query by an alias as temporary
    ///     query root.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the object to filter on.
    /// </typeparam>
    public interface IFilterableQuery<TSource> : IFilterableQuery<TSource, IFilterableQuery<TSource>>
    {
    }
}