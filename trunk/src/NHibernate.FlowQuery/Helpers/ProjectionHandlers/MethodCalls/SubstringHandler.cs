namespace NHibernate.FlowQuery.Helpers.ProjectionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles method calls to <see cref="string.Substring(int)" /> and
    ///     <see cref="string.Substring(int, int)" />.
    /// </summary>
    public class SubstringHandler : MethodCallProjectionHandlerBase
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
            int start = ExpressionHelper.GetValue<int>(expression.Arguments[0]) + 1;

            int length = expression.Arguments.Count > 1
                ? ExpressionHelper.GetValue<int>(expression.Arguments[1])
                : int.MaxValue;

            return new SqlFunctionProjection
                (
                "substring",
                NHibernateUtil.String,
                projection,
                Projections.Constant(start),
                Projections.Constant(length)
                );
        }
    }
}