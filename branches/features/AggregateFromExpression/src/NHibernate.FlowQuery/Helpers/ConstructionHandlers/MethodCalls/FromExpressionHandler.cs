namespace NHibernate.FlowQuery.Helpers.ConstructionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    public class FromExpressionHandler : IMethodCallConstructionHandler
    {
        /// <inheritdoc />
        public int Handle(MethodCallExpression expression, object[] arguments, out object value, out bool wasHandled)
        {
            var lambda = ExpressionHelper.GetValue<Expression>(expression.Arguments[0]);

            wasHandled = true;

            return ConstructionHelper.Construct(lambda, arguments, out value);
        }
    }
}