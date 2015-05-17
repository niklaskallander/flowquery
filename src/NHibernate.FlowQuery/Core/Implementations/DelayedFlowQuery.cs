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
    ///     A class used for delayed queries.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity for this query.
    /// </typeparam>
    public class DelayedFlowQuery<TSource>
        : QueryableFlowQueryBase<TSource, IDelayedFlowQuery<TSource>>, IDelayedFlowQuery<TSource>
        where TSource : class
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DelayedFlowQuery{TSource}" /> class.
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
        protected internal DelayedFlowQuery
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
                return true;
            }
        }

        /// <inheritdoc />
        public virtual Lazy<bool> Any()
        {
            Lazy<int> count = Take(1).Count();

            return new Lazy<bool>(() => count.Value > 0);
        }

        /// <inheritdoc />
        public virtual Lazy<bool> Any(params ICriterion[] criterions)
        {
            return Where(criterions).Any();
        }

        /// <inheritdoc />
        public virtual Lazy<bool> Any(string property, IsExpression expression)
        {
            return Where(property, expression).Any();
        }

        /// <inheritdoc />
        public virtual Lazy<bool> Any(Expression<Func<TSource, bool>> expression)
        {
            return Where(expression).Any();
        }

        /// <inheritdoc />
        public virtual Lazy<bool> Any(Expression<Func<TSource, object>> property, IsExpression expression)
        {
            return Where(property, expression).Any();
        }

        /// <inheritdoc />
        public virtual Lazy<bool> Any(Expression<Func<TSource, WhereDelegate, bool>> expression)
        {
            return Where(expression).Any();
        }

        /// <inheritdoc />
        public virtual IDelayedFlowQuery<TSource> Copy()
        {
            return new DelayedFlowQuery<TSource>(CriteriaFactory, Alias, Options, this);
        }

        /// <inheritdoc />
        public virtual Lazy<int> Count()
        {
            Projection = Projections.RowCount();

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return SelectDelayedValue<int>();
        }

        /// <inheritdoc />
        public virtual Lazy<int> Count(string property)
        {
            Projection = IsDistinct
                ? Projections.CountDistinct(property)
                : Projections.Count(property);

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return SelectDelayedValue<int>();
        }

        /// <inheritdoc />
        public virtual Lazy<int> Count(IProjection projection)
        {
            Projection = Projections.Count(projection);

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return SelectDelayedValue<int>();
        }

        /// <inheritdoc />
        public virtual Lazy<int> Count(Expression<Func<TSource, object>> property)
        {
            string propertyName = ExpressionHelper.GetPropertyName(property);

            return Count(propertyName);
        }

        /// <inheritdoc />
        public virtual Lazy<long> CountLong()
        {
            Projection = Projections.RowCountInt64();

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return SelectDelayedValue<long>();
        }

        /// <inheritdoc />
        public virtual Lazy<Dictionary<TKey, TValue>> SelectDictionary<TKey, TValue>
            (
            Expression<Func<TSource, TKey>> key,
            Expression<Func<TSource, TValue>> value
            )
        {
            ISelectSetup<TSource, Pair<TKey, TValue>> setup = Select<Pair<TKey, TValue>>()
                .For(x => x.Key).Use(key)
                .For(x => x.Value).Use(value);

            Project(setup);

            return SelectDelayedDictionary<TKey, TValue>();
        }

        /// <inheritdoc />
        IDetachedFlowQuery<TSource> IDelayedFlowQuery<TSource>.Detached()
        {
            return Detached();
        }

        /// <inheritdoc />
        IDelayedFlowQuery<TSource> IDelayedFlowQuery<TSource>.Distinct()
        {
            return Distinct();
        }

        /// <inheritdoc />
        IImmediateFlowQuery<TSource> IDelayedFlowQuery<TSource>.Immediate()
        {
            return Immediate();
        }

        /// <inheritdoc />
        IDelayedFlowQuery<TSource> IDelayedFlowQuery<TSource>.Indistinct()
        {
            return Indistinct();
        }

        /// <inheritdoc />
        IStreamedFlowQuery<TSource> IDelayedFlowQuery<TSource>.Streamed()
        {
            return Streamed();
        }
    }
}