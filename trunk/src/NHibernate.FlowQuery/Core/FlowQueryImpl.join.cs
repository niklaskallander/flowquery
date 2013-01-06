using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Helpers;
using NHibernate.FlowQuery.Revealing.Conventions;
using NHibernate.SqlCommand;

namespace NHibernate.FlowQuery.Core
{
    public partial class FlowQueryImpl<TSource>
    {
        #region Methods

        #region Base Join

        protected virtual IFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, JoinType joinType, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, joinType, joinOnClause, Reveal.DefaultConvention);
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, JoinType joinType, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            if (revealConvention == null)
            {
                revealConvention = Reveal.DefaultConvention ?? new CustomConvention(x => x);
            }

            return Join(Reveal.ByConvention(projection, revealConvention), alias, joinType, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(string property, Expression<Func<TAlias>> aliasProjection, JoinType joinType, Expression<Func<TAlias, bool>> joinOnClause)
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

        protected virtual IFlowQuery<TSource> Join<TAlias>(System.Linq.Expressions.Expression propertyProjection, string root, Expression<Func<TAlias>> aliasProjection, JoinType joinType, Expression<Func<TAlias, bool>> joinOnClause)
        {
            string property = ExpressionHelper.GetPropertyName(propertyProjection, root);

            return Join(property, aliasProjection, joinType, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, JoinType joinType, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection.Body, projection.Parameters[0].Name, alias, joinType, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, JoinType joinType, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection.Body, projection.Parameters[0].Name, alias, joinType, joinOnClause);
        }

        #endregion

        #region Join

        protected virtual IFlowQuery<TSource> Join<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(property, alias);
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return InnerJoin(property, alias, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return InnerJoin(projection, alias, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return InnerJoin(projection, alias, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return InnerJoin(projection, alias, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return InnerJoin(projection, alias, revealConvention);
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return InnerJoin(projection, alias, joinOnClause, revealConvention);
        }

        #endregion

        #region InnerJoin

        protected virtual IFlowQuery<TSource> InnerJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias, JoinType.InnerJoin, null);
        }

        protected virtual IFlowQuery<TSource> InnerJoin<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(property, alias, JoinType.InnerJoin, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.InnerJoin, null);
        }

        protected virtual IFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.InnerJoin, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.InnerJoin, null);
        }

        protected virtual IFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.InnerJoin, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.InnerJoin, null);
        }

        protected virtual IFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.InnerJoin, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.InnerJoin, null, revealConvention);
        }

        protected virtual IFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.InnerJoin, joinOnClause, revealConvention);
        }

        #endregion

        #region Left Outer Join

        protected virtual IFlowQuery<TSource> LeftOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias, JoinType.LeftOuterJoin, null);
        }

        protected virtual IFlowQuery<TSource> LeftOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(property, alias, JoinType.LeftOuterJoin, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin, null);
        }

        protected virtual IFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin, null);
        }

        protected virtual IFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin, null);
        }

        protected virtual IFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin, null, revealConvention);
        }

        protected virtual IFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin, joinOnClause, revealConvention);
        }

        #endregion

        #region Right Outer Join

        protected virtual IFlowQuery<TSource> RightOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias, JoinType.RightOuterJoin, null);
        }

        protected virtual IFlowQuery<TSource> RightOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(property, alias, JoinType.RightOuterJoin, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.RightOuterJoin, null);
        }

        protected virtual IFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.RightOuterJoin, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.RightOuterJoin, null);
        }

        protected virtual IFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.RightOuterJoin, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.RightOuterJoin, null);
        }

        protected virtual IFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.RightOuterJoin, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.RightOuterJoin, null, revealConvention);
        }

        protected virtual IFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.RightOuterJoin, joinOnClause, revealConvention);
        }

        #endregion

        #region Full Join

        protected virtual IFlowQuery<TSource> FullJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias, JoinType.FullJoin, null);
        }

        protected virtual IFlowQuery<TSource> FullJoin<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(property, alias, JoinType.FullJoin, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.FullJoin, null);
        }

        protected virtual IFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.FullJoin, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.FullJoin, null);
        }

        protected virtual IFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.FullJoin, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.FullJoin, null);
        }

        protected virtual IFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, JoinType.FullJoin, joinOnClause);
        }

        protected virtual IFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.FullJoin, null, revealConvention);
        }

        protected virtual IFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.FullJoin, joinOnClause, revealConvention);
        }

        #endregion

        #endregion



        #region IFlowQuery<TSource> Join Members

        #region Join

        IFlowQuery<TSource> IFlowQuery<TSource>.Join<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Join<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(property, alias, joinOnClause);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join<TAlias>(projection, alias, joinOnClause);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, joinOnClause);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return Join(projection, alias, joinOnClause);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return Join(projection, alias, revealConvention);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return Join(projection, alias, joinOnClause, revealConvention);
        }

        #endregion

        #region Inner Join

        IFlowQuery<TSource> IFlowQuery<TSource>.InnerJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(property, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.InnerJoin<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return InnerJoin(property, alias, joinOnClause);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return InnerJoin(projection, alias, joinOnClause);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return InnerJoin(projection, alias, joinOnClause);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return InnerJoin(projection, alias, joinOnClause);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return InnerJoin(projection, alias, revealConvention);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return InnerJoin(projection, alias, joinOnClause, revealConvention);
        }

        #endregion

        #region Left Outer Join

        IFlowQuery<TSource> IFlowQuery<TSource>.LeftOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return LeftOuterJoin(property, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.LeftOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return LeftOuterJoin(property, alias, joinOnClause);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return LeftOuterJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return LeftOuterJoin(projection, alias, joinOnClause);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return LeftOuterJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return LeftOuterJoin(projection, alias, joinOnClause);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return LeftOuterJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return LeftOuterJoin(projection, alias, joinOnClause);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return LeftOuterJoin(projection, alias, revealConvention);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return LeftOuterJoin(projection, alias, joinOnClause, revealConvention);
        }

        #endregion

        #region Right Outer Join

        IFlowQuery<TSource> IFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return RightOuterJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return RightOuterJoin(projection, alias, joinOnClause);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return RightOuterJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return RightOuterJoin(projection, alias, joinOnClause);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.RightOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return RightOuterJoin(property, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.RightOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return RightOuterJoin(property, alias, joinOnClause);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return RightOuterJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return RightOuterJoin(projection, alias, joinOnClause);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return RightOuterJoin(projection, alias, revealConvention);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return RightOuterJoin(projection, alias, joinOnClause, revealConvention);
        }

        #endregion

        #region Full Join

        IFlowQuery<TSource> IFlowQuery<TSource>.FullJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return FullJoin(property, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.FullJoin<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return FullJoin(property, alias, joinOnClause);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return FullJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return FullJoin(projection, alias, joinOnClause);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return FullJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return FullJoin(projection, alias, joinOnClause);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return FullJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause)
        {
            return FullJoin(projection, alias, joinOnClause);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return FullJoin(projection, alias, revealConvention);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention)
        {
            return FullJoin(projection, alias, joinOnClause, revealConvention);
        }

        #endregion

        #endregion
    }
}
