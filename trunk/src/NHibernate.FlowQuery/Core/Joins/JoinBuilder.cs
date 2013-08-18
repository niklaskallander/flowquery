using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core.Implementors;
using NHibernate.FlowQuery.Helpers;
using NHibernate.FlowQuery.Revealing.Conventions;
using NHibernate.SqlCommand;

namespace NHibernate.FlowQuery.Core.Joins
{
    public class JoinBuilder<TSource, TFlowQuery> : IJoinBuilder<TSource, TFlowQuery>
        where TSource : class
        where TFlowQuery : class, IFlowQuery<TSource, TFlowQuery>
    {
        protected internal virtual FlowQueryImplementor<TSource, TFlowQuery> Implementor { get; set; }

        protected internal virtual TFlowQuery Query { get; set; }

        protected internal virtual JoinType JoinType { get; set; }

        protected internal JoinBuilder(FlowQueryImplementor<TSource, TFlowQuery> implementor, TFlowQuery query, JoinType joinType)
        {
            if (implementor == null)
            {
                throw new ArgumentNullException("implementor");
            }

            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            if (!object.ReferenceEquals(implementor, query))
            {
                throw new ArgumentException("implementor and query must be the same refence", "query");
            }

            Implementor = implementor;

            JoinType = joinType;

            Query = query;
        }

        protected virtual TFlowQuery JoinBase<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause = null, IRevealConvention revealConvention = null)
        {
            if (revealConvention == null)
            {
                revealConvention = Reveal.DefaultConvention ?? new CustomConvention(x => x);
            }

            return JoinBase(Reveal.ByConvention(projection, revealConvention), alias, joinOnClause);
        }

        protected virtual TFlowQuery JoinBase<TAlias>(string property, Expression<Func<TAlias>> aliasProjection, Expression<Func<bool>> joinOnClause = null)
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
                withCriterion = RestrictionHelper.GetCriterion(joinOnClause, null, Implementor.Aliases);
            }

            Implementor.Joins.Add(new Join()
            {
                Alias = alias,
                JoinType = JoinType,
                Property = property,
                WithClause = withCriterion
            });

            return Query;
        }

        protected virtual TFlowQuery JoinBase<TAlias>(LambdaExpression projection, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause = null)
        {
            string property = ExpressionHelper.GetPropertyName(projection.Body, projection.Parameters[0].Name);

            return JoinBase(property, alias, joinOnClause);
        }

        public virtual TFlowQuery Join<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return JoinBase(property, alias);
        }

        public virtual TFlowQuery Join<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause)
        {
            return JoinBase(property, alias, joinOnClause);
        }

        public virtual TFlowQuery Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return JoinBase(projection, alias);
        }

        public virtual TFlowQuery Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause)
        {
            return JoinBase(projection, alias, joinOnClause);
        }

        public virtual TFlowQuery Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return JoinBase(projection, alias);
        }

        public virtual TFlowQuery Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause)
        {
            return JoinBase(projection, alias, joinOnClause);
        }

        public virtual TFlowQuery Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return JoinBase(projection, alias);
        }

        public virtual TFlowQuery Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause)
        {
            return JoinBase(projection, alias, joinOnClause);
        }

        public virtual TFlowQuery Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return JoinBase(projection, alias, joinOnClause, revealConvention);
        }

        public virtual TFlowQuery Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return JoinBase(projection, alias, null, revealConvention);
        }
    }
}