using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Expressions;
using NHibernate.FlowQuery.Helpers;
using NHibernate.Metadata;

namespace NHibernate.FlowQuery.Core.Implementors
{
    public class ImmediateFlowQueryImplementor<TSource> : QueryableFlowQueryImplementor<TSource, IImmediateFlowQuery<TSource>>, IImmediateFlowQuery<TSource>
        where TSource : class
    {
        protected internal ImmediateFlowQueryImplementor(Func<System.Type, string, ICriteria> criteriaFactory, Func<System.Type, IClassMetadata> metaDataFactory, string alias = null, FlowQueryOptions options = null, IMorphableFlowQuery query = null)
            : base(criteriaFactory, metaDataFactory, alias, options, query)
        { }

        public virtual bool Any()
        {
            var count = Take(1).Count();

            return count > 0;
        }

        public virtual bool Any(params ICriterion[] criterions)
        {
            return Where(criterions).Any();
        }

        public virtual bool Any(string property, IsExpression expression)
        {
            return Where(property, expression).Any();
        }

        public virtual bool Any(Expression<Func<TSource, bool>> expression)
        {
            return Where(expression).Any();
        }

        public virtual bool Any(Expression<Func<TSource, object>> property, IsExpression expression)
        {
            return Where(property, expression).Any();
        }

        public virtual bool Any(Expression<Func<TSource, WhereDelegate, bool>> expression)
        {
            return Where(expression).Any();
        }

        public virtual int Count()
        {
            Projection = Projections.RowCount();

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return SelectImmediateValue<int>();
        }

        public virtual int Count(string property)
        {
            Projection = IsDistinct
                ? Projections.CountDistinct(property)
                : Projections.Count(property);

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return SelectImmediateValue<int>();
        }

        public virtual int Count(IProjection projection)
        {
            Projection = Projections.Count(projection);

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return SelectImmediateValue<int>();
        }

        public virtual int Count(Expression<Func<TSource, object>> property)
        {
            string propertyName = ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name);

            return Count(propertyName);
        }

        public virtual long CountLong()
        {
            Projection = Projections.RowCountInt64();

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return SelectImmediateValue<long>();
        }

        IImmediateFlowQuery<TSource> IImmediateFlowQuery<TSource>.Distinct()
        {
            return base.Distinct();
        }

        IImmediateFlowQuery<TSource> IImmediateFlowQuery<TSource>.Indistinct()
        {
            return base.Indistinct();
        }

        public override bool IsDelayed
        {
            get { return false; }
        }

        public virtual Dictionary<TKey, TValue> SelectDictionary<TKey, TValue>(Expression<Func<TSource, TKey>> key, Expression<Func<TSource, TValue>> value)
        {
            var setup = Select<Pair<TKey, TValue>>()
                .For(x => x.Key).Use(key)
                .For(x => x.Value).Use(value);

            Project(setup);

            return SelectImmediateDictionary<TKey, TValue>();
        }

        IImmediateFlowQuery<TSource> IImmediateFlowQuery<TSource>.Copy()
        {
            return new ImmediateFlowQueryImplementor<TSource>(CriteriaFactory, MetaDataFactory, Alias, Options, this);
        }

        IDelayedFlowQuery<TSource> IImmediateFlowQuery<TSource>.Delayed()
        {
            return base.Delayed();
        }

        IDetachedFlowQuery<TSource> IImmediateFlowQuery<TSource>.Detached()
        {
            return base.Detached();
        }
    }
}