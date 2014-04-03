using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core.Selection;
using NHibernate.FlowQuery.Helpers;
using NHibernate.Metadata;
using NHibernate.Transform;

namespace NHibernate.FlowQuery.Core.Implementors
{
    public class MorphableFlowQueryImplementorBase<TSource, TFlowQuery> : FlowQueryImplementor<TSource, TFlowQuery>, IMorphableFlowQuery<TSource, TFlowQuery>, IMorphableFlowQuery
        where TSource : class
        where TFlowQuery : class, IFlowQuery<TSource, TFlowQuery>
    {
        protected internal MorphableFlowQueryImplementorBase(Func<System.Type, string, ICriteria> criteriaFactory, Func<System.Type, IClassMetadata> metaDataFactory, string alias = null, FlowQueryOptions options = null, IMorphableFlowQuery query = null)
            : base(criteriaFactory, metaDataFactory, alias, options, query)
        {
            if (query != null)
            {
                CommentValue = query.CommentValue;
               
                Constructor = query.Constructor;

                FetchSizeValue = query.FetchSizeValue;

                IsDistinct = query.IsDistinct;
                IsReadOnly = query.IsReadOnly;

                if (query.Mappings != null)
                {
                    Mappings = query.Mappings.ToDictionary(x => x.Key, x => x.Value);
                }

                Projection = query.Projection;

                ResultTransformer = query.ResultTransformer;

                TimeoutValue = query.TimeoutValue;
            }
        }

        protected virtual TFlowQuery ProjectWithConstruction<TDestination>(Expression<Func<TSource, TDestination>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var mappings = new Dictionary<string, IProjection>();

            var list = ProjectionHelper.GetProjectionListForExpression(expression.Body, expression.Parameters[0].Name, Data, ref mappings);

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
            return Project<TSource>(properties);
        }

        public virtual TFlowQuery Project(IProjection projection)
        {
            return Project<TSource>(projection);
        }

        protected virtual TFlowQuery Project<TDestination>(params string[] properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }

            return Project<TDestination>
            (
                Projections
                    .ProjectionList()
                        .AddProperties(properties)
            );
        }

        protected virtual TFlowQuery Project<TDestination>(IProjection projection)
        {
            if (projection == null)
            {
                throw new ArgumentNullException("projection");
            }

            System.Type type = typeof(TDestination);

            return ProjectionBase<TDestination>(projection, setResultTransformer: !(type.IsValueType || type == typeof(string)));
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
                        .AddProperties(Data, properties)
            );
        }

        protected virtual TFlowQuery Project<TDestination>(Expression<Func<TSource, TDestination>> expression)
        {
            return ProjectWithConstruction(expression);
        }

        public string CommentValue { get; protected set; }

        public LambdaExpression Constructor { get; protected set; }

        public int FetchSizeValue { get; protected set; }

        public bool IsDistinct { get; protected set; }

        public bool? IsReadOnly { get; protected set; }

        public Dictionary<string, IProjection> Mappings { get; protected set; }

        public IProjection Projection { get; protected set; }

        public IResultTransformer ResultTransformer { get; protected set; }

        public int? TimeoutValue { get; protected set; }

        public virtual IDelayedFlowQuery<TSource> Delayed()
        {
            return new DelayedFlowQueryImplementor<TSource>(CriteriaFactory, MetaDataFactory, Alias, Options, this);
        }

        public virtual IDetachedFlowQuery<TSource> Detached()
        {
            return new DetachedFlowQueryImplementor<TSource>(CriteriaFactory, MetaDataFactory, Alias, Options, this);
        }

        public virtual IImmediateFlowQuery<TSource> Immediate()
        {
            return new ImmediateFlowQueryImplementor<TSource>(CriteriaFactory, MetaDataFactory, Alias, Options, this);
        }
    }
}