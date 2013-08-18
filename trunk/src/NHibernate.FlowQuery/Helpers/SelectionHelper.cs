using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Helpers
{
    public static class SelectionHelper
    {
        public static FlowQuerySelection<TDestination> SelectList<TSource, TDestination>(QuerySelection query)
            where TSource : class
        {
            ICriteria criteria = CriteriaHelper.BuildCriteria<TSource, TDestination>(query);

            if (query.Constructor != null)
            {
                bool canHandle = ConstructionHelper.CanHandle(query.Constructor);

                if (canHandle)
                {
                    IEnumerable enumerable = query.IsDelayed
                        ? criteria.Future<object>()
                        : (IEnumerable)criteria.List();

                    return new FlowQuerySelection<TDestination>(() => ConstructionHelper.GetListByExpression<TDestination>(query.Constructor, enumerable));
                }
            }

            IEnumerable<TDestination> selection = query.IsDelayed
                ? criteria.Future<TDestination>()
                : criteria.List<TDestination>();

            return new FlowQuerySelection<TDestination>(() => selection);
        }

        public static Func<Dictionary<TKey, TValue>> SelectDictionary<TSource, TKey, TValue>(QuerySelection query)
            where TSource : class
        {
            FlowQuerySelection<Pair<TKey, TValue>> selection = SelectList<TSource, Pair<TKey, TValue>>(query);

            return () => selection.ToDictionary(x => x.Key, x => x.Value);
        }

        public static Func<TDestination> SelectValue<TSource, TDestination>(QuerySelection query)
            where TSource : class
        {
            ICriteria criteria = CriteriaHelper.BuildCriteria<TSource, TDestination>(query);

            if (query.IsDelayed)
            {
                var value = criteria.FutureValue<TDestination>();

                return () => value.Value;
            }

            return () => criteria.UniqueResult<TDestination>();
        }
    }
}