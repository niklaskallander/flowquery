namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.Misc
{
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.Dialect.Function;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles projection of arithmetic expressions.
    /// </summary>
    public class ArithmeticHandler : AbstractHandler
    {
        /// <summary>
        ///     The support expression types.
        /// </summary>
        public static readonly ExpressionType[] SupportExpressionTypes =
        {
            ExpressionType.Add,
            ExpressionType.Subtract,
            ExpressionType.Divide,
            ExpressionType.Multiply
        };

        /// <inheritdoc />
        public override bool CanHandleProjectionOf
            (
            Expression expression,
            HelperContext context
            )
        {
            return SupportExpressionTypes.Contains(expression.NodeType)
                && expression.Type != typeof(string);
        }

        /// <inheritdoc />
        public override IProjection Project
            (
            Expression expression,
            HelperContext context
            )
        {
            string operation = GetArithmeticOperation(expression.NodeType);

            var binary = (BinaryExpression)expression;

            return new SqlFunctionProjection
                (
                new VarArgsSQLFunction("(", operation, ")"),
                NHibernateUtil.GuessType(binary.Left.Type),
                ProjectionHelper.GetProjection(binary.Left, context),
                ProjectionHelper.GetProjection(binary.Right, context)
                );
        }

        /// <summary>
        ///     Gets a arithmetic <see cref="string" /> representation of the given <see cref="ExpressionType" /> type.
        /// </summary>
        /// <param name="type">
        ///     The <see cref="ExpressionType" /> type.
        /// </param>
        /// <returns>
        ///     The arithmetic <see cref="string" /> representation.
        /// </returns>
        private static string GetArithmeticOperation(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.Divide:
                    return "/";

                case ExpressionType.Multiply:
                    return "*";

                case ExpressionType.Subtract:
                    return "-";

                default:
                    return "+";
            }
        }
    }
}