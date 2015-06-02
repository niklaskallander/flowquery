namespace NHibernate.FlowQuery.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core.CustomProjections;
    using NHibernate.FlowQuery.Helpers.ExpressionHandlers;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     A static utility class providing methods to create <see cref="IProjection" />s from
    ///     <see cref="System.Linq.Expressions.Expression" />s.
    /// </summary>
    public static class ProjectionHelper
    {
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

            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return GetMemberProjection((MemberExpression)expression, context);

                case ExpressionType.New:
                    return ForNewExpression((NewExpression)expression, context);

                case ExpressionType.MemberInit:
                    return ForMemberInitExpression((MemberInitExpression)expression, context);

                default:

                    if (expression.NodeType == ExpressionType.Parameter && expression.ToString() == context.RootAlias)
                    {
                        throw new NotSupportedException
                            (
                            "Unable to select the root entity like 'x => x', select without an expression instead"
                            );
                    }

                    return GetValueProjection(expression);
            }
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
        ///     Creates <see cref="IProjection" />s for all <see cref="System.Linq.Expressions.Expression" />s within
        ///     the given <see cref="MemberInitExpression" /> and returns them in a new <see cref="ProjectionList" />
        ///     instance.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="context">
        ///     The context for the projection.
        /// </param>
        /// <returns>
        ///     The resolved <see cref="ProjectionList" /> for the given <see cref="MemberInitExpression" /> expression.
        /// </returns>
        private static ProjectionList ForMemberInitExpression
            (
            MemberInitExpression expression,
            HelperContext context
            )
        {
            ProjectionList list = Projections.ProjectionList();

            ForMemberInitExpression(expression, context, ref list);

            return list;
        }

        /// <summary>
        ///     Creates <see cref="IProjection" />s for all <see cref="System.Linq.Expressions.Expression" />s within
        ///     the given <see cref="MemberInitExpression" /> and adds them to the given <see cref="ProjectionList" />
        ///     <paramref name="list" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="context">
        ///     The context for the projection.
        /// </param>
        /// <param name="list">
        ///     The <see cref="ProjectionList" /> instance.
        /// </param>
        private static void ForMemberInitExpression
            (
            MemberInitExpression expression,
            HelperContext context,
            ref ProjectionList list
            )
        {
            ForNewExpression(expression.NewExpression, context, ref list);

            foreach (MemberBinding memberBinding in expression.Bindings)
            {
                var memberAssigment = memberBinding as MemberAssignment;

                if (memberAssigment != null)
                {
                    switch (memberAssigment.Expression.NodeType)
                    {
                        case ExpressionType.MemberInit:

                            ForMemberInitExpression
                                (
                                    memberAssigment.Expression as MemberInitExpression,
                                    context,
                                    ref list
                                );

                            break;

                        case ExpressionType.New:

                            ForNewExpression
                                (
                                    memberAssigment.Expression as NewExpression,
                                    context,
                                    ref list
                                );

                            break;

                        default:

                            IProjection projection = GetProjection(memberAssigment.Expression, context);

                            string member = memberAssigment.Member.Name;

                            list.Add(new FqAliasProjection(projection, member));

                            if (!context.Data.Mappings.ContainsKey(member))
                            {
                                context.Data.Mappings.Add(member, projection);
                            }

                            break;
                    }
                }
            }
        }

        /// <summary>
        ///     Creates <see cref="IProjection" />s for all <see cref="System.Linq.Expressions.Expression" />s within
        ///     the given <see cref="NewExpression" /> and returns them in a new <see cref="ProjectionList" /> instance.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="context">
        ///     The context for the projection.
        /// </param>
        /// <returns>
        ///     The resolved <see cref="ProjectionList" /> for the given <see cref="NewExpression" /> expression.
        /// </returns>
        private static ProjectionList ForNewExpression
            (
            NewExpression expression,
            HelperContext context
            )
        {
            ProjectionList list = Projections.ProjectionList();

            ForNewExpression(expression, context, ref list);

            return list;
        }

        /// <summary>
        ///     Creates <see cref="IProjection" />s for all <see cref="System.Linq.Expressions.Expression" />s within
        ///     the given <see cref="NewExpression" /> and adds them to the given <see cref="ProjectionList" />
        ///     <paramref name="list" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="context">
        ///     The context for the projection.
        /// </param>
        /// <param name="list">
        ///     The <see cref="ProjectionList" /> instance.
        /// </param>
        private static void ForNewExpression
            (
            NewExpression expression,
            HelperContext context,
            ref ProjectionList list
            )
        {
            foreach (Expression argument in expression.Arguments)
            {
                switch (argument.NodeType)
                {
                    case ExpressionType.MemberInit:

                        ForMemberInitExpression(argument as MemberInitExpression, context, ref list);

                        break;

                    case ExpressionType.New:

                        ForNewExpression(argument as NewExpression, context, ref list);

                        break;

                    default:

                        IProjection projection = GetProjection(argument, context);

                        list.Add(projection);

                        break;
                }
            }
        }

        /// <summary>
        ///     Creates a <see cref="IProjection" /> for the given <see cref="MemberExpression" />.
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
        private static IProjection GetMemberProjection
            (
            MemberExpression expression,
            HelperContext context
            )
        {
            if (ExpressionHelper.IsRooted(expression, context.RootAlias, context.Data))
            {
                string property = ExpressionHelper.GetPropertyName(expression, context.RootAlias, true, context);

                return Projections.Property(property);
            }

            object value = ExpressionHelper.GetValue(expression);

            return Projections.Constant(value, TypeHelper.GuessType(expression.Type));
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
                if (handler.CanHandleProjectionOf(expression, context))
                {
                    IProjection projection = handler.Project(expression, context);

                    if (projection != null)
                    {
                        return projection;
                    }
                }
            }

            const string ErrorMessageFormat
                = "The expression contains unsupported features. Unable to resolve expression: '{0}'.";

            throw new NotSupportedException(string.Format(ErrorMessageFormat, expression));
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
            object value = ExpressionHelper.GetValue(expression);

            return Projections.Constant(value, TypeHelper.GuessType(expression.Type));
        }
    }
}