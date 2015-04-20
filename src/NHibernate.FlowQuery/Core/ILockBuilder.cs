namespace NHibernate.FlowQuery.Core
{
    /// <summary>
    ///     A helper utility used to build locking strategies with a nice syntax.
    /// </summary>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the underlying query for this <see cref="ILockBuilder{TQuery}" /> 
    ///     instance.
    /// </typeparam>
    public interface ILockBuilder<out TQuery>
    {
        /// <summary>
        ///     Specifies a forced lock. Corresponds to <see cref="NHibernate" />'s <see cref="LockMode.Force" /> mode.
        /// </summary>
        /// <returns>
        ///     The underlying <see cref="IFlowQuery{TSource, TQuery}" /> query.
        /// </returns>
        TQuery Force();

        /// <summary>
        ///     Specifies that no lock should be used. Corresponds to <see cref="NHibernate" />'s 
        ///     <see cref="LockMode.None" /> mode.
        /// </summary>
        /// <returns>
        ///     The underlying <see cref="IFlowQuery{TSource, TQuery}" /> query.
        /// </returns>
        TQuery None();

        /// <summary>
        ///     Specifies a read lock. Corresponds to <see cref="NHibernate" />'s <see cref="LockMode.Read" /> mode.
        /// </summary>
        /// <returns>
        ///     The underlying <see cref="IFlowQuery{TSource, TQuery}" /> query.
        /// </returns>
        TQuery Read();

        /// <summary>
        ///     Specifies an upgrade lock. Corresponds to <see cref="NHibernate" />'s <see cref="LockMode.Upgrade" /> 
        ///     mode.
        /// </summary>
        /// <returns>
        ///     The underlying <see cref="IFlowQuery{TSource, TQuery}" /> query.
        /// </returns>
        TQuery Upgrade();

        /// <summary>
        ///     Specifies an upgrade lock. Corresponds to <see cref="NHibernate" />'s 
        ///     <see cref="LockMode.UpgradeNoWait" /> mode.
        /// </summary>
        /// <returns>
        ///     The underlying <see cref="IFlowQuery{TSource, TQuery}" /> query.
        /// </returns>
        TQuery UpgradeNoWait();

        /// <summary>
        ///     Specifies a write lock. Corresponds to <see cref="NHibernate" />'s <see cref="LockMode.Write" /> mode.
        /// </summary>
        /// <returns>
        ///     The underlying <see cref="IFlowQuery{TSource, TQuery}" /> query.
        /// </returns>
        TQuery Write();
    }
}