using System;
using System.Collections.Generic;
using NHibernate.FlowQuery.AutoMapping;

namespace NHibernate.FlowQuery.Test.FlowQuery.Core.IFlowQueryTest.Mappers
{
    public class CustomMapper : DefaultMapper
    {
        #region Fields (1)

        private Dictionary<System.Type, Dictionary<System.Type, Delegate>> m_Maps;

        #endregion Fields

        #region Constructors (1)

        public CustomMapper()
        {
            m_Maps = new Dictionary<System.Type, Dictionary<System.Type, Delegate>>();
        }

        #endregion Constructors

        #region Methods (3)

        public virtual void AddMap<TSource, TDestination>(Func<TSource, TDestination> mappingDelegate)
            where TDestination : new()
        {
            if (!m_Maps.ContainsKey(typeof(TSource)))
            {
                m_Maps.Add(typeof(TSource), new Dictionary<System.Type, Delegate>());
            }
            m_Maps[typeof(TSource)].Add(typeof(TDestination), mappingDelegate);
        }

        protected override TDestination Map<TSource, TDestination>(TSource source)
        {
            Func<TSource, TDestination> mappingDelegate = m_Maps[typeof(TSource)][typeof(TDestination)] as Func<TSource, TDestination>;

            return mappingDelegate(source);
        }

        #endregion Methods
    }
}