namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Expressions;
    using NHibernate.FlowQuery.Helpers;

    /// <summary>
    ///     A class used for immediate queries.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity for this query.
    /// </typeparam>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the class or interface implementing or extending this interface.
    /// </typeparam>
    public abstract class ImmediateFlowQueryBase<TSource, TQuery>
        : QueryableFlowQueryBase<TSource, TQuery>,
          IImmediateFlowQueryBase<TSource, TQuery>
        where TSource : class
        where TQuery : class, IImmediateFlowQueryBase<TSource, TQuery>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ImmediateFlowQueryBase{TSource,TQuery}" /> class.
        /// </summary>
        /// <param name="criteriaFactory">
        ///     The criteria factory.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="criteriaFactory" /> is null.
        /// </exception>
        protected ImmediateFlowQueryBase
            (
            Func<Type, string, ICriteria> criteriaFactory,
            string alias = null,
            FlowQueryOptions options = null,
            IMorphableFlowQuery query = null
            )
            : base(criteriaFactory, alias, options, query)
        {
        }

        /// <inheritdoc />
        public override bool IsDelayed
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc />
        public virtual bool Any()
        {
            int count = Take(1).Count();

            return count > 0;
        }

        /// <inheritdoc />
        public virtual bool Any(params ICriterion[] criterions)
        {
            return Where(criterions).Any();
        }

        /// <inheritdoc />
        public virtual bool Any(string property, IsExpression expression)
        {
            return Where(property, expression).Any();
        }

        /// <inheritdoc />
        public virtual bool Any(Expression<Func<TSource, bool>> expression)
        {
            return Where(expression).Any();
        }

        /// <inheritdoc />
        public virtual bool Any(Expression<Func<TSource, object>> property, IsExpression expression)
        {
            return Where(property, expression).Any();
        }

        /// <inheritdoc />
        public virtual bool Any(Expression<Func<TSource, WhereDelegate, bool>> expression)
        {
            return Where(expression).Any();
        }

        /// <inheritdoc />
        public abstract TQuery Copy();

        /// <inheritdoc />
        public virtual int Count()
        {
            Projection = Projections.RowCount();

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return SelectImmediateValue<int>();
        }

        /// <inheritdoc />
        public virtual int Count(string property)
        {
            Projection = IsDistinct
                ? Projections.CountDistinct(property)
                : Projections.Count(property);

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return SelectImmediateValue<int>();
        }

        /// <inheritdoc />
        public virtual int Count(IProjection projection)
        {
            Projection = Projections.Count(projection);

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return SelectImmediateValue<int>();
        }

        /// <inheritdoc />
        public virtual int Count(Expression<Func<TSource, object>> property)
        {
            string propertyName = ExpressionHelper.GetPropertyName(property);

            return Count(propertyName);
        }

        /// <inheritdoc />
        public virtual long CountLong()
        {
            Projection = Projections.RowCountInt64();

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return SelectImmediateValue<long>();
        }
    }
}