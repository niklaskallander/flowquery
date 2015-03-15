namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Core.Structures;

    /// <summary>
    ///     A class used for immediate queries.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying entity for this query.
    /// </typeparam>
    public class ImmediateFlowQuery<TSource>
        : ImmediateFlowQueryBase<TSource, IImmediateFlowQuery<TSource>>, IImmediateFlowQuery<TSource>
        where TSource : class
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ImmediateFlowQuery{TSource}" /> class.
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
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="criteriaFactory" /> is null.
        /// </exception>
        protected internal ImmediateFlowQuery
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
        public override IImmediateFlowQuery<TSource> Copy()
        {
            return new ImmediateFlowQuery<TSource>(CriteriaFactory, Alias, Options, this);
        }

        /// <inheritdoc />
        public virtual Dictionary<TKey, TValue> SelectDictionary<TKey, TValue>
            (
            Expression<Func<TSource, TKey>> key,
            Expression<Func<TSource, TValue>> value
            )
        {
            ISelectSetup<TSource, Pair<TKey, TValue>> setup = Select<Pair<TKey, TValue>>()
                .For(x => x.Key).Use(key)
                .For(x => x.Value).Use(value);

            Project(setup);

            return SelectImmediateDictionary<TKey, TValue>();
        }
    }
}