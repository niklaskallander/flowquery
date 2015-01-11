namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Defines the functionality required of a class used to resolve <see cref="IProjection" /> instances from
    ///     <see cref="MethodCallExpression" />.
    /// </summary>
    public abstract class MethodCallExpressionHandlerBase : IMethodCallExpressionHandler
    {
        /// <inheritdoc />
        public virtual bool CanHandleConstruction(MethodCallExpression expression)
        {
            return false;
        }

        /// <inheritdoc />
        public virtual bool CanHandleProjection(MethodCallExpression expression, HelperContext context)
        {
            return true;
        }

        /// <inheritdoc />
        public virtual int Construct
            (
            MethodCallExpression expression,
            object[] arguments,
            out object value,
            out bool wasHandled
            )
        {
            value = null;
            wasHandled = false;

            return 0;
        }

        /// <inheritdoc />
        public virtual IProjection Project(MethodCallExpression expression, HelperContext context)
        {
            Expression subExpression = expression.Object ?? expression.Arguments[0];

            IProjection projection = ProjectionHelper.GetProjection(subExpression, context);

            return ProjectCore(expression, subExpression, projection, context);
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
        /// <param name="context">
        ///     The helper context.
        /// </param>
        /// <returns>
        ///     The <see cref="IProjection" /> or null if no <see cref="IProjection" /> could be resolved.
        /// </returns>
        protected abstract IProjection ProjectCore
            (
            MethodCallExpression expression,
            Expression subExpression,
            IProjection projection,
            HelperContext context
            );
    }
}