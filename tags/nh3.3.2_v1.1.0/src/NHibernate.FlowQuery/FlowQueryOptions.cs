using System;

namespace NHibernate.FlowQuery
{
    public class FlowQueryOptions
    {
        #region Fields (1)

        private Action<ICriteria> m_Options;

        #endregion Fields

        #region Constructors (1)

        public FlowQueryOptions()
        {
            m_Options = delegate { };
        }

        #endregion Constructors

        #region Methods (2)

        public FlowQueryOptions Add(Action<ICriteria> option)
        {
            m_Options += option;

            return this;
        }

        public void Use(ICriteria criteria)
        {
            m_Options(criteria);
        }

        #endregion Methods
    }
}