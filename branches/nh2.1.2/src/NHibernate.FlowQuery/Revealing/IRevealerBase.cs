using System;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Revealing.Conventions;

namespace NHibernate.FlowQuery.Revealing
{
    public interface IRevealerBase
    {
        #region Data Members (1)

        IRevealConvention RevealConvention { get; }

        #endregion Data Members

        #region Operations (2)

        string Reveal(Expression<Func<object>> expression);

        string Reveal(Expression<Func<object>> expression, IRevealConvention convention);

        #endregion Operations
    }
}