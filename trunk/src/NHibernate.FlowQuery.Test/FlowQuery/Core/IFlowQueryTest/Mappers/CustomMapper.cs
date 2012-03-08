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

        public virtual void AddMap<TSource, TReturn>(Func<TSource, TReturn> mappingDelegate)
            where TReturn : new()
        {
            if (!m_Maps.ContainsKey(typeof(TSource)))
            {
                m_Maps.Add(typeof(TSource), new Dictionary<System.Type, Delegate>());
            }
            m_Maps[typeof(TSource)].Add(typeof(TReturn), mappingDelegate);
        }

        protected override TReturn Map<TSource, TReturn>(TSource source)
        {
            Func<TSource, TReturn> mappingDelegate = m_Maps[typeof(TSource)][typeof(TReturn)] as Func<TSource, TReturn>;

            return mappingDelegate(source);
        }

        #endregion Methods
    }
}