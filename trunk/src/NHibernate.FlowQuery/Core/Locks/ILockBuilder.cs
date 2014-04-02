namespace NHibernate.FlowQuery.Core.Locks
{
    public interface ILockBuilder<TSource, TQuery>
        where TSource : class
        where TQuery : class, IFlowQuery<TSource, TQuery>
    {
        TQuery Force();

        TQuery None();

        TQuery Read();

        TQuery Upgrade();

        TQuery UpgradeNoWait();

        TQuery Write();
    }
}