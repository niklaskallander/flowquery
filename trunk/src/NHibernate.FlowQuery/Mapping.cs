using System;
using NHibernate.FlowQuery.AutoMapping;

namespace NHibernate.FlowQuery
{
    public static class Mapping
    {
        private static IMapper m_Mapper;

        static Mapping()
        {
            m_Mapper = new DefaultMapper();
        }

        public static TReturn Map<TSource, TReturn>(TSource source)
             where TReturn : new()
        {
            return m_Mapper.Map<TSource, TReturn>(source);
        }

        public static void SetMapper(IMapper mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }

            m_Mapper = mapper;
        }
    }
}