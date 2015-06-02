namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.Misc
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles projection of <see cref="LambdaExpression" /> / <see cref="ExpressionType.Lambda" />.
    /// </summary>
    public class LambdaHandler : AbstractHandler
    {
        /// <inheritdoc />
        public override bool CanHandleConstructionOf
            (
            Expression expression
            )
        {
            return expression is LambdaExpression;
        }

        /// <inheritdoc />
        public override bool CanHandleProjectionOf
            (
            Expression expression,
            HelperContext context
            )
        {
            return expression is LambdaExpression;
        }

        /// <inheritdoc />
        public override int Construct
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
        public override IProjection Project
            (
            Expression expression,
            HelperContext context
            )
        {
            return ProjectionHelper.GetProjection(((LambdaExpression)expression).Body, context);
        }
    }
}