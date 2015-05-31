namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.MethodCalls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Defines the functionality required of a class used to resolve <see cref="IProjection" /> instances from
    ///     <see cref="MethodCallExpression" />.
    /// </summary>
    public abstract class MethodCallExpressionHandlerBase : IMethodCallExpressionHandler
    {
        /// <summary>
        ///     The method names supported by this <see cref="MethodCallExpressionHandlerBase" /> instance.
        /// </summary>
        private readonly IEnumerable<string> _supportedMethodNames;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MethodCallExpressionHandlerBase" /> class.
        /// </summary>
        /// <param name="supportedMethodNames">
        ///     The supported method names.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="supportedMethodNames" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="supportedMethodNames" /> is empty.
        /// </exception>
        protected MethodCallExpressionHandlerBase(params string[] supportedMethodNames)
        {
            if (supportedMethodNames == null)
            {
                throw new ArgumentNullException("supportedMethodNames");
            }

            if (supportedMethodNames.Length == 0)
            {
                throw new ArgumentException("supportedMethodNames cannot be null", "supportedMethodNames");
            }

            _supportedMethodNames = supportedMethodNames;
        }

        /// <summary>
        ///     Gets a value indicating whether the inheriting class can handle construction.
        /// </summary>
        /// <value>
        ///     Indicates whether the inheriting class can handle construction.
        /// </value>
        protected virtual bool CanHandleConstruction
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the inheriting class can handle projection.
        /// </summary>
        /// <value>
        ///     Indicates whether the inheriting class can handle construction.
        /// </value>
        protected virtual bool CanHandleProjection
        {
            get
            {
                return true;
            }
        }

        /// <inheritdoc />
        public virtual bool CanHandleConstructionOf(Expression expression)
        {
            return CanHandleConstruction
                && ExpressionIsOfDesiredKind(expression, _supportedMethodNames);
        }

        /// <inheritdoc />
        public virtual bool CanHandleProjectionOf
            (
            Expression expression,
            HelperContext context
            )
        {
            return CanHandleProjection
                && ExpressionIsOfDesiredKind(expression, _supportedMethodNames);
        }

        /// <inheritdoc />
        public virtual int Construct
            (
            Expression expression,
            object[] arguments,
            out object value,
            out bool wasHandled
            )
        {
            value = null;
            wasHandled = false;

            return 0;
        }

        /// <inheritdoc />
        public virtual IProjection Project
            (
            Expression expression,
            HelperContext context
            )
        {
            var methodCall = (MethodCallExpression)expression;

            Expression subExpression = methodCall.Object ?? methodCall.Arguments[0];

            IProjection projection = ProjectionHelper.GetProjection(subExpression, context);

            return ProjectCore(methodCall, subExpression, projection, context);
        }

        /// <summary>
        ///     Verifies that the given <see cref="Expression" /> expression is of desired kind.
        /// </summary>
        /// <param name="expression">
        ///     The given <see cref="Expression" /> expression.
        /// </param>
        /// <param name="supportedMethodNames">
        ///     The supported method names.
        /// </param>
        /// <returns>
        ///     If the expression is of desired kind; return true, otherwise false.
        /// </returns>
        protected virtual bool ExpressionIsOfDesiredKind
            (
            Expression expression,
            IEnumerable<string> supportedMethodNames
            )
        {
            var methodCall = expression as MethodCallExpression;

            return methodCall != null
                && supportedMethodNames.Any(x => methodCall.Method.Name == x);
        }

        /// <summary>
        ///     Handles the given <see cref="MethodCallExpression" />.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="MethodCallExpression" />.
        /// </param>
        /// <param name="subExpression">
        ///     The sub-expression (normally the first argument of the method call or the property on
        ///     which the call was made).
        /// </param>
        /// <param name="projection">
        ///     The projection for the sub-expression (normally the first argument of the method call or the property on
        ///     which the call was made).
        /// </param>
        /// <param name="context">
        ///     The helper context.
        /// </param>
        /// <returns>
        ///     The <see cref="IProjection" /> or null if no <see cref="IProjection" /> could be resolved.
        /// </returns>
        protected abstract IProjection ProjectCore
            (
            MethodCallExpression expression,
            Expression subExpression,
            IProjection projection,
            HelperContext context
            );
    }
}