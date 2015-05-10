namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Collections;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core.Structures;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Revealing.Conventions;
    using NHibernate.SqlCommand;

    /// <summary>
    ///     A helper utility used to create joins with a nice syntax.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the source for the query used to create this 
    ///     <see cref="JoinBuilder{TSource, TQuery}" /> instance.
    /// </typeparam>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the underlying query for this <see cref="JoinBuilder{TSource, TQuery}" /> 
    ///     instance.
    /// </typeparam>
    public class JoinBuilder<TSource, TQuery> : IJoinBuilder<TSource, TQuery>
        where TQuery : class
    {
        /// <summary>
        ///     The query instance.
        /// </summary>
        private readonly IFilterableQuery _implementor;

        /// <summary>
        ///     The join type.
        /// </summary>
        private readonly JoinType _joinType;

        /// <summary>
        ///     The query instance.
        /// </summary>
        private readonly TQuery _query;

        /// <summary>
        ///     Initializes a new instance of the <see cref="JoinBuilder{TSource,TQuery}"/> class.
        /// </summary>
        /// <param name="implementor">
        ///     The query instance.
        /// </param>
        /// <param name="query">
        ///     The query instance again.
        /// </param>
        /// <param name="joinType">
        ///     The join type.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="implementor" /> and/or <paramref name="query" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="implementor" /> and <paramref name="query" /> is not the same object reference.
        /// </exception>
        protected internal JoinBuilder
            (
            IFilterableQuery implementor,
            TQuery query,
            JoinType joinType
            )
        {
            if (implementor == null)
            {
                throw new ArgumentNullException("implementor");
            }

            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            if (!ReferenceEquals(implementor, query))
            {
                throw new ArgumentException("implementor and query must be the same reference", "query");
            }

            _implementor = implementor;

            _joinType = joinType;

            _query = query;
        }

        /// <inheritdoc />
        public virtual TQuery Join<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return JoinBase(property, alias);
        }

        /// <inheritdoc />
        public virtual TQuery Join<TAlias>
            (
            string property,
            Expression<Func<TAlias>> alias,
            Expression<Func<bool>> joinOnClause
            )
        {
            return JoinBase(property, alias, true, joinOnClause);
        }

        /// <inheritdoc />
        public virtual TQuery Join<TAlias>(Expression<Func<TSource, object>> projection, Expression<Func<TAlias>> alias)
        {
            return JoinBase(projection, alias);
        }

        /// <inheritdoc />
        public virtual TQuery Join<TAlias>
            (
            Expression<Func<TSource, object>> projection,
            Expression<Func<TAlias>> alias,
            Expression<Func<bool>> joinOnClause
            )
        {
            return JoinBase(projection, alias, joinOnClause);
        }

        /// <inheritdoc />
        public virtual TQuery Join<TAlias>
            (
            Expression<Func<TSource, object>> projection,
            Expression<Func<TAlias>> alias,
            IRevealConvention revealConvention
            )
        {
            return JoinBaseWithConvention(projection, alias, null, revealConvention);
        }

        /// <inheritdoc />
        public virtual TQuery Join<TAlias>
            (
            Expression<Func<TSource, object>> projection,
            Expression<Func<TAlias>> alias,
            Expression<Func<bool>> joinOnClause,
            IRevealConvention revealConvention
            )
        {
            return JoinBaseWithConvention(projection, alias, joinOnClause, revealConvention);
        }

        /// <summary>
        ///     Joins the specified property or association path with the provided alias with an optional extra filter.
        /// </summary>
        /// <param name="property">
        ///     The property or association path to join.
        /// </param>
        /// <param name="aliasProjection">
        ///     The alias to use for the join.
        /// </param>
        /// <param name="isCollection">
        ///     The property or association path is a collection.
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
        /// <exception cref="InvalidOperationException">
        ///     The property or association path specified by <paramref name="property"/> is already joined/aliased, or
        ///     the alias specified by <paramref name="aliasProjection" /> is already used.
        /// </exception>
        protected virtual TQuery JoinBase<TAlias>
            (
            string property,
            Expression<Func<TAlias>> aliasProjection,
            bool isCollection = true,
            Expression<Func<bool>> joinOnClause = null
            )
        {
            string alias = ExpressionHelper.GetPropertyName(aliasProjection, true);

            if (_implementor.Aliases.ContainsKey(property))
            {
                if (_implementor.Aliases[property] == alias)
                {
                    // already exists
                    return _query;
                }

                throw new InvalidOperationException("Property already aliased");
            }

            if (_implementor.Aliases.ContainsValue(alias))
            {
                throw new InvalidOperationException("Alias already in use");
            }

            _implementor.Aliases.Add(property, alias);

            ICriterion withCriterion = null;

            if (joinOnClause != null)
            {
                withCriterion = RestrictionHelper
                    .GetCriterion
                    (
                        joinOnClause,
                        new HelperContext(_implementor.Data, joinOnClause, HelperType.Filter)
                    );
            }

            _implementor.Joins.Add(new Join(property, alias, _joinType, withCriterion, isCollection));

            return _query;
        }

        /// <summary>
        ///     Joins the specified property or association path with the provided alias with an optional extra filter.
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
        protected virtual TQuery JoinBase<TAlias>
            (
            LambdaExpression projection,
            Expression<Func<TAlias>> alias,
            Expression<Func<bool>> joinOnClause = null
            )
        {
            string property = ExpressionHelper.GetPropertyName(projection);

            return JoinBase(property, alias, typeof(IEnumerable).IsAssignableFrom(projection.Body.Type), joinOnClause);
        }

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
        protected virtual TQuery JoinBaseWithConvention<TAlias>
            (
            Expression<Func<TSource, object>> projection,
            Expression<Func<TAlias>> alias,
            Expression<Func<bool>> joinOnClause = null,
            IRevealConvention revealConvention = null
            )
        {
            if (revealConvention == null)
            {
                revealConvention = Reveal.DefaultConvention ?? new CustomConvention(x => x);
            }

            return JoinBase
            (
                Reveal.ByConvention(projection, revealConvention),
                alias,
                typeof(IEnumerable).IsAssignableFrom(projection.Body.Type),
                joinOnClause
            );
        }
    }
}