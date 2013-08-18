using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core.Selection;
using NHibernate.FlowQuery.Helpers;
using NHibernate.Transform;

namespace NHibernate.FlowQuery.Core.Implementors
{
    public class MorphableFlowQueryImplementorBase<TSource, TFlowQuery> : FlowQueryImplementor<TSource, TFlowQuery>, IMorphableFlowQuery<TSource, TFlowQuery>, IMorphableFlowQuery
        where TSource : class
        where TFlowQuery : class, IFlowQuery<TSource, TFlowQuery>
    {
        protected internal MorphableFlowQueryImplementorBase(Func<System.Type, string, ICriteria> criteriaFactory, string alias = null, FlowQueryOptions options = null, IMorphableFlowQuery query = null)
            : base(criteriaFactory, alias, options, query)
        {
            if (query != null)
            {
                Constructor = query.Constructor;

                IsDistinct = query.IsDistinct;

                if (query.Mappings != null)
                {
                    Mappings = query.Mappings.ToDictionary(x => x.Key, x => x.Value);
                }

                Projection = query.Projection;

                ResultTransformer = query.ResultTransformer;
            }
        }

        protected virtual TFlowQuery ProjectWithConstruction<TDestination>(Expression<Func<TSource, TDestination>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            Dictionary<string, IProjection> mappings = new Dictionary<string, IProjection>();

            var list = ProjectionHelper.GetProjectionListForExpression(expression.Body, expression.Parameters[0].Name, Aliases, ref mappings);

            if (list == null || list.Length == 0)
            {
                throw new NotSupportedException("The provided expression contains unsupported features please revise your code.");
            }

            return ProjectionBase<TDestination>(list, mappings, expression, false);
        }

        public virtual TFlowQuery Distinct()
        {
            IsDistinct = true;

            return Query;
        }

        public virtual TFlowQuery Indistinct()
        {
            IsDistinct = false;

            return Query;
        }

        protected virtual TFlowQuery ProjectionBase<TDestination>(IProjection projection, Dictionary<string, IProjection> mappings = null, LambdaExpression constructor = null, bool setResultTransformer = true)
        {
            Constructor = constructor;

            Mappings = mappings;

            Projection = projection;

            if (setResultTransformer)
            {
                ResultTransformer = Transformers.AliasToBean<TDestination>();
            }

            return Query;
        }

        protected virtual TFlowQuery Project<TDestination>(ISelectSetup<TSource, TDestination> setup)
        {
            if (setup == null)
            {
                throw new ArgumentNullException("setup");
            }

            return ProjectionBase<TDestination>(setup.ProjectionList, setup.Mappings);
        }

        public virtual TFlowQuery Project(params string[] properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }

            return Project
            (
                Projections
                    .ProjectionList()
                        .AddProperties(properties)
            );
        }

        public virtual TFlowQuery Project(IProjection projection)
        {
            return Project<TSource>(projection);
        }

        protected virtual TFlowQuery Project(PropertyProjection projection)
        {
            return Project<object>(projection);
        }

        protected virtual TFlowQuery Project<TDestination>(IProjection projection)
        {
            if (projection == null)
            {
                throw new ArgumentNullException("projecton");
            }

            return ProjectionBase<TDestination>(projection);
        }

        protected virtual TFlowQuery Project<TDestination>(PropertyProjection projection)
        {
            if (projection == null)
            {
                throw new ArgumentNullException("projection");
            }

            return ProjectionBase<TDestination>(projection, setResultTransformer: false);
        }

        public virtual TFlowQuery Project(params Expression<Func<TSource, object>>[] properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }

            return Project
            (
                Projections
                    .ProjectionList()
                        .AddProperties(Aliases, properties)
            );
        }

        protected virtual TFlowQuery Project<TDestination>(Expression<Func<TSource, TDestination>> expression)
        {
            return ProjectWithConstruction(expression);
        }

        public virtual LambdaExpression Constructor { get; protected set; }

        public virtual bool IsDistinct { get; protected set; }

        public virtual Dictionary<string, IProjection> Mappings { get; protected set; }

        public virtual IProjection Projection { get; protected set; }

        public virtual IResultTransformer ResultTransformer { get; protected set; }

        public virtual IDelayedFlowQuery<TSource> Delayed()
        {
            return new DelayedFlowQueryImplementor<TSource>(CriteriaFactory, Alias, Options, this);
        }

        public virtual IDetachedFlowQuery<TSource> Detached()
        {
            return new DetachedFlowQueryImplementor<TSource>(CriteriaFactory, Alias, Options, this);
        }

        public virtual IImmediateFlowQuery<TSource> Immediate()
        {
            return new ImmediateFlowQueryImplementor<TSource>(CriteriaFactory, Alias, Options, this);
        }
    }
}