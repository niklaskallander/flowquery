namespace NHibernate.FlowQuery
{
    using System;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;

    /// <summary>
    ///     A helper class defining a set of extension methods for <see cref="DetachedCriteria" /> to create
    ///     <see cref="NHibernate.FlowQuery" /> sub-queries.
    /// </summary>
    public static class DetachedCriteriaExtensions
    {
        /// <summary>
        ///     Creates an instance of <see cref="IDetachedImmutableFlowQuery" /> from the specified
        ///     <see cref="DetachedCriteria" /> instance.
        /// </summary>
        /// <param name="criteria">
        ///     The <see cref="DetachedCriteria" /> instance.
        /// </param>
        /// <returns>
        ///     The created <see cref="IDetachedImmutableFlowQuery" /> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="criteria" /> is null.
        /// </exception>
        public static IDetachedImmutableFlowQuery DetachedFlowQuery(this DetachedCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException("criteria");
            }

            return new DetachedImmutableFlowQuery(criteria);
        }
    }
}