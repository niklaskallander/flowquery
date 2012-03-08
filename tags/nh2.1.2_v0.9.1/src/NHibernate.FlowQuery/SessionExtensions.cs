using System;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery
{
    public static class SessionExtensions
    {
        #region Methods (2)

        public static IFlowQuery<TSource> FlowQuery<TSource>(this ISession session)
            where TSource : class
        {
            return new FlowQueryImpl<TSource>(session);
        }

        public static IFlowQuery<TSource> FlowQuery<TSource>(this ISession session, Expression<Func<TSource>> alias)
            where TSource : class
        {
            return new FlowQueryImpl<TSource>(session, alias);
        }

        public static IFlowQuery<TSource> FlowQuery<TSource>(this ISession session, FlowQueryOptions options)
            where TSource : class
        {
            return new FlowQueryImpl<TSource>(session, options);
        }

        public static IFlowQuery<TSource> FlowQuery<TSource>(this ISession session, Expression<Func<TSource>> alias, FlowQueryOptions options)
            where TSource : class
        {
            return new FlowQueryImpl<TSource>(session, alias, options);
        }

        #endregion Methods


        //tjoho
    }
}