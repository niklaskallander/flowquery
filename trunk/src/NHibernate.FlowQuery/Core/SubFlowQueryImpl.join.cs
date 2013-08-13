using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Helpers;
using NHibernate.FlowQuery.Revealing.Conventions;
using NHibernate.SqlCommand;

namespace NHibernate.FlowQuery.Core
{
    public partial class SubFlowQueryImpl<TSource> : SubFlowQuery, ISubFlowQuery<TSource>
    {
        #region Base Join

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, JoinType joinType, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, joinType, joinOnClause, Reveal.DefaultConvention);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, JoinType joinType, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            if (revealConvention == null)
            {
                revealConvention = Reveal.DefaultConvention ?? new CustomConvention(x => x);
            }

            return Join(Reveal.ByConvention(projection, revealConvention), alias, joinType, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(string property, Expression<Func<TAlias>> aliasProjection, JoinType joinType, Expression<Func<TAlias, bool>> joinOnClause)
        {
            string alias = ExpressionHelper.GetPropertyName(aliasProjection);

            if (PropertyAliases.ContainsKey(property))
            {
                if (PropertyAliases[property] == alias)
                {
                    // Already exists
                    return this;
                }
                throw new InvalidOperationException("Property already aliased");
            }

            if (PropertyAliases.ContainsValue(alias))
            {
                throw new InvalidOperationException("Alias already in use");
            }

            PropertyAliases.Add(property, alias);

            ICriterion withCriterion = null;

            if (joinOnClause != null)
            {
                withCriterion = RestrictionHelper.GetCriterion(joinOnClause, joinOnClause.Parameters[0].Name, PropertyAliases);
            }

            Criteria.CreateAlias(property, alias, joinType, withCriterion);

            return this;
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(System.Linq.Expressions.Expression propertyProjection, string root, Expression<Func<TAlias>> aliasProjection, JoinType joinType, Expression<Func<TAlias, bool>> joinOnClause)
        {
            string property = ExpressionHelper.GetPropertyName(propertyProjection, root);

            return Join(property, aliasProjection, joinType, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, JoinType joinType, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection.Body, projection.Parameters[0].Name, alias, joinType, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, JoinType joinType, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection.Body, projection.Parameters[0].Name, alias, joinType, joinOnClause);
        }

        #endregion

        #region Join

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(property, alias);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return InnerJoin(property, alias, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return InnerJoin(projection, alias, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return InnerJoin(projection, alias, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return InnerJoin(projection, alias, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return InnerJoin(projection, alias, revealConvention);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return InnerJoin(projection, alias, joinOnClause, revealConvention);
        }

        #endregion

        #region InnerJoin

        protected virtual ISubFlowQuery<TSource> InnerJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias, JoinType.InnerJoin, null);
        }

        protected virtual ISubFlowQuery<TSource> InnerJoin<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(property, alias, JoinType.InnerJoin, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.InnerJoin, null);
        }

        protected virtual ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.InnerJoin, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.InnerJoin, null);
        }

        protected virtual ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.InnerJoin, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.InnerJoin, null);
        }

        protected virtual ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.InnerJoin, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.InnerJoin, null, revealConvention);
        }

        protected virtual ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.InnerJoin, joinOnClause, revealConvention);
        }

        #endregion

        #region Left Outer Join

        protected virtual ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias, JoinType.LeftOuterJoin, null);
        }

        protected virtual ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(property, alias, JoinType.LeftOuterJoin, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin, null);
        }

        protected virtual ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin, null);
        }

        protected virtual ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin, null);
        }

        protected virtual ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin, null, revealConvention);
        }

        protected virtual ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin, joinOnClause, revealConvention);
        }

        #endregion

        #region Right Outer Join

        protected virtual ISubFlowQuery<TSource> RightOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias, JoinType.RightOuterJoin, null);
        }

        protected virtual ISubFlowQuery<TSource> RightOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(property, alias, JoinType.RightOuterJoin, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.RightOuterJoin, null);
        }

        protected virtual ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.RightOuterJoin, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.RightOuterJoin, null);
        }

        protected virtual ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.RightOuterJoin, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.RightOuterJoin, null);
        }

        protected virtual ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.RightOuterJoin, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.RightOuterJoin, null, revealConvention);
        }

        protected virtual ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.RightOuterJoin, joinOnClause, revealConvention);
        }

        #endregion

        #region Full Join

        protected virtual ISubFlowQuery<TSource> FullJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias, JoinType.FullJoin, null);
        }

        protected virtual ISubFlowQuery<TSource> FullJoin<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(property, alias, JoinType.FullJoin, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.FullJoin, null);
        }

        protected virtual ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.FullJoin, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.FullJoin, null);
        }

        protected virtual ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.FullJoin, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.FullJoin, null);
        }

        protected virtual ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.FullJoin, joinOnClause);
        }

        protected virtual ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.FullJoin, null, revealConvention);
        }

        protected virtual ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.FullJoin, joinOnClause, revealConvention);
        }

        #endregion



        #region ISubFlowQuery<TSource> Join Members

        #region Join

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Join<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Join<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(property, alias, joinOnClause);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join<TAlias>(projection, alias, joinOnClause);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, joinOnClause);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, joinOnClause);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return Join(projection, alias, revealConvention);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return Join(projection, alias, joinOnClause, revealConvention);
        }

        #endregion

        #region Inner Join

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.InnerJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(property, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.InnerJoin<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return InnerJoin(property, alias, joinOnClause);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return InnerJoin(projection, alias, joinOnClause);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return InnerJoin(projection, alias, joinOnClause);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return InnerJoin(projection, alias, joinOnClause);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return InnerJoin(projection, alias, revealConvention);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return InnerJoin(projection, alias, joinOnClause, revealConvention);
        }

        #endregion

        #region Left Outer Join

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.LeftOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return LeftOuterJoin(property, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.LeftOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return LeftOuterJoin(property, alias, joinOnClause);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return LeftOuterJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return LeftOuterJoin(projection, alias, joinOnClause);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return LeftOuterJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return LeftOuterJoin(projection, alias, joinOnClause);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return LeftOuterJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return LeftOuterJoin(projection, alias, joinOnClause);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return LeftOuterJoin(projection, alias, revealConvention);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return LeftOuterJoin(projection, alias, joinOnClause, revealConvention);
        }

        #endregion

        #region Right Outer Join

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return RightOuterJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return RightOuterJoin(projection, alias, joinOnClause);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return RightOuterJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return RightOuterJoin(projection, alias, joinOnClause);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.RightOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return RightOuterJoin(property, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.RightOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return RightOuterJoin(property, alias, joinOnClause);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return RightOuterJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return RightOuterJoin(projection, alias, joinOnClause);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return RightOuterJoin(projection, alias, revealConvention);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return RightOuterJoin(projection, alias, joinOnClause, revealConvention);
        }

        #endregion

        #region Full Join

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.FullJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return FullJoin(property, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.FullJoin<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return FullJoin(property, alias, joinOnClause);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return FullJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return FullJoin(projection, alias, joinOnClause);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return FullJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return FullJoin(projection, alias, joinOnClause);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return FullJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return FullJoin(projection, alias, joinOnClause);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return FullJoin(projection, alias, revealConvention);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return FullJoin(projection, alias, joinOnClause, revealConvention);
        }

        #endregion

        #endregion
    }
}