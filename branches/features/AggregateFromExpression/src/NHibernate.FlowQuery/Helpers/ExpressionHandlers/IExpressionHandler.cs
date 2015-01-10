namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers
{
    using NHibernate.Criterion;

    /// <summary>
    ///     Defines the functionality required by a class that can extend the <see cref="FlowQuery" /> functionality for
    ///     projections and construction of query results.
    /// </summary>
    /// <typeparam name="TExpression">
    ///     The <see cref="Type" /> of the <see cref="System.Linq.Expressions.Expression" /> expression this handler can
    ///     handle.
    /// </typeparam>
    public interface IExpressionHandler<in TExpression>
        where TExpression : System.Linq.Expressions.Expression
    {
        /// <summary>
        ///     Determines whether this <see cref="IExpressionHandler{TExpression}" /> can handle construction of the
        ///     given <see cref="T:TExpression" /> expression.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="T:TExpression" /> expression.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the handler can handle construction of the given <see cref="T:TExpression" /> expression
        ///     otherwise <c>false</c>.
        /// </returns>
        bool CanHandleConstruction(TExpression expression);

        /// <summary>
        ///     Determines whether this <see cref="IExpressionHandler{TExpression}" /> can handle projection of the
        ///     given <see cref="T:TExpression" /> expression.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="T:TExpression" /> expression.
        /// </param>
        /// <param name="context">
        ///     The helper context.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the handler can handle projection of the given <see cref="T:TExpression" /> expression
        ///     otherwise <c>false</c>.
        /// </returns>
        bool CanHandleProjection
            (
            TExpression expression,
            HelperContext context
            );

        /// <summary>
        ///     Handles construction of the given <see cref="T:TExpression" /> expression.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="T:TExpression" />.
        /// </param>
        /// <param name="arguments">
        ///     The values provided by NHibernate (may contain more values than necessary for this handler).
        /// </param>
        /// <param name="value">
        ///     The constructed value.
        /// </param>
        /// <param name="wasHandled">
        ///     Indicates whether the given expression was handled or not.
        /// </param>
        /// <returns>
        ///     The number of arguments in <paramref name="arguments" /> that was used for the construction.
        /// </returns>
        int Construct
            (
            TExpression expression,
            object[] arguments,
            out object value,
            out bool wasHandled);

        /// <summary>
        ///     Handles projection of the given <see cref="T:TExpression" /> expression.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="T:TExpression" /> expression.
        /// </param>
        /// <param name="context">
        ///     The helper context.
        /// </param>
        /// <returns>
        ///     The <see cref="IProjection" /> or null if no <see cref="IProjection" /> could be resolved.
        /// </returns>
        IProjection Project
            (
            TExpression expression,
            HelperContext context);
    }
}