using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NHibernate.FlowQuery.Core
{
    public class FlowQuerySelection<TSource> : IEnumerable<TSource>
    {
        #region Fields (1)

        private IEnumerable<TSource> m_Selection;

        #endregion Fields

        #region Constructors (1)

        public FlowQuerySelection(IEnumerable<TSource> selection)
        {
            if (selection == null)
            {
                throw new ArgumentNullException("selection");
            }

            m_Selection = selection;
        }

        #endregion Constructors

        #region Methods (2)

        public virtual FlowQuerySelection<TReturn> ToMap<TReturn>()
            where TReturn : new()
        {
            IEnumerable<TReturn> mapped = from item
                                          in m_Selection
                                          select Mapping.Map<TSource, TReturn>(item);

            return new FlowQuerySelection<TReturn>(mapped);
        }

        protected virtual IEnumerator<TSource> GetEnumerator()
        {
            return m_Selection.GetEnumerator();
        }

        public static implicit operator List<TSource>(FlowQuerySelection<TSource> selection)
        {
            if (selection == null)
            {
                return null;
            }

            return new List<TSource>(selection.m_Selection);
        }

        #endregion Methods

        public static implicit operator TSource(FlowQuerySelection<TSource> selection)
        {
            if (selection == null || selection.m_Selection == null || selection.m_Selection.Count() == 0)
            {
                return default(TSource);
            }

            return selection.m_Selection.First();
        }

        public static implicit operator TSource[](FlowQuerySelection<TSource> selection)
        {
            if (selection == null || selection.m_Selection == null)
            {
                return null;
            }

            return selection.m_Selection.ToArray();
        }

        #region IEnumerable<TSource> Members

        IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}