namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.Misc
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles projection of <see cref="LambdaExpression" /> / <see cref="ExpressionType.Lambda" />.
    /// </summary>
    public class LambdaHandler : IExpressionHandler
    {
        /// <inheritdoc />
        public bool CanHandleConstructionOf
            (
            Expression expression
            )
        {
            return true;
        }

        /// <inheritdoc />
        public bool CanHandleProjectionOf
            (
            Expression expression,
            HelperContext context
            )
        {
            return true;
        }

        /// <inheritdoc />
        public int Construct
            (
            Expression expression,
            object[] arguments,
            out object value,
            out bool wasHandled
            )
        {
            wasHandled = true;

            return ConstructionHelper.Invoke(((LambdaExpression)expression).Body, arguments, out value);
        }

        /// <inheritdoc />
        public IProjection Project
            (
            Expression expression,
            HelperContext context
            )
        {
            return ProjectionHelper.GetProjection(((LambdaExpression)expression).Body, context);
        }
    }
}