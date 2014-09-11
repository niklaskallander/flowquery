namespace NHibernate.FlowQuery.Helpers.ProjectionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles method calls to <see cref="NHibernate.FlowQuery.Property.As{TDestination}(string)" />.
    /// </summary>
    public class AsHandler : MethodCallProjectionHandlerBase
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
            return Projections.Property(ExpressionHelper.GetPropertyName(subExpression, root));
        }
    }
}