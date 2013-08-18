using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;

namespace NHibernate.FlowQuery.Core.SelectSetup
{
    public interface ISelectSetup<TSource, TReturn>
        where TSource : class
    {
        #region Data Members (2)

        Dictionary<string, IProjection> Mappings { get; }

        ProjectionList ProjectionList { get; }

        #endregion Data Members

        #region Operations (3)

        ISelectSetupPart<TSource, TReturn> For(string property);

        ISelectSetupPart<TSource, TReturn> For(Expression<Func<TReturn, object>> expression);

        FlowQuerySelection<TReturn> Select();

        #endregion Operations
    }
}