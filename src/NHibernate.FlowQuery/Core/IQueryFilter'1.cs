namespace NHibernate.FlowQuery.Core
{
    /// <summary>
    ///     Specifies a <see cref="IQueryFilter{T}" /> used to filter a query by an alias as temporary query root.
    /// </summary>
    /// <typeparam name="T">
    ///     The <see cref="System.Type" /> of the object to filter on.
    /// </typeparam>
    public interface IQueryFilter<T>
    {
        /// <summary>
        ///     Applies this <see cref="IQueryFilter{T}" /> to the given <see cref="IFilterableQuery{TSource}" />.
        /// </summary>
        /// <param name="query">
        ///     The <see cref="IFilterableQuery{TSource}" /> query to which this <see cref="IQueryFilter{T}" /> filter
        ///     should be applied.
        /// </param>
        void Apply(IFilterableQuery<T> query);
    }
}