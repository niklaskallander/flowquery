namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.Dialect.Function;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles method calls to <see cref="string.TrimStart" />.
    /// </summary>
    public class TrimStartHandler : AbstractMethodCallHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TrimStartHandler" /> class.
        /// </summary>
        public TrimStartHandler()
            : base(supportedMethodNames: "TrimStart")
        {
        }

        /// <summary>
        ///     Trims the start of the given <see cref="IProjection" />.
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
                new StandardSQLFunction("ltrim"), 
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