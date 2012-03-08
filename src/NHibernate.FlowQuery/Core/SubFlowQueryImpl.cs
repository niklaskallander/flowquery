using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Expressions;
using NHibernate.FlowQuery.Helpers;
using NHibernate.FlowQuery.Revealing.Conventions;
using NHibernate.SqlCommand;

namespace NHibernate.FlowQuery.Core
{
    public class SubFlowQueryImpl<TSource> : SubFlowQuery, ISubFlowQuery<TSource>
    {
        #region Constructors (1)

        public SubFlowQueryImpl(DetachedCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException("criteria");
            }

            Criteria = criteria;

            PropertyAliases = new Dictionary<string, string>();

            OrderByStatements = new List<OrderByStatement>();
        }

        #endregion Constructors

        #region Properties (2)

        protected virtual List<OrderByStatement> OrderByStatements { get; private set; }

        protected virtual Dictionary<string, string> PropertyAliases { get; private set; }

        #endregion Properties

        #region Methods (32)

        #region OrderBy

        protected virtual ISubFlowQuery<TSource> OrderBy(string property)
        {
            Criteria.AddOrder(Order.Asc(property));

            return this;
        }

        protected virtual ISubFlowQuery<TSource> OrderBy(IProjection projection)
        {
            Criteria.AddOrder(Order.Asc(projection));

            return this;
        }

        protected virtual ISubFlowQuery<TSource> OrderBy(Expression<Func<TSource, object>> property)
        {
            return OrderBy(ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name));
        }

        protected virtual ISubFlowQuery<TSource> OrderByDescending(string property)
        {
            Criteria.AddOrder(Order.Desc(property));

            return this;
        }

        protected virtual ISubFlowQuery<TSource> OrderByDescending(IProjection projection)
        {
            Criteria.AddOrder(Order.Desc(projection));

            return this;
        }

        protected virtual ISubFlowQuery<TSource> OrderByDescending(Expression<Func<TSource, object>> property)
        {
            return OrderByDescending(ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name));
        }

        #endregion

        #region Count

        protected virtual ISubFlowQuery<TSource> Count()
        {
            return Count(Projections.RowCount(), false);
        }

        protected virtual ISubFlowQuery<TSource> Count(string property)
        {
            return Count(Projections.Property(property));
        }

        protected virtual ISubFlowQuery<TSource> Count(IProjection projection)
        {
            return Count(projection, true);
        }

        protected virtual ISubFlowQuery<TSource> Count(IProjection projection, bool wrap)
        {
            if (wrap)
            {
                projection = Projections.Count(projection);
            }

            Criteria
                .SetProjection(projection);

            return this;
        }

        protected virtual ISubFlowQuery<TSource> Count(Expression<Func<TSource, object>> property)
        {
            return Count(ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name));
        }

        protected virtual ISubFlowQuery<TSource> CountDistinct(string property)
        {
            Criteria
                .SetProjection(Projections.CountDistinct(property));

            return this;
        }

        protected virtual ISubFlowQuery<TSource> CountDistinct(Expression<Func<TSource, object>> property)
        {
            return CountDistinct(ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name));
        }

        protected virtual ISubFlowQuery<TSource> CountLong()
        {
            return Count(Projections.RowCountInt64(), false);
        }

        #endregion

        protected virtual ISubFlowQuery<TSource> And(params ICriterion[] criterions)
        {
            return Where(criterions);
        }

        protected virtual ISubFlowQuery<TSource> And(string property, IsExpression expression)
        {
            return Where(property, expression);
        }

        protected virtual ISubFlowQuery<TSource> And(Expression<Func<TSource, bool>> expression)
        {
            return Where(expression);
        }

        protected virtual ISubFlowQuery<TSource> And(Expression<Func<TSource, object>> property, IsExpression expression)
        {
            return Where(property, expression);
        }

        protected virtual ISubFlowQuery<TSource> FullJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias, JoinType.FullJoin);
        }

        protected virtual ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.FullJoin);
        }

        protected virtual ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.FullJoin);
        }

        protected virtual ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.FullJoin);
        }

        protected virtual ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.FullJoin, revealConvention);
        }

        protected virtual ISubFlowQuery<TSource> InnerJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias, JoinType.InnerJoin);
        }

        protected virtual ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.InnerJoin);
        }

        protected virtual ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.InnerJoin);
        }

        protected virtual ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.InnerJoin);
        }

        protected virtual ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.InnerJoin, revealConvention);
        }

        /// <summary>
        /// Performs an Inner Join.
        /// </summary>
        protected virtual ISubFlowQuery<TSource> Join<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias, JoinType.InnerJoin);
        }

        /// <summary>
        /// Performs an Inner Join.
        /// </summary>
        protected virtual ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.InnerJoin);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.InnerJoin);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, JoinType joinType)
        {
            return Join(projection.Body, projection.Parameters[0].Name, alias, joinType);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, JoinType joinType)
        {
            return Join(projection.Body, projection.Parameters[0].Name, alias, joinType);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return InnerJoin(projection, alias, revealConvention);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, JoinType joinType)
        {
            return Join(projection, alias, joinType, Reveal.DefaultConvention);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, JoinType joinType, IRevealConvention revealConvention)
        {
            if (revealConvention == null)
            {
                throw new ArgumentNullException("revealConvention");
            }

            return Join(Reveal.ByConvention(projection, revealConvention), alias, joinType);
        }

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(string property, Expression<Func<TAlias>> aliasProjection, JoinType joinType)
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

        protected virtual ISubFlowQuery<TSource> Join<TAlias>(System.Linq.Expressions.Expression propertyProjection, string root, Expression<Func<TAlias>> aliasProjection, JoinType joinType)
        {
            string property = ExpressionHelper.GetPropertyName(propertyProjection, root);

            return Join(property, aliasProjection, joinType);
        }

        protected virtual ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias, JoinType.LeftOuterJoin);
        }

        protected virtual ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin);
        }

        protected virtual ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin);
        }

        protected virtual ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin);
        }

        protected virtual ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.LeftOuterJoin, revealConvention);
        }

        protected virtual ISubFlowQuery<TSource> Limit(int limit)
        {
            Criteria.SetMaxResults(limit);

            return this;
        }

        protected virtual ISubFlowQuery<TSource> Limit(int limit, int offset)
        {
            Criteria.SetFirstResult(offset);

            return Limit(limit);
        }

        protected virtual ISubFlowQuery<TSource> RightOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias, JoinType.RightOuterJoin);
        }

        protected virtual ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.RightOuterJoin);
        }

        protected virtual ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.RightOuterJoin);
        }

        protected virtual ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias, JoinType.RightOuterJoin);
        }

        protected virtual ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return Join(projection, alias, JoinType.RightOuterJoin, revealConvention);
        }

        protected virtual ISubFlowQuery<TSource> Select(params string[] properties)
        {
            return Select
            (
                Projections
                    .ProjectionList()
                        .AddProperties(properties)
            );
        }

        protected virtual ISubFlowQuery<TSource> Select(IProjection projection)
        {
            Criteria.SetProjection(projection);

            return this;
        }

        protected virtual ISubFlowQuery<TSource> Select(params Expression<Func<TSource, object>>[] expressions)
        {
            return Select
            (
                Projections
                    .ProjectionList()
                        .AddProperties(expressions)
            );
        }

        protected virtual ISubFlowQuery<TSource> SelectDistinct(IProjection projection)
        {
            return Select(Projections.Distinct(projection));
        }

        protected virtual ISubFlowQuery<TSource> SelectDistinct(params string[] properties)
        {
            return SelectDistinct
            (
                Projections
                    .ProjectionList()
                        .AddProperties(properties)
            );
        }

        protected virtual ISubFlowQuery<TSource> SelectDistinct(params Expression<Func<TSource, object>>[] expressions)
        {
            return SelectDistinct
            (
                Projections
                    .ProjectionList()
                        .AddProperties(expressions)
            );
        }

        protected virtual ISubFlowQuery<TSource> SetRootAlias<TRoot>(Expression<Func<TRoot>> rootAlias)
        {
            if (rootAlias == null)
            {
                throw new ArgumentNullException("rootAlias");
            }

            string rootAliasName = ExpressionHelper.GetPropertyName(rootAlias);

            PropertyAliases.Add(rootAliasName, rootAliasName);

            return this;
        }

        protected virtual ISubFlowQuery<TSource> Skip(int skip)
        {
            Criteria.SetFirstResult(skip);

            return this;
        }

        protected virtual ISubFlowQuery<TSource> Take(int take)
        {
            return Limit(take);
        }

        protected virtual ISubFlowQuery<TSource> Where(params ICriterion[] criterions)
        {
            if (criterions == null)
            {
                throw new ArgumentNullException();
            }

            foreach (var criterion in criterions)
            {
                if (criterion == null)
                {
                    throw new ArgumentNullException("the list of criterions contains null items");
                }
                Criteria.Add(criterion);
            }
            return this;
        }

        protected virtual ISubFlowQuery<TSource> Where(string property, IsExpression expression)
        {
            ICriterion criterion = expression.Compile(property);
            if (expression.Negate)
            {
                criterion = Restrictions.Not(criterion);
            }
            return Where(criterion);
        }

        protected virtual ISubFlowQuery<TSource> Where(Expression<Func<TSource, bool>> expression)
        {
            return Where(RestrictionHelper.GetCriterion(expression.Body, expression.Parameters[0].Name, PropertyAliases));
        }

        protected virtual ISubFlowQuery<TSource> Where(Expression<Func<TSource, object>> property, IsExpression expression)
        {
            return Where(ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name), expression);
        }

        #endregion Methods



        #region ISubFlowQuery<TSource> Members

        #region Count

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Count()
        {
            return Count();
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Count(string property)
        {
            return Count(property);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Count(IProjection projection)
        {
            return Count(projection);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Count(Expression<Func<TSource, object>> property)
        {
            return Count(property);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.CountDistinct(string property)
        {
            return CountDistinct(property);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.CountDistinct(Expression<Func<TSource, object>> property)
        {
            return CountDistinct(property);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.CountLong()
        {
            return CountLong();
        }

        #endregion

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.FullJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return FullJoin(property, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return FullJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return FullJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return FullJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return FullJoin(projection, alias, revealConvention);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.InnerJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(property, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return InnerJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return InnerJoin(projection, alias, revealConvention);
        }

        /// <summary>
        /// Performs an Inner Join.
        /// </summary>
        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Join<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return Join(property, alias);
        }

        /// <summary>
        /// Performs an Inner Join.
        /// </summary>
        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return Join(projection, alias, revealConvention);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return Join(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.LeftOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return LeftOuterJoin(property, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return LeftOuterJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return LeftOuterJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return LeftOuterJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return LeftOuterJoin(projection, alias, revealConvention);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.RightOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias)
        {
            return RightOuterJoin(property, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias)
        {
            return RightOuterJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias)
        {
            return RightOuterJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias)
        {
            return RightOuterJoin(projection, alias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention)
        {
            return RightOuterJoin(projection, alias, revealConvention);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Select(params string[] properties)
        {
            return Select(properties);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Select(IProjection projection)
        {
            return Select(projection);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Select(params Expression<Func<TSource, object>>[] expressions)
        {
            return Select(expressions);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.SelectDistinct(params Expression<Func<TSource, object>>[] expressions)
        {
            return SelectDistinct(expressions);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.SelectDistinct(IProjection projection)
        {
            return SelectDistinct(projection);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.SelectDistinct(params string[] properties)
        {
            return SelectDistinct(properties);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.SetRootAlias<TRoot>(Expression<Func<TRoot>> rootAlias)
        {
            return SetRootAlias(rootAlias);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Limit(int limit)
        {
            return Limit(limit);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Limit(int limit, int offset)
        {
            return Limit(limit, offset);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Skip(int skip)
        {
            return Skip(skip);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Take(int take)
        {
            return Take(take);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Where(string property, IsExpression expression)
        {
            return Where(property, expression);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Where(Expression<Func<TSource, object>> property, IsExpression expression)
        {
            return Where(property, expression);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Where(Expression<Func<TSource, bool>> expression)
        {
            return Where(expression);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.Where(params ICriterion[] criterions)
        {
            return Where(criterions);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.And(string property, IsExpression expression)
        {
            return And(property, expression);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.And(Expression<Func<TSource, object>> property, IsExpression expression)
        {
            return And(property, expression);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.And(Expression<Func<TSource, bool>> expression)
        {
            return And(expression);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.And(params ICriterion[] criterions)
        {
            return And(criterions);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.OrderBy(string property)
        {
            return OrderBy(property);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.OrderBy(Expression<Func<TSource, object>> property)
        {
            return OrderBy(property);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.OrderBy(IProjection projection)
        {
            return OrderBy(projection);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.OrderByDescending(string property)
        {
            return OrderByDescending(property);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.OrderByDescending(Expression<Func<TSource, object>> property)
        {
            return OrderByDescending(property);
        }

        ISubFlowQuery<TSource> ISubFlowQuery<TSource>.OrderByDescending(IProjection projection)
        {
            return OrderByDescending(projection);
        }

        #endregion ISubFlowQuery<TSource> Members
    }
}