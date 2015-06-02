namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.Misc
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles projection of member access expressions.
    /// </summary>
    public class MemberAccessHandler : AbstractHandler
    {
        /// <inheritdoc />
        public override bool CanHandleProjectionOf
            (
            Expression expression,
            HelperContext context
            )
        {
            return expression.NodeType == ExpressionType.MemberAccess;
        }

        /// <inheritdoc />
        public override IProjection Project
            (
            Expression expression,
            HelperContext context
            )
        {
            if (ExpressionHelper.IsRooted(expression, context.RootAlias, context.Data))
            {
                string property = ExpressionHelper.GetPropertyName(expression, context.RootAlias, true, context);

                return Projections.Property(property);
            }

            object value = ExpressionHelper.GetValue(expression);

            return Projections.Constant(value, TypeHelper.GuessType(expression.Type));
        }
    }
}