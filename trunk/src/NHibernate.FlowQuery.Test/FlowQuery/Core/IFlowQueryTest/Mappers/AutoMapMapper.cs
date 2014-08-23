namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest.Mappers
{
    using AutoMapper;

    using NHibernate.FlowQuery.AutoMapping;

    public class AutoMapMapper : DefaultMapper
    {
        public override TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }
    }
}