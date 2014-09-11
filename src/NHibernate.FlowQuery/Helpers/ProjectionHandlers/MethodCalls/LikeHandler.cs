namespace NHibernate.FlowQuery.Helpers.ProjectionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    /// <summary>
    ///     Handles method calls to <see cref="string.StartsWith(string)" />, <see cref="string.EndsWith(string)" />, or
    ///     <see cref="string.Contains(string)" />.
    /// </summary>
    public class LikeHandler : IMethodCallProjectionHandler
    {
        /// <inheritdoc />
        public virtual IProjection Handle
            (
            MethodCallExpression expression,
            string root,
            QueryHelperData data
            )
        {
            ICriterion criterion = RestrictionHelper.GetCriterionForMethodCall(expression, root, data);

            return Projections
                .Conditional
                (
                    criterion,
                    Projections.Constant(true, NHibernateUtil.Boolean),
                    Projections.Constant(false, NHibernateUtil.Boolean)
                );
        }
    }
}