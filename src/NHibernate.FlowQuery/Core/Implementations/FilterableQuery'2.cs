namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Expressions;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.SqlCommand;

    using IsEmptyExpression = NHibernate.FlowQuery.Expressions.IsEmptyExpression;

    /// <summary>
    ///     A class implementing the basic functionality of a filterable <see cref="NHibernate.FlowQuery" /> query.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity to be queried.
    /// </typeparam>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the class or interface implementing or extending this interface.
    /// </typeparam>
    public abstract class FilterableQuery<TSource, TQuery> : FilterableQueryBase, IFilterableQuery<TSource, TQuery>
        where TQuery : class, IFilterableQuery<TSource, TQuery>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FilterableQuery{TSource,TQuery}" /> class.
        /// </summary>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="query">
        ///     The query.
        /// </param>
        protected FilterableQuery
            (
            string alias = null,
            IFilterableQuery query = null
            )
            : base(alias, query)
        {
            Query = this as TQuery;

            if (Query == null)
            {
                throw new ArgumentException("The provided TQuery must the type of this instance");
            }

            Inner = new JoinBuilder<TSource, TQuery>(this, Query, JoinType.InnerJoin);
            LeftOuter = new JoinBuilder<TSource, TQuery>(this, Query, JoinType.LeftOuterJoin);
            RightOuter = new JoinBuilder<TSource, TQuery>(this, Query, JoinType.RightOuterJoin);
            Full = new JoinBuilder<TSource, TQuery>(this, Query, JoinType.FullJoin);
        }

        /// <inheritdoc />
        public IJoinBuilder<TSource, TQuery> Full { get; private set; }

        /// <inheritdoc />
        public IJoinBuilder<TSource, TQuery> Inner { get; private set; }

        /// <inheritdoc />
        public IJoinBuilder<TSource, TQuery> LeftOuter { get; private set; }

        /// <inheritdoc />
        public IJoinBuilder<TSource, TQuery> RightOuter { get; private set; }

        /// <summary>
        ///     Gets the query.
        /// </summary>
        /// <value>
        ///     The query.
        /// </value>
        protected TQuery Query { get; private set; }

        /// <inheritdoc />
        public virtual TQuery And
            (
            IDetachedImmutableFlowQuery subquery,
            IsEmptyExpression expression
            )
        {
            return Where(subquery, expression);
        }

        /// <inheritdoc />
        public virtual TQuery And
            (
            DetachedCriteria subquery,
            IsEmptyExpression expression
            )
        {
            return Where(subquery, expression);
        }

        /// <inheritdoc />
        public virtual TQuery And(params ICriterion[] criterions)
        {
            return Where(criterions);
        }

        /// <inheritdoc />
        public virtual TQuery And
            (
            string property,
            IsExpression expression
            )
        {
            return Where(property, expression);
        }

        /// <inheritdoc />
        public virtual TQuery And(Expression<Func<TSource, bool>> expression)
        {
            return Where(expression);
        }

        /// <inheritdoc />
        public virtual TQuery And
            (
            Expression<Func<TSource, object>> property,
            IsExpression expression
            )
        {
            return Where(property, expression);
        }

        /// <inheritdoc />
        public virtual TQuery And(Expression<Func<TSource, WhereDelegate, bool>> expression)
        {
            return Where(expression);
        }

        /// <inheritdoc />
        public TQuery ApplyFilterOn<TAlias>
            (
            Expression<Func<TAlias>> alias,
            IQueryFilter<TAlias> filter
            )
        {
            string aliasName = ExpressionHelper.GetPropertyName(alias);

            IFilterableQuery<TAlias> filterableQuery =
                new DelegatedFilterableQuery<TAlias, TSource, TQuery>(Query, aliasName);

            filter.Apply(filterableQuery);

            return Query;
        }

        /// <inheritdoc />
        public TQuery ApplyFilter(IQueryFilter<TSource> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            IFilterableQuery<TSource> filterableQuery = new WrappedFilterableQuery<TSource, TQuery>(Query);

            filter.Apply(filterableQuery);

            return Query;
        }

        /// <inheritdoc />
        public virtual TQuery Where(params ICriterion[] criterions)
        {
            if (criterions == null)
            {
                throw new ArgumentNullException("criterions");
            }

            foreach (ICriterion criterion in criterions.Where(x => x != null))
            {
                Criterions.Add(criterion);
            }

            return Query;
        }

        /// <inheritdoc />
        public virtual TQuery Where
            (
            string property,
            IsExpression expression
            )
        {
            ICriterion criterion = expression.Compile(property);

            return Where(criterion);
        }

        /// <inheritdoc />
        public virtual TQuery Where(Expression<Func<TSource, bool>> expression)
        {
            ICriterion filter = RestrictionHelper
                .GetCriterion
                (
                    expression,
                    new HelperContext(Data, expression, HelperType.Filter)
                );

            return Where(filter);
        }

        /// <inheritdoc />
        public virtual TQuery Where
            (
            Expression<Func<TSource, object>> property,
            IsExpression expression
            )
        {
            return Where(ExpressionHelper.GetPropertyName(property), expression);
        }

        /// <inheritdoc />
        public virtual TQuery Where(Expression<Func<TSource, WhereDelegate, bool>> expression)
        {
            ICriterion filter = RestrictionHelper
                .GetCriterion
                (
                    expression,
                    new HelperContext(Data, expression, HelperType.Filter)
                );

            return Where(filter);
        }

        /// <inheritdoc />
        public virtual TQuery Where
            (
            IDetachedImmutableFlowQuery subquery,
            IsEmptyExpression expression
            )
        {
            return Where(subquery.Criteria, expression);
        }

        /// <inheritdoc />
        public virtual TQuery Where
            (
            DetachedCriteria subquery,
            IsEmptyExpression expression
            )
        {
            ICriterion criterion = expression.Compile(subquery);

            return Where(criterion);
        }
    }
}