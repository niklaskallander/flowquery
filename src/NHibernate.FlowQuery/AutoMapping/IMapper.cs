namespace NHibernate.FlowQuery.AutoMapping
{
    public interface IMapper
    {
        TDestination Map<TSource, TDestination>(TSource source) where TDestination : new();
    }
}