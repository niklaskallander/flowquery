﻿namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles method calls to <see cref="Aggregate.CountDistinct{TDestination}(TDestination)" />.
    /// </summary>
    public class CountDistinctHandler : MethodCallExpressionHandlerBase
    {
        /// <inheritdoc />
        protected override IProjection ProjectCore
            (
            MethodCallExpression expression,
            Expression subExpression,
            IProjection projection,
            HelperContext context
            )
        {
            return Projections.CountDistinct(ExpressionHelper.GetPropertyName(subExpression, context.RootAlias));
        }
    }
}