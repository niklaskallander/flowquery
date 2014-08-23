namespace NHibernate.FlowQuery.Core.Structures
{
    /// <summary>
    ///     A simple key and value pair class.
    /// </summary>
    /// <typeparam name="TKey">
    ///     The <see cref="System.Type" /> of the key.
    /// </typeparam>
    /// <typeparam name="TValue">
    ///     The <see cref="System.Type" /> of the value.
    /// </typeparam>
    /// <remarks>
    ///     This class is mainly used for <see cref="IImmediateFlowQuery{TSource}.SelectDictionary{TKey,TValue}" /> and
    ///     <see cref="IDelayedFlowQuery{TSource}.SelectDictionary{TKey,TValue}" />. Initially hoped to be able to use
    ///     <see cref="System.Collections.Generic.KeyValuePair{TKey,TValue}" /> but since it didn't have a 
    ///     parameter-less constructor I didn't get the simple usage I wanted and resorted to this solution instead.
    /// </remarks>
    public class Pair<TKey, TValue>
    {
        /// <summary>
        ///     Gets or sets the key.
        /// </summary>
        /// <value>
        ///     The key for this <see cref="Pair{TKey, TValue}" />.
        /// </value>
        public virtual TKey Key { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        /// <value>
        ///     The value for this <see cref="Pair{TKey, TValue}" />.
        /// </value>
        public virtual TValue Value { get; set; }
    }
}