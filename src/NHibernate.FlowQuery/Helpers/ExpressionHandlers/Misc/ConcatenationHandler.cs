namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.Misc
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles projections for string concatenation expressions.
    /// </summary>
    public class ConcatenationHandler : AbstractHandler
    {
        /// <inheritdoc />
        public override bool CanHandleProjectionOf
            (
            Expression expression,
            HelperContext context
            )
        {
            return expression.NodeType == ExpressionType.Add
                && expression.Type == typeof(string);
        }

        /// <inheritdoc />
        public override IProjection Project
            (
            Expression expression,
            HelperContext context
            )
        {
            var projections = new List<IProjection>();

            var binary = (BinaryExpression)expression;

            foreach (Expression expressionPart in binary.Flatten())
            {
                IProjection projection = ProjectionHelper.GetProjection(expressionPart, context);

                projections.Add(projection);
            }

            return new SqlFunctionProjection("concat", NHibernateUtil.String, projections.ToArray());
        }
    }
}