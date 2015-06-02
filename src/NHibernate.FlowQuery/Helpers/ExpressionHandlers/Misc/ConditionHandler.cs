namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.Misc
{
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles the projection of condition expressions (not to be confused with conditional expressions).
    /// </summary>
    public class ConditionHandler : AbstractHandler
    {
        /// <summary>
        ///     The supported expression types.
        /// </summary>
        private static readonly ExpressionType[] SupportedExpressionTypes =
        {
            ExpressionType.AndAlso,
            ExpressionType.NotEqual,
            ExpressionType.OrElse,
            ExpressionType.Equal,
            ExpressionType.GreaterThan,
            ExpressionType.GreaterThanOrEqual,
            ExpressionType.LessThan,
            ExpressionType.LessThanOrEqual
        };

        /// <inheritdoc />
        public override bool CanHandleProjectionOf
            (
            Expression expression,
            HelperContext context
            )
        {
            return SupportedExpressionTypes.Contains(expression.NodeType);
        }

        /// <inheritdoc />
        public override IProjection Project
            (
            Expression expression,
            HelperContext context
            )
        {
            return Projections
                .Conditional
                (
                    RestrictionHelper.GetCriterion(expression, context),
                    Projections.Constant(true),
                    Projections.Constant(false)
                );
        }
    }
}