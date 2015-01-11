namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles method calls to <see cref="NHibernate.FlowQuery.Property.As{TDestination}(string)" />.
    /// </summary>
    public class AsHandler : MethodCallExpressionHandlerBase
    {
        /// <inheritdoc />
        protected override IProjection ProjectCore
            (
            MethodCallExpression expression,
            Expression subExpression,
            IProjection projection,
            HelperContext context
            )
        {
            return Projections.Property(ExpressionHelper.GetPropertyName(subExpression, context.RootAlias));
        }
    }
}