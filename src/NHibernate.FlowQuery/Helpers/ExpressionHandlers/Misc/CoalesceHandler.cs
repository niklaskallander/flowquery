namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.Misc
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles projection of coalesce expressions.
    /// </summary>
    public class CoalesceHandler : AbstractHandler
    {
        /// <inheritdoc />
        public override bool CanHandleProjectionOf
            (
            Expression expression,
            HelperContext context
            )
        {
            return expression.NodeType == ExpressionType.Coalesce;
        }

        /// <inheritdoc />
        public override IProjection Project
            (
            Expression expression,
            HelperContext context
            )
        {
            var binary = (BinaryExpression)expression;

            IProjection original = ProjectionHelper.GetProjection(binary.Left, context);
            IProjection fallback = ProjectionHelper.GetProjection(binary.Right, context);

            return Projections.Conditional(Restrictions.IsNull(original), fallback, original);
        }
    }
}