using System.Linq;
using NHibernate.FlowQuery.Core.Implementors;

namespace NHibernate.FlowQuery.Core.Locks
{
    public class LockBuilder<TSource, TQuery> : ILockBuilder<TSource, TQuery> 
        where TSource : class
        where TQuery : class, IFlowQuery<TSource, TQuery>
    {
        private readonly FlowQueryImplementor<TSource, TQuery> _implementor;
        private readonly TQuery _query;
        private readonly string _alias;

        public LockBuilder(FlowQueryImplementor<TSource, TQuery> implementor, TQuery query, string alias)
        {
            _implementor = implementor;
            _query = query;
            _alias = alias;
        }

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

        public virtual TQuery Force()
        {
            return Add(LockMode.Force);
        }

        public virtual TQuery None()
        {
            return Add(LockMode.None);
        }

        public virtual TQuery Read()
        {
            return Add(LockMode.Read);
        }

        public virtual TQuery Upgrade()
        {
            return Add(LockMode.Upgrade);
        }

        public virtual TQuery UpgradeNoWait()
        {
            return Add(LockMode.UpgradeNoWait);
        }

        public virtual TQuery Write()
        {
            return Add(LockMode.Write);
        }
    }
}