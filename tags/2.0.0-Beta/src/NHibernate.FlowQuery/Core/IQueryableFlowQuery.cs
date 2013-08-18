namespace NHibernate.FlowQuery.Core
{
    public interface IQueryableFlowQuery : IMorphableFlowQuery
    {
        bool IsDelayed { get; }
    }
}