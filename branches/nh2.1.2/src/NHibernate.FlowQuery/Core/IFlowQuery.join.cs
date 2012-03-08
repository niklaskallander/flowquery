using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Revealing.Conventions;

namespace NHibernate.FlowQuery.Core
{
    public partial interface IFlowQuery<TSource>
    {
        #region Join

        /// <summary>
        /// Performs an Inner Join.
        /// </summary>
        IFlowQuery<TSource> Join<TAlias>(string property, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Performs an Inner Join.
        /// </summary>
        IFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Performs an Inner Join.
        /// </summary>
        IFlowQuery<TSource> Join<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Performs an Inner Join.
        /// Uses the default revealing convention to convert the projection to a string.
        /// </summary>
        IFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Performs an Inner Join.
        /// Uses the provided revealing convention to convert the projection to a string.
        /// </summary>
        IFlowQuery<TSource> Join<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention);

        #endregion

        #region Inner Join

        IFlowQuery<TSource> InnerJoin<TAlias>(string property, Expression<Func<TAlias>> alias);

        IFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias);

        IFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Uses the default revealing convention to convert the projection to a string.
        /// </summary>
        IFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Uses the provided revealing convention to convert the projection to a string.
        /// </summary>
        IFlowQuery<TSource> InnerJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention);

        #endregion

        #region Left Outer Join

        IFlowQuery<TSource> LeftOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias);

        IFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias);

        IFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Uses the default revealing convention to convert the projection to a string.
        /// </summary>
        IFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Uses the provided revealing convention to convert the projection to a string.
        /// </summary>
        IFlowQuery<TSource> LeftOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention);

        #endregion

        #region Right Outer Join

        IFlowQuery<TSource> RightOuterJoin<TAlias>(string property, Expression<Func<TAlias>> alias);

        IFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias);

        IFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Uses the default revealing convention to convert the projection to a string.
        /// </summary>
        IFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Uses the provided revealing convention to convert the projection to a string.
        /// </summary>
        IFlowQuery<TSource> RightOuterJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention);

        #endregion

        #region FullJoin

        IFlowQuery<TSource> FullJoin<TAlias>(string property, Expression<Func<TAlias>> alias);

        IFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<TSource, TAlias>> projection, Expression<Func<TAlias>> alias);

        IFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<TSource, IEnumerable<TAlias>>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Uses the default revealing convention to convert the projection to a string.
        /// </summary>
        IFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias);

        /// <summary>
        /// Uses the provided revealing convention to convert the projection to a string.
        /// </summary>
        IFlowQuery<TSource> FullJoin<TAlias>(Expression<Func<object>> projection, Expression<Func<TAlias>> alias, IRevealConvention revealConvention);

        #endregion
    }
}