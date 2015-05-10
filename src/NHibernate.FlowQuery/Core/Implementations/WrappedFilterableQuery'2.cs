namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Expressions;

    using IsEmptyExpression = NHibernate.FlowQuery.Expressions.IsEmptyExpression;

    /// <summary>
    ///     A class wrapping a filterable query.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity to be queried.
    /// </typeparam>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the wrapped query.
    /// </typeparam>
    public class WrappedFilterableQuery<TSource, TQuery> : IFilterableQuery<TSource>
        where TQuery : class, IFilterableQuery<TSource, TQuery>
    {
        /// <summary>
        ///     The wrapped query instance.
        /// </summary>
        private readonly TQuery _query;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WrappedFilterableQuery{TSource,TQuery}" /> class.
        /// </summary>
        /// <param name="query">
        ///     The wrapped query instance.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The wrapped query instance is a null-reference.
        /// </exception>
        public WrappedFilterableQuery(TQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            _query = query;
        }

        /// <inheritdoc />
        public IJoinBuilder<TSource, IFilterableQuery<TSource>> Full
        {
            get
            {
                return new WrappedJoinBuilder<TSource, TQuery>(_query.Full, this);
            }
        }

        /// <inheritdoc />
        public IJoinBuilder<TSource, IFilterableQuery<TSource>> Inner
        {
            get
            {
                return new WrappedJoinBuilder<TSource, TQuery>(_query.Inner, this);
            }
        }

        /// <inheritdoc />
        public IJoinBuilder<TSource, IFilterableQuery<TSource>> LeftOuter
        {
            get
            {
                return new WrappedJoinBuilder<TSource, TQuery>(_query.LeftOuter, this);
            }
        }

        /// <inheritdoc />
        public IJoinBuilder<TSource, IFilterableQuery<TSource>> RightOuter
        {
            get
            {
                return new WrappedJoinBuilder<TSource, TQuery>(_query.RightOuter, this);
            }
        }

        /// <inheritdoc />
        public IFilterableQuery<TSource> And
            (
            string property,
            IsExpression expression
            )
        {
            _query.And(property, expression);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<TSource> And(Expression<Func<TSource, bool>> expression)
        {
            _query.And(expression);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<TSource> And
            (
            Expression<Func<TSource, object>> property,
            IsExpression expression
            )
        {
            _query.And(property, expression);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<TSource> And(Expression<Func<TSource, WhereDelegate, bool>> expression)
        {
            _query.And(expression);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<TSource> And
            (
            IDetachedImmutableFlowQuery subquery,
            IsEmptyExpression expression
            )
        {
            _query.And(subquery, expression);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<TSource> And
            (
            DetachedCriteria subquery,
            IsEmptyExpression expression
            )
        {
            _query.And(subquery, expression);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<TSource> ApplyFilterOn<TAlias>
            (
            Expression<Func<TAlias>> alias,
            IQueryFilter<TAlias> filter
            )
        {
            _query.ApplyFilterOn(alias, filter);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<TSource> ApplyFilter(IQueryFilter<TSource> filter)
        {
            _query.ApplyFilter(filter);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<TSource> Where
            (
            string property,
            IsExpression expression
            )
        {
            _query.Where(property, expression);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<TSource> Where(Expression<Func<TSource, bool>> expression)
        {
            _query.Where(expression);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<TSource> Where
            (
            Expression<Func<TSource, object>> property,
            IsExpression expression
            )
        {
            _query.Where(property, expression);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<TSource> Where(Expression<Func<TSource, WhereDelegate, bool>> expression)
        {
            _query.Where(expression);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<TSource> Where
            (
            IDetachedImmutableFlowQuery subquery,
            IsEmptyExpression expression
            )
        {
            _query.Where(subquery, expression);

            return this;
        }

        /// <inheritdoc />
        public IFilterableQuery<TSource> Where
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