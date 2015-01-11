namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.Transform;

    /// <summary>
    ///     A class implementing the basic functionality and structure required of a transformable
    ///     <see cref="NHibernate.FlowQuery" /> query.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity to be queried.
    /// </typeparam>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the class or interface implementing or extending this interface.
    /// </typeparam>
    public class MorphableFlowQueryBase<TSource, TQuery>
        : FlowQueryBase<TSource, TQuery>, IMorphableFlowQuery<TSource, TQuery>, IMorphableFlowQuery
        where TSource : class
        where TQuery : class, IFlowQuery<TSource, TQuery>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MorphableFlowQueryBase{TSource,TQuery}" /> class.
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
        /// <exception cref="ArgumentException">
        ///     The "this" reference is not of the type <see cref="T:TQuery" /> as specified by 
        ///     <typeparamref name="TQuery" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="criteriaFactory" /> is null and the specific query alteration is not an implementation
        ///     of <see cref="T:IDetachedFlowQuery{TSource}" />.
        /// </exception>
        protected internal MorphableFlowQueryBase
            (
            Func<Type, string, ICriteria> criteriaFactory,
            string alias = null,
            FlowQueryOptions options = null,
            IMorphableFlowQuery query = null
            )
            : base(criteriaFactory, alias, options, query)
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

        /// <inheritdoc />
        public string CommentValue { get; protected set; }

        /// <inheritdoc />
        public LambdaExpression Constructor { get; protected set; }

        /// <inheritdoc />
        public int FetchSizeValue { get; protected set; }

        /// <inheritdoc />
        public bool IsDistinct { get; protected set; }

        /// <inheritdoc />
        public bool? IsReadOnly { get; protected set; }

        /// <inheritdoc />
        public Dictionary<string, IProjection> Mappings { get; protected set; }

        /// <inheritdoc />
        public IProjection Projection { get; protected set; }

        /// <inheritdoc />
        public IResultTransformer ResultTransformer { get; protected set; }

        /// <inheritdoc />
        public int? TimeoutValue { get; protected set; }

        /// <inheritdoc />
        public virtual IDelayedFlowQuery<TSource> Delayed()
        {
            return new DelayedFlowQuery<TSource>(CriteriaFactory, Alias, Options, this);
        }

        /// <inheritdoc />
        public virtual IDetachedFlowQuery<TSource> Detached()
        {
            return new DetachedFlowQuery<TSource>(CriteriaFactory, Alias, Options, this);
        }

        /// <inheritdoc />
        public virtual TQuery Distinct()
        {
            IsDistinct = true;

            return Query;
        }

        /// <inheritdoc />
        public virtual IImmediateFlowQuery<TSource> Immediate()
        {
            return new ImmediateFlowQuery<TSource>(CriteriaFactory, Alias, Options, this);
        }

        /// <inheritdoc />
        public virtual TQuery Indistinct()
        {
            IsDistinct = false;

            return Query;
        }

        /// <inheritdoc />
        public virtual TQuery Project(params string[] properties)
        {
            return Project<TSource>(properties);
        }

        /// <inheritdoc />
        public virtual TQuery Project(IProjection projection)
        {
            return Project<TSource>(projection);
        }

        /// <inheritdoc />
        public virtual TQuery Project(params Expression<Func<TSource, object>>[] properties)
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

        /// <summary>
        ///     Specifies a <see cref="ISelectSetup{TSource, TDestination}" /> to project.
        /// </summary>
        /// <param name="setup">
        ///     The setup.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the result.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="T:TQuery" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="setup" /> is null.
        /// </exception>
        protected virtual TQuery Project<TDestination>(ISelectSetup<TSource, TDestination> setup)
        {
            if (setup == null)
            {
                throw new ArgumentNullException("setup");
            }

            return ProjectionBase<TDestination>(setup.ProjectionList, setup.Mappings);
        }

        /// <summary>
        ///     Specifies a list of properties to project.
        /// </summary>
        /// <param name="properties">
        ///     The properties to project.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the result.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="properties" /> is null.
        /// </exception>
        protected virtual TQuery Project<TDestination>(params string[] properties)
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

        /// <summary>
        ///     Specifies a projection.
        /// </summary>
        /// <param name="projection">
        ///     The projection.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the result.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="T:TQuery" /> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="projection" /> is null.
        /// </exception>
        protected virtual TQuery Project<TDestination>(IProjection projection)
        {
            if (projection == null)
            {
                throw new ArgumentNullException("projection");
            }

            Type type = typeof(TDestination);

            return ProjectionBase<TDestination>
            (
                projection,
                setResultTransformer: !(type.IsValueType || type == typeof(string))
            );
        }

        /// <summary>
        ///     Specifies an expression to project.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the result.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="T:TQuery" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="expression" /> is null.
        /// </exception>
        protected virtual TQuery Project<TDestination>(Expression<Func<TSource, TDestination>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            IProjection list = ProjectionHelper
                .GetProjection
                (
                    expression,
                    Data
                );

            if (list == null || (list is ProjectionList && ((ProjectionList)list).Length == 0))
            {
                throw new NotSupportedException
                (
                    "The provided expression contains unsupported features please revise your code."
                );
            }

            return ProjectionBase<TDestination>(list, Data.Mappings, expression, false);
        }

        /// <summary>
        ///     Specifies a projection.
        /// </summary>
        /// <param name="projection">
        ///     The projection.
        /// </param>
        /// <param name="mappings">
        ///     The mappings.
        /// </param>
        /// <param name="constructor">
        ///     The constructor.
        /// </param>
        /// <param name="setResultTransformer">
        ///     A value indicating whether to specify a result transformer.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the result.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="T:TQuery" />.
        /// </returns>
        protected virtual TQuery ProjectionBase<TDestination>
            (
            IProjection projection,
            Dictionary<string, IProjection> mappings = null,
            LambdaExpression constructor = null,
            bool setResultTransformer = true
            )
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
    }
}