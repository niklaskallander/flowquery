using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NHibernate.FlowQuery.Core
{
    public class FlowQuerySelection<TSource> : IEnumerable<TSource>
    {
        private IEnumerable<TSource> _selection;

        private readonly Func<IEnumerable<TSource>> _delayedSelection;

        public FlowQuerySelection(Func<IEnumerable<TSource>> delayedSelection)
        {
            if (delayedSelection == null)
            {
                throw new ArgumentNullException("delayedSelection");
            }

            _delayedSelection = delayedSelection;

            IsDelayed = true;
        }

        public FlowQuerySelection(IEnumerable<TSource> selection)
        {
            if (selection == null)
            {
                throw new ArgumentNullException("selection");
            }

            _selection = selection;

            IsDelayed = false;
        }

        public virtual bool IsDelayed { get; private set; }

        protected virtual IEnumerable<TSource> Selection
        {
            get
            {
                if (_selection == null)
                {
                    if (IsDelayed && _delayedSelection != null)
                    {
                        _selection = _delayedSelection();

                        IsDelayed = false;
                    }
                }

                return _selection;
            }
        }

        public virtual FlowQuerySelection<TDestination> ToMap<TDestination>()
            where TDestination : new()
        {
            IEnumerable<TSource> currentSelection = Selection;

            IEnumerable<TDestination> delayedSelection = new FlowQuerySelection<TDestination>(() =>

                from item
                in currentSelection
                select Mapping.Map<TSource, TDestination>(item)
            );

            return new FlowQuerySelection<TDestination>(delayedSelection);
        }

        public virtual IEnumerator<TSource> GetEnumerator()
        {
            return Selection.GetEnumerator();
        }

        public static implicit operator List<TSource>(FlowQuerySelection<TSource> selection)
        {
            if (selection == null)
            {
                return null;
            }

            return new List<TSource>(selection.Selection);
        }

        public static implicit operator TSource(FlowQuerySelection<TSource> selection)
        {
            if (selection == null || selection.Selection == null)
            {
                return default(TSource);
            }

            return selection.Selection.FirstOrDefault();
        }

        public static implicit operator TSource[](FlowQuerySelection<TSource> selection)
        {
            if (selection == null || selection.Selection == null)
            {
                return null;
            }

            return selection.Selection.ToArray();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}