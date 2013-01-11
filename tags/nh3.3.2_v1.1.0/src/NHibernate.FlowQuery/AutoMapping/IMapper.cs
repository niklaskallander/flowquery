namespace NHibernate.FlowQuery.AutoMapping
{
    public interface IMapper
    {
        #region Operations (1)

        TReturn Map<TSource, TReturn>(TSource source) where TReturn : new();

        #endregion Operations
    }
}