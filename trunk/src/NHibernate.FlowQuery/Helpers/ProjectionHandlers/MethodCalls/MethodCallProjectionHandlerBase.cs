namespace NHibernate.FlowQuery.Helpers.ProjectionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Defines the functionality required of a class used to resolve <see cref="IProjection" /> instances from 
    ///     <see cref="MethodCallExpression" />.
    /// </summary>
    public abstract class MethodCallProjectionHandlerBase : IMethodCallProjectionHandler
    {
        /// <inheritdoc />
        public virtual IProjection Handle(MethodCallExpression expression, string root, QueryHelperData data)
        {
            Expression subExpression = expression.Object ?? expression.Arguments[0];

            IProjection projection = ProjectionHelper.GetProjection(subExpression, root, data);

            return HandleCore(expression, subExpression, projection, root, data);
        }

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
        ///     The <see cref="IProjection"/> or null if no <see cref="IProjection" /> could be resolved.
        /// </returns>
        protected abstract IProjection HandleCore
            (
            MethodCallExpression expression,
            Expression subExpression,
            IProjection projection,
            string root,
            QueryHelperData data
            );
    }
}
