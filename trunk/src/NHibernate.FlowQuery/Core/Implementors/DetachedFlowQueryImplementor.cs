using System;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Helpers;
using NHibernate.Metadata;

namespace NHibernate.FlowQuery.Core.Implementors
{
    public class DetachedFlowQueryImplementor<TSource> : MorphableFlowQueryImplementorBase<TSource, IDetachedFlowQuery<TSource>>, IDetachedFlowQuery<TSource>
        where TSource : class
    {
        protected internal DetachedFlowQueryImplementor(Func<System.Type, string, ICriteria> criteriaFactory, Func<System.Type, IClassMetadata> metaDataFactory, string alias = null, FlowQueryOptions options = null, IMorphableFlowQuery query = null)
            : base(criteriaFactory, metaDataFactory, alias, options, query)
        { }

        public virtual IDetachedFlowQuery<TSource> Count()
        {
            Projection = Projections.RowCount();

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return this;
        }

        public virtual IDetachedFlowQuery<TSource> Count(string property)
        {
            Projection = IsDistinct
                ? Projections.CountDistinct(property)
                : Projections.Count(property);

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return this;
        }

        public virtual IDetachedFlowQuery<TSource> Count(IProjection projection)
        {
            Projection = Projections.Count(projection);

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return this;
        }

        public virtual IDetachedFlowQuery<TSource> Count(Expression<Func<TSource, object>> property)
        {
            string propertyName = ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name);

            return Count(propertyName);
        }

        public virtual IDetachedFlowQuery<TSource> CountLong()
        {
            Project(Projections.RowCountInt64());

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return this;
        }

        IDetachedFlowQuery<TSource> IDetachedFlowQuery<TSource>.Distinct()
        {
            return base.Distinct();
        }

        IDetachedFlowQuery<TSource> IDetachedFlowQuery<TSource>.Indistinct()
        {
            return base.Indistinct();
        }

        public virtual IDetachedFlowQuery<TSource> Select(string[] properties)
        {
            return Project(properties);
        }

        public virtual IDetachedFlowQuery<TSource> Select(IProjection projection)
        {
            return Project(projection);
        }

        public virtual IDetachedFlowQuery<TSource> Select(params Expression<Func<TSource, object>>[] expressions)
        {
            return Project(expressions);
        }

        public virtual DetachedCriteria Criteria
        {
            get { return CriteriaHelper.BuildDetachedCriteria(this); }
        }

        public virtual IDetachedFlowQuery<TSource> SetRootAlias<TAlias>(Expression<Func<TAlias>> alias)
            where TAlias : class
        {
            if (alias == null)
            {
                throw new ArgumentNullException("alias");
            }

            string rootAliasName = ExpressionHelper.GetPropertyName(alias);

            Aliases.Add(rootAliasName, rootAliasName);

            return this;
        }

        IDetachedFlowQuery<TSource> IDetachedFlowQuery<TSource>.Copy()
        {
            return new DetachedFlowQueryImplementor<TSource>(CriteriaFactory, MetaDataFactory, Alias, Options, this);
        }

        IDelayedFlowQuery<TSource> IDetachedFlowQuery<TSource>.Delayed()
        {
            return base.Delayed();
        }

        IImmediateFlowQuery<TSource> IDetachedFlowQuery<TSource>.Immediate()
        {
            return base.Immediate();
        }
    }
}