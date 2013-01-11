using System;
using NHibernate.FlowQuery.AutoMapping;

namespace NHibernate.FlowQuery
{
    public static class Mapping
    {
        #region Fields (1)

        private static IMapper m_Mapper;

        #endregion Fields

        #region Constructors (1)

        static Mapping()
        {
            m_Mapper = new DefaultMapper();
        }

        #endregion Constructors

        #region Methods (2)

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

        #endregion Methods
    }
}