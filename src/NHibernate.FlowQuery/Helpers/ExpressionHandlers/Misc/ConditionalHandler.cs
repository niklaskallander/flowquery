namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.Misc
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles projection of conditional expressions.
    /// </summary>
    public class ConditionalHandler : AbstractHandler
    {
        /// <inheritdoc />
        public override bool CanHandleProjectionOf(Expression expression, HelperContext context)
        {
            return expression is ConditionalExpression;
        }

        /// <inheritdoc />
        public override IProjection Project(Expression expression, HelperContext context)
        {
            var condition = (ConditionalExpression)expression;

            return Projections
                .Conditional
                (
                    RestrictionHelper.GetCriterion(condition.Test, context),
                    ProjectionHelper.GetProjection(condition.IfTrue, context),
                    ProjectionHelper.GetProjection(condition.IfFalse, context)
                );
        }
    }
}