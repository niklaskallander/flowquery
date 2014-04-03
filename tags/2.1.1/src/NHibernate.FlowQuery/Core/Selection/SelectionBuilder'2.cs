namespace NHibernate.FlowQuery.Core.Selection
{
    public delegate FlowQuerySelection<TDestination> SelectionBuilder<TSource, TDestination>(ISelectSetup<TSource, TDestination> selectSetup)
        where TSource : class;
}
