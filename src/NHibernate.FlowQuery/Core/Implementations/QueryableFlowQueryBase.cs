namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers;

    /// <summary>
    ///     A class implementing the basic functionality required of a projectable <see cref="NHibernate.FlowQuery" />
    ///     query.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity to be queried.
    /// </typeparam>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the class extending this class.
    /// </typeparam>
    public abstract class QueryableFlowQueryBase<TSource, TQuery>
        : MorphableFlowQueryBase<TSource, TQuery>, IQueryableFlowQuery<TSource, TQuery>, IQueryableFlowQuery
        where TSource : class
        where TQuery : class, IQueryableFlowQuery<TSource, TQuery>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QueryableFlowQueryBase{TSource,TQuery}" /> class.
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
        protected internal QueryableFlowQueryBase
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
        public abstract bool IsDelayed { get; }

        /// <inheritdoc />
        public virtual TQuery ClearTimeout()
        {
            TimeoutValue = null;

            return Query;
        }

        /// <inheritdoc />
        public virtual TQuery Comment(string comment)
        {
            CommentValue = comment;

            return Query;
        }

        /// <inheritdoc />
        public virtual TQuery FetchSize(int size)
        {
            FetchSizeValue = size;

            return Query;
        }

        /// <inheritdoc />
        public virtual IPartialSelection<TSource, TDestination> PartialSelect<TDestination>
            (
            Expression<Func<TSource, TDestination>> projection = null
            )
        {
            return new PartialSelection<TSource, TDestination>(Select).Add(projection);
        }

        /// <inheritdoc />
        public virtual TQuery ReadOnly(bool isReadOnly = true)
        {
            IsReadOnly = isReadOnly;

            return Query;
        }

        /// <inheritdoc />
        public virtual FlowQuerySelection<TSource> Select()
        {
            Constructor = null;
            Mappings = null;
            Projection = null;
            ResultTransformer = null;

            return SelectList<TSource>();
        }

        /// <inheritdoc />
        public virtual ISelectSetup<TSource, TDestination> Select<TDestination>()
        {
            return new SelectSetup<TSource, TDestination>(Select, Data);
        }

        /// <inheritdoc />
        public virtual FlowQuerySelection<TDestination> Select<TDestination>(ISelectSetup<TSource, TDestination> setup)
        {
            Project(setup);

            return SelectList<TDestination>();
        }

        /// <inheritdoc />
        public virtual FlowQuerySelection<TSource> Select(params string[] properties)
        {
            Project(properties);

            return SelectList<TSource>();
        }

        /// <inheritdoc />
        public virtual FlowQuerySelection<TSource> Select(IProjection projection)
        {
            Project(projection);

            return SelectList<TSource>();
        }

        /// <inheritdoc />
        public virtual FlowQuerySelection<TDestination> Select<TDestination>(params string[] properties)
        {
            Project<TDestination>(properties);

            return SelectList<TDestination>();
        }

        /// <inheritdoc />
        public virtual FlowQuerySelection<TDestination> Select<TDestination>(IProjection projection)
        {
            Project<TDestination>(projection);

            return SelectList<TDestination>();
        }

        /// <inheritdoc />
        public virtual FlowQuerySelection<TSource> Select(params Expression<Func<TSource, object>>[] properties)
        {
            Project(properties);

            return SelectList<TSource>();
        }

        /// <inheritdoc />
        public virtual FlowQuerySelection<TDestination> Select<TDestination>
            (
            Expression<Func<TSource, TDestination>> projection
            )
        {
            Project(projection);

            return SelectList<TDestination>();
        }

        /// <inheritdoc />
        public virtual FlowQuerySelection<TDestination> Select<TDestination>
            (
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

            return Select(expression);
        }

        /// <inheritdoc />
        public virtual TQuery Timeout(int seconds)
        {
            TimeoutValue = seconds;

            return Query;
        }

        /// <summary>
        ///     Creates a delayed dictionary selection.
        /// </summary>
        /// <typeparam name="TKey">
        ///     The <see cref="System.Type" /> of the dictionary keys.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The <see cref="System.Type" /> of the dictionary values.
        /// </typeparam>
        /// <returns>
        ///     The created delayed dictionary selection.
        /// </returns>
        protected virtual Lazy<Dictionary<TKey, TValue>> SelectDelayedDictionary<TKey, TValue>()
        {
            Func<Dictionary<TKey, TValue>> valueDelegate = SelectDictionary<TKey, TValue>();

            return new Lazy<Dictionary<TKey, TValue>>(valueDelegate);
        }

        /// <summary>
        ///     Creates a delayed selection.
        /// </summary>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        /// <returns>
        ///     The created delayed selection.
        /// </returns>
        protected virtual Lazy<TDestination> SelectDelayedValue<TDestination>()
        {
            Func<TDestination> valueDelegate = SelectValue<TDestination>();

            return new Lazy<TDestination>(valueDelegate);
        }

        /// <summary>
        ///     Creates a wrapped dictionary selection.
        /// </summary>
        /// <typeparam name="TKey">
        ///     The <see cref="System.Type" /> of the dictionary keys.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The <see cref="System.Type" /> of the dictionary values.
        /// </typeparam>
        /// <returns>
        ///     The created wrapped dictionary selection.
        /// </returns>
        protected virtual Func<Dictionary<TKey, TValue>> SelectDictionary<TKey, TValue>()
        {
            return SelectionHelper.SelectDictionary<TSource, TKey, TValue>(QuerySelection.Create(this));
        }

        /// <summary>
        ///     Creates a dictionary selection.
        /// </summary>
        /// <typeparam name="TKey">
        ///     The <see cref="System.Type" /> of the dictionary keys.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The <see cref="System.Type" /> of the dictionary values.
        /// </typeparam>
        /// <returns>
        ///     The created dictionary selection.
        /// </returns>
        protected virtual Dictionary<TKey, TValue> SelectImmediateDictionary<TKey, TValue>()
        {
            Func<Dictionary<TKey, TValue>> valueDelegate = SelectDictionary<TKey, TValue>();

            return valueDelegate.Invoke();
        }

        /// <summary>
        ///     Creates a selection.
        /// </summary>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        /// <returns>
        ///     The created selection.
        /// </returns>
        protected virtual TDestination SelectImmediateValue<TDestination>()
        {
            Func<TDestination> valueDelegate = SelectValue<TDestination>();

            return valueDelegate.Invoke();
        }

        /// <summary>
        ///     Creates a <see cref="FlowQuerySelection{TDestination}" />.
        /// </summary>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TDestination}" />.
        /// </returns>
        protected virtual FlowQuerySelection<TDestination> SelectList<TDestination>()
        {
            return SelectionHelper.SelectList<TSource, TDestination>(QuerySelection.Create(this));
        }

        /// <summary>
        ///     Creates a wrapped selection.
        /// </summary>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the selection.
        /// </typeparam>
        /// <returns>
        ///     The created wrapped selection.
        /// </returns>
        protected virtual Func<TDestination> SelectValue<TDestination>()
        {
            return SelectionHelper.SelectValue<TSource, TDestination>(QuerySelection.Create(this));
        }
    }
}