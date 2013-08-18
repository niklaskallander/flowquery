using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core.SelectSetup;
using NHibernate.FlowQuery.Helpers;

namespace NHibernate.FlowQuery.Core.Implementors
{
    public abstract class QueryableFlowQueryImplementor<TSource, TFlowQuery> : MorphableFlowQueryImplementorBase<TSource, TFlowQuery>, IQueryableFlowQuery<TSource>, IQueryableFlowQuery
        where TSource : class
        where TFlowQuery : class, IFlowQuery<TSource, TFlowQuery>
    {
        protected internal QueryableFlowQueryImplementor(Func<System.Type, string, ICriteria> criteriaFactory, string alias = null, FlowQueryOptions options = null, IMorphableFlowQuery query = null)
            : base(criteriaFactory, alias, options, query)
        { }

        protected virtual FlowQuerySelection<TReturn> SelectList<TReturn>()
        {
            return SelectionHelper.SelectList<TSource, TReturn>(QuerySelection.Create(this));
        }

        protected virtual Func<Dictionary<TKey, TValue>> SelectDictionary<TKey, TValue>()
        {
            return SelectionHelper.SelectDictionary<TSource, TKey, TValue>(QuerySelection.Create(this));
        }

        protected virtual Dictionary<TKey, TValue> SelectImmediateDictionary<TKey, TValue>()
        {
            var valueDelegate = SelectDictionary<TKey, TValue>();

            return valueDelegate.Invoke();
        }

        protected virtual Lazy<Dictionary<TKey, TValue>> SelectDelayedDictionary<TKey, TValue>()
        {
            var valueDelegate = SelectDictionary<TKey, TValue>();

            return new Lazy<Dictionary<TKey, TValue>>(valueDelegate);
        }

        protected virtual Func<TReturn> SelectValue<TReturn>()
        {
            return SelectionHelper.SelectValue<TSource, TReturn>(QuerySelection.Create(this));
        }

        protected virtual TReturn SelectImmediateValue<TReturn>()
        {
            var valueDelegate = SelectValue<TReturn>();

            return valueDelegate.Invoke();
        }

        protected virtual Lazy<TReturn> SelectDelayedValue<TReturn>()
        {
            var valueDelegate = SelectValue<TReturn>();

            return new Lazy<TReturn>(valueDelegate);
        }

        public virtual FlowQuerySelection<TSource> Select()
        {
            Constructor = null;
            Mappings = null;
            Projection = null;
            ResultTransformer = null;

            return SelectList<TSource>();
        }

        public virtual ISelectSetup<TSource, TReturn> Select<TReturn>()
        {
            return new SelectSetup<TSource, TReturn>(Select<TReturn>, Aliases);
        }

        public virtual FlowQuerySelection<TReturn> Select<TReturn>(ISelectSetup<TSource, TReturn> setup)
        {
            Project<TReturn>(setup);

            return SelectList<TReturn>();
        }

        public virtual FlowQuerySelection<TSource> Select(params string[] properties)
        {
            Project(properties);

            return SelectList<TSource>();
        }

        public virtual FlowQuerySelection<TSource> Select(IProjection projection)
        {
            Project(projection);

            return SelectList<TSource>();
        }

        public virtual FlowQuerySelection<object> Select(PropertyProjection projection)
        {
            Project(projection);

            return SelectList<object>();
        }

        public virtual FlowQuerySelection<TReturn> Select<TReturn>(IProjection projection)
        {
            Project<TReturn>(projection);

            return SelectList<TReturn>();
        }

        public virtual FlowQuerySelection<TReturn> Select<TReturn>(PropertyProjection projection)
        {
            Project<TReturn>(projection);

            return SelectList<TReturn>();
        }

        public virtual FlowQuerySelection<TSource> Select(params Expression<Func<TSource, object>>[] properties)
        {
            Project(properties);

            return SelectList<TSource>();
        }

        public virtual FlowQuerySelection<TReturn> Select<TReturn>(Expression<Func<TSource, TReturn>> expression)
        {
            Project<TReturn>(expression);

            return SelectList<TReturn>();
        }

        public abstract bool IsDelayed { get; }
    }
}