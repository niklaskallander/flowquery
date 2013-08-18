using System;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core.Joins;
using NHibernate.FlowQuery.Core.Orders;
using NHibernate.FlowQuery.Expressions;

namespace NHibernate.FlowQuery.Core
{
    public interface IFlowQuery<TSource, TFlowQuery>
        where TSource : class
        where TFlowQuery : class, IFlowQuery<TSource, TFlowQuery>
    {
        #region Where, And

        TFlowQuery Where(params ICriterion[] criterions);

        TFlowQuery Where(string property, IsExpression expression);

        TFlowQuery Where(Expression<Func<TSource, bool>> expression);

        TFlowQuery Where(Expression<Func<TSource, object>> property, IsExpression expression);

        TFlowQuery Where(Expression<Func<TSource, WhereDelegate, bool>> expression);

        TFlowQuery And(params ICriterion[] criterions);

        TFlowQuery And(string property, IsExpression expression);

        TFlowQuery And(Expression<Func<TSource, bool>> expression);

        TFlowQuery And(Expression<Func<TSource, object>> property, IsExpression expression);

        TFlowQuery And(Expression<Func<TSource, WhereDelegate, bool>> expression);

        /// <summary>
        /// If you haven't set any values to non-nullable datatypes or non-zero-based datatypes you
        /// must exclude these properties manually, e.g. for DateTime, Enums, bool etc.
        /// </summary>
        TFlowQuery RestrictByExample(TSource exampleInstance, Action<IExampleWrapper<TSource>> example);

        #endregion

        #region Join

        IJoinBuilder<TSource, TFlowQuery> Inner { get; }

        IJoinBuilder<TSource, TFlowQuery> LeftOuter { get; }

        IJoinBuilder<TSource, TFlowQuery> RightOuter { get; }

        IJoinBuilder<TSource, TFlowQuery> Full { get; }

        #endregion

        #region OrderBy

        IOrderBuilder<TSource, TFlowQuery> Order { get; }

        IOrderBuilder<TSource, TFlowQuery> Then { get; }

        #endregion

        #region Skip, Take, Limit

        TFlowQuery Limit(int limit);

        TFlowQuery Limit(int limit, int offset);

        TFlowQuery Skip(int skip);

        TFlowQuery Take(int take);

        #endregion
    }
}