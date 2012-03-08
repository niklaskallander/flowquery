using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Helpers;
using NHibernate.FlowQuery.Revealing.Conventions;
using NHibernate.SqlCommand;

namespace NHibernate.FlowQuery.Core
{
    public partial class FlowQueryImpl<TSource>
    {
        #region Methods

        #region Base Join

        protected virtual IFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, JoinType joinType)
        {
            return Join(projection, alias, joinType, Reveal.DefaultConvention);
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, JoinType joinType, IRevealConvention revealConvention)
        {
            if (revealConvention == null)
            {
                throw new ArgumentNullException("revealConvention");
            }

            return Join(Reveal.ByConvention(projection, revealConvention), alias, joinType);
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(string property, Expression<Func<TAlias>> aliasProjection, JoinType joinType)
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

            Criteria.CreateAlias(property, alias, joinType);

            return this;
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(System.Linq.Expressions.Expression propertyProjection, string root, Expression<Func<TAlias>> aliasProjection, JoinType joinType)
        {
            string property = ExpressionHelper.GetPropertyName(propertyProjection, root);

            return Join(property, aliasProjection, joinType);
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, JoinType joinType)
        {
            return Join(projection.Body, projection.Parameters[0].Name, alias, joinType);
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, JoinType joinType)
        {
            return Join(projection.Body, projection.Parameters[0].Name, alias, joinType);
        }

        #endregion

        #region Join

        protected virtual IFlowQuery<TSource> Join<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(property, alias);
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }


        protected virtual IFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        protected virtual IFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return InnerJoin(projection, alias, revealConvention);
        }

        #endregion

        #region InnerJoin

        protected virtual IFlowQuery<TSource> InnerJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias, JoinType.InnerJoin);
        }


        protected virtual IFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.InnerJoin);
        }


        protected virtual IFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.InnerJoin);
        }


        protected virtual IFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.InnerJoin);
        }


        protected virtual IFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.InnerJoin, revealConvention);
        }


        #endregion

        #region Left Outer Join

        protected virtual IFlowQuery<TSource> LeftOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias, JoinType.LeftOuterJoin);
        }

        protected virtual IFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin);
        }

        protected virtual IFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin);
        }

        protected virtual IFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin);
        }

        protected virtual IFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin, revealConvention);
        }

        #endregion

        #region Right Outer Join

        protected virtual IFlowQuery<TSource> RightOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias, JoinType.RightOuterJoin);
        }

        protected virtual IFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.RightOuterJoin);
        }

        protected virtual IFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.RightOuterJoin);
        }

        protected virtual IFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.RightOuterJoin);
        }

        protected virtual IFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.RightOuterJoin, revealConvention);
        }

        #endregion

        #region Full Join

        protected virtual IFlowQuery<TSource> FullJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias, JoinType.FullJoin);
        }

        protected virtual IFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.FullJoin);
        }

        protected virtual IFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.FullJoin);
        }

        protected virtual IFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.FullJoin);
        }

        protected virtual IFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.FullJoin, revealConvention);
        }

        #endregion

        #endregion



        #region IFlowQuery<TSource> Join Members

        #region Join

        IFlowQuery<TSource> IFlowQuery<TSource>.Join<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return Join(projection, alias, revealConvention);
        }

        #endregion

        #region Inner Join

        IFlowQuery<TSource> IFlowQuery<TSource>.InnerJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(property, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return InnerJoin(projection, alias, revealConvention);
        }

        #endregion

        #region Left Outer Join

        IFlowQuery<TSource> IFlowQuery<TSource>.LeftOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return LeftOuterJoin(property, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return LeftOuterJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return LeftOuterJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return LeftOuterJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return LeftOuterJoin(projection, alias, revealConvention);
        }

        #endregion

        #region Right Outer Join

        IFlowQuery<TSource> IFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return RightOuterJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return RightOuterJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.RightOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return RightOuterJoin(property, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return RightOuterJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return RightOuterJoin(projection, alias, revealConvention);
        }

        #endregion

        #region Full Join

        IFlowQuery<TSource> IFlowQuery<TSource>.FullJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return FullJoin(property, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return FullJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return FullJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return FullJoin(projection, alias);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return FullJoin(projection, alias, revealConvention);
        }

        #endregion

        #endregion
    }
}
