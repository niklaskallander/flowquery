namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.MethodCalls
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Defines the functionality required of a class used to resolve <see cref="IProjection" /> instances from
    ///     simple <see cref="MethodCallExpression" />s.
    /// </summary>
    public class SimpleMethodCallHandler : AbstractMethodCallHandler
    {
        /// <summary>
        ///     The resolver.
        /// </summary>
        private readonly Func<IProjection, IProjection> _resolver;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SimpleMethodCallHandler" /> class.
        /// </summary>
        /// <param name="resolver">
        ///     The resolver.
        /// </param>
        /// <param name="supportedMethodNames">
        ///     The supported method names.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="resolver" /> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="supportedMethodNames" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="supportedMethodNames" /> is empty.
        /// </exception>
        public SimpleMethodCallHandler(Func<IProjection, IProjection> resolver, params string[] supportedMethodNames)
            : base(supportedMethodNames)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException("resolver");
            }

            _resolver = resolver;
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
            return _resolver(projection);
        }
    }
}