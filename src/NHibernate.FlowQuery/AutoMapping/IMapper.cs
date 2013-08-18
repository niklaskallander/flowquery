namespace NHibernate.FlowQuery.AutoMapping
{
    public interface IMapper
    {
        #region Operations (1)

        TDestination Map<TSource, TDestination>(TSource source) where TDestination : new();

        #endregion Operations
    }
}