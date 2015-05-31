namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles method calls to <see cref="string.Trim()" />.
    /// </summary>
    public class TrimHandler : MethodCallExpressionHandlerBase
    {
        /// <summary>
        ///     The <see cref="TrimEndHandler" />.
        /// </summary>
        private static readonly TrimEndHandler End;

        /// <summary>
        ///     The <see cref="TrimStartHandler" />.
        /// </summary>
        private static readonly TrimStartHandler Start;

        /// <summary>
        ///     Initializes static members of the <see cref="TrimHandler" /> class.
        /// </summary>
        static TrimHandler()
        {
            Start = new TrimStartHandler();
            End = new TrimEndHandler();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TrimHandler" /> class.
        /// </summary>
        public TrimHandler()
            : base(supportedMethodNames: "Trim")
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
            return Start.Trim(End.Trim(projection));
        }
    }
}