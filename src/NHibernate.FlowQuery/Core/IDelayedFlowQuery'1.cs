namespace NHibernate.FlowQuery.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Expressions;

    /// <summary>
    ///     An interface defining the functionality required of a delayed query.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity for this query.
    /// </typeparam>
    /// <remarks>
    ///     Execution of delayed queries will be deferred until the results are required by user code. This makes it 
    ///     possible for <see cref="NHibernate" /> to batch multiple queries in one round-trip to the database instead 
    ///     of making one round-trip per query.
    /// </remarks>
    /// <seealso cref="IDetachedFlowQuery{TSource}" />
    /// <seealso cref="IDetachedImmutableFlowQuery" />
    /// <seealso cref="IImmediateFlowQuery{TSource}" />
    public interface IDelayedFlowQuery<TSource> : IQueryableFlowQuery<TSource, IDelayedFlowQuery<TSource>>
    {
        /// <summary>
        ///     Returns a value indicating whether any objects in the underlying data source matches the filters
        ///     specified for this query.
        /// </summary>
        /// <returns>
        ///     A value indicating whether any objects in the underlying data source matches the filters
        ///     specified for this query.
        /// </returns>
        Lazy<bool> Any();

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
        Lazy<bool> Any(params ICriterion[] criterions);

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
        Lazy<bool> Any(string property, IsExpression expression);

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
        Lazy<bool> Any(Expression<Func<TSource, bool>> expression);

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
        Lazy<bool> Any(Expression<Func<TSource, object>> property, IsExpression expression);

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
        Lazy<bool> Any(Expression<Func<TSource, WhereDelegate, bool>> expression);

        /// <summary>
        ///     Returns a copy of this <see cref="IDelayedFlowQuery{TSource}" /> instance.
        /// </summary>
        /// <returns>
        ///     A copy of this <see cref="IDelayedFlowQuery{TSource}" /> instance.
        /// </returns>
        IDelayedFlowQuery<TSource> Copy();

        /// <summary>
        ///     Returns a value indicating the number of objects in the underlying data source that matches the filters
        ///     specified for this query.
        /// </summary>
        /// <returns>
        ///     A value indicating the number of objects in the underlying data source that matches the filters
        ///     specified for this query.
        /// </returns>
        Lazy<int> Count();

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
        Lazy<int> Count(string property);

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
        Lazy<int> Count(IProjection projection);

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
        Lazy<int> Count(Expression<Func<TSource, object>> property);

        /// <summary>
        ///     Returns a value indicating the number of objects in the underlying data source that matches the filters
        ///     specified for this query.
        /// </summary>
        /// <returns>
        ///     A value indicating the number of objects in the underlying data source that matches the filters
        ///     specified for this query.
        /// </returns>
        Lazy<long> CountLong();

        /// <summary>
        ///     Returns a copy of this <see cref="IDelayedFlowQuery{TSource}" /> but in the form of a 
        ///     <see cref="IDetachedFlowQuery{TSource}" /> instead.
        /// </summary>
        /// <returns>
        ///     A copy of this <see cref="IDelayedFlowQuery{TSource}" /> but in the form of a 
        ///     <see cref="IDetachedFlowQuery{TSource}" /> instead.
        /// </returns>
        IDetachedFlowQuery<TSource> Detached();

        /// <summary>
        ///     Specifies that any projections/selections on this query should be performed distinctly.
        /// </summary>
        /// <returns>
        ///     This <see cref="IDelayedFlowQuery{TSource}" /> query.
        /// </returns>
        IDelayedFlowQuery<TSource> Distinct();

        /// <summary>
        ///     Returns a copy of this <see cref="IDelayedFlowQuery{TSource}" /> but in the form of a 
        ///     <see cref="IImmediateFlowQuery{TSource}" /> instead.
        /// </summary>
        /// <returns>
        ///     A copy of this <see cref="IDelayedFlowQuery{TSource}" /> but in the form of a 
        ///     <see cref="IImmediateFlowQuery{TSource}" /> instead.
        /// </returns>
        IImmediateFlowQuery<TSource> Immediate();

        /// <summary>
        ///     Specifies that any projections/selections on this query should be performed indistinctly.
        /// </summary>
        /// <returns>
        ///     This <see cref="IDelayedFlowQuery{TSource}" /> query.
        /// </returns>
        IDelayedFlowQuery<TSource> Indistinct();

        /// <summary>
        ///     Returns a dictionary representing the objects in the underlying data source that matches the filters
        ///     specified for this query.
        /// </summary>
        /// <param name="key">
        ///     The projection to use for the keys in the returned dictionary.
        /// </param>
        /// <param name="value">
        ///     The projection to use for the values in the returned dictionary.
        /// </param>
        /// <typeparam name="TKey">
        ///     The <see cref="System.Type" /> of the keys.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The <see cref="System.Type" /> of the values.
        /// </typeparam>
        /// <returns>
        ///     The created dictionary.
        /// </returns>
        Lazy<Dictionary<TKey, TValue>> SelectDictionary<TKey, TValue>
            (
            Expression<Func<TSource, TKey>> key,
            Expression<Func<TSource, TValue>> value
            );

        /// <summary>
        ///     Transform this query into a <see cref="IStreamedFlowQuery{TSource}" /> query instance.
        /// </summary>
        /// <returns>
        ///     A new <see cref="IStreamedFlowQuery{TSource}" /> instance created from this query.
        /// </returns>
        /// <remarks>
        ///     Streamed queries are executed by <see cref="NHibernate" /> immediately (just like 
        ///     <see cref="IImmediateFlowQuery{TSource}" />), with a separate round-trip to the database for each query.
        ///     The difference between a <see cref="IStreamedFlowQuery{TSource}" /> and a 
        ///     <see cref="IImmediateFlowQuery{TSource}" /> is that the entire result set for a 
        ///     <see cref="IStreamedFlowQuery{TSource}" /> never will be buffered into memory before it reaches 
        ///     user-code. It's entirely up to user-code to load it into memory, if it is deemed necessary for the 
        ///     particular situation.
        /// </remarks>
        IStreamedFlowQuery<TSource> Streamed();
    }
}