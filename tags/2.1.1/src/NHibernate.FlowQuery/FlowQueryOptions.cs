using System;

namespace NHibernate.FlowQuery
{
    public class FlowQueryOptions
    {
        private Action<ICriteria> m_Options;

        public FlowQueryOptions()
        {
            m_Options = delegate { };
        }

        /// <summary>
        /// Add an option filter to the options collection
        /// </summary>
        public virtual FlowQueryOptions Add(Action<ICriteria> option)
        {
            if (option != null)
            {
                m_Options += option;
            }

            return this;
        }

        /// <summary>
        /// All added options will be run against the provided criteria object.
        /// </summary>
        protected internal virtual void Use(ICriteria criteria)
        {
            if (criteria != null)
            {
                m_Options(criteria);
            }
        }
    }
}