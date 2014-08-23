namespace NHibernate.FlowQuery.Core
{
    using NHibernate.FlowQuery.Core.Implementations;

    /// <summary>
    ///     A helper utility used to build fetching strategies with a nice syntax.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the source for the <see cref="IFlowQuery{TSource, TQuery}" /> query
    ///     used to create this <see cref="IFetchBuilder{TSource, TQuery}" /> instance.
    /// </typeparam>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the underlying <see cref="IFlowQuery{TSource, TQuery}" /> query for this
    ///     <see cref="IFetchBuilder{TSource, TQuery}" /> instance.
    /// </typeparam>
    public interface IFetchBuilder<TSource, out TQuery>
        where TSource : class
        where TQuery : class, IFlowQuery<TSource, TQuery>
    {
        /// <summary>
        ///     Specifies an eager fetching strategy for the provided association path. Behaves the same as 
        ///     <see cref="IFetchBuilder{TSource, TQuery}.WithJoin()" />. Corresponds to <see cref="NHibernate" />'s 
        ///     <see cref="FetchMode" />.<see cref="FetchMode.Eager" />.
        /// </summary>
        /// <returns>
        ///     The underlying <see cref="T:TQuery"/> query for this <see cref="IFetchBuilder{TSource, TQuery}" /> 
        ///     instance.
        /// </returns>
        TQuery Eagerly();

        /// <summary>
        ///     Specifies a lazy fetching strategy for the provided association path. Behaves the same as 
        ///     <see cref="IFetchBuilder{TSource, TQuery}.WithSelect()" />. Corresponds to <see cref="NHibernate" />'s 
        ///     <see cref="FetchMode" />.<see cref="FetchMode.Lazy" />.
        /// </summary>
        /// <returns>
        ///     The underlying <see cref="T:TQuery"/> query for this <see cref="FetchBuilder{TSource,TQuery}" />
        ///     instance.
        /// </returns>
        TQuery Lazily();

        /// <summary>
        ///     Specifies a join fetching strategy for the provided association path. Behaves the same as 
        ///     <see cref="IFetchBuilder{TSource, TQuery}.Eagerly()" />. Corresponds to <see cref="NHibernate" />'s 
        ///     <see cref="FetchMode" />.<see cref="FetchMode.Join" />.
        /// </summary>
        /// <returns>
        ///     The underlying <see cref="T:TQuery"/> query for this <see cref="FetchBuilder{TSource, TQuery}" />
        ///     instance.
        /// </returns>
        TQuery WithJoin();

        /// <summary>
        ///     Specifies a select fetching strategy for the provided association path. Behaves the same as 
        ///     <see cref="IFetchBuilder{TSource, TQuery}.Lazily()" />. Corresponds to <see cref="NHibernate" />'s 
        ///     <see cref="FetchMode" />.<see cref="FetchMode.Select" />.
        /// </summary>
        /// <returns>
        ///     The underlying <see cref="T:TQuery"/> query for this <see cref="FetchBuilder{TSource, TQuery}" />
        ///     instance.
        /// </returns>
        TQuery WithSelect();
    }
}