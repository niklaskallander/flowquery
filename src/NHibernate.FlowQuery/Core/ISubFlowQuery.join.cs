using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Revealing.Conventions;

namespace NHibernate.FlowQuery.Core
{
    public partial interface ISubFlowQuery<TSource>
    {
        #region Join

        /// <summary>
        /// Performs an Inner Join.
        /// </summary>
        ISubFlowQuery<TSource> Join<TAlias>(string property, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Performs an Inner Join.
        /// </summary>
        ISubFlowQuery<TSource> Join<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause);

        /// <summary>
        /// Performs an Inner Join.
        /// </summary>
        ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Performs an Inner Join.
        /// </summary>
        ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause);

        /// <summary>
        /// Performs an Inner Join.
        /// </summary>
        ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Performs an Inner Join.
        /// </summary>
        ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause);

        /// <summary>
        /// Performs an Inner Join.
        /// Uses the default revealing convention to convert the projection to a string.
        /// </summary>
        ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Performs an Inner Join.
        /// Uses the default revealing convention to convert the projection to a string.
        /// </summary>
        ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause);

        /// <summary>
        /// Performs an Inner Join.
        /// Uses the provided revealing convention to convert the projection to a string.
        /// </summary>
        ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention);

        /// <summary>
        /// Performs an Inner Join.
        /// Uses the provided revealing convention to convert the projection to a string.
        /// </summary>
        ISubFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention);

        #endregion

        #region Inner Join

        ISubFlowQuery<TSource> InnerJoin<TAlias>(string property, Expression<Func<TAlias>> alias);

        ISubFlowQuery<TSource> InnerJoin<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause);

        ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias);

        ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause);

        ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias);

        ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause);

        /// <summary>
        /// Uses the default revealing convention to convert the projection to a string.
        /// </summary>
        ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Uses the default revealing convention to convert the projection to a string.
        /// </summary>
        ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause);

        /// <summary>
        /// Uses the provided revealing convention to convert the projection to a string.
        /// </summary>
        ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention);

        /// <summary>
        /// Uses the provided revealing convention to convert the projection to a string.
        /// </summary>
        ISubFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention);

        #endregion

        #region Left Outer Join

        ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias);

        ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause);

        ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias);

        ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause);

        ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias);

        ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause);

        /// <summary>
        /// Uses the default revealing convention to convert the projection to a string.
        /// </summary>
        ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Uses the default revealing convention to convert the projection to a string.
        /// </summary>
        ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause);

        /// <summary>
        /// Uses the provided revealing convention to convert the projection to a string.
        /// </summary>
        ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention);

        /// <summary>
        /// Uses the provided revealing convention to convert the projection to a string.
        /// </summary>
        ISubFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention);

        #endregion

        #region Right Outer Join

        ISubFlowQuery<TSource> RightOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias);

        ISubFlowQuery<TSource> RightOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause);

        ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias);

        ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause);

        ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias);

        ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause);

        /// <summary>
        /// Uses the default revealing convention to convert the projection to a string.
        /// </summary>
        ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Uses the default revealing convention to convert the projection to a string.
        /// </summary>
        ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause);

        /// <summary>
        /// Uses the provided revealing convention to convert the projection to a string.
        /// </summary>
        ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention);

        /// <summary>
        /// Uses the provided revealing convention to convert the projection to a string.
        /// </summary>
        ISubFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention);

        #endregion

        #region FullJoin

        ISubFlowQuery<TSource> FullJoin<TAlias>(string property, Expression<Func<TAlias>> alias);

        ISubFlowQuery<TSource> FullJoin<TAlias>(string property, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause);

        ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias);

        ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause);

        ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias);

        ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause);

        /// <summary>
        /// Uses the default revealing convention to convert the projection to a string.
        /// </summary>
        ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Uses the default revealing convention to convert the projection to a string.
        /// </summary>
        ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause);

        /// <summary>
        /// Uses the provided revealing convention to convert the projection to a string.
        /// </summary>
        ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention);

        /// <summary>
        /// Uses the provided revealing convention to convert the projection to a string.
        /// </summary>
        ISubFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, Expression<Func<TAlias, bool>> joinOnClause, IRevealConvention revealConvention);

        #endregion
    }
}