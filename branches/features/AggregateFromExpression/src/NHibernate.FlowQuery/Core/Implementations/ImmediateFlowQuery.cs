namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core.Structures;
    using NHibernate.FlowQuery.Expressions;
    using NHibernate.FlowQuery.Helpers;

    /// <summary>
    ///     A class used for immediate queries.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity for this query.
    /// </typeparam>
    public class ImmediateFlowQuery<TSource>
        : QueryableFlowQueryBase<TSource, IImmediateFlowQuery<TSource>>, IImmediateFlowQuery<TSource>
        where TSource : class
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ImmediateFlowQuery{TSource}" /> class.
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
        protected internal ImmediateFlowQuery
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
        public virtual IImmediateFlowQuery<TSource> Copy()
        {
            return new ImmediateFlowQuery<TSource>(CriteriaFactory, Alias, Options, this);
        }

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
            string propertyName = ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name);

            return Count(propertyName);
        }

        /// <inheritdoc />
        public virtual long CountLong()
        {
            Projection = Projections.RowCountInt64();

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return SelectImmediateValue<long>();
        }

        /// <inheritdoc />
        public virtual Dictionary<TKey, TValue> SelectDictionary<TKey, TValue>
            (
            Expression<Func<TSource, TKey>> key,
            Expression<Func<TSource, TValue>> value
            )
        {
            ISelectSetup<TSource, Pair<TKey, TValue>> setup = Select<Pair<TKey, TValue>>()
                .For(x => x.Key).Use(key)
                .For(x => x.Value).Use(value);

            Project(setup);

            return SelectImmediateDictionary<TKey, TValue>();
        }

        /// <inheritdoc />
        IDelayedFlowQuery<TSource> IImmediateFlowQuery<TSource>.Delayed()
        {
            return Delayed();
        }

        /// <inheritdoc />
        IDetachedFlowQuery<TSource> IImmediateFlowQuery<TSource>.Detached()
        {
            return Detached();
        }

        /// <inheritdoc />
        IImmediateFlowQuery<TSource> IImmediateFlowQuery<TSource>.Distinct()
        {
            return Distinct();
        }

        /// <inheritdoc />
        IImmediateFlowQuery<TSource> IImmediateFlowQuery<TSource>.Indistinct()
        {
            return Indistinct();
        }
    }
}