namespace NHibernate.FlowQuery.Core.Locks
{
    public class Lock
    {
        public Lock(string alias, LockMode lockMode)
        {
            Alias = alias;

            LockMode = lockMode;
        }

        public virtual string Alias { get; private set; }

        public virtual LockMode LockMode { get; private set; }
    }
}