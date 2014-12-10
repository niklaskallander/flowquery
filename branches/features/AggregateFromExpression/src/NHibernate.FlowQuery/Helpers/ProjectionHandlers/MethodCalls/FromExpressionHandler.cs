namespace NHibernate.FlowQuery.Helpers.ProjectionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    public class FromExpressionHandler : MethodCallProjectionHandlerBase
    {
        /// <summary>
        ///     Handles the given <see cref="MethodCallExpression" />.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="MethodCallExpression" />.
        /// </param>
        /// <param name="subExpression">
        ///     The sub-expression (normally the first argument of the method call or the property on
        ///     which the call was made).
        /// </param>
        /// <param name="projection">
        ///     The projection for the sub-expression (normally the first argument of the method call or the property on
        ///     which the call was made).
        /// </param>
        /// <param name="root">
        ///     The entity root name.
        /// </param>
        /// <param name="data">
        ///     The <see cref="QueryHelperData" />.
        /// </param>
        /// <returns>
        ///     The <see cref="IProjection" /> or null if no <see cref="IProjection" /> could be resolved.
        /// </returns>
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