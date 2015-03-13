namespace NHibernate.FlowQuery.Helpers
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;

    using NHibernate.FlowQuery.Core;

    /// <summary>
    ///     Streams the result from a query into a <see cref="IResultStream{T}" />.
    /// </summary>
    /// <typeparam name="T">
    ///     The <see cref="System.Type" /> of the results.
    /// </typeparam>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder",
        Justification = "Reviewed. Suppression is OK here.")]
    public class ResultStreamer<T> : IList
    {
        /// <summary>
        ///     The converter used to convert each query result item into the destination <see cref="System.Type" />
        ///     specified by <typeparamref name="T" />.
        /// </summary>
        private readonly Func<object, T> _converter;

        /// <summary>
        ///     The <see cref="IResultStream{T}" /> into which this <see cref="ResultStreamer{T}" /> will stream each
        ///     query result item.
        /// </summary>
        private readonly IResultStream<T> _resultStream;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ResultStreamer{T}" /> class.
        /// </summary>
        /// <param name="resultStream">
        ///     The <see cref="IResultStream{T}" /> into which this <see cref="ResultStreamer{T}" /> will stream each
        ///     query result item.
        /// </param>
        /// <param name="converter">
        ///     The converter used to convert each query result item into the destination <see cref="System.Type" />
        ///     specified by <typeparamref name="T" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="resultStream" /> and/or <paramref name="converter" /> is null.
        /// </exception>
        public ResultStreamer
            (
            IResultStream<T> resultStream,
            Func<object, T> converter
            )
        {
            if (resultStream == null)
            {
                throw new ArgumentNullException("resultStream");
            }

            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }

            _resultStream = resultStream;
            _converter = converter;
        }

        /// <summary>
        ///     Called by <see cref="NHibernate" /> for each query result item to stream.
        /// </summary>
        /// <param name="value">
        ///     The query result item to stream.
        /// </param>
        /// <returns>
        ///     Always returns 0 (result is not used by <see cref="NHibernate" />).
        /// </returns>
        public int Add(object value)
        {
            _resultStream.Receive(_converter(value));

            return 0;
        }

        #region Not Implemented IList Members

        /// <summary>
        ///     Gets the number of elements contained in the <see cref="System.Collections.ICollection" />.
        /// </summary>
        /// <value>
        ///     The number of elements contained in the <see cref="System.Collections.ICollection" />.
        /// </value>
        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the <see cref="System.Collections.IList" /> has a fixed size.
        /// </summary>
        /// <value>
        ///     True if the <see cref="System.Collections.IList" /> has a fixed size; otherwise, false.
        /// </value>
        public bool IsFixedSize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the <see cref="System.Collections.IList" /> is read-only.
        /// </summary>
        /// <value>
        ///     True if the <see cref="System.Collections.IList" /> is read-only; otherwise, false.
        /// </value>
        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///     Gets a value indicating whether access to the <see cref="System.Collections.ICollection" /> is 
        ///     synchronized (thread safe).
        /// </summary>
        /// <value>
        ///     True if access to the <see cref="System.Collections.ICollection" /> is synchronized (thread safe); 
        ///     otherwise, false.
        /// </value>
        public bool IsSynchronized
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///     Gets an object that can be used to synchronize access to the 
        ///     <see cref="System.Collections.ICollection" />.
        /// </summary>
        /// <value>
        ///     An object that can be used to synchronize access to the <see cref="System.Collections.ICollection" />.
        /// </value>
        public object SyncRoot
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///     Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">
        ///     The zero-based index of the element to get or set.
        /// </param>
        /// <returns>
        ///     The element at the specified index.
        /// </returns>
        public object this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///     Removes all items from the <see cref="System.Collections.IList" />.
        /// </summary>
        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Determines whether the <see cref="System.Collections.IList" /> contains a specific value.
        /// </summary>
        /// <param name="value">
        ///     The object to locate in the <see cref="System.Collections.IList" />.
        /// </param>
        /// <returns>
        ///     True if the <see cref="System.Object" /> is found in the <see cref="System.Collections.IList" />; 
        ///     otherwise, false.
        /// </returns>
        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Copies the elements of the <see cref="System.Collections.ICollection" /> to an 
        ///     <see cref="System.Array" />, starting at a particular <see cref="System.Array" /> index.
        /// </summary>
        /// <param name="array">
        ///     The one-dimensional <see cref="System.Array" /> that is the destination of the elements copied from 
        ///     <see cref="System.Collections.ICollection" />. The <see cref="System.Array" /> must have zero-based 
        ///     indexing.
        /// </param>
        /// <param name="index">
        ///     The zero-based index in array at which copying begins.
        /// </param>
        public void CopyTo
            (
            Array array,
            int index
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="System.Collections.IEnumerator" /> object that can be used to iterate through the 
        ///     collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Determines the index of a specific item in the <see cref="System.Collections.IList" />.
        /// </summary>
        /// <param name="value">
        ///     The object to locate in the <see cref="System.Collections.IList" />.
        /// </param>
        /// <returns>
        ///     The index of value if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Inserts an item to the <see cref="System.Collections.IList" /> at the specified index.
        /// </summary>
        /// <param name="index">
        ///     The zero-based index at which value should be inserted.
        /// </param>
        /// <param name="value">
        ///     The object to insert into the <see cref="System.Collections.IList" />.
        /// </param>
        public void Insert
            (
            int index,
            object value
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Removes the first occurrence of a specific object from the <see cref="System.Collections.IList" />.
        /// </summary>
        /// <param name="value">
        ///     The object to remove from the <see cref="System.Collections.IList" />.
        /// </param>
        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Removes the System.Collections.IList item at the specified index.
        /// </summary>
        /// <param name="index">
        ///     The zero-based index of the item to remove.
        /// </param>
        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}