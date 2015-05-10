namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Helpers;
    using NHibernate.FlowQuery.Revealing.Conventions;

    /// <summary>
    ///     A helper utility used to create joins with a nice syntax.
    /// </summary>
    /// <typeparam name="T">
    ///     The <see cref="System.Type" /> of the alias.
    /// </typeparam>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the source for the query used to create this
    ///     <see cref="JoinBuilder{TSource, TQuery}" /> instance.
    /// </typeparam>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the underlying query for this <see cref="JoinBuilder{TSource, TQuery}" />
    ///     instance.
    /// </typeparam>
    public class DelegatedJoinBuilder<T, TSource, TQuery> : IJoinBuilder<T, IFilterableQuery<T>>
    {
        /// <summary>
        ///     The alias.
        /// </summary>
        private readonly string _alias;

        /// <summary>
        ///     The wrapped join builder instance.
        /// </summary>
        private readonly IJoinBuilder<TSource, TQuery> _joinBuilder;

        /// <summary>
        ///     The query instance.
        /// </summary>
        private readonly IFilterableQuery<T> _query;

        /// <summary>
        ///     The expression re-baser.
        /// </summary>
        private readonly ExpressionRebaser _rebaser;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DelegatedJoinBuilder{T,TSource,TQuery}" /> class.
        /// </summary>
        /// <param name="joinBuilder">
        ///     The wrapped join builder instance.
        /// </param>
        /// <param name="query">
        ///     The query instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The wrapped join builder instance is a null-reference.
        /// </exception>
        public DelegatedJoinBuilder
            (
            IJoinBuilder<TSource, TQuery> joinBuilder,
            IFilterableQuery<T> query,
            string alias
            )
        {
            if (joinBuilder == null)
            {
                throw new ArgumentNullException("joinBuilder");
            }

            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            _alias = alias;

            _joinBuilder = joinBuilder;

            _query = query;

            _rebaser = new ExpressionRebaser(typeof(T), _alias);
        }

        /// <inheritdoc />
        public IFilterableQuery<T> Join<TAlias>
            (
            string property,
            Expression<Func<TAlias>> alias
            )
        {
            if (!property.Contains("."))
            {
                property = _alias + "." + property;
            }

            _joinBuilder.Join(property, alias);

            return _query;
        }

        /// <inheritdoc />
        public IFilterableQuery<T> Join<TAlias>
            (
            string property,
            Expression<Func<TAlias>> alias,
            Expression<Func<bool>> joinOnClause
            )
        {
            if (!property.Contains("."))
            {
                property = _alias + "." + property;
            }

            _joinBuilder.Join(property, alias, joinOnClause);

            return _query;
        }

        /// <inheritdoc />
        public IFilterableQuery<T> Join<TAlias>
            (
            Expression<Func<T, object>> projection,
            Expression<Func<TAlias>> alias
            )
        {
            var temp = _rebaser.RebaseTo<TSource, object>(projection);

            _joinBuilder.Join(temp, alias);

            return _query;
        }

        /// <inheritdoc />
        public IFilterableQuery<T> Join<TAlias>
            (
            Expression<Func<T, object>> projection,
            Expression<Func<TAlias>> alias,
            IRevealConvention revealConvention
            )
        {
            var temp = _rebaser.RebaseTo<TSource, object>(projection);

            _joinBuilder.Join(temp, alias, revealConvention);

            return _query;
        }

        /// <inheritdoc />
        public IFilterableQuery<T> Join<TAlias>
            (
            Expression<Func<T, object>> projection,
            Expression<Func<TAlias>> alias,
            Expression<Func<bool>> joinOnClause
            )
        {
            var temp = _rebaser.RebaseTo<TSource, object>(projection);

            _joinBuilder.Join(temp, alias, joinOnClause);

            return _query;
        }

        /// <inheritdoc />
        public IFilterableQuery<T> Join<TAlias>
            (
            Expression<Func<T, object>> projection,
            Expression<Func<TAlias>> alias,
            Expression<Func<bool>> joinOnClause,
            IRevealConvention revealConvention
            )
        {
            var temp = _rebaser.RebaseTo<TSource, object>(projection);

            _joinBuilder.Join(temp, alias, joinOnClause, revealConvention);

            return _query;
        }
    }
}