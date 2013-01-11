using AutoMapper;
using NHibernate.FlowQuery.AutoMapping;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest.Mappers
{
    public class AutoMapMapper : DefaultMapper
    {
        protected override TReturn Map<TSource, TReturn>(TSource source)
        {
            return Mapper.Map<TSource, TReturn>(source);
        }
    }
}