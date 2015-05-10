namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Expressions;
    using NHibernate.FlowQuery.Helpers;

    using IsEmptyExpression = NHibernate.FlowQuery.Expressions.IsEmptyExpression;

    /// <summary>
    ///     A class wrapping a filterable query.
    /// </summary>
    /// <typeparam name="T">
    ///     The <see cref="System.Type" /> of the alias.
    /// </typeparam>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity to be queried.
    /// </typeparam>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the wrapped query.
    /// </typeparam>
    public class DelegatedFilterableQuery<T, TSource, TQuery> : IFilterableQuery<T>
        where TQuery : class, IFilterableQuery<TSource, TQuery>
    {
        /// <summary>
        ///     The name of the alias to delegate to.
        /// </summary>
        private readonly string _alias;

        /// <summary>
        ///     The wrapped query instance.
        /// </summary>
        private readonly TQuery _query;

        /// <summary>
        ///     The expression re-baser.
        /// </summary>
        private readonly ExpressionRebaser _rebaser;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DelegatedFilterableQuery{T,TSource,TQuery}" /> class.
        /// </summary>
        /// <param name="query">
        ///     The wrapped query instance.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The wrapped query instance is a null-reference.
        /// </exception>
        public DelegatedFilterableQuery
            (
            TQuery query,
            string alias
            )
        {
            if (string.IsNullOrWhiteSpace(alias))
            {
                throw new ArgumentException("alias is null or white-space", "alias");
            }

            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            _alias = alias;
            _query = query;

            _rebaser = new ExpressionRebaser(typeof(T), _alias);
        }

        /// <inheritdoc />
        public IJoinBuilder<T, IFilterableQuery<T>> Full
        {
            get
            {
                return new DelegatedJoinBuilder<T, TSource, TQuery>(_query.Full, this, _alias);
            }
        }

        /// <inheritdoc />
        public IJoinBuilder<T, IFilterableQuery<T>> Inner
        {
            get
            {
                return new DelegatedJoinBuilder<T, TSource, TQuery>(_query.Inner, this, _alias);
            }
        }

        /// <inheritdoc />
        public IJoinBuilder<T, IFilterableQuery<T>> LeftOuter
        {
            get
            {
                return new DelegatedJoinBuilder<T, TSource, TQuery>(_query.LeftOuter, this, _alias);
            }
        }

        /// <inheritdoc />
        public IJoinBuilder<T, IFilterableQuery<T>> RightOuter
        {
            get
            {
                return new DelegatedJoinBuilder<T, TSource, TQuery>(_query.RightOuter, this, _alias);
            }
        }

        /// <inheritdoc />
        public IFilterableQuery<T> And
            (
            string property,
            IsExpression expression
            )
        {
            if (!property.Contains("."))
            {
                property = _alias + "." + property;
            }

            _query.And(property, expression);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<T> And(Expression<Func<T, bool>> expression)
        {
            Expression<Func<TSource, bool>> temp = _rebaser.RebaseTo<TSource, bool>(expression);

            _query.And(temp);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<T> And
            (
            Expression<Func<T, object>> property,
            IsExpression expression
            )
        {
            Expression<Func<TSource, object>> temp = _rebaser.RebaseTo<TSource, object>(property);

            _query.And(temp, expression);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<T> And(Expression<Func<T, WhereDelegate, bool>> expression)
        {
            Expression<Func<TSource, WhereDelegate, bool>> temp =
                _rebaser.RebaseTo<TSource, WhereDelegate, bool>(expression);

            _query.And(temp);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<T> And
            (
            IDetachedImmutableFlowQuery subquery,
            IsEmptyExpression expression
            )
        {
            _query.And(subquery, expression);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<T> And
            (
            DetachedCriteria subquery,
            IsEmptyExpression expression
            )
        {
            _query.And(subquery, expression);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<T> ApplyFilter(IQueryFilter<T> filter)
        {
            IFilterableQuery<T> filterableQuery = new WrappedFilterableQuery<T, IFilterableQuery<T>>(this);

            filter.Apply(filterableQuery);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<T> ApplyFilterOn<TAlias>
            (
            Expression<Func<TAlias>> alias,
            IQueryFilter<TAlias> filter
            )
        {
            _query.ApplyFilterOn(alias, filter);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<T> Where
            (
            string property,
            IsExpression expression
            )
        {
            if (!property.Contains("."))
            {
                property = _alias + "." + property;
            }

            _query.Where(property, expression);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<T> Where(Expression<Func<T, bool>> expression)
        {
            Expression<Func<TSource, bool>> temp = _rebaser.RebaseTo<TSource, bool>(expression);

            _query.Where(temp);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<T> Where
            (
            Expression<Func<T, object>> property,
            IsExpression expression
            )
        {
            Expression<Func<TSource, object>> temp = _rebaser.RebaseTo<TSource, object>(property);

            _query.Where(temp, expression);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<T> Where(Expression<Func<T, WhereDelegate, bool>> expression)
        {
            Expression<Func<TSource, WhereDelegate, bool>> temp =
                _rebaser.RebaseTo<TSource, WhereDelegate, bool>(expression);

            _query.Where(temp);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<T> Where
            (
            IDetachedImmutableFlowQuery subquery,
            IsEmptyExpression expression
            )
        {
            _query.Where(subquery, expression);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<T> Where
            (
            DetachedCriteria subquery,
            IsEmptyExpression expression
            )
        {
            _query.Where(subquery, expression);

            return this;
        }
    }
}