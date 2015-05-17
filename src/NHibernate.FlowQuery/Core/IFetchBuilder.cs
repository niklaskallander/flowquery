namespace NHibernate.FlowQuery.Core
{
    using NHibernate.FlowQuery.Core.Implementations;

    /// <summary>
    ///     A helper utility used to build fetching strategies with a nice syntax.
    /// </summary>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the underlying query for this <see cref="IFetchBuilder{TQuery}" /> 
    ///     instance.
    /// </typeparam>
    public interface IFetchBuilder<out TQuery>
    {
        /// <summary>
        ///     Specifies an eager fetching strategy for the provided association path. Behaves the same as 
        ///     <see cref="IFetchBuilder{TQuery}.WithJoin()" />. Corresponds to <see cref="NHibernate" />'s 
        ///     <see cref="FetchMode" />.<see cref="FetchMode.Eager" />.
        /// </summary>
        /// <returns>
        ///     The underlying <see cref="T:TQuery"/> query for this <see cref="IFetchBuilder{TQuery}" /> 
        ///     instance.
        /// </returns>
        TQuery Eagerly();

        /// <summary>
        ///     Specifies a lazy fetching strategy for the provided association path. Behaves the same as 
        ///     <see cref="IFetchBuilder{TQuery}.WithSelect()" />. Corresponds to <see cref="NHibernate" />'s 
        ///     <see cref="FetchMode" />.<see cref="FetchMode.Lazy" />.
        /// </summary>
        /// <returns>
        ///     The underlying <see cref="T:TQuery"/> query for this <see cref="FetchBuilder{TQuery}" />
        ///     instance.
        /// </returns>
        TQuery Lazily();

        /// <summary>
        ///     Specifies a join fetching strategy for the provided association path. Behaves the same as 
        ///     <see cref="IFetchBuilder{TQuery}.Eagerly()" />. Corresponds to <see cref="NHibernate" />'s 
        ///     <see cref="FetchMode" />.<see cref="FetchMode.Join" />.
        /// </summary>
        /// <returns>
        ///     The underlying <see cref="T:TQuery"/> query for this <see cref="FetchBuilder{TQuery}" />
        ///     instance.
        /// </returns>
        TQuery WithJoin();

        /// <summary>
        ///     Specifies a select fetching strategy for the provided association path. Behaves the same as 
        ///     <see cref="IFetchBuilder{TQuery}.Lazily()" />. Corresponds to <see cref="NHibernate" />'s 
        ///     <see cref="FetchMode" />.<see cref="FetchMode.Select" />.
        /// </summary>
        /// <returns>
        ///     The underlying <see cref="T:TQuery"/> query for this <see cref="FetchBuilder{TQuery}" />
        ///     instance.
        /// </returns>
        TQuery WithSelect();
    }
}