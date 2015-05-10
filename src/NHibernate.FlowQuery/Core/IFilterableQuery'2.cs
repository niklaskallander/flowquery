namespace NHibernate.FlowQuery.Core
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Expressions;

    using IsEmptyExpression = NHibernate.FlowQuery.Expressions.IsEmptyExpression;

    /// <summary>
    ///     Specifies a <see cref="IFilterableQuery{TSource,TQuery}" /> used to filter a query by an alias as temporary
    ///     query root.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the object to filter on.
    /// </typeparam>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of this query.
    /// </typeparam>
    public interface IFilterableQuery<TSource, out TQuery>
    {
        /// <summary>
        ///     Gets the <see cref="IJoinBuilder{TSource,TQuery}" /> instance for adding full joins.
        /// </summary>
        /// <value>
        ///     The <see cref="IJoinBuilder{TSource, TQuery}" /> instance for adding full joins.
        /// </value>
        /// <seealso cref="Inner" />
        /// <seealso cref="LeftOuter" />
        /// <seealso cref="RightOuter" />
        IJoinBuilder<TSource, TQuery> Full { get; }

        /// <summary>
        ///     Gets the <see cref="IJoinBuilder{TSource, TQuery}" /> instance for adding inner joins.
        /// </summary>
        /// <value>
        ///     The <see cref="IJoinBuilder{TSource, TQuery}" /> instance for adding inner joins.
        /// </value>
        /// <seealso cref="Full" />
        /// <seealso cref="LeftOuter" />
        /// <seealso cref="RightOuter" />
        IJoinBuilder<TSource, TQuery> Inner { get; }

        /// <summary>
        ///     Gets the <see cref="IJoinBuilder{TSource, TQuery}" /> instance for adding left outer joins.
        /// </summary>
        /// <value>
        ///     The <see cref="IJoinBuilder{TSource, TQuery}" /> instance for adding left outer joins.
        /// </value>
        /// <seealso cref="Full" />
        /// <seealso cref="Inner" />
        /// <seealso cref="RightOuter" />
        IJoinBuilder<TSource, TQuery> LeftOuter { get; }

        /// <summary>
        ///     Gets the <see cref="IJoinBuilder{TSource, TQuery}" /> instance for adding right outer joins.
        /// </summary>
        /// <value>
        ///     The <see cref="IJoinBuilder{TSource, TQuery}" /> instance for adding right outer joins.
        /// </value>
        /// <seealso cref="Full" />
        /// <seealso cref="Inner" />
        /// <seealso cref="LeftOuter" />
        IJoinBuilder<TSource, TQuery> RightOuter { get; }

        /// <summary>
        ///     Adds a filter to the query.
        /// </summary>
        /// <param name="property">
        ///     The property to filter.
        /// </param>
        /// <param name="expression">
        ///     The filter.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        /// <remarks>
        ///     An alias for <see cref="Where(string,IsExpression)" />.
        /// </remarks>
        TQuery And
            (
            string property,
            IsExpression expression
            );

        /// <summary>
        ///     Adds one or more filters to the query.
        /// </summary>
        /// <param name="expression">
        ///     The filters.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        /// <remarks>
        ///     An alias for <see cref="Where(Expression{Func{TSource, bool}})" />.
        /// </remarks>
        TQuery And(Expression<Func<TSource, bool>> expression);

        /// <summary>
        ///     Adds a filter to the query.
        /// </summary>
        /// <param name="property">
        ///     The property to filter.
        /// </param>
        /// <param name="expression">
        ///     The filter.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        /// <remarks>
        ///     An alias for <see cref="Where(Expression{Func{TSource, object}}, IsExpression)" />.
        /// </remarks>
        TQuery And
            (
            Expression<Func<TSource, object>> property,
            IsExpression expression
            );

        /// <summary>
        ///     Adds one or more filters to the query.
        /// </summary>
        /// <param name="expression">
        ///     The filters.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        /// <remarks>
        ///     An alias for <see cref="Where(Expression{Func{TSource, WhereDelegate, bool}})" />.
        /// </remarks>
        TQuery And(Expression<Func<TSource, WhereDelegate, bool>> expression);

        /// <summary>
        ///     Adds a subquery filter to the query.
        /// </summary>
        /// <param name="subquery">
        ///     The subquery.
        /// </param>
        /// <param name="expression">
        ///     The filter.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        /// <remarks>
        ///     An alias for <see cref="Where(IDetachedImmutableFlowQuery, Expressions.IsEmptyExpression)" />.
        /// </remarks>
        TQuery And
            (
            IDetachedImmutableFlowQuery subquery,
            IsEmptyExpression expression
            );

        /// <summary>
        ///     Adds a subquery filter to the query.
        /// </summary>
        /// <param name="subquery">
        ///     The subquery.
        /// </param>
        /// <param name="expression">
        ///     The filter.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        /// <remarks>
        ///     An alias for <see cref="Where(DetachedCriteria, IsEmptyExpression)" />.
        /// </remarks>
        TQuery And
            (
            DetachedCriteria subquery,
            IsEmptyExpression expression
            );

        /// <summary>
        ///     Applies the given <see cref="IQueryFilter{T}" /> to the root entity of this query.
        /// </summary>
        /// <param name="filter">
        ///     The <see cref="IQueryFilter{T}" /> filter to apply.
        /// </param>
        /// <returns>
        ///     The query instance.
        /// </returns>
        TQuery ApplyFilter
            (
            IQueryFilter<TSource> filter
            );

        /// <summary>
        ///     Applies the given <see cref="IQueryFilter{T}" /> to the given alias of this query.
        /// </summary>
        /// <param name="alias">
        ///     The alias on which to apply the given <see cref="IQueryFilter{T}" /> filter.
        /// </param>
        /// <param name="filter">
        ///     The <see cref="IQueryFilter{T}" /> filter to apply.
        /// </param>
        /// <typeparam name="TAlias">
        ///     The <see cref="System.Type" /> of the given alias.
        /// </typeparam>
        /// <returns>
        ///     The query instance.
        /// </returns>
        TQuery ApplyFilterOn<TAlias>
            (
            Expression<Func<TAlias>> alias,
            IQueryFilter<TAlias> filter
            );

        /// <summary>
        ///     Adds a filter to the query.
        /// </summary>
        /// <param name="property">
        ///     The property to filter.
        /// </param>
        /// <param name="expression">
        ///     The filter.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Where
            (
            string property,
            IsExpression expression
            );

        /// <summary>
        ///     Adds one or more filters to the query.
        /// </summary>
        /// <param name="expression">
        ///     The filters.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Where(Expression<Func<TSource, bool>> expression);

        /// <summary>
        ///     Adds a filter to the query.
        /// </summary>
        /// <param name="property">
        ///     The property to filter.
        /// </param>
        /// <param name="expression">
        ///     The filter.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Where
            (
            Expression<Func<TSource, object>> property,
            IsExpression expression
            );

        /// <summary>
        ///     Adds one or more filters to the query.
        /// </summary>
        /// <param name="expression">
        ///     The filters.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Where(Expression<Func<TSource, WhereDelegate, bool>> expression);

        /// <summary>
        ///     Adds a subquery filter to the query.
        /// </summary>
        /// <param name="subquery">
        ///     The subquery.
        /// </param>
        /// <param name="expression">
        ///     The filter.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Where
            (
            IDetachedImmutableFlowQuery subquery,
            IsEmptyExpression expression
            );

        /// <summary>
        ///     Adds a subquery filter to the query.
        /// </summary>
        /// <param name="subquery">
        ///     The subquery.
        /// </param>
        /// <param name="expression">
        ///     The filter.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        TQuery Where
            (
            DetachedCriteria subquery,
            IsEmptyExpression expression
            );
    }
}