namespace NHibernate.FlowQuery.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.Dialect.Function;
    using NHibernate.FlowQuery.Core.CustomProjections;
    using NHibernate.FlowQuery.Helpers.ExpressionHandlers;
    using NHibernate.Type;

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
            switch (expression.NodeType)
            {
                case ExpressionType.AndAlso:
                case ExpressionType.NotEqual:
                case ExpressionType.OrElse:
                case ExpressionType.Equal:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                    return GetConditionalProjection
                        (
                            Expression.Condition(expression, Expression.Constant(true), Expression.Constant(false)),
                            context
                        );

                case ExpressionType.Add:
                case ExpressionType.Subtract:
                case ExpressionType.Divide:
                case ExpressionType.Multiply:
                    return GetArithmeticProjection((BinaryExpression)expression, context);

                case ExpressionType.Conditional:
                    return GetConditionalProjection((ConditionalExpression)expression, context);

                case ExpressionType.Call:
                    return GetMethodCallProjection((MethodCallExpression)expression, context);

                case ExpressionType.MemberAccess:
                    return GetMemberProjection((MemberExpression)expression, context);

                case ExpressionType.Convert:
                    return GetConvertProjection((UnaryExpression)expression, context);

                case ExpressionType.Coalesce:
                    return GetCoalesceProjection((BinaryExpression)expression, context);

                case ExpressionType.Lambda:
                    return GetProjection(((LambdaExpression)expression).Body, context);

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
        ///     Returns all inner <see cref="Expression" />s for the given <see cref="BinaryExpression" /> in a list.
        /// </summary>
        /// <param name="expression">
        ///     The expression to flatten out.
        /// </param>
        /// <returns>
        ///     The list of <see cref="Expression" />s.
        /// </returns>
        private static IEnumerable<Expression> FlattenBinaryExpression(BinaryExpression expression)
        {
            var expressions = new List<Expression>();

            if (expression.Left is BinaryExpression)
            {
                expressions.AddRange(FlattenBinaryExpression(expression.Left as BinaryExpression));
            }
            else
            {
                expressions.Add(expression.Left);
            }

            if (expression.Right is BinaryExpression)
            {
                expressions.AddRange(FlattenBinaryExpression(expression.Right as BinaryExpression));
            }
            else
            {
                expressions.Add(expression.Right);
            }

            return expressions;
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
        ///     Gets a arithmetic <see cref="string" /> representation of the given <see cref="ExpressionType" /> type.
        /// </summary>
        /// <param name="type">
        ///     The <see cref="ExpressionType" /> type.
        /// </param>
        /// <returns>
        ///     The arithmetic <see cref="string" /> representation.
        /// </returns>
        private static string GetArithmeticOperation(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.Divide:
                    return "/";

                case ExpressionType.Multiply:
                    return "*";

                case ExpressionType.Subtract:
                    return "-";

                default:
                    return "+";
            }
        }

        /// <summary>
        ///     Creates a <see cref="IProjection" /> for the given arithmetic <see cref="BinaryExpression" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="context">
        ///     The context for the projection.
        /// </param>
        /// <returns>
        ///     The created <see cref="IProjection" />.
        /// </returns>
        private static IProjection GetArithmeticProjection
            (
            BinaryExpression expression,
            HelperContext context
            )
        {
            if (expression.NodeType == ExpressionType.Add && expression.Type == typeof(string))
            {
                return GetConcatenationProjection(expression, context);
            }

            string operation = GetArithmeticOperation(expression.NodeType);

            return new SqlFunctionProjection
                (
                new VarArgsSQLFunction("(", operation, ")"),
                NHibernateUtil.GuessType(expression.Left.Type),
                GetProjection(expression.Left, context),
                GetProjection(expression.Right, context)
                );
        }

        /// <summary>
        ///     Creates a <see cref="IProjection" /> instance from the given coalesce <see cref="BinaryExpression" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="context">
        ///     The context for the projection.
        /// </param>
        /// <returns>
        ///     The created <see cref="IProjection" />.
        /// </returns>
        private static IProjection GetCoalesceProjection
            (
            BinaryExpression expression,
            HelperContext context
            )
        {
            IProjection original = GetProjection(expression.Left, context);
            IProjection fallback = GetProjection(expression.Right, context);

            return Projections.Conditional(Restrictions.IsNull(original), fallback, original);
        }

        /// <summary>
        ///     Creates a <see cref="IProjection" /> from the given concatenation <see cref="BinaryExpression" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="context">
        ///     The context for the projection.
        /// </param>
        /// <returns>
        ///     The created <see cref="IProjection" />.
        /// </returns>
        private static IProjection GetConcatenationProjection
            (
            BinaryExpression expression,
            HelperContext context
            )
        {
            var projections = new List<IProjection>();

            foreach (Expression expressionPart in FlattenBinaryExpression(expression))
            {
                IProjection projection = GetProjection(expressionPart, context);

                projections.Add(projection);
            }

            return new SqlFunctionProjection("concat", NHibernateUtil.String, projections.ToArray());
        }

        /// <summary>
        ///     Creates a <see cref="IProjection" /> from the given <see cref="ConditionalExpression" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="context">
        ///     The context for the projection.
        /// </param>
        /// <returns>
        ///     The created <see cref="IProjection" />.
        /// </returns>
        private static IProjection GetConditionalProjection
            (
            ConditionalExpression expression,
            HelperContext context
            )
        {
            return Projections
                .Conditional
                (
                    RestrictionHelper.GetCriterion(expression.Test, context),
                    GetProjection(expression.IfTrue, context),
                    GetProjection(expression.IfFalse, context)
                );
        }

        /// <summary>
        ///     Creates a <see cref="IProjection" /> for the given <see cref="UnaryExpression" />.
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
        private static IProjection GetConvertProjection
            (
            UnaryExpression expression,
            HelperContext context
            )
        {
            IProjection projection = GetProjection(expression.Operand, context);

            if (!expression.IsLiftedToNull)
            {
                IType type = TypeHelper.GuessType(expression.Type, true);

                if (type != null)
                {
                    return new FqCastProjection(type, projection);
                }
            }

            return projection;
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
        ///     Creates a <see cref="IProjection" /> from the given <see cref="MethodCallExpression" />.
        /// </summary>
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
        private static IProjection GetMethodCallProjection
            (
            MethodCallExpression expression,
            HelperContext context
            )
        {
            IEnumerable<IMethodCallExpressionHandler> handlers = FlowQueryHelper
                .GetMethodCallHandlers(expression.Method.Name.ToLower());

            foreach (IMethodCallExpressionHandler handler in handlers)
            {
                if (handler.CanHandleProjection(expression, context))
                {
                    IProjection projection = handler.Project(expression, context);

                    if (projection != null)
                    {
                        return projection;
                    }
                }
            }

            throw new NotSupportedException
                (
                string.Format
                    (
                        "The expression contains unsupported features. Unable to resolve method call: '{0}'.",
                        expression.Method.Name
                    )
                );
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