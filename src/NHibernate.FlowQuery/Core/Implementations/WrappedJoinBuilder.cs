namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Revealing.Conventions;

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
    public class WrappedJoinBuilder<TSource, TQuery> : IJoinBuilder<TSource, IFilterableQuery<TSource>>
    {
        /// <summary>
        ///     The wrapped join builder instance.
        /// </summary>
        private readonly IJoinBuilder<TSource, TQuery> _joinBuilder;

        /// <summary>
        ///     The query instance.
        /// </summary>
        private readonly IFilterableQuery<TSource> _query;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WrappedJoinBuilder{TSource,TQuery}" /> class.
        /// </summary>
        /// <param name="joinBuilder">
        ///     The wrapped join builder instance.
        /// </param>
        /// <param name="query">
        ///     The query instance.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The wrapped join builder instance is a null-reference.
        /// </exception>
        public WrappedJoinBuilder
            (
            IJoinBuilder<TSource, TQuery> joinBuilder,
            IFilterableQuery<TSource> query
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

            _joinBuilder = joinBuilder;

            _query = query;
        }

        /// <inheritdoc />
        public IFilterableQuery<TSource> Join<TAlias>
            (
            string property,
            Expression<Func<TAlias>> alias
            )
        {
            _joinBuilder.Join(property, alias);

            return _query;
        }

        /// <inheritdoc />
        public IFilterableQuery<TSource> Join<TAlias>
            (
            string property,
            Expression<Func<TAlias>> alias,
            Expression<Func<bool>> joinOnClause
            )
        {
            _joinBuilder.Join(property, alias, joinOnClause);

            return _query;
        }

        /// <inheritdoc />
        public IFilterableQuery<TSource> Join<TAlias>
            (
            Expression<Func<TSource, object>> projection,
            Expression<Func<TAlias>> alias
            )
        {
            _joinBuilder.Join(projection, alias);

            return _query;
        }

        /// <inheritdoc />
        public IFilterableQuery<TSource> Join<TAlias>
            (
            Expression<Func<TSource, object>> projection,
            Expression<Func<TAlias>> alias,
            IRevealConvention revealConvention
            )
        {
            _joinBuilder.Join(projection, alias, revealConvention);

            return _query;
        }

        /// <inheritdoc />
        public IFilterableQuery<TSource> Join<TAlias>
            (
            Expression<Func<TSource, object>> projection,
            Expression<Func<TAlias>> alias,
            Expression<Func<bool>> joinOnClause
            )
        {
            _joinBuilder.Join(projection, alias, joinOnClause);

            return _query;
        }

        /// <inheritdoc />
        public IFilterableQuery<TSource> Join<TAlias>
            (
            Expression<Func<TSource, object>> projection,
            Expression<Func<TAlias>> alias,
            Expression<Func<bool>> joinOnClause,
            IRevealConvention revealConvention
            )
        {
            _joinBuilder.Join(projection, alias, joinOnClause, revealConvention);

            return _query;
        }
    }
}