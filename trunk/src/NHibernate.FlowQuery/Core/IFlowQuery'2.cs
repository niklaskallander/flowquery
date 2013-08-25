using System;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core.Joins;
using NHibernate.FlowQuery.Expressions;

namespace NHibernate.FlowQuery.Core
{
    public interface IFlowQuery<TSource, TFlowQuery>
        where TSource : class
        where TFlowQuery : class, IFlowQuery<TSource, TFlowQuery>
    {
        TFlowQuery And(params ICriterion[] criterions);

        TFlowQuery And(string property, IsExpression expression);

        TFlowQuery And(Expression<Func<TSource, bool>> expression);

        TFlowQuery And(Expression<Func<TSource, object>> property, IsExpression expression);

        TFlowQuery And(Expression<Func<TSource, WhereDelegate, bool>> expression);

        TFlowQuery ClearOrders();

        TFlowQuery ClearJoins();

        TFlowQuery ClearLimit();

        TFlowQuery Where(params ICriterion[] criterions);

        TFlowQuery Where(string property, IsExpression expression);

        TFlowQuery Where(Expression<Func<TSource, bool>> expression);

        TFlowQuery Where(Expression<Func<TSource, object>> property, IsExpression expression);

        TFlowQuery Where(Expression<Func<TSource, WhereDelegate, bool>> expression);

        /// <summary>
        /// If you haven't set any values to non-nullable datatypes or non-zero-based datatypes you
        /// must exclude these properties manually, e.g. for DateTime, Enums, bool etc.
        /// </summary>
        TFlowQuery RestrictByExample(TSource exampleInstance, Action<IExampleWrapper<TSource>> example);

        IJoinBuilder<TSource, TFlowQuery> Inner { get; }

        IJoinBuilder<TSource, TFlowQuery> LeftOuter { get; }

        IJoinBuilder<TSource, TFlowQuery> RightOuter { get; }

        IJoinBuilder<TSource, TFlowQuery> Full { get; }

        TFlowQuery OrderBy(string property, bool ascending = true);

        TFlowQuery OrderBy(IProjection projection, bool ascending = true);

        TFlowQuery OrderBy(Expression<Func<TSource, object>> property, bool ascending = true);

        /// <summary>
        /// OrderBy an alias in your projections. NOTE: Only works when projecting with MemberInitializers ( standalone and in new expressions ) and SelectSetup.
        /// </summary>
        TFlowQuery OrderBy<TProjection>(Expression<Func<TProjection, object>> projectionProperty, bool ascending = true);

        /// <summary>
        /// OrderBy an alias in your projections. NOTE: Only works when projecting with MemberInitializers ( standalone and in new expressions ) and SelectSetup.
        /// </summary>
        TFlowQuery OrderBy<TProjection>(string property, bool ascending = true);

        TFlowQuery OrderByDescending(string property);

        TFlowQuery OrderByDescending(IProjection projection);

        TFlowQuery OrderByDescending(Expression<Func<TSource, object>> property);

        /// <summary>
        /// OrderBy an alias in your projections. NOTE: Only works when projecting with MemberInitializers ( standalone and in new exressions ) and SelectSetup.
        /// </summary>
        TFlowQuery OrderByDescending<TProjection>(Expression<Func<TProjection, object>> projectionProperty);

        /// <summary>
        /// OrderBy an alias in your projections. NOTE: Only works when projecting with MemberInitializers ( standalone and in new exressions ) and SelectSetup.
        /// </summary>
        TFlowQuery OrderByDescending<TProjection>(string property);

        TFlowQuery Limit(int limit);

        TFlowQuery Limit(int limit, int offset);

        TFlowQuery Skip(int skip);

        TFlowQuery Take(int take);
    }
}