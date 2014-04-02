using System;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Revealing.Conventions;

namespace NHibernate.FlowQuery.Revealing
{
    public interface IRevealerBase
    {
        IRevealConvention RevealConvention { get; }

        string Reveal(Expression<Func<object>> expression);

        string Reveal(Expression<Func<object>> expression, IRevealConvention convention);
    }
}