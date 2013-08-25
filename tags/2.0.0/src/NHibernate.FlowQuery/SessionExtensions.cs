using System;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Core.Implementors;
using NHibernate.FlowQuery.Helpers;

namespace NHibernate.FlowQuery
{
    public static class SessionExtensions
    {
        private const string RootAlias = "this";

        private static string Alias<TSource>(Expression<Func<TSource>> alias)
        {
            string aliasName = null;

            if (alias != null)
            {
                aliasName = ExpressionHelper.GetPropertyName(alias);
            }

            return aliasName;
        }

        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>(this ISession session)
            where TSource : class
        {
            return FlowQuery<TSource>(session, (FlowQueryOptions)null);
        }

        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>(this ISession session, Expression<Func<TSource>> alias)
            where TSource : class
        {
            return FlowQuery<TSource>(session, alias, null);
        }

        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>(this ISession session, FlowQueryOptions options)
            where TSource : class
        {
            return FlowQuery<TSource>(session, null, options);
        }

        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>(this ISession session, Expression<Func<TSource>> alias, FlowQueryOptions options)
            where TSource : class
        {
            return FlowQuery<TSource>((t, a) => session.CreateCriteria(t, a), alias, options);
        }

        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>(this IStatelessSession session)
            where TSource : class
        {
            return FlowQuery<TSource>(session, (FlowQueryOptions)null);
        }

        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>(this IStatelessSession session, Expression<Func<TSource>> alias)
            where TSource : class
        {
            return FlowQuery<TSource>(session, alias, null);
        }

        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>(this IStatelessSession session, FlowQueryOptions options)
            where TSource : class
        {
            return FlowQuery<TSource>(session, null, options);
        }

        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>(this IStatelessSession session, Expression<Func<TSource>> alias, FlowQueryOptions options)
            where TSource : class
        {
            return FlowQuery<TSource>((t, a) => session.CreateCriteria(t, a), alias, options);
        }

        private static IImmediateFlowQuery<TSource> FlowQuery<TSource>(Func<System.Type, string, ICriteria> factory, Expression<Func<TSource>> alias, FlowQueryOptions options)
            where TSource : class
        {
            return new ImmediateFlowQueryImplementor<TSource>(factory, Alias(alias) ?? RootAlias, options);
        }
    }
}