namespace NHibernate.FlowQuery.Core
{
    using NHibernate.Criterion;

    /// <summary>
    ///     An interface defining the functionality required to build <see cref="ICriteria" /> and
    ///     <see cref="DetachedCriteria" /> instances from <see cref="NHibernate.FlowQuery" /> queries.
    /// </summary>
    public interface ICriteriaBuilder
    {
        /// <summary>
        ///     Builds a <see cref="ICriteria" /> from the given <see cref="IQueryableFlowQuery" /> query.
        /// </summary>
        /// <param name="query">
        ///     The query from which to build a <see cref="ICriteria" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the underlying entity for the given query.
        /// </typeparam>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the result.
        /// </typeparam>
        /// <returns>
        ///     The built <see cref="ICriteria" />.
        /// </returns>
        ICriteria Build<TSource, TDestination>(IQueryableFlowQuery query);

        /// <summary>
        ///     Builds a <see cref="DetachedCriteria" /> from the given <see cref="IMorphableFlowQuery" /> query.
        /// </summary>
        /// <param name="query">
        ///     The query from which to build a <see cref="ICriteria" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the underlying entity for the given query.
        /// </typeparam>
        /// <returns>
        ///     The built <see cref="DetachedCriteria" />.
        /// </returns>
        DetachedCriteria Build<TSource>(IMorphableFlowQuery query);
    }
}