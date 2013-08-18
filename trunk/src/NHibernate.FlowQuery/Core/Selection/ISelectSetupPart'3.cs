using System;
using System.Linq.Expressions;
using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Core.Selection
{
    public interface ISelectSetupPart<TSource, TDestination>
        where TSource : class
    {
        #region Operations (3)

        ISelectSetup<TSource, TDestination> Use(string property);

        ISelectSetup<TSource, TDestination> Use(IProjection projection);

        ISelectSetup<TSource, TDestination> Use<TProjection>(Expression<Func<TSource, TProjection>> expression);

        #endregion Operations
    }
}