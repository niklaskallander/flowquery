using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Core.Selection
{
    public interface ISelectSetup<TSource, TDestination>
        where TSource : class
    {
        #region Data Members (2)

        Dictionary<string, IProjection> Mappings { get; }

        ProjectionList ProjectionList { get; }

        #endregion Data Members

        #region Operations (3)

        ISelectSetupPart<TSource, TDestination> For(string property);

        ISelectSetupPart<TSource, TDestination> For(Expression<Func<TDestination, object>> expression);

        FlowQuerySelection<TDestination> Select();

        #endregion Operations
    }
}