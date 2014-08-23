namespace NHibernate.FlowQuery.Core
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    /// <summary>
    ///     An example wrapper used by <see cref="IFlowQuery{TSource,TQuery}.RestrictByExample" /> to filter a query
    ///     based on properties specified on an underlying entity.
    /// </summary>
    /// <typeparam name="TEntity">
    ///     The type of the underlying entity on which the filter should apply.
    /// </typeparam>
    public interface IExampleWrapper<TEntity>
    {
        /// <summary>
        ///     Gets the wrapped <see cref="Example" /> object.
        /// </summary>
        /// <value>
        ///     The wrapped <see cref="Example" /> object.
        /// </value>
        Example Example { get; }

        /// <summary>
        ///     Use the "like" operator for all string-valued properties.
        /// </summary>
        /// <returns>
        ///     This <see cref="IExampleWrapper{TEntity}" /> object. Useful for chaining.
        /// </returns>
        /// <remarks>
        ///     The default <see cref="MatchMode" /> is <see cref="MatchMode.Exact" />.
        /// </remarks>
        IExampleWrapper<TEntity> EnableLike();

        /// <summary>
        ///     Use the "like" operator for all string-valued properties with
        ///     the specified <see cref="MatchMode" />.
        /// </summary>
        /// <param name="matchMode">
        ///     The <see cref="MatchMode" /> to convert the string to the pattern
        ///     for the <c>like</c> comparison.
        /// </param>
        /// <returns>
        ///     This <see cref="IExampleWrapper{TEntity}" /> object. Useful for chaining.
        /// </returns>
        IExampleWrapper<TEntity> EnableLike(MatchMode matchMode);

        /// <summary>
        ///     Excludes all properties having null values from the filter.
        /// </summary>
        /// <returns>
        ///     This <see cref="IExampleWrapper{TEntity}" /> object. Useful for chaining.
        /// </returns>
        IExampleWrapper<TEntity> ExcludeNulls();

        /// <summary>
        ///     Excludes the specified property from the filter.
        /// </summary>
        /// <param name="property">
        ///     The name of the property to exclude from the filter.
        /// </param>
        /// <returns>
        ///     This <see cref="IExampleWrapper{TEntity}" /> object. Useful for chaining.
        /// </returns>
        IExampleWrapper<TEntity> ExcludeProperty(string property);

        /// <summary>
        ///     Excludes the specified property from the filter.
        /// </summary>
        /// <param name="property">
        ///     The property to exclude from the filter.
        /// </param>
        /// <returns>
        ///     This <see cref="IExampleWrapper{TEntity}" /> object. Useful for chaining.
        /// </returns>
        IExampleWrapper<TEntity> ExcludeProperty(Expression<Func<TEntity, object>> property);

        /// <summary>
        ///     Excludes all properties having zero (0) values from the filter.
        /// </summary>
        /// <returns>
        ///     This <see cref="IExampleWrapper{TEntity}" /> object. Useful for chaining.
        /// </returns>
        IExampleWrapper<TEntity> ExcludeZeroes();
    }
}