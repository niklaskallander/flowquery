namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.Misc
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core.CustomProjections;
    using NHibernate.Type;

    using Expression = System.Linq.Expressions.Expression;
    using TypeHelper = NHibernate.FlowQuery.Helpers.TypeHelper;

    /// <summary>
    ///     Handles projection of convert expressions.
    /// </summary>
    public class ConvertHandler : AbstractHandler
    {
        /// <inheritdoc />
        public override bool CanHandleProjectionOf
            (
            Expression expression,
            HelperContext context
            )
        {
            return expression.NodeType == ExpressionType.Convert;
        }

        /// <inheritdoc />
        public override IProjection Project
            (
            Expression expression,
            HelperContext context
            )
        {
            var unary = (UnaryExpression)expression;

            IProjection projection = ProjectionHelper.GetProjection(unary.Operand, context);

            if (!unary.IsLiftedToNull)
            {
                IType type = TypeHelper.GuessType(unary.Type, true);

                if (type != null)
                {
                    return new FqCastProjection(type, projection);
                }
            }

            return projection;
        }
    }
}