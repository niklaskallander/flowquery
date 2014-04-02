namespace NHibernate.FlowQuery.Core.Fetches
{
    public interface IFetchBuilder<TSource, TQuery>
        where TSource : class
        where TQuery : class, IFlowQuery<TSource, TQuery>
    {
        TQuery WithJoin();

        TQuery WithSelect();

        TQuery Eagerly();

        TQuery Lazily();
    }
}