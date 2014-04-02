using System;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Core.Implementors;
using NHibernate.FlowQuery.Helpers;
using NHibernate.Metadata;

namespace NHibernate.FlowQuery
{
    using Type = System.Type;

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

        private static Func<Type, IClassMetadata> MetaData(this ISession session)
        {
            return session.SessionFactory.GetClassMetadata;
        }

        private static Func<Type, IClassMetadata> MetaData(this IStatelessSession session)
        {
            return session.GetSessionImplementation().Factory.GetClassMetadata;
        }

        #region FlowQuery

        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>(this ISession session)
            where TSource : class
        {
            return FlowQuery<TSource>(session, options: null);
        }

        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>(this ISession session, Expression<Func<TSource>> alias)
            where TSource : class
        {
            return FlowQuery(session, alias, null);
        }

        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>(this ISession session, FlowQueryOptions options)
            where TSource : class
        {
            return FlowQuery<TSource>(session, null, options);
        }

        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>(this ISession session, Expression<Func<TSource>> alias, FlowQueryOptions options)
            where TSource : class
        {
            return FlowQuery(session.CreateCriteria, session.MetaData(), alias, options);
        }

        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>(this IStatelessSession session)
            where TSource : class
        {
            return FlowQuery<TSource>(session, options: null);
        }

        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>(this IStatelessSession session, Expression<Func<TSource>> alias)
            where TSource : class
        {
            return FlowQuery(session, alias, null);
        }

        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>(this IStatelessSession session, FlowQueryOptions options)
            where TSource : class
        {
            return FlowQuery<TSource>(session, null, options);
        }

        public static IImmediateFlowQuery<TSource> FlowQuery<TSource>(this IStatelessSession session, Expression<Func<TSource>> alias, FlowQueryOptions options)
            where TSource : class
        {
            return FlowQuery(session.CreateCriteria, session.MetaData(), alias, options);
        }

        private static IImmediateFlowQuery<TSource> FlowQuery<TSource>(Func<Type, string, ICriteria> criteriaFactory, Func<Type, IClassMetadata> metaFactory, Expression<Func<TSource>> alias, FlowQueryOptions options)
            where TSource : class
        {
            return new ImmediateFlowQueryImplementor<TSource>(criteriaFactory, metaFactory, Alias(alias) ?? RootAlias, options);
        }

        #endregion

        #region Immediate

        public static IImmediateFlowQuery<TSource> ImmediateFlowQuery<TSource>(this ISession session)
            where TSource : class
        {
            return ImmediateFlowQuery<TSource>(session, options: null);
        }

        public static IImmediateFlowQuery<TSource> ImmediateFlowQuery<TSource>(this ISession session, Expression<Func<TSource>> alias)
            where TSource : class
        {
            return ImmediateFlowQuery(session, alias, null);
        }

        public static IImmediateFlowQuery<TSource> ImmediateFlowQuery<TSource>(this ISession session, FlowQueryOptions options)
            where TSource : class
        {
            return ImmediateFlowQuery<TSource>(session, null, options);
        }

        public static IImmediateFlowQuery<TSource> ImmediateFlowQuery<TSource>(this ISession session, Expression<Func<TSource>> alias, FlowQueryOptions options)
            where TSource : class
        {
            return ImmediateFlowQuery(session.CreateCriteria, session.MetaData(), alias, options);
        }

        public static IImmediateFlowQuery<TSource> ImmediateFlowQuery<TSource>(this IStatelessSession session)
            where TSource : class
        {
            return ImmediateFlowQuery<TSource>(session, options: null);
        }

        public static IImmediateFlowQuery<TSource> ImmediateFlowQuery<TSource>(this IStatelessSession session, Expression<Func<TSource>> alias)
            where TSource : class
        {
            return ImmediateFlowQuery(session, alias, null);
        }

        public static IImmediateFlowQuery<TSource> ImmediateFlowQuery<TSource>(this IStatelessSession session, FlowQueryOptions options)
            where TSource : class
        {
            return ImmediateFlowQuery<TSource>(session, null, options);
        }

        public static IImmediateFlowQuery<TSource> ImmediateFlowQuery<TSource>(this IStatelessSession session, Expression<Func<TSource>> alias, FlowQueryOptions options)
            where TSource : class
        {
            return ImmediateFlowQuery(session.CreateCriteria, session.MetaData(), alias, options);
        }

        private static IImmediateFlowQuery<TSource> ImmediateFlowQuery<TSource>(Func<Type, string, ICriteria> criteriaFactory, Func<Type, IClassMetadata> metaFactory, Expression<Func<TSource>> alias, FlowQueryOptions options)
            where TSource : class
        {
            return new ImmediateFlowQueryImplementor<TSource>(criteriaFactory, metaFactory, Alias(alias) ?? RootAlias, options);
        }

        #endregion

        #region Delayed

        public static IDelayedFlowQuery<TSource> DelayedFlowQuery<TSource>(this ISession session)
            where TSource : class
        {
            return DelayedFlowQuery<TSource>(session, options: null);
        }

        public static IDelayedFlowQuery<TSource> DelayedFlowQuery<TSource>(this ISession session, Expression<Func<TSource>> alias)
            where TSource : class
        {
            return DelayedFlowQuery(session, alias, null);
        }

        public static IDelayedFlowQuery<TSource> DelayedFlowQuery<TSource>(this ISession session, FlowQueryOptions options)
            where TSource : class
        {
            return DelayedFlowQuery<TSource>(session, null, options);
        }

        public static IDelayedFlowQuery<TSource> DelayedFlowQuery<TSource>(this ISession session, Expression<Func<TSource>> alias, FlowQueryOptions options)
            where TSource : class
        {
            return DelayedFlowQuery(session.CreateCriteria, session.MetaData(), alias, options);
        }

        public static IDelayedFlowQuery<TSource> DelayedFlowQuery<TSource>(this IStatelessSession session)
            where TSource : class
        {
            return DelayedFlowQuery<TSource>(session, options: null);
        }

        public static IDelayedFlowQuery<TSource> DelayedFlowQuery<TSource>(this IStatelessSession session, Expression<Func<TSource>> alias)
            where TSource : class
        {
            return DelayedFlowQuery(session, alias, null);
        }

        public static IDelayedFlowQuery<TSource> DelayedFlowQuery<TSource>(this IStatelessSession session, FlowQueryOptions options)
            where TSource : class
        {
            return DelayedFlowQuery<TSource>(session, null, options);
        }

        public static IDelayedFlowQuery<TSource> DelayedFlowQuery<TSource>(this IStatelessSession session, Expression<Func<TSource>> alias, FlowQueryOptions options)
            where TSource : class
        {
            return DelayedFlowQuery(session.CreateCriteria, session.MetaData(), alias, options);
        }

        private static IDelayedFlowQuery<TSource> DelayedFlowQuery<TSource>(Func<Type, string, ICriteria> criteriaFactory, Func<Type, IClassMetadata> metaFactory, Expression<Func<TSource>> alias, FlowQueryOptions options)
            where TSource : class
        {
            return new DelayedFlowQueryImplementor<TSource>(criteriaFactory, metaFactory, Alias(alias) ?? RootAlias, options);
        }

        #endregion

        #region Detached

        public static IDetachedFlowQuery<TSource> DetachedFlowQuery<TSource>(this ISession session)
            where TSource : class
        {
            return DetachedFlowQuery<TSource>(session, options: null);
        }

        public static IDetachedFlowQuery<TSource> DetachedFlowQuery<TSource>(this ISession session, Expression<Func<TSource>> alias)
            where TSource : class
        {
            return DetachedFlowQuery(session, alias, null);
        }

        public static IDetachedFlowQuery<TSource> DetachedFlowQuery<TSource>(this ISession session, FlowQueryOptions options)
            where TSource : class
        {
            return DetachedFlowQuery<TSource>(session, null, options);
        }

        public static IDetachedFlowQuery<TSource> DetachedFlowQuery<TSource>(this ISession session, Expression<Func<TSource>> alias, FlowQueryOptions options)
            where TSource : class
        {
            return DetachedFlowQuery(session.CreateCriteria, session.MetaData(), alias, options);
        }

        public static IDetachedFlowQuery<TSource> DetachedFlowQuery<TSource>(this IStatelessSession session)
            where TSource : class
        {
            return DetachedFlowQuery<TSource>(session, options: null);
        }

        public static IDetachedFlowQuery<TSource> DetachedFlowQuery<TSource>(this IStatelessSession session, Expression<Func<TSource>> alias)
            where TSource : class
        {
            return DetachedFlowQuery(session, alias, null);
        }

        public static IDetachedFlowQuery<TSource> DetachedFlowQuery<TSource>(this IStatelessSession session, FlowQueryOptions options)
            where TSource : class
        {
            return DetachedFlowQuery<TSource>(session, null, options);
        }

        public static IDetachedFlowQuery<TSource> DetachedFlowQuery<TSource>(this IStatelessSession session, Expression<Func<TSource>> alias, FlowQueryOptions options)
            where TSource : class
        {
            return DetachedFlowQuery(session.CreateCriteria, session.MetaData(), alias, options);
        }

        private static IDetachedFlowQuery<TSource> DetachedFlowQuery<TSource>(Func<Type, string, ICriteria> criteriaFactory, Func<Type, IClassMetadata> metaFactory, Expression<Func<TSource>> alias, FlowQueryOptions options)
            where TSource : class
        {
            return new DetachedFlowQueryImplementor<TSource>(criteriaFactory, metaFactory, Alias(alias) ?? RootAlias, options);
        }

        #endregion
    }
}