using System;
using System.Linq.Expressions;
using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Core.Orders
{
    public interface IOrderBuilder<TSource, TFlowQuery>
        where TSource : class
        where TFlowQuery : class, IFlowQuery<TSource, TFlowQuery>
    {
        TFlowQuery By(string property, bool ascending = true);

        TFlowQuery By(IProjection projection, bool ascending = true);

        TFlowQuery By(Expression<Func<TSource, object>> property, bool ascending = true);

        /// <summary>
        /// OrderBy an alias in your projections. NOTE: Only works when projecting with MemberInitializers ( standalone and in new expressions ) and SelectSetup.
        /// </summary>
        TFlowQuery By<TProjection>(Expression<Func<TProjection, object>> projectionProperty, bool ascending = true);

        /// <summary>
        /// OrderBy an alias in your projections. NOTE: Only works when projecting with MemberInitializers ( standalone and in new expressions ) and SelectSetup.
        /// </summary>
        TFlowQuery By<TProjection>(string property, bool ascending = true);

        TFlowQuery ByAscending(string property);

        TFlowQuery ByAscending(IProjection projection);

        TFlowQuery ByAscending(Expression<Func<TSource, object>> property);

        /// <summary>
        /// OrderBy an alias in your projections. NOTE: Only works when projecting with MemberInitializers ( standalone and in new exressions ) and SelectSetup.
        /// </summary>
        TFlowQuery ByAscending<TProjection>(Expression<Func<TProjection, object>> projectionProperty);

        /// <summary>
        /// OrderBy an alias in your projections. NOTE: Only works when projecting with MemberInitializers ( standalone and in new exressions ) and SelectSetup.
        /// </summary>
        TFlowQuery ByAscending<TProjection>(string property);

        TFlowQuery ByDescending(string property);

        TFlowQuery ByDescending(IProjection projection);

        TFlowQuery ByDescending(Expression<Func<TSource, object>> property);

        /// <summary>
        /// OrderBy an alias in your projections. NOTE: Only works when projecting with MemberInitializers ( standalone and in new exressions ) and SelectSetup.
        /// </summary>
        TFlowQuery ByDescending<TProjection>(Expression<Func<TProjection, object>> projectionProperty);

        /// <summary>
        /// OrderBy an alias in your projections. NOTE: Only works when projecting with MemberInitializers ( standalone and in new exressions ) and SelectSetup.
        /// </summary>
        TFlowQuery ByDescending<TProjection>(string property);
    }
}