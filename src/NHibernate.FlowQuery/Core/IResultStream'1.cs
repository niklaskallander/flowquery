namespace NHibernate.FlowQuery.Core
{
    /// <summary>
    ///     Defines the basic contract used for streaming query results instead of loading the entire result set into
    ///     memory.
    /// </summary>
    /// <typeparam name="T">
    ///     The <see cref="System.Type" /> of the items in the result set.
    /// </typeparam>
    public interface IResultStream<in T>
    {
        /// <summary>
        ///     Called when a new item is available.
        /// </summary>
        /// <param name="item">
        ///     The available item.
        /// </param>
        void Receive(T item);

        /// <summary>
        ///     Called when there are no more items left in the result set.
        /// </summary>
        void EndOfStream();
    }
}