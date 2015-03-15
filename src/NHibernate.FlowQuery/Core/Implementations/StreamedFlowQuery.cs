namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers;

    /// <summary>
    ///     A class used for streamed queries.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity for this query.
    /// </typeparam>
    public class StreamedFlowQuery<TSource>
        : ImmediateFlowQueryBase<TSource, IStreamedFlowQuery<TSource>>, IStreamedFlowQuery<TSource>
        where TSource : class
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StreamedFlowQuery{TSource}" /> class.
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
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="criteriaFactory" /> is null.
        /// </exception>
        protected internal StreamedFlowQuery
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
        public override IStreamedFlowQuery<TSource> Copy()
        {
            return new StreamedFlowQuery<TSource>(CriteriaFactory, Alias, Options, this);
        }

        /// <inheritdoc />
        public virtual void Select(IResultStream<TSource> stream)
        {
            Constructor = null;
            Mappings = null;
            Projection = null;
            ResultTransformer = null;

            SelectStream(stream);
        }

        /// <inheritdoc />
        public virtual void Select<TDestination>
            (
            IResultStream<TDestination> stream,
            ISelectSetup<TSource, TDestination> setup
            )
        {
            Project(setup);

            SelectStream(stream);
        }

        /// <inheritdoc />
        public virtual void Select
            (
            IResultStream<TSource> stream,
            string property,
            params string[] properties
            )
        {
            Project(property, properties);

            SelectStream(stream);
        }

        /// <inheritdoc />
        public virtual void Select
            (
            IResultStream<TSource> stream,
            IProjection projection
            )
        {
            Project(projection);

            SelectStream(stream);
        }

        /// <inheritdoc />
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        public virtual void Select<TDestination>
            (
            IResultStream<TDestination> stream,
            string property,
            params string[] properties
            )
        {
            Project<TDestination>(property, properties);

            SelectStream(stream);
        }

        /// <inheritdoc />
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        public virtual void Select<TDestination>
            (
            IResultStream<TDestination> stream,
            IProjection projection
            )
        {
            Project<TDestination>(projection);

            SelectStream(stream);
        }

        /// <inheritdoc />
        public virtual void Select
            (
            IResultStream<TSource> stream,
            params Expression<Func<TSource, object>>[] properties
            )
        {
            Project(properties);

            SelectStream(stream);
        }

        /// <inheritdoc />
        public virtual void Select<TDestination>
            (
            IResultStream<TDestination> stream,
            Expression<Func<TSource, TDestination>> projection
            )
        {
            Project(projection);

            SelectStream(stream);
        }

        /// <inheritdoc />
        public virtual void Select<TDestination>
            (
            IResultStream<TDestination> stream,
            IPartialSelection<TSource, TDestination> combiner
            )
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

            Select(stream, expression);
        }

        /// <summary>
        ///     Populates the given <see cref="IResultStream{TDestination}" /> with the results of this query in a
        ///     streamed fashion.
        /// </summary>
        /// <param name="stream">
        ///     The <see cref="IResultStream{TDestination}" /> to stream the results into.
        /// </param>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        protected virtual void SelectStream<TDestination>(IResultStream<TDestination> stream)
        {
            SelectionHelper.SelectStream<TSource, TDestination>(QuerySelection.Create(this), stream);
        }
    }
}