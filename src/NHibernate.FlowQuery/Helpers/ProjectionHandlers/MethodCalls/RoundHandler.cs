namespace NHibernate.FlowQuery.Helpers.ProjectionHandlers.MethodCalls
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.Type;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles method calls to <see cref="Math.Round(double)" />, <see cref="Math.Round(double, int)" />, 
    ///     <see cref="Math.Round(decimal)" />, or <see cref="Math.Round(decimal, int)" />.
    /// </summary>
    public sealed class RoundHandler : MethodCallProjectionHandlerBase
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
            int digits = 0;

            if (expression.Arguments.Count >= 2 && expression.Arguments[1].Type == typeof(int))
            {
                digits = ExpressionHelper.GetValue<int>(expression.Arguments[1]);
            }

            IType numberType;

            if (expression.Method.ReturnType == typeof(decimal))
            {
                numberType = NHibernateUtil.Decimal;
            }
            else
            {
                numberType = NHibernateUtil.Double;
            }

            return new SqlFunctionProjection
            (
                "round",
                numberType,
                projection,
                Projections.Constant(digits)
            );
        }
    }
}