namespace NHibernate.FlowQuery.Helpers.ConstructionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    /// <summary>
    ///     Defines the functionality required of a class used to resolve values from 
    ///     <see cref="System.Linq.Expressions.MethodCallExpression" />s.
    /// </summary>
    public interface IMethodCallConstructionHandler
    {
        /// <summary>
        ///     Handles the given <see cref="MethodCallExpression" />.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="MethodCallExpression" />.
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
        int Handle(MethodCallExpression expression, object[] arguments, out object value, out bool wasHandled);
    }
}