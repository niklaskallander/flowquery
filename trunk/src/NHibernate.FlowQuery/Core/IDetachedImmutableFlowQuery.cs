namespace NHibernate.FlowQuery.Core
{
    using NHibernate.Criterion;

    /// <summary>
    ///     An interface defining the structure required of an immutable detached query.
    /// </summary>
    /// <seealso cref="IDelayedFlowQuery{TSource}" />
    /// <seealso cref="IDetachedFlowQuery{TSource}" />
    /// <seealso cref="IImmediateFlowQuery{TSource}" />
    public interface IDetachedImmutableFlowQuery
    {
        /// <summary>
        ///     Gets the <see cref="DetachedCriteria" /> instance this query represents.
        /// </summary>
        /// <value>
        ///     The <see cref="DetachedCriteria" /> instance this query represents.
        /// </value>
        DetachedCriteria Criteria { get; }
    }
}