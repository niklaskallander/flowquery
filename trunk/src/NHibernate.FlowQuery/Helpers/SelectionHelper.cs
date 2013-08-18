using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Helpers
{
    public static class SelectionHelper
    {
        public static FlowQuerySelection<TReturn> SelectList<TSource, TReturn>(QuerySelection query)
            where TSource : class
        {
            ICriteria criteria = CriteriaHelper.BuildCriteria<TSource, TReturn>(query);

            if (query.Constructor != null)
            {
                bool canHandle = ConstructionHelper.CanHandle(query.Constructor);

                if (canHandle)
                {
                    IEnumerable enumerable = query.IsDelayed
                        ? criteria.Future<object>()
                        : (IEnumerable)criteria.List();

                    return new FlowQuerySelection<TReturn>(() => ConstructionHelper.GetListByExpression<TReturn>(query.Constructor, enumerable));
                }
            }

            IEnumerable<TReturn> selection = query.IsDelayed
                ? criteria.Future<TReturn>()
                : criteria.List<TReturn>();

            return new FlowQuerySelection<TReturn>(() => selection);
        }

        public static Func<Dictionary<TKey, TValue>> SelectDictionary<TSource, TKey, TValue>(QuerySelection query)
            where TSource : class
        {
            FlowQuerySelection<Pair<TKey, TValue>> selection = SelectList<TSource, Pair<TKey, TValue>>(query);

            return () => selection.ToDictionary(x => x.Key, x => x.Value);
        }

        public static Func<TReturn> SelectValue<TSource, TReturn>(QuerySelection query)
            where TSource : class
        {
            ICriteria criteria = CriteriaHelper.BuildCriteria<TSource, TReturn>(query);

            if (query.IsDelayed)
            {
                var value = criteria.FutureValue<TReturn>();

                return () => value.Value;
            }

            return () => criteria.UniqueResult<TReturn>();
        }
    }
}