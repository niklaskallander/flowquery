namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;

    using NHibernate.Criterion;

    /// <summary>
    ///     A subquery class simply wrapping a <see cref="DetachedCriteria" /> instance.
    /// </summary>
    public class DetachedImmutableFlowQuery : IDetachedImmutableFlowQuery
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DetachedImmutableFlowQuery" /> class.
        /// </summary>
        /// <param name="criteria">
        ///     The <see cref="DetachedCriteria" /> instance to wrap.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="criteria" /> is null.
        /// </exception>
        protected internal DetachedImmutableFlowQuery(DetachedCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException("criteria");
            }

            Criteria = criteria;
        }

        /// <inheritdoc />
        public virtual DetachedCriteria Criteria { get; private set; }
    }
}