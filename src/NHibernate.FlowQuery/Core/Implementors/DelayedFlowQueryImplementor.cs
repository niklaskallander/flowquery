using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Expressions;
using NHibernate.FlowQuery.Helpers;
using NHibernate.Metadata;

namespace NHibernate.FlowQuery.Core.Implementors
{
    public class DelayedFlowQueryImplementor<TSource> : QueryableFlowQueryImplementor<TSource, IDelayedFlowQuery<TSource>>, IDelayedFlowQuery<TSource>
        where TSource : class
    {
        protected internal DelayedFlowQueryImplementor(Func<System.Type, string, ICriteria> criteriaFactory, Func<System.Type, IClassMetadata> metaDataFactory, string alias = null, FlowQueryOptions options = null, IMorphableFlowQuery query = null)
            : base(criteriaFactory, metaDataFactory, alias, options, query)
        { }

        public virtual Lazy<bool> Any()
        {
            var count = Take(1).Count();

            return new Lazy<bool>(() => count.Value > 0);
        }

        public virtual Lazy<bool> Any(params ICriterion[] criterions)
        {
            return Where(criterions).Any();
        }

        public virtual Lazy<bool> Any(string property, IsExpression expression)
        {
            return Where(property, expression).Any();
        }

        public virtual Lazy<bool> Any(Expression<Func<TSource, bool>> expression)
        {
            return Where(expression).Any();
        }

        public virtual Lazy<bool> Any(Expression<Func<TSource, object>> property, IsExpression expression)
        {
            return Where(property, expression).Any();
        }

        public virtual Lazy<bool> Any(Expression<Func<TSource, WhereDelegate, bool>> expression)
        {
            return Where(expression).Any();
        }

        public virtual Lazy<int> Count()
        {
            Projection = Projections.RowCount();

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return SelectDelayedValue<int>();
        }

        public virtual Lazy<int> Count(string property)
        {
            Projection = IsDistinct
                ? Projections.CountDistinct(property)
                : Projections.Count(property);

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return SelectDelayedValue<int>();
        }

        public virtual Lazy<int> Count(IProjection projection)
        {
            Projection = Projections.Count(projection);

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return SelectDelayedValue<int>();
        }

        public virtual Lazy<int> Count(Expression<Func<TSource, object>> property)
        {
            string propertyName = ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name);

            return Count(propertyName);
        }

        public virtual Lazy<long> CountLong()
        {
            Projection = Projections.RowCountInt64();

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return SelectDelayedValue<long>();
        }

        IDelayedFlowQuery<TSource> IDelayedFlowQuery<TSource>.Distinct()
        {
            return base.Distinct();
        }

        IDelayedFlowQuery<TSource> IDelayedFlowQuery<TSource>.Indistinct()
        {
            return base.Indistinct();
        }

        public override bool IsDelayed
        {
            get { return true; }
        }

        public virtual Lazy<Dictionary<TKey, TValue>> SelectDictionary<TKey, TValue>(Expression<Func<TSource, TKey>> key, Expression<Func<TSource, TValue>> value)
        {
            var setup = Select<Pair<TKey, TValue>>()
                .For(x => x.Key).Use(key)
                .For(x => x.Value).Use(value);

            Project(setup);

            return SelectDelayedDictionary<TKey, TValue>();
        }

        IDelayedFlowQuery<TSource> IDelayedFlowQuery<TSource>.Copy()
        {
            return new DelayedFlowQueryImplementor<TSource>(CriteriaFactory, MetaDataFactory, Alias, Options, this);
        }

        IDetachedFlowQuery<TSource> IDelayedFlowQuery<TSource>.Detached()
        {
            return base.Detached();
        }

        IImmediateFlowQuery<TSource> IDelayedFlowQuery<TSource>.Immediate()
        {
            return base.Immediate();
        }
    }
}