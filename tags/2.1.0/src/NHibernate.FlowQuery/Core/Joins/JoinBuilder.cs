using System;
using System.Collections;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core.Implementors;
using NHibernate.FlowQuery.Helpers;
using NHibernate.FlowQuery.Revealing.Conventions;
using NHibernate.SqlCommand;

namespace NHibernate.FlowQuery.Core.Joins
{
    public class JoinBuilder<TSource, TQuery> : IJoinBuilder<TSource, TQuery>
        where TSource : class
        where TQuery : class, IFlowQuery<TSource, TQuery>
    {
        // ReSharper disable once StaticFieldInGenericType
        private static readonly System.Type Enumerable = typeof(IEnumerable);

        protected internal FlowQueryImplementor<TSource, TQuery> Implementor { get; private set; }

        protected internal TQuery Query { get; private set; }

        protected internal JoinType JoinType { get; private set; }

        protected internal JoinBuilder(FlowQueryImplementor<TSource, TQuery> implementor, TQuery query, JoinType joinType)
        {
            if (implementor == null)
            {
                throw new ArgumentNullException("implementor");
            }

            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            if (!ReferenceEquals(implementor, query))
            {
                throw new ArgumentException("implementor and query must be the same refence", "query");
            }

            Implementor = implementor;

            JoinType = joinType;

            Query = query;
        }

        protected virtual TQuery JoinBaseWithConvention<TAlias>(Expression<Func<TSource, object>> projection, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause = null, IRevealConvention revealConvention = null)
        {
            if (revealConvention == null)
            {
                revealConvention = Reveal.DefaultConvention ?? new CustomConvention(x => x);
            }

            return JoinBase(Reveal.ByConvention(projection, revealConvention), alias, Enumerable.IsAssignableFrom(projection.Body.Type), joinOnClause);
        }

        protected virtual TQuery JoinBase<TAlias>(string property, Expression<Func<TAlias>> aliasProjection, bool isCollection = true, Expression<Func<bool>> joinOnClause = null)
        {
            string alias = ExpressionHelper.GetPropertyName(aliasProjection);

            if (Implementor.Aliases.ContainsKey(property))
            {
                if (Implementor.Aliases[property] == alias)
                {
                    // Already exists
                    return Query;
                }

                throw new InvalidOperationException("Property already aliased");
            }

            if (Implementor.Aliases.ContainsValue(alias))
            {
                throw new InvalidOperationException("Alias already in use");
            }

            Implementor.Aliases.Add(property, alias);

            ICriterion withCriterion = null;

            if (joinOnClause != null)
            {
                withCriterion = RestrictionHelper.GetCriterion(joinOnClause, null, Implementor.Data);
            }

            Implementor.Joins.Add(new Join
            {
                Alias = alias,
                IsCollection = isCollection,
                JoinType = JoinType,
                Property = property,
                WithClause = withCriterion
            });

            return Query;
        }

        protected virtual TQuery JoinBase<TAlias>(LambdaExpression projection, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause = null)
        {
            string property = ExpressionHelper.GetPropertyName(projection.Body, projection.Parameters[0].Name);

            return JoinBase(property, alias, Enumerable.IsAssignableFrom(projection.Body.Type), joinOnClause);
        }

        public virtual TQuery Join<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return JoinBase(property, alias);
        }

        public virtual TQuery Join<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause)
        {
            return JoinBase(property, alias, true, joinOnClause);
        }

        public virtual TQuery Join<TAlias>(Expression<Func<TSource, object>> projection, Expression<Func<TAlias>> alias)
        {
            return JoinBase(projection, alias);
        }

        public virtual TQuery Join<TAlias>(Expression<Func<TSource, object>> projection, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause)
        {
            return JoinBase(projection, alias, joinOnClause);
        }

        public virtual TQuery Join<TAlias>(Expression<Func<TSource, object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return JoinBaseWithConvention(projection, alias, null, revealConvention);
        }

        public virtual TQuery Join<TAlias>(Expression<Func<TSource, object>> projection, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return JoinBaseWithConvention(projection, alias, joinOnClause, revealConvention);
        }
    }
}