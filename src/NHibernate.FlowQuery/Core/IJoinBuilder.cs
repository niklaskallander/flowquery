namespace NHibernate.FlowQuery.Core
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Revealing.Conventions;

    /// <summary>
    ///     A helper utility used to create joins with a nice syntax.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the source for the query used to create this 
    ///     <see cref="IJoinBuilder{TSource, TQuery}" /> instance.
    /// </typeparam>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the underlying query for this <see cref="IJoinBuilder{TSource, TQuery}" />
    ///     instance.
    /// </typeparam>
    public interface IJoinBuilder<TSource, out TQuery>
    {
        /// <summary>
        ///     Joins the specified property or association path with the provided alias.
        /// </summary>
        /// <param name="property">
        ///     The property or association path to join.
        /// </param>
        /// <param name="alias">
        ///     The alias to use for the join.
        /// </param>
        /// <typeparam name="TAlias">
        ///     The <see cref="System.Type" /> of the provided property or association path.
        /// </typeparam>
        /// <returns>
        ///     The underlying <see cref="IFlowQuery{TSource, TQuery}" /> query.
        /// </returns>
        TQuery Join<TAlias>(string property, Expression<Func<TAlias>> alias);

        /// <summary>
        ///     Joins the specified property or association path with the provided alias and with the extra filter.
        /// </summary>
        /// <param name="property">
        ///     The property or association path to join.
        /// </param>
        /// <param name="alias">
        ///     The alias to use for the join.
        /// </param>
        /// <param name="joinOnClause">
        ///     An extra filter for the join.
        /// </param>
        /// <typeparam name="TAlias">
        ///     The <see cref="System.Type" /> of the provided property or association path.
        /// </typeparam>
        /// <returns>
        ///     The underlying <see cref="IFlowQuery{TSource, TQuery}" /> query.
        /// </returns>
        /// <remarks>
        ///     <paramref name="joinOnClause" /> does not have an alias parameter for the root entity of the query. This
        ///     is due to a design detail in <see cref="NHibernate" />. Instead, specify an explicit alias for the root
        ///     entity and use that alias instead.
        /// </remarks>
        TQuery Join<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause);

        /// <summary>
        ///     Joins the specified property or association path with the provided alias.
        /// </summary>
        /// <param name="projection">
        ///     The property or association path to join.
        /// </param>
        /// <param name="alias">
        ///     The alias to use for the join.
        /// </param>
        /// <typeparam name="TAlias">
        ///     The <see cref="System.Type" /> of the provided property or association path.
        /// </typeparam>
        /// <returns>
        ///     The underlying <see cref="IFlowQuery{TSource, TQuery}" /> query.
        /// </returns>
        TQuery Join<TAlias>(Expression<Func<TSource, object>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        ///     Joins the specified property or association path with the provided alias.
        /// </summary>
        /// <param name="projection">
        ///     The property or association path to join.
        /// </param>
        /// <param name="alias">
        ///     The alias to use for the join.
        /// </param>
        /// <param name="revealConvention">
        ///     A convention used to reveal a private or protected member of <see cref="T:TSource" /> from a public
        ///     property (in case a backing field is mapped instead of a public property).
        /// </param>
        /// <typeparam name="TAlias">
        ///     The <see cref="System.Type" /> of the provided property or association path.
        /// </typeparam>
        /// <returns>
        ///     The underlying <see cref="IFlowQuery{TSource, TQuery}" /> query.
        /// </returns>
        TQuery Join<TAlias>
            (
            Expression<Func<TSource, object>> projection,
            Expression<Func<TAlias>> alias,
            IRevealConvention revealConvention
            );

        /// <summary>
        ///     Joins the specified property or association path with the provided alias and with the extra filter.
        /// </summary>
        /// <param name="projection">
        ///     The property or association path to join.
        /// </param>
        /// <param name="alias">
        ///     The alias to use for the join.
        /// </param>
        /// <param name="joinOnClause">
        ///     An extra filter for the join.
        /// </param>
        /// <typeparam name="TAlias">
        ///     The <see cref="System.Type" /> of the provided property or association path.
        /// </typeparam>
        /// <returns>
        ///     The underlying <see cref="IFlowQuery{TSource, TQuery}" /> query.
        /// </returns>
        /// <remarks>
        ///     <paramref name="joinOnClause" /> does not have an alias parameter for the root entity of the query. This
        ///     is due to a design detail in <see cref="NHibernate" />. Instead, specify an explicit alias for the root
        ///     entity and use that alias instead.
        /// </remarks>
        TQuery Join<TAlias>
            (
            Expression<Func<TSource, object>> projection,
            Expression<Func<TAlias>> alias,
            Expression<Func<bool>> joinOnClause
            );

        /// <summary>
        ///     Joins the specified property or association path with the provided alias and with the extra filter.
        /// </summary>
        /// <param name="projection">
        ///     The property or association path to join.
        /// </param>
        /// <param name="alias">
        ///     The alias to use for the join.
        /// </param>
        /// <param name="joinOnClause">
        ///     An extra filter for the join.
        /// </param>
        /// <param name="revealConvention">
        ///     A convention used to reveal a private or protected member of <see cref="T:TSource" /> from a public
        ///     property (in case a backing field is mapped instead of a public property).
        /// </param>
        /// <typeparam name="TAlias">
        ///     The <see cref="System.Type" /> of the provided property or association path.
        /// </typeparam>
        /// <returns>
        ///     The underlying <see cref="IFlowQuery{TSource, TQuery}" /> query.
        /// </returns>
        /// <remarks>
        ///     <paramref name="joinOnClause" /> does not have an alias parameter for the root entity of the query. This
        ///     is due to a design detail in <see cref="NHibernate" />. Instead, specify an explicit alias for the root
        ///     entity and use that alias instead.
        /// </remarks>
        TQuery Join<TAlias>
            (
            Expression<Func<TSource, object>> projection,
            Expression<Func<TAlias>> alias,
            Expression<Func<bool>> joinOnClause,
            IRevealConvention revealConvention
            );
    }
}