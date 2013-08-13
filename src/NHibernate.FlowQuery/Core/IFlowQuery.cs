using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Expressions;

namespace NHibernate.FlowQuery.Core
{
    public partial interface IFlowQuery<TSource>
        where TSource : class
    {
        ICriteria Criteria { get; }

        #region Select

        FlowQuerySelection<TSource> Select();

        ISelectSetup<TSource, TReturn> Select<TReturn>();

        FlowQuerySelection<TReturn> Select<TReturn>(ISelectSetup<TSource, TReturn> setup);

        FlowQuerySelection<TSource> Select(params string[] properties);

        FlowQuerySelection<TSource> Select(IProjection projection);

        FlowQuerySelection<object> Select(PropertyProjection projection);

        FlowQuerySelection<TReturn> Select<TReturn>(IProjection projection);

        FlowQuerySelection<TReturn> Select<TReturn>(PropertyProjection projection);

        FlowQuerySelection<TSource> Select(params Expression<Func<TSource, object>>[] properties);

        FlowQuerySelection<TReturn> Select<TReturn>(Expression<Func<TSource, TReturn>> expression);

        #endregion

        #region SelectDictionary

        Dictionary<TKey, TValue> SelectDictionary<TKey, TValue>(Expression<Func<TSource, TKey>> key, Expression<Func<TSource, TValue>> value);

        #endregion

        #region SelectDistinct

        ISelectSetup<TSource, TReturn> SelectDistinct<TReturn>();

        ISelectSetup<TSource, TSource> SelectDistinct();

        FlowQuerySelection<TReturn> SelectDistinct<TReturn>(ISelectSetup<TSource, TReturn> setup);

        FlowQuerySelection<TSource> SelectDistinct(IProjection projection);

        FlowQuerySelection<object> SelectDistinct(PropertyProjection projection);

        FlowQuerySelection<TSource> SelectDistinct(params string[] properties);

        FlowQuerySelection<TReturn> SelectDistinct<TReturn>(IProjection projection);

        FlowQuerySelection<TReturn> SelectDistinct<TReturn>(PropertyProjection projection);

        FlowQuerySelection<TReturn> SelectDistinct<TReturn>(Expression<Func<TSource, TReturn>> expression);

        FlowQuerySelection<TSource> SelectDistinct(params Expression<Func<TSource, object>>[] expressions);

        #endregion

        #region Where, And

        IFlowQuery<TSource> Where(params ICriterion[] criterions);

        IFlowQuery<TSource> Where(string property, IsExpression expression);

        IFlowQuery<TSource> Where(Expression<Func<TSource, bool>> expression);

        IFlowQuery<TSource> Where(Expression<Func<TSource, object>> property, IsExpression expression);

        IFlowQuery<TSource> Where(Expression<Func<TSource, WhereDelegate, bool>> expression);

        IFlowQuery<TSource> And(params ICriterion[] criterions);

        IFlowQuery<TSource> And(string property, IsExpression expression);

        IFlowQuery<TSource> And(Expression<Func<TSource, bool>> expression);

        IFlowQuery<TSource> And(Expression<Func<TSource, object>> property, IsExpression expression);

        IFlowQuery<TSource> And(Expression<Func<TSource, WhereDelegate, bool>> expression);

        bool Any();

        bool Any(params ICriterion[] criterions);

        bool Any(string property, IsExpression expression);

        bool Any(Expression<Func<TSource, bool>> expression);

        bool Any(Expression<Func<TSource, object>> property, IsExpression expression);

        bool Any(Expression<Func<TSource, WhereDelegate, bool>> expression);

        /// <summary>
        /// If you haven't set any values to non-nullable datatypes or non-zero-based datatypes you
        /// must exclude these properties manually, e.g. for DateTime, Enums, bool etc.
        /// </summary>
        IFlowQuery<TSource> RestrictWithExample(TSource exampleInstance, Func<IExampleWrapper<TSource>, IExampleWrapper<TSource>> example);

        #endregion

        #region Count

        int Count();

        int Count(string property);

        int Count(IProjection projection);

        int Count(Expression<Func<TSource, object>> property);

        int CountDistinct(string property);

        int CountDistinct(Expression<Func<TSource, object>> property);

        long CountLong();

        #endregion

        #region OrderBy

        IFlowQuery<TSource> OrderBy(string property);

        IFlowQuery<TSource> OrderBy(IProjection projection);

        IFlowQuery<TSource> OrderBy(Expression<Func<TSource, object>> property);

        /// <summary>
        /// OrderBy an alias in your projections. NOTE: Only works when projecting with MemberInitializers ( standalone and in new expressions ) and SelectSetup.
        /// </summary>
        IFlowQuery<TSource> OrderBy<TProjection>(Expression<Func<TProjection, object>> projectionProperty);

        /// <summary>
        /// OrderBy an alias in your projections. NOTE: Only works when projecting with MemberInitializers ( standalone and in new expressions ) and SelectSetup.
        /// </summary>
        IFlowQuery<TSource> OrderBy<TProjection>(string property);

        IFlowQuery<TSource> OrderByDescending(string property);

        IFlowQuery<TSource> OrderByDescending(IProjection projection);

        IFlowQuery<TSource> OrderByDescending(Expression<Func<TSource, object>> property);

        /// <summary>
        /// OrderBy an alias in your projections. NOTE: Only works when projecting with MemberInitializers ( standalone and in new exressions ) and SelectSetup.
        /// </summary>
        IFlowQuery<TSource> OrderByDescending<TProjection>(Expression<Func<TProjection, object>> projectionProperty);

        /// <summary>
        /// OrderBy an alias in your projections. NOTE: Only works when projecting with MemberInitializers ( standalone and in new exressions ) and SelectSetup.
        /// </summary>
        IFlowQuery<TSource> OrderByDescending<TProjection>(string property);

        #endregion

        #region Skip, Take, Limit

        IFlowQuery<TSource> Limit(int limit);

        IFlowQuery<TSource> Limit(int limit, int offset);

        IFlowQuery<TSource> Skip(int skip);

        IFlowQuery<TSource> Take(int take);

        #endregion
    }
}