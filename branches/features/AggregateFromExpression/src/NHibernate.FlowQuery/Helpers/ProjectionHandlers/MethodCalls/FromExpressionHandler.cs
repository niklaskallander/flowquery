namespace NHibernate.FlowQuery.Helpers.ProjectionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles <see cref="MethodCallExpression" /> expressions representing calls to 
    ///     <see cref="Aggregate.FromExpression{TIn,TOut}" />.
    /// </summary>
    public class FromExpressionHandler : MethodCallProjectionHandlerBase
    {
        /// <inheritdoc />
        protected override IProjection HandleCore
            (
            MethodCallExpression expression,
            Expression subExpression,
            IProjection projection,
            string root,
            QueryHelperData data
            )
        {
            var fromExpression = ExpressionHelper.GetValue(expression.Arguments[0]) as LambdaExpression;

            if (fromExpression == null)
            {
                return null;
            }

            return ProjectionHelper
                .GetProjectionListForExpression(fromExpression.Body, fromExpression.Parameters[0].Name, data);
        }
    }
}