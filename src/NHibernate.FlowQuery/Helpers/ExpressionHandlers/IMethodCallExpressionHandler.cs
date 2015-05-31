namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers
{
    using System.Linq.Expressions;

    /// <summary>
    ///     Defines the functionality required by a class that can extend the <see cref="FlowQuery" /> functionality for
    ///     projections and construction of query results from <see cref="MethodCallExpression" /> expressions.
    /// </summary>
    public interface IMethodCallExpressionHandler : IExpressionHandler
    {
    }
}