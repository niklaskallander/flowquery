namespace NHibernate.FlowQuery.Core
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Expressions;

    /// <summary>
    ///     An interface defining the functionality required of a immediate query.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity for this query.
    /// </typeparam>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the class or interface implementing or extending this interface.
    /// </typeparam>
    /// <remarks>
    ///     Immediate queries are executed by <see cref="NHibernate" /> immediately (no pun intended), with a separate 
    ///     round-trip to the database for each query. To reduce the number of round-trips to the database you should
    ///     use delayed queries instead (<see cref="Delayed()" />, <see cref="IDelayedFlowQuery{TSource}" />).
    /// </remarks>
    /// <seealso cref="IDelayedFlowQuery{TSource}" />
    /// <seealso cref="IDetachedFlowQuery{TSource}" />
    /// <seealso cref="IDetachedImmutableFlowQuery" />
    public interface IImmediateFlowQueryBase<TSource, out TQuery> : IQueryableFlowQueryBase<TSource, TQuery>
    {
        /// <summary>
        ///     Returns a value indicating whether any objects in the underlying data source matches the filters
        ///     specified for this query.
        /// </summary>
        /// <returns>
        ///     A value indicating whether any objects in the underlying data source matches the filters
        ///     specified for this query.
        /// </returns>
        bool Any();

        /// <summary>
        ///     Returns a value indicating whether any objects in the underlying data source matches the filters
        ///     specified for this query.
        /// </summary>
        /// <param name="criterions">
        ///     Extra filters to add to the query before evaluating any matches.
        /// </param>
        /// <returns>
        ///     A value indicating whether any objects in the underlying data source matches the filters
        ///     specified for this query.
        /// </returns>
        bool Any(params ICriterion[] criterions);

        /// <summary>
        ///     Returns a value indicating whether any objects in the underlying data source matches the filters
        ///     specified for this query.
        /// </summary>
        /// <param name="property">
        ///     A property name on which to apply the provided <see cref="IsExpression" /> filter.
        /// </param>
        /// <param name="expression">
        ///     A filter to apply on the provided property name.
        /// </param>
        /// <returns>
        ///     A value indicating whether any objects in the underlying data source matches the filters
        ///     specified for this query.
        /// </returns>
        bool Any(string property, IsExpression expression);

        /// <summary>
        ///     Returns a value indicating whether any objects in the underlying data source matches the filters
        ///     specified for this query.
        /// </summary>
        /// <param name="expression">
        ///     Extra filter(s) to add to the query before evaluating any matches.
        /// </param>
        /// <returns>
        ///     A value indicating whether any objects in the underlying data source matches the filters
        ///     specified for this query.
        /// </returns>
        bool Any(Expression<Func<TSource, bool>> expression);

        /// <summary>
        ///     Returns a value indicating whether any objects in the underlying data source matches the filters
        ///     specified for this query.
        /// </summary>
        /// <param name="property">
        ///     A property name on which to apply the provided <see cref="IsExpression" /> filter.
        /// </param>
        /// <param name="expression">
        ///     A filter to apply on the provided property name.
        /// </param>
        /// <returns>
        ///     A value indicating whether any objects in the underlying data source matches the filters
        ///     specified for this query.
        /// </returns>
        bool Any(Expression<Func<TSource, object>> property, IsExpression expression);

        /// <summary>
        ///     Returns a value indicating whether any objects in the underlying data source matches the filters
        ///     specified for this query.
        /// </summary>
        /// <param name="expression">
        ///     Extra filter(s) to add to the query before evaluating any matches.
        /// </param>
        /// <returns>
        ///     A value indicating whether any objects in the underlying data source matches the filters
        ///     specified for this query.
        /// </returns>
        bool Any(Expression<Func<TSource, WhereDelegate, bool>> expression);

        /// <summary>
        ///     Returns a value indicating the number of objects in the underlying data source that matches the filters
        ///     specified for this query.
        /// </summary>
        /// <returns>
        ///     A value indicating the number of objects in the underlying data source that matches the filters
        ///     specified for this query.
        /// </returns>
        int Count();

        /// <summary>
        ///     Returns a value indicating the number of objects in the underlying data source that matches the filters
        ///     specified for this query.
        /// </summary>
        /// <param name="property">
        ///     The property to count.
        /// </param>
        /// <returns>
        ///     A value indicating the number of objects in the underlying data source that matches the filters
        ///     specified for this query.
        /// </returns>
        int Count(string property);

        /// <summary>
        ///     Returns a value indicating the number of objects in the underlying data source that matches the filters
        ///     specified for this query.
        /// </summary>
        /// <param name="projection">
        ///     The projection to count.
        /// </param>
        /// <returns>
        ///     A value indicating the number of objects in the underlying data source that matches the filters
        ///     specified for this query.
        /// </returns>
        int Count(IProjection projection);

        /// <summary>
        ///     Returns a value indicating the number of objects in the underlying data source that matches the filters
        ///     specified for this query.
        /// </summary>
        /// <param name="property">
        ///     The property to count.
        /// </param>
        /// <returns>
        ///     A value indicating the number of objects in the underlying data source that matches the filters
        ///     specified for this query.
        /// </returns>
        int Count(Expression<Func<TSource, object>> property);

        /// <summary>
        ///     Returns a value indicating the number of objects in the underlying data source that matches the filters
        ///     specified for this query.
        /// </summary>
        /// <returns>
        ///     A value indicating the number of objects in the underlying data source that matches the filters
        ///     specified for this query.
        /// </returns>
        long CountLong();

        /// <summary>
        ///     Returns a copy of this <see cref="T:TQuery" /> instance.
        /// </summary>
        /// <returns>
        ///     A copy of this <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Copy();

        /// <summary>
        ///     Specifies that any projections/selections on this query should be performed distinctly.
        /// </summary>
        /// <returns>
        ///     This <see cref="T:TQuery" /> query.
        /// </returns>
        TQuery Distinct();

        /// <summary>
        ///     Specifies that any projections/selections on this query should be performed indistinctly.
        /// </summary>
        /// <returns>
        ///     This <see cref="T:TQuery" /> query.
        /// </returns>
        TQuery Indistinct();

        /// <summary>
        ///     Returns a copy of this <see cref="IImmediateFlowQuery{TSource}" /> but in the form of a 
        ///     <see cref="IDelayedFlowQuery{TSource}" /> instead.
        /// </summary>
        /// <returns>
        ///     A copy of this <see cref="IImmediateFlowQuery{TSource}" /> but in the form of a 
        ///     <see cref="IDelayedFlowQuery{TSource}" /> instead.
        /// </returns>
        IDelayedFlowQuery<TSource> Delayed();

        /// <summary>
        ///     Returns a copy of this <see cref="IImmediateFlowQuery{TSource}" /> but in the form of a 
        ///     <see cref="IDetachedFlowQuery{TSource}" /> instead.
        /// </summary>
        /// <returns>
        ///     A copy of this <see cref="IImmediateFlowQuery{TSource}" /> but in the form of a 
        ///     <see cref="IDetachedFlowQuery{TSource}" /> instead.
        /// </returns>
        IDetachedFlowQuery<TSource> Detached();
    }
}