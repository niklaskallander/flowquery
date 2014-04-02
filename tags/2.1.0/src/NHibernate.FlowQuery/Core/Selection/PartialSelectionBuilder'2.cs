namespace NHibernate.FlowQuery.Core.Selection
{
    public delegate FlowQuerySelection<TDestination> PartialSelectionBuilder<TSource, TDestination>(PartialSelection<TSource, TDestination> selectSetup)
        where TSource : class;
}
