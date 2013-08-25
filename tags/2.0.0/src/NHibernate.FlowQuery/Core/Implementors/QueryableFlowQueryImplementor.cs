using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core.Selection;
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

        protected virtual FlowQuerySelection<TDestination> SelectList<TDestination>()
        {
            return SelectionHelper.SelectList<TSource, TDestination>(QuerySelection.Create(this));
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

        protected virtual Func<TDestination> SelectValue<TDestination>()
        {
            return SelectionHelper.SelectValue<TSource, TDestination>(QuerySelection.Create(this));
        }

        protected virtual TDestination SelectImmediateValue<TDestination>()
        {
            var valueDelegate = SelectValue<TDestination>();

            return valueDelegate.Invoke();
        }

        protected virtual Lazy<TDestination> SelectDelayedValue<TDestination>()
        {
            var valueDelegate = SelectValue<TDestination>();

            return new Lazy<TDestination>(valueDelegate);
        }

        public virtual FlowQuerySelection<TSource> Select()
        {
            Constructor = null;
            Mappings = null;
            Projection = null;
            ResultTransformer = null;

            return SelectList<TSource>();
        }

        public virtual ISelectSetup<TSource, TDestination> Select<TDestination>()
        {
            return new SelectSetup<TSource, TDestination>(Select<TDestination>, Aliases);
        }

        public virtual FlowQuerySelection<TDestination> Select<TDestination>(ISelectSetup<TSource, TDestination> setup)
        {
            Project<TDestination>(setup);

            return SelectList<TDestination>();
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

        public virtual FlowQuerySelection<TDestination> Select<TDestination>(params string[] properties)
        {
            Project<TDestination>(properties);

            return SelectList<TDestination>();
        }

        public virtual FlowQuerySelection<TDestination> Select<TDestination>(IProjection projection)
        {
            Project<TDestination>(projection);

            return SelectList<TDestination>();
        }

        public virtual FlowQuerySelection<TSource> Select(params Expression<Func<TSource, object>>[] properties)
        {
            Project(properties);

            return SelectList<TSource>();
        }

        public virtual FlowQuerySelection<TDestination> Select<TDestination>(Expression<Func<TSource, TDestination>> expression)
        {
            Project<TDestination>(expression);

            return SelectList<TDestination>();
        }

        public virtual FlowQuerySelection<TDestination> Select<TDestination>(PartialSelection<TSource, TDestination> combiner)
        {
            if (combiner == null)
            {
                throw new ArgumentNullException("combiner");
            }

            if (combiner.Count == 0)
            {
                throw new ArgumentException("No projection is made in ExpressionCombiner'2", "combiner");
            }

            Expression<Func<TSource, TDestination>> expression = combiner.Compile();

            return Select<TDestination>(expression);
        }

        public virtual PartialSelection<TSource, TDestination> PartialSelect<TDestination>(Expression<Func<TSource, TDestination>> expression = null)
        {
            return new PartialSelection<TSource, TDestination>(Select).Add(expression);
        }

        public abstract bool IsDelayed { get; }
    }
}