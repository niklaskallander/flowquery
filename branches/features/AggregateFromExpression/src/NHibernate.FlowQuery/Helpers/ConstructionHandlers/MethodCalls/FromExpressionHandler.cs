namespace NHibernate.FlowQuery.Helpers.ConstructionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    /// <summary>
    ///     Handles <see cref="MethodCallExpression" /> expressions representing calls to 
    ///     <see cref="Aggregate.FromExpression{TIn,TOut}" />.
    /// </summary>
    public class FromExpressionHandler : IMethodCallConstructionHandler
    {
        /// <inheritdoc />
        public int Handle(MethodCallExpression expression, object[] arguments, out object value, out bool wasHandled)
        {
            var lambda = ExpressionHelper.GetValue<Expression>(expression.Arguments[0]);

            wasHandled = true;

            return ConstructionHelper.Invoke(lambda, arguments, out value);
        }
    }
}