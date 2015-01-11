namespace NHibernate.FlowQuery.Core.Structures
{
    /// <summary>
    ///     A class representing a locking strategy.
    /// </summary>
    public class Lock
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Lock"/> class.
        /// </summary>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="lockMode">
        ///     The <see cref="LockMode" /> value.
        /// </param>
        public Lock(string alias, LockMode lockMode)
        {
            Alias = alias;

            LockMode = lockMode;
        }

        /// <summary>
        ///     Gets the alias.
        /// </summary>
        /// <value>
        ///     The alias.
        /// </value>
        public virtual string Alias { get; private set; }

        /// <summary>
        ///     Gets the <see cref="LockMode" /> value.
        /// </summary>
        /// <value>
        ///     The <see cref="LockMode" /> value.
        /// </value>
        public virtual LockMode LockMode { get; private set; }
    }
}