namespace NHibernate.FlowQuery.Helpers.ProjectionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.Dialect.Function;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles method calls to <see cref="string.TrimStart" />.
    /// </summary>
    public class TrimStartHandler : MethodCallProjectionHandlerBase
    {
        /// <summary>
        ///     Trims the start of the given <see cref="IProjection" />.
        /// </summary>
        /// <param name="projection">
        ///     The <see cref="IProjection" /> to trim.
        /// </param>
        /// <returns>
        ///     The trimmed <see cref="IProjection"/>.
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
        protected override IProjection HandleCore
            (
            MethodCallExpression expression,
            Expression subExpression,
            IProjection projection,
            string root,
            QueryHelperData data)
        {
            return Trim(projection);
        }
    }
}