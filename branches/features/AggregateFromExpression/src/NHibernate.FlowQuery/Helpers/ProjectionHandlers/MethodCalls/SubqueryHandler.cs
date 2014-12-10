namespace NHibernate.FlowQuery.Helpers.ProjectionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles method calls to <see cref="Aggregate.Subquery{T}(IDetachedImmutableFlowQuery)" /> and
    ///     <see cref="Aggregate.Subquery{T}(DetachedCriteria)" />.
    /// </summary>
    public class SubqueryHandler : MethodCallProjectionHandlerBase
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
            object value = ExpressionHelper.GetValue(expression.Arguments[0]);

            var criteria = value as DetachedCriteria;

            if (criteria == null)
            {
                var query = value as IDetachedImmutableFlowQuery;

                if (query != null)
                {
                    criteria = query.Criteria;
                }
            }

            if (criteria != null)
            {
                return Projections.SubQuery(criteria);
            }

            return null;
        }
    }
}