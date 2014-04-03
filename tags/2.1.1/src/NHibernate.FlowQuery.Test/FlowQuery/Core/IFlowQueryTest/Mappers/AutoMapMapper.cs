using AutoMapper;
using NHibernate.FlowQuery.AutoMapping;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest.Mappers
{
    public class AutoMapMapper : DefaultMapper
    {
        protected override TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }
    }
}