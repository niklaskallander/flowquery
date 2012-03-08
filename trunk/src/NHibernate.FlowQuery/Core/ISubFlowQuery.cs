using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Expressions;
using NHibernate.FlowQuery.Revealing.Conventions;

namespace NHibernate.FlowQuery.Core
{
    public interface ISubFlowQuery<TSource>
    {
        #region OrderBy

        ISubFlowQuery<TSource> OrderBy(string property);

        ISubFlowQuery<TSource> OrderBy(IProjection projection);

        ISubFlowQuery<TSource> OrderBy(Expression<Func<TSource, object>> property);

        ISubFlowQuery<TSource> OrderByDescending(string property);

        ISubFlowQuery<TSource> OrderByDescending(IProjection projection);

        ISubFlowQuery<TSource> OrderByDescending(Expression<Func<TSource, object>> property);

        #endregion

        #region Count

        ISubFlowQuery<TSource> Count();

        ISubFlowQuery<TSource> Count(string property);

        ISubFlowQuery<TSource> Count(IProjection projection);

        ISubFlowQuery<TSource> Count(Expression<Func<TSource, object>> property);

        ISubFlowQuery<TSource> CountDistinct(string property);

        ISubFlowQuery<TSource> CountDistinct(Expression<Func<TSource, object>> property);

        ISubFlowQuery<TSource> CountLong();

        #endregion


        #region Operations (34)

        ISubFlowQuery<TSource> And(params ICriterion[] criterions);

        ISubFlowQuery<TSource> And(string property, IsExpression expression);

        ISubFlowQuery<TSource> And(Expression<Func<TSource, bool>> expression);

        ISubFlowQuery<TSource> And(Expression<Func<TSource, object>> property, IsExpression expression);

        ISubFlowQuery<TSource> FullJoin<TAlias>(string property, Expression<Func<TAlias>> alias);

        ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias);

        ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Uses the default revealing convention to convert the projection into a string.
        /// </summary>
        ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Uses the provided revealing convention to convert the projection into a string.
        /// </summary>
        ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention);

        ISubFlowQuery<TSource> InnerJoin<TAlias>(string property, Expression<Func<TAlias>> alias);

        ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias);

        ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Uses the default revealing convention to convert the projection into a string.
        /// </summary>
        ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Uses the provided revealing convention to convert the projection into a string.
        /// </summary>
        ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention);

        /// <summary>
        /// Performs an Inner Join.
        /// </summary>
        ISubFlowQuery<TSource> Join<TAlias>(string property, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Performs an Inner Join.
        /// </summary>
        ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Performs an Inner Join.
        /// </summary>
        ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Performs an Inner Join.
        /// Uses the default revealing convention to convert the projection into a string.
        /// </summary>
        ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Performs an Inner Join.
        /// Uses the provided revealing convention to convert the projection into a string.
        /// </summary>
        ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention);

        ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias);

        ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias);

        ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Uses the default revealing convention to convert the projection into a string.
        /// </summary>
        ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Uses the provided revealing convention to convert the projection into a string.
        /// </summary>
        ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention);

        ISubFlowQuery<TSource> Limit(int limit);

        ISubFlowQuery<TSource> Limit(int limit, int offset);

        ISubFlowQuery<TSource> RightOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias);

        ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias);

        ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Uses the default revealing convention to convert the projection into a string.
        /// </summary>
        ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Uses the provided revealing convention to convert the projection into a string.
        /// </summary>
        ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention);

        ISubFlowQuery<TSource> Select(params string[] properties);

        ISubFlowQuery<TSource> Select(IProjection projection);

        ISubFlowQuery<TSource> Select(params Expression<Func<TSource, object>>[] expressions);

        ISubFlowQuery<TSource> SelectDistinct(IProjection projection);

        ISubFlowQuery<TSource> SelectDistinct(params string[] properties);

        ISubFlowQuery<TSource> SelectDistinct(params Expression<Func<TSource, object>>[] expressions);

        ISubFlowQuery<TSource> SetRootAlias<TRoot>(Expression<Func<TRoot>> rootAlias);

        ISubFlowQuery<TSource> Skip(int skip);

        ISubFlowQuery<TSource> Take(int take);

        ISubFlowQuery<TSource> Where(params ICriterion[] criterions);

        ISubFlowQuery<TSource> Where(string property, IsExpression expression);

        ISubFlowQuery<TSource> Where(Expression<Func<TSource, bool>> expression);

        ISubFlowQuery<TSource> Where(Expression<Func<TSource, object>> property, IsExpression expression);

        #endregion Operations
    }
}