namespace NHibernate.FlowQuery.Helpers.ProjectionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles method calls to <see cref="string.Trim()" />.
    /// </summary>
    public class TrimHandler : MethodCallProjectionHandlerBase
    {
        /// <summary>
        ///     The <see cref="TrimStartHandler" />.
        /// </summary>
        private static readonly TrimStartHandler Start;

        /// <summary>
        ///     The <see cref="TrimEndHandler" />.
        /// </summary>
        private static readonly TrimEndHandler End;

        /// <summary>
        ///     Initializes static members of the <see cref="TrimHandler"/> class.
        /// </summary>
        static TrimHandler()
        {
            Start = new TrimStartHandler();
            End = new TrimEndHandler();
        }

        /// <inheritdoc />
        protected override IProjection HandleCore
            (
            MethodCallExpression expression,
            Expression subExpression,
            IProjection projection,
            string root,
            QueryHelperData data)
        {
            return Start.Trim(End.Trim(projection));
        }
    }
}