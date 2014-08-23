namespace NHibernate.FlowQuery.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.Dialect.Function;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.CustomProjections;
    using NHibernate.Type;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     A static utility class providing methods to create <see cref="IProjection" />s from
    ///     <see cref="Expression" />s.
    /// </summary>
    public static class ProjectionHelper
    {
        /// <summary>
        ///     Returns all inner <see cref="Expression" />s for the given <see cref="BinaryExpression" /> in a list.
        /// </summary>
        /// <param name="expression">
        ///     The expression to flatten out.
        /// </param>
        /// <returns>
        ///     The list of <see cref="Expression" />s.
        /// </returns>
        public static IEnumerable<Expression> FlattenBinaryExpression(BinaryExpression expression)
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
        ///     Creates <see cref="IProjection" />s for all <see cref="Expression" />s within the given
        ///     <see cref="MemberInitExpression" /> and adds them to the given <see cref="ProjectionList" />
        ///     <paramref name="list" /> and also to <paramref name="mappings" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="root">
        ///     The root name.
        /// </param>
        /// <param name="data">
        ///     The <see cref="QueryHelperData" /> info.
        /// </param>
        /// <param name="list">
        ///     The <see cref="ProjectionList" /> instance.
        /// </param>
        /// <param name="mappings">
        ///     The mappings.
        /// </param>
        public static void ForMemberInitExpression
            (
            MemberInitExpression expression,
            string root,
            QueryHelperData data,
            ref ProjectionList list,
            ref Dictionary<string, IProjection> mappings
            )
        {
            ForNewExpression(expression.NewExpression, root, data, ref list, ref mappings);

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
                                root,
                                data,
                                ref list,
                                ref mappings
                            );

                            break;

                        case ExpressionType.New:

                            ForNewExpression
                            (
                                memberAssigment.Expression as NewExpression,
                                root,
                                data,
                                ref list,
                                ref mappings
                            );

                            break;

                        default:

                            IProjection projection = GetProjection(memberAssigment.Expression, root, data);

                            string member = memberAssigment.Member.Name;

                            list.Add(new FqAliasProjection(projection, member));

                            if (!mappings.ContainsKey(member))
                            {
                                mappings.Add(member, projection);
                            }

                            break;
                    }
                }
            }
        }

        /// <summary>
        ///     Creates <see cref="IProjection" />s for all <see cref="Expression" />s within the given
        ///     <see cref="NewExpression" /> and adds them to the given <see cref="ProjectionList" />
        ///     <paramref name="list" /> and also to <paramref name="mappings" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="root">
        ///     The root name.
        /// </param>
        /// <param name="data">
        ///     The <see cref="QueryHelperData" /> info.
        /// </param>
        /// <param name="list">
        ///     The <see cref="ProjectionList" /> instance.
        /// </param>
        /// <param name="mappings">
        ///     The mappings.
        /// </param>
        public static void ForNewExpression
            (
            NewExpression expression,
            string root,
            QueryHelperData data,
            ref ProjectionList list,
            ref Dictionary<string, IProjection> mappings
            )
        {
            foreach (Expression argument in expression.Arguments)
            {
                switch (argument.NodeType)
                {
                    case ExpressionType.MemberInit:

                        ForMemberInitExpression(argument as MemberInitExpression, root, data, ref list, ref mappings);

                        break;

                    case ExpressionType.New:

                        ForNewExpression(argument as NewExpression, root, data, ref list, ref mappings);

                        break;

                    default:

                        IProjection projection = GetProjection(argument, root, data);

                        list.Add(projection);

                        break;
                }
            }
        }

        /// <summary>
        ///     Creates a <see cref="IProjection" /> for the given <see cref="Expression" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="root">
        ///     The root name.
        /// </param>
        /// <param name="data">
        ///     The <see cref="QueryHelperData" /> info.
        /// </param>
        /// <returns>
        ///     The resolved <see cref="IProjection" /> instance.
        /// </returns>
        /// <exception cref="NotSupportedException">
        ///     The <see cref="Expression" /> could not be resolved as it may contain unsupported features or similar.
        /// </exception>
        public static IProjection GetProjection(Expression expression, string root, QueryHelperData data)
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
                        root,
                        data
                    );

                case ExpressionType.Add:
                case ExpressionType.Subtract:
                case ExpressionType.Divide:
                case ExpressionType.Multiply:
                    return GetArithmeticProjection(expression as BinaryExpression, root, data);

                case ExpressionType.Conditional:
                    return GetConditionalProjection(expression as ConditionalExpression, root, data);

                case ExpressionType.Call:
                    return GetMethodCallProjection(expression as MethodCallExpression, root, data);

                case ExpressionType.MemberAccess:

                    if (ExpressionHelper.IsRooted(expression, root, data))
                    {
                        string property = ExpressionHelper.GetPropertyName(expression, root);

                        return Projections.Property(property);
                    }

                    object value = ExpressionHelper.GetValue(expression);

                    return Projections.Constant(value, TypeHelper.GuessType(expression.Type));

                case ExpressionType.Convert:

                    var unaryExpression = (UnaryExpression)expression;

                    IProjection projection = GetProjection(unaryExpression.Operand, root, data);

                    if (!unaryExpression.IsLiftedToNull)
                    {
                        IType type = TypeHelper.GuessType(unaryExpression.Type, true);

                        if (type != null)
                        {
                            return new FqCastProjection(type, projection);
                        }
                    }

                    return projection;

                case ExpressionType.Coalesce:
                    return GetCoalesceProjection(expression as BinaryExpression, root, data);

                default:

                    if (expression.NodeType == ExpressionType.Parameter && expression.ToString() == root)
                    {
                        throw new NotSupportedException
                        (
                            "Unable to select the root entity like 'x => x', select without an expression instead"
                        );
                    }

                    value = ExpressionHelper.GetValue(expression);

                    return Projections.Constant(value, TypeHelper.GuessType(expression.Type));
            }
        }

        /// <summary>
        ///     Creates a <see cref="ProjectionList" /> for the given <see cref="Expression" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="root">
        ///     The root name.
        /// </param>
        /// <param name="data">
        ///     The <see cref="QueryHelperData" /> info.
        /// </param>
        /// <param name="mappings">
        ///     The mappings.
        /// </param>
        /// <returns>
        ///     The resolved <see cref="IProjection" /> instance.
        /// </returns>
        /// <exception cref="NotSupportedException">
        ///     The <see cref="Expression" /> could not be resolved as it may contain unsupported features or similar.
        /// </exception>
        public static ProjectionList GetProjectionListForExpression
            (
            Expression expression,
            string root,
            QueryHelperData data,
            ref Dictionary<string, IProjection> mappings
            )
        {
            ProjectionList list = Projections.ProjectionList();

            switch (expression.NodeType)
            {
                case ExpressionType.New:
                    ForNewExpression(expression as NewExpression, root, data, ref list, ref mappings);
                    break;

                case ExpressionType.MemberInit:
                    ForMemberInitExpression(expression as MemberInitExpression, root, data, ref list, ref mappings);
                    break;

                default:

                    IProjection projection = GetProjection(expression, root, data);

                    list.Add(projection);

                    break;
            }

            return list;
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
        /// <param name="root">
        ///     The root name.
        /// </param>
        /// <param name="data">
        ///     The <see cref="QueryHelperData" /> info.
        /// </param>
        /// <returns>
        ///     The created <see cref="IProjection" />.
        /// </returns>
        private static IProjection GetArithmeticProjection
            (
            BinaryExpression expression,
            string root,
            QueryHelperData data
            )
        {
            if (expression.NodeType == ExpressionType.Add && expression.Type == typeof(string))
            {
                return GetConcatenationProjection(expression, root, data);
            }

            string operation = GetArithmeticOperation(expression.NodeType);

            return new SqlFunctionProjection
            (
                new VarArgsSQLFunction("(", operation, ")"),
                NHibernateUtil.GuessType(expression.Left.Type),
                GetProjection(expression.Left, root, data),
                GetProjection(expression.Right, root, data)
            );
        }

        /// <summary>
        ///     Creates a <see cref="IProjection" /> instance from the given coalesce <see cref="BinaryExpression" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="root">
        ///     The root name.
        /// </param>
        /// <param name="data">
        ///     The <see cref="QueryHelperData" /> info.
        /// </param>
        /// <returns>
        ///     The created <see cref="IProjection" />.
        /// </returns>
        private static IProjection GetCoalesceProjection
            (
            BinaryExpression expression,
            string root,
            QueryHelperData data
            )
        {
            IProjection original = GetProjection(expression.Left, root, data);
            IProjection fallback = GetProjection(expression.Right, root, data);

            return Projections.Conditional(Restrictions.IsNull(original), fallback, original);
        }

        /// <summary>
        ///     Creates a <see cref="IProjection" /> from the given concatenation <see cref="BinaryExpression" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="root">
        ///     The root name.
        /// </param>
        /// <param name="data">
        ///     The <see cref="QueryHelperData" /> info.
        /// </param>
        /// <returns>
        ///     The created <see cref="IProjection" />.
        /// </returns>
        private static IProjection GetConcatenationProjection
            (
            BinaryExpression expression,
            string root,
            QueryHelperData data
            )
        {
            var projections = new List<IProjection>();

            foreach (Expression expressionPart in FlattenBinaryExpression(expression))
            {
                IProjection projection = GetProjection(expressionPart, root, data);

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
        /// <param name="root">
        ///     The root name.
        /// </param>
        /// <param name="data">
        ///     The <see cref="QueryHelperData" /> info.
        /// </param>
        /// <returns>
        ///     The created <see cref="IProjection" />.
        /// </returns>
        private static IProjection GetConditionalProjection
            (
            ConditionalExpression expression,
            string root,
            QueryHelperData data
            )
        {
            return Projections
                .Conditional
                (
                    RestrictionHelper.GetCriterion(expression.Test, root, data),
                    GetProjection(expression.IfTrue, root, data),
                    GetProjection(expression.IfFalse, root, data)
                );
        }

        /// <summary>
        ///     Creates a <see cref="IProjection" /> from the given <see cref="MethodCallExpression" />.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="root">
        ///     The root name.
        /// </param>
        /// <param name="data">
        ///     The <see cref="QueryHelperData" /> info.
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
            string root,
            QueryHelperData data
            )
        {
            Expression subExpression = expression.Object ?? expression.Arguments[0];

            IProjection projection = GetProjection(subExpression, root, data);

            switch (expression.Method.Name)
            {
                case "Sum":
                    return Projections.Sum(projection);

                case "Min":
                    return Projections.Min(projection);

                case "Max":
                    return Projections.Max(projection);

                case "GroupBy":
                    return Projections.GroupProperty(projection);

                case "Average":
                    return Projections.Avg(projection);

                case "Count":
                    return Projections.Count(projection);

                case "CountDistinct":
                    return Projections.CountDistinct(ExpressionHelper.GetPropertyName(subExpression, root));

                case "As":
                    return Projections.Property(ExpressionHelper.GetPropertyName(subExpression, root));

                case "Substring":

                    int start = ExpressionHelper.GetValue<int>(expression.Arguments[0]) + 1;

                    int length = expression.Arguments.Count > 1
                        ? ExpressionHelper.GetValue<int>(expression.Arguments[1])
                        : int.MaxValue;

                    return new SqlFunctionProjection
                    (
                        "substring",
                        NHibernateUtil.String,
                        projection,
                        Projections.Constant(start),
                        Projections.Constant(length)
                    );

                case "StartsWith":
                case "EndsWith":
                case "Contains":

                    ICriterion criterion = RestrictionHelper.GetCriterionForMethodCall(expression, root, data);

                    return Projections
                        .Conditional
                        (
                            criterion,
                            Projections.Constant(true, NHibernateUtil.Boolean),
                            Projections.Constant(false, NHibernateUtil.Boolean)
                        );

                case "Subquery":

                    object value = ExpressionHelper.GetValue(expression.Arguments[0]);

                    var criteria = value as DetachedCriteria;

                    if (criteria == null)
                    {
                        var query = value as IDetachedImmutableFlowQuery;

                        if (query != null)
                        {
                            criteria = query.Criteria;
                        }
                    }

                    if (criteria != null)
                    {
                        return Projections.SubQuery(criteria);
                    }

                    break;
            }

            throw new NotSupportedException("the expression contains unsupported features, please revise your code");
        }
    }
}