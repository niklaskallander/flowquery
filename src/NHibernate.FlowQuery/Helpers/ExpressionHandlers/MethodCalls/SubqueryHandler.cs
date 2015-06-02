namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles method calls to <see cref="Aggregate.Subquery{T}(IDetachedImmutableFlowQuery)" /> and
    ///     <see cref="Aggregate.Subquery{T}(DetachedCriteria)" />.
    /// </summary>
    public class SubqueryHandler : AbstractMethodCallHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SubqueryHandler" /> class.
        /// </summary>
        public SubqueryHandler()
            : base(supportedMethodNames: "Subquery")
        {
        }

        /// <inheritdoc />
        protected override IProjection ProjectCore
            (
            MethodCallExpression expression,
            Expression subExpression,
            IProjection projection,
            HelperContext context
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