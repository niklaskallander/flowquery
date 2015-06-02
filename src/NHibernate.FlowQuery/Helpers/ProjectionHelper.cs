namespace NHibernate.FlowQuery.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers.ExpressionHandlers;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     A static utility class providing methods to create <see cref="IProjection" />s from
    ///     <see cref="System.Linq.Expressions.Expression" />s.
    /// </summary>
    public static class ProjectionHelper
    {
        /// <summary>
        ///     The message to use when throwing NotSupportedExceptions.
        /// </summary>
        private const string NotSupportedMessage
                = "The expression contains unsupported features. Unable to resolve expression: '{0}'.";

        /// <summary>
        ///     Creates a <see cref="IProjection" /> for the given <see cref="Expression" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="context">
        ///     The context for the projection.
        /// </param>
        /// <returns>
        ///     The resolved <see cref="IProjection" /> instance.
        /// </returns>
        /// <exception cref="NotSupportedException">
        ///     The <see cref="Expression" /> could not be resolved as it may contain unsupported features or similar.
        /// </exception>
        public static IProjection GetProjection
            (
            Expression expression,
            HelperContext context
            )
        {
            IEnumerable<IExpressionHandler> handlers = FlowQueryHelper
                .GetExpressionHandlers(expression.NodeType)
                .Where(x => x.CanHandleProjectionOf(expression, context))
                .ToArray();

            if (handlers.Any())
            {
                return GetProjectionUsing(handlers, expression, context);
            }

            if (expression.NodeType == ExpressionType.Parameter && expression.ToString() == context.RootAlias)
            {
                throw new NotSupportedException
                    (
                    "Unable to select the root entity like 'x => x', select without an expression instead"
                    );
            }

            return GetValueProjection(expression);
        }

        /// <summary>
        ///     Creates a <see cref="IProjection" /> for the given <see cref="Expression" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="data">
        ///     The <see cref="QueryHelperData" /> data.
        /// </param>
        /// <param name="type">
        ///     The <see cref="HelperType" /> type.
        /// </param>
        /// <returns>
        ///     The resolved <see cref="IProjection" /> instance.
        /// </returns>
        /// <exception cref="NotSupportedException">
        ///     The <see cref="Expression" /> could not be resolved as it may contain unsupported features or similar.
        /// </exception>
        public static IProjection GetProjection
            (
            LambdaExpression expression,
            QueryHelperData data,
            HelperType type = HelperType.Select
            )
        {
            return GetProjection(expression.Body, new HelperContext(data, expression.Parameters[0].Name, type));
        }

        /// <summary>
        ///     Creates a <see cref="IProjection" /> from the given <see cref="Expression" />.
        /// </summary>
        /// <param name="handlers">
        ///     The set of <see cref="IExpressionHandler" /> instances to use when resolving the
        ///     <see cref="IProjection" /> of the given <see cref="Expression" />.
        /// </param>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="context">
        ///     The context for the projection.
        /// </param>
        /// <returns>
        ///     The created <see cref="IProjection" />.
        /// </returns>
        /// <exception cref="NotSupportedException">
        ///     The <see cref="Expression" /> could not be resolved as it may contain unsupported features or similar.
        /// </exception>
        private static IProjection GetProjectionUsing
            (
            IEnumerable<IExpressionHandler> handlers,
            Expression expression,
            HelperContext context
            )
        {
            foreach (IExpressionHandler handler in handlers)
            {
                IProjection projection = handler.Project(expression, context);

                if (projection != null)
                {
                    return projection;
                }
            }

            throw new NotSupportedException(string.Format(NotSupportedMessage, expression));
        }

        /// <summary>
        ///     Creates a <see cref="IProjection" /> for the evaluated value of the given <see cref="Expression" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <returns>
        ///     The resolved <see cref="IProjection" /> instance.
        /// </returns>
        private static IProjection GetValueProjection(Expression expression)
        {
            try
            {
                object value = ExpressionHelper.GetValue(expression);

                return Projections.Constant(value, TypeHelper.GuessType(expression.Type));
            }
            catch (Exception exception)
            {
                throw new NotSupportedException(string.Format(NotSupportedMessage, expression), exception);
            }
        }
    }
}