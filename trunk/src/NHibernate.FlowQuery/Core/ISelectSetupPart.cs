using System;
using System.Linq.Expressions;
using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Core
{
    public interface ISelectSetupPart<TSource, TReturn>
    {
        #region Operations (3)

        ISelectSetup<TSource, TReturn> Use(string property);

        ISelectSetup<TSource, TReturn> Use(IProjection projection);

        ISelectSetup<TSource, TReturn> Use(Expression<Func<TSource, object>> expression);

        #endregion Operations
    }
}