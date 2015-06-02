namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.Dialect.Function;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles method calls to <see cref="string.TrimEnd" />.
    /// </summary>
    public class TrimEndHandler : AbstractMethodCallHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TrimEndHandler" /> class.
        /// </summary>
        public TrimEndHandler()
            : base(supportedMethodNames: "TrimEnd")
        {
        }

        /// <summary>
        ///     Trims the end of the given <see cref="IProjection" />.
        /// </summary>
        /// <param name="projection">
        ///     The <see cref="IProjection" /> to trim.
        /// </param>
        /// <returns>
        ///     The trimmed <see cref="IProjection" />.
        /// </returns>
        protected internal virtual IProjection Trim(IProjection projection)
        {
            return new SqlFunctionProjection
                (
                new StandardSQLFunction("rtrim"),
                NHibernateUtil.String,
                projection
                );
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
            return Trim(projection);
        }
    }
}