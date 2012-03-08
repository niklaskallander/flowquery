using NHibernate.FlowQuery.Expressions;

namespace NHibernate.FlowQuery.Core
{
    public delegate bool WhereDelegate(object property, IsExpression expression);
}