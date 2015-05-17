namespace NHibernate.FlowQuery.Core.Implementations
{
    using System.Linq;

    using NHibernate.FlowQuery.Core.Structures;

    /// <summary>
    ///     A helper utility used to build locking strategies with a nice syntax.
    /// </summary>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the underlying query for this <see cref="LockBuilder{TQuery}" /> instance.
    /// </typeparam>
    public class LockBuilder<TQuery> : ILockBuilder<TQuery>
    {
        /// <summary>
        ///     The alias for this lock.
        /// </summary>
        private readonly string _alias;

        /// <summary>
        ///     The query instance.
        /// </summary>
        private readonly IFlowQuery _implementor;

        /// <summary>
        ///     The query instance.
        /// </summary>
        private readonly TQuery _query;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LockBuilder{TQuery}" /> class.
        /// </summary>
        /// <param name="implementor">
        ///     The query instance.
        /// </param>
        /// <param name="query">
        ///     The query instance again.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        public LockBuilder(IFlowQuery implementor, TQuery query, string alias)
        {
            _implementor = implementor;
            _query = query;
            _alias = alias;
        }

        /// <inheritdoc />
        public virtual TQuery Force()
        {
            return Add(LockMode.Force);
        }

        /// <inheritdoc />
        public virtual TQuery None()
        {
            return Add(LockMode.None);
        }

        /// <inheritdoc />
        public virtual TQuery Read()
        {
            return Add(LockMode.Read);
        }

        /// <inheritdoc />
        public virtual TQuery Upgrade()
        {
            return Add(LockMode.Upgrade);
        }

        /// <inheritdoc />
        public virtual TQuery UpgradeNoWait()
        {
            return Add(LockMode.UpgradeNoWait);
        }

        /// <inheritdoc />
        public virtual TQuery Write()
        {
            return Add(LockMode.Write);
        }

        /// <summary>
        ///     Creates the locking strategy and adds it to the query.
        /// </summary>
        /// <param name="lockMode">
        ///     The <see cref="LockMode" /> for the locking strategy.
        /// </param>
        /// <returns>
        ///     The underlying <see cref="IFlowQuery{TSource, TQuery}" /> query.
        /// </returns>
        protected virtual TQuery Add(LockMode lockMode)
        {
            Lock existingLock = _implementor.Locks
                .SingleOrDefault(x => x.Alias == _alias);

            if (existingLock != null)
            {
                _implementor.Locks
                    .Remove(existingLock);
            }

            _implementor.Locks
                .Add(new Lock(_alias, lockMode));

            return _query;
        }
    }
}