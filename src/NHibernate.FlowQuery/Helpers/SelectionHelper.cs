namespace NHibernate.FlowQuery.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Structures;

    /// <summary>
    ///     A static utility class providing methods to create query selection results.
    /// </summary>
    public static class SelectionHelper
    {
        /// <summary>
        ///     Creates a dictionary query selection.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <typeparam name="TKey">
        ///     The <see cref="System.Type" /> of the dictionary keys.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The <see cref="System.Type" /> of the dictionary values.
        /// </typeparam>
        /// <returns>
        ///     An action to retrieve the selected dictionary.
        /// </returns>
        public static Func<Dictionary<TKey, TValue>> SelectDictionary<TSource, TKey, TValue>(QuerySelection query)
            where TSource : class
        {
            FlowQuerySelection<Pair<TKey, TValue>> selection = SelectList<TSource, Pair<TKey, TValue>>(query);

            return () => selection.ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        ///     Creates a regular <see cref="FlowQuerySelection{TDestination}" /> instance.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the result.
        /// </typeparam>
        /// <returns>
        ///     The created <see cref="FlowQuerySelection{TDestination}" /> instance.
        /// </returns>
        public static FlowQuerySelection<TDestination> SelectList<TSource, TDestination>(IQueryableFlowQuery query)
            where TSource : class
        {
            ICriteriaBuilder criteriaBuilder = GetCriteriaBuilder(query);

            ICriteria criteria = criteriaBuilder.Build<TSource, TDestination>(query);

            if (query.Constructor != null)
            {
                IEnumerable enumerable = query.IsDelayed
                    ? criteria.Future<object>()
                    : (IEnumerable)criteria.List();

                return new FlowQuerySelection<TDestination>
                    (
                    () => ConstructionHelper.GetListByExpression<TDestination>(query.Constructor, enumerable)
                    );
            }

            IEnumerable<TDestination> selection = query.IsDelayed
                ? criteria.Future<TDestination>()
                : criteria.List<TDestination>();

            // ReSharper disable once PossibleMultipleEnumeration
            return new FlowQuerySelection<TDestination>(() => selection);
        }

        /// <summary>
        ///     Populates the given <see cref="IResultStream{TDestination}" /> with the results, from the execution of
        ///     the given <see cref="IQueryableFlowQuery" />, in a streamed fashion.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the result.
        /// </typeparam>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <param name="resultStream">
        ///     The <see cref="IResultStream{TDestination}" /> to stream the results into.
        /// </param>
        public static void SelectStream<TSource, TDestination>
            (
            IQueryableFlowQuery query,
            IResultStream<TDestination> resultStream
            )
        {
            Func<object, TDestination> converter = null;

            if (query.Constructor != null)
            {
                converter = ConstructionHelper.GetObjectByExpressionConverter<TDestination>(query.Constructor);
            }

            if (converter == null)
            {
                converter = x => (TDestination)x;
            }

            var streamer = new ResultStreamer<TDestination>(resultStream, converter);

            ICriteriaBuilder criteriaBuilder = GetCriteriaBuilder(query);

            ICriteria criteria = criteriaBuilder.Build<TSource, TDestination>(query);

            criteria.List(streamer);

            resultStream.EndOfStream();
        }

        /// <summary>
        ///     Creates a value query selection.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <typeparam name="TDestination">
        ///     The <see cref="System.Type" /> of the result.
        /// </typeparam>
        /// <returns>
        ///     An action to retrieve the selected value.
        /// </returns>
        public static Func<TDestination> SelectValue<TSource, TDestination>(IQueryableFlowQuery query)
            where TSource : class
        {
            ICriteriaBuilder criteriaBuilder = GetCriteriaBuilder(query);

            ICriteria criteria = criteriaBuilder.Build<TSource, TDestination>(query);

            if (query.IsDelayed)
            {
                IFutureValue<TDestination> value = criteria.FutureValue<TDestination>();

                return () => value.Value;
            }

            return criteria.UniqueResult<TDestination>;
        }

        /// <summary>
        ///     Resolves an appropriate <see cref="ICriteriaBuilder" /> for the given
        ///     <see cref="IQueryableFlowQuery" />.
        /// </summary>
        /// <param name="query">
        ///     The <see cref="IQueryableFlowQuery" />.
        /// </param>
        /// <returns>
        ///     The <see cref="ICriteriaBuilder" />.
        /// </returns>
        private static ICriteriaBuilder GetCriteriaBuilder(IQueryableFlowQuery query)
        {
            if (query != null && query.Options != null && query.Options.CriteriaBuilder != null)
            {
                return query.Options.CriteriaBuilder;
            }

            return FlowQueryOptions.GlobalCriteriaBuilder;
        }
    }
}