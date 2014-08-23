namespace NHibernate.FlowQuery.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     A class representing the result of an actual data selection made using <see cref="NHibernate.FlowQuery" />.
    /// </summary>
    /// <typeparam name="T">
    ///     The <see cref="System.Type" /> of the result data.
    /// </typeparam>
    public class FlowQuerySelection<T> : IEnumerable<T>
    {
        /// <summary>
        ///     If the selection is "delayed" (as indicated by <see cref="IsDelayed" />) this member contains an action 
        ///     to retrieve the result when needed.
        /// </summary>
        private readonly Func<IEnumerable<T>> _delayedSelection;

        /// <summary>
        ///     The selection.
        /// </summary>
        private IEnumerable<T> _selection;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FlowQuerySelection{T}" /> class.
        /// </summary>
        /// <param name="delayedSelection">
        ///     An action to retrieve the result of a delayed selection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="delayedSelection" /> is null.
        /// </exception>
        public FlowQuerySelection(Func<IEnumerable<T>> delayedSelection)
        {
            if (delayedSelection == null)
            {
                throw new ArgumentNullException("delayedSelection");
            }

            _delayedSelection = delayedSelection;

            IsDelayed = true;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FlowQuerySelection{T}" /> class.
        /// </summary>
        /// <param name="selection">
        ///     The selection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="selection" /> is null.
        /// </exception>
        public FlowQuerySelection(IEnumerable<T> selection)
        {
            if (selection == null)
            {
                throw new ArgumentNullException("selection");
            }

            _selection = selection;

            IsDelayed = false;
        }

        /// <summary>
        ///     Gets a value indicating whether this <see cref="FlowQuerySelection{T}" /> instance represents a delayed
        ///     selection.
        /// </summary>
        /// <value>
        ///     The value indicating whether this <see cref="FlowQuerySelection{T}" /> instance represents a delayed
        ///     selection.
        /// </value>
        public virtual bool IsDelayed { get; private set; }

        /// <summary>
        ///     Gets the selection.
        /// </summary>
        /// <value>
        ///     The selection.
        /// </value>
        protected virtual IEnumerable<T> Selection
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

        /// <summary>
        ///     Casts the selection into a <see cref="List{T}" /> instance.
        /// </summary>
        /// <param name="selection">
        ///     The selection to cast.
        /// </param>
        /// <returns>
        ///     The created <see cref="List{T}" /> instance.
        /// </returns>
        public static implicit operator List<T>(FlowQuerySelection<T> selection)
        {
            if (selection == null)
            {
                return null;
            }

            return new List<T>(selection.Selection);
        }

        /// <summary>
        ///     Casts the selection into a <see cref="T:T" /> instance using a first-or-default strategy. NOTE: this 
        ///     implicit casting operator does NOT verify that the selection only contains one item.
        /// </summary>
        /// <param name="selection">
        ///     The selection to cast.
        /// </param>
        /// <returns>
        ///     The <see cref="T:T" /> instance or the default value of <see cref="T:T" /> if the selection contain no 
        ///     items.
        /// </returns>
        public static implicit operator T(FlowQuerySelection<T> selection)
        {
            if (selection == null || selection.Selection == null)
            {
                return default(T);
            }

            return selection.Selection.FirstOrDefault();
        }

        /// <summary>
        ///     Casts the selection into a <see cref="T:T[]" /> instance.
        /// </summary>
        /// <param name="selection">
        ///     The selection to cast.
        /// </param>
        /// <returns>
        ///     The created <see cref="T:T[]" /> instance.
        /// </returns>
        public static implicit operator T[](FlowQuerySelection<T> selection)
        {
            if (selection == null || selection.Selection == null)
            {
                return null;
            }

            return selection.Selection.ToArray();
        }

        /// <summary>
        ///     Retrieves the <see cref="IEnumerator{T}" /> for the underlying selection.
        /// </summary>
        /// <returns>
        ///     The <see cref="IEnumerator{T}" /> for the underlying selection.
        /// </returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            return Selection.GetEnumerator();
        }

        /// <summary>
        ///     Maps the underlying selection into a new <see cref="FlowQuerySelection{T}" /> instance of the specified
        ///     type (<typeparamref name="TDestination" />) using the default mapping strategy specified for
        ///     <see cref="Mapping.Map{TSource, TDestination}" />. 
        /// </summary>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the mapped <see cref="FlowQuerySelection{T}" /> instance.
        /// </typeparam>
        /// <returns>
        ///     The mapped <see cref="FlowQuerySelection{T}" /> instance.
        /// </returns>
        public virtual FlowQuerySelection<TDestination> ToMap<TDestination>()
            where TDestination : new()
        {
            IEnumerable<T> currentSelection = Selection;

            IEnumerable<TDestination> delayedSelection =

                new FlowQuerySelection<TDestination>(() =>

                    currentSelection
                        .Select(Mapping.Map<T, TDestination>)
                );

            return new FlowQuerySelection<TDestination>(delayedSelection);
        }

        /// <summary>
        ///     Retrieves the <see cref="IEnumerator" /> for the underlying selection.
        /// </summary>
        /// <returns>
        ///     The <see cref="IEnumerator" /> for the underlying selection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}