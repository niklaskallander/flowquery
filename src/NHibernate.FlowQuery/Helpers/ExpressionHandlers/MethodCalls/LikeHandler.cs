﻿namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles method calls to <see cref="string.StartsWith(string)" />, <see cref="string.EndsWith(string)" />, or
    ///     <see cref="string.Contains(string)" />.
    /// </summary>
    public class LikeHandler : AbstractMethodCallHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LikeHandler"/> class.
        /// </summary>
        public LikeHandler()
            : base(supportedMethodNames: new[] { "Contains", "EndsWith", "StartsWith" })
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
            ICriterion criterion = RestrictionHelper.GetCriterionForMethodCall(expression, context);

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