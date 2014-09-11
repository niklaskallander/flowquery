namespace NHibernate.FlowQuery.Helpers.ProjectionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    /// <summary>
    ///     Defines the functionality required of a class used to resolve <see cref="IProjection" /> instances from 
    ///     <see cref="MethodCallExpression" />.
    /// </summary>
    public interface IMethodCallProjectionHandler
    {
        /// <summary>
        ///     Handles the given <see cref="MethodCallExpression" />.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="MethodCallExpression" />.
        /// </param>
        /// <param name="root">
        ///     The entity root name.
        /// </param>
        /// <param name="data">
        ///     The <see cref="QueryHelperData" />.
        /// </param>
        /// <returns>
        ///     The <see cref="IProjection"/> or null if no <see cref="IProjection" /> could be resolved.
        /// </returns>
        IProjection Handle(MethodCallExpression expression, string root, QueryHelperData data);
    }
}