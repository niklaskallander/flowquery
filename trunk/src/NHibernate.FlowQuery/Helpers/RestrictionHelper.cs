namespace NHibernate.FlowQuery.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Expressions;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     A static utility class providing methods to create <see cref="ICriterion" />s from
    ///     <see cref="System.Linq.Expressions.Expression" />s.
    /// </summary>
    public static class RestrictionHelper
    {
        /// <summary>
        ///     Creates a <see cref="ICriterion" /> for the given <see cref="Expression" />.
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
        ///     The created <see cref="ICriterion" />.
        /// </returns>
        public static ICriterion GetCriterion(Expression expression, string root, QueryHelperData data)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.AndAlso:
                case ExpressionType.OrElse:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                    return GetCriterionForBinary(expression as BinaryExpression, root, data);

                case ExpressionType.Call:
                    return GetCriterionForMethodCall(expression as MethodCallExpression, root, data);

                case ExpressionType.Lambda:
                    return GetCriterion(((LambdaExpression)expression).Body, root, data);

                case ExpressionType.Not:
                    return Restrictions.Not(GetCriterion(((UnaryExpression)expression).Operand, root, data));

                case ExpressionType.MemberAccess:
                    return Restrictions.Eq(ExpressionHelper.GetPropertyName(expression, root), true);

                case ExpressionType.Invoke:
                    return GetCriterionForInvoke(expression as InvocationExpression, root, data);

                default:
                    return Restrictions.Eq(Projections.Constant(ExpressionHelper.GetValue<bool>(expression)), true);
            }
        }

        /// <summary>
        ///     Creates a <see cref="ICriterion" /> for the given <see cref="BinaryExpression" />.
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
        ///     The created <see cref="ICriterion" />.
        /// </returns>
        public static ICriterion GetCriterionForBinary(BinaryExpression expression, string root, QueryHelperData data)
        {
            return GetCriterionForBinary(expression.Left, expression.Right, expression.NodeType, root, data);
        }

        /// <summary>
        ///     Creates a <see cref="ICriterion" /> for a binary operation.
        /// </summary>
        /// <param name="a">
        ///     Expression A.
        /// </param>
        /// <param name="b">
        ///     Expression B.
        /// </param>
        /// <param name="type">
        ///     The operation type.
        /// </param>
        /// <param name="root">
        ///     The root name.
        /// </param>
        /// <param name="data">
        ///     The <see cref="QueryHelperData" /> info.
        /// </param>
        /// <returns>
        ///     The created <see cref="ICriterion" />.
        /// </returns>
        public static ICriterion GetCriterionForBinary
            (
            Expression a,
            Expression b,
            ExpressionType type,
            string root,
            QueryHelperData data
            )
        {
            if (a.NodeType == ExpressionType.Convert)
            {
                return GetCriterionForBinary(((UnaryExpression)a).Operand, b, type, root, data);
            }

            if (b.NodeType == ExpressionType.Convert)
            {
                return GetCriterionForBinary(a, ((UnaryExpression)b).Operand, type, root, data);
            }

            switch (type)
            {
                case ExpressionType.AndAlso:
                    return Restrictions.And(GetCriterion(a, root, data), GetCriterion(b, root, data));

                case ExpressionType.OrElse:
                    return Restrictions.Or(GetCriterion(a, root, data), GetCriterion(b, root, data));

                case ExpressionType.ExclusiveOr:

                    ICriterion criterionA = GetCriterion(a, root, data);
                    ICriterion criterionB = GetCriterion(b, root, data);

                    return Restrictions
                        .Or
                        (
                            Restrictions.And(criterionA, Restrictions.Not(criterionB)),
                            Restrictions.And(criterionB, Restrictions.Not(criterionA))
                        );
            }

            bool isProjectedA = IsProjected(a, root, data);
            bool isProjectedB = IsProjected(b, root, data);

            // Projection Projection
            if (isProjectedA && isProjectedB)
            {
                return GetProjectionProjectionCriterion
                (
                    ProjectionHelper.GetProjection(a, root, data),
                    ProjectionHelper.GetProjection(b, root, data),
                    type
                );
            }

            if (isProjectedA || !isProjectedB)
            {
                return GetProjectionValueCriterion(a, ExpressionHelper.GetValue(b), type, root, data, false);
            }

            return GetProjectionValueCriterion(b, ExpressionHelper.GetValue(a), type, root, data, true);
        }

        /// <summary>
        ///     Creates a <see cref="ICriterion" /> for the given <see cref="InvocationExpression" />.
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
        ///     The created <see cref="ICriterion" />.
        /// </returns>
        public static ICriterion GetCriterionForInvoke
            (
            InvocationExpression expression,
            string root,
            QueryHelperData data
            )
        {
            if (expression.Expression.Type == typeof(WhereDelegate))
            {
                string property = ExpressionHelper.GetPropertyName(expression.Arguments[0], root);

                var isExpression = ExpressionHelper.GetValue<IsExpression>(expression.Arguments[1]);

                ICriterion criterion = isExpression.Compile(property);

                return criterion;
            }

            return Restrictions.Eq(Projections.Constant(ExpressionHelper.GetValue<bool>(expression)), true);
        }

        /// <summary>
        ///     Creates a <see cref="ICriterion" /> for the given <see cref="MethodCallExpression" />.
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
        ///     The created <see cref="ICriterion" />.
        /// </returns>
        public static ICriterion GetCriterionForMethodCall
            (
            MethodCallExpression expression,
            string root,
            QueryHelperData data
            )
        {
            if (IsProjected(expression, root, data))
            {
                // Not a value expression
                IProjection projection = ProjectionHelper
                    .GetProjection
                    (
                        expression.Object ?? expression.Arguments[0],
                        root,
                        data
                    );

                int i = expression.Object == null ? 1 : 0;

                switch (expression.Method.Name)
                {
                    case "In":
                    case "IsIn":

                        object value = ExpressionHelper.GetValue(expression.Arguments[i]);

                        if (!(value is ICollection))
                        {
                            if (value is IEnumerable)
                            {
                                var objs = new List<object>();

                                foreach (object obj in value as IEnumerable)
                                {
                                    objs.Add(obj);
                                }

                                return Restrictions
                                    .In
                                    (
                                        projection,
                                        objs.ToArray()
                                    );
                            }

                            if (value is IDetachedImmutableFlowQuery)
                            {
                                return Subqueries
                                    .PropertyIn
                                    (
                                        ExpressionHelper.GetPropertyName(expression, root),
                                        (value as IDetachedImmutableFlowQuery).Criteria
                                    );
                            }
                        }

                        return Restrictions
                            .In
                            (
                                projection,
                                value as ICollection
                            );

                    case "Between":
                    case "IsBetween":
                        return Restrictions
                            .Between
                            (
                                projection,
                                ExpressionHelper.GetValue(expression.Arguments[i]),
                                ExpressionHelper.GetValue(expression.Arguments[i + 1])
                            );

                    case "Like":
                    case "IsLike":
                        return GetLikeCriterion(projection, expression.Arguments[i], MatchMode.Exact);

                    case "StartsWith":
                        return GetLikeCriterion(projection, expression.Arguments[i], MatchMode.Start);

                    case "EndsWith":
                        return GetLikeCriterion(projection, expression.Arguments[i], MatchMode.End);

                    case "Contains":
                        return GetLikeCriterion(projection, expression.Arguments[i], MatchMode.Anywhere);

                    case "IsLessThan":
                        return Restrictions.Lt(projection, ExpressionHelper.GetValue(expression.Arguments[i]));

                    case "IsLessThanOrEqualTo":
                        return Restrictions.Le(projection, ExpressionHelper.GetValue(expression.Arguments[i]));

                    case "IsGreaterThan":
                        return Restrictions.Gt(projection, ExpressionHelper.GetValue(expression.Arguments[i]));

                    case "IsGreaterThanOrEqualTo":
                        return Restrictions.Ge(projection, ExpressionHelper.GetValue(expression.Arguments[i]));

                    case "IsEqualTo":
                        return Restrictions.Eq(projection, ExpressionHelper.GetValue(expression.Arguments[i]));

                    case "IsNull":
                        return Restrictions.IsNull(projection);

                    case "IsNotNull":
                        return Restrictions.IsNotNull(projection);
                }

                throw new NotSupportedException
                (
                    "The expression contains unsupported features, please revise your code"
                );
            }

            return Restrictions.Eq(Projections.Constant(ExpressionHelper.GetValue<bool>(expression)), true);
        }

        /// <summary>
        ///     Creates a <see cref="ICriterion" /> for a "like" filter.
        /// </summary>
        /// <param name="projection">
        ///     The projection.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="matchMode">
        ///     The match mode.
        /// </param>
        /// <returns>
        ///     The created <see cref="ICriterion" />.
        /// </returns>
        public static ICriterion GetLikeCriterion(IProjection projection, Expression value, MatchMode matchMode)
        {
            return Restrictions.Like(projection, ExpressionHelper.GetValue<string>(value), matchMode);
        }

        /// <summary>
        ///     Creates <see cref="ICriterion" /> for a comparison between a projection and a projection.
        /// </summary>
        /// <param name="projectionA">
        ///     Projection a.
        /// </param>
        /// <param name="projectionB">
        ///     Projection b.
        /// </param>
        /// <param name="type">
        ///     The comparison type.
        /// </param>
        /// <returns>
        ///     The created <see cref="ICriterion" />.
        /// </returns>
        /// <exception cref="NotSupportedException">
        ///     The operation could not be resolved as it may contain unsupported features or similar.
        /// </exception>
        public static ICriterion GetProjectionProjectionCriterion
            (
            IProjection projectionA,
            IProjection projectionB,
            ExpressionType type
            )
        {
            Func<string, string, ICriterion> case1;
            Func<string, IProjection, ICriterion> case2;
            Func<IProjection, string, ICriterion> case3;
            Func<IProjection, IProjection, ICriterion> case4;

            switch (type)
            {
                case ExpressionType.Equal:
                    case1 = Restrictions.EqProperty;
                    case2 = Restrictions.EqProperty;
                    case3 = Restrictions.EqProperty;
                    case4 = Restrictions.EqProperty;
                    break;

                case ExpressionType.GreaterThan:
                    case1 = Restrictions.GtProperty;
                    case2 = Restrictions.GtProperty;
                    case3 = Restrictions.GtProperty;
                    case4 = Restrictions.GtProperty;
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    case1 = Restrictions.GeProperty;
                    case2 = Restrictions.GeProperty;
                    case3 = Restrictions.GeProperty;
                    case4 = Restrictions.GeProperty;
                    break;

                case ExpressionType.LessThan:
                    case1 = Restrictions.LtProperty;
                    case2 = Restrictions.LtProperty;
                    case3 = Restrictions.LtProperty;
                    case4 = Restrictions.LtProperty;
                    break;

                case ExpressionType.LessThanOrEqual:
                    case1 = Restrictions.LeProperty;
                    case2 = Restrictions.LeProperty;
                    case3 = Restrictions.LeProperty;
                    case4 = Restrictions.LeProperty;
                    break;

                case ExpressionType.NotEqual:
                    case1 = Restrictions.NotEqProperty;
                    case2 = Restrictions.NotEqProperty;
                    case3 = Restrictions.NotEqProperty;
                    case4 = Restrictions.NotEqProperty;
                    break;

                default:
                    throw new NotSupportedException
                    (
                        "The expression contains unsupported features, please revise your code"
                    );
            }

            var a = projectionA as PropertyProjection;
            var b = projectionB as PropertyProjection;

            if (a != null && b != null)
            {
                return case1(a.PropertyName, b.PropertyName);
            }

            if (a != null)
            {
                return case2(a.PropertyName, projectionB);
            }

            if (b != null)
            {
                return case3(projectionA, b.PropertyName);
            }

            return case4(projectionA, projectionB);
        }

        /// <summary>
        ///     Creates <see cref="ICriterion" /> for a comparison between a value and a projection.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="type">
        ///     The comparison type.
        /// </param>
        /// <param name="root">
        ///     The root name.
        /// </param>
        /// <param name="data">
        ///     The <see cref="QueryHelperData" /> info.
        /// </param>
        /// <param name="overTurned">
        ///     Indicates whether the comparison has been reversed to simplify other code.
        /// </param>
        /// <returns>
        ///     The created <see cref="ICriterion" />.
        /// </returns>
        /// <exception cref="NotSupportedException">
        ///     The <see cref="Expression" /> could not be resolved as it may contain unsupported features or similar.
        /// </exception>
        public static ICriterion GetProjectionValueCriterion
            (
            Expression expression,
            object value,
            ExpressionType type,
            string root,
            QueryHelperData data,
            bool overTurned
            )
        {
            if (overTurned)
            {
                switch (type)
                {
                    case ExpressionType.GreaterThan:
                        type = ExpressionType.LessThan;
                        break;

                    case ExpressionType.GreaterThanOrEqual:
                        type = ExpressionType.LessThanOrEqual;
                        break;

                    case ExpressionType.LessThan:
                        type = ExpressionType.GreaterThan;
                        break;

                    case ExpressionType.LessThanOrEqual:
                        type = ExpressionType.GreaterThanOrEqual;
                        break;
                }
            }

            IProjection projection = ProjectionHelper.GetProjection(expression, root, data);

            switch (type)
            {
                case ExpressionType.Equal:

                    if (value == null)
                    {
                        return Restrictions.IsNull(projection);
                    }

                    if (value is bool)
                    {
                        return GetCriterion((bool)value ? expression : Expression.Not(expression), root, data);
                    }

                    return Restrictions.Eq(projection, value);

                case ExpressionType.NotEqual:

                    if (value == null)
                    {
                        return Restrictions.IsNotNull(projection);
                    }

                    if (value is bool)
                    {
                        return GetCriterion(!(bool)value ? expression : Expression.Not(expression), root, data);
                    }

                    return Restrictions.Not(Restrictions.Eq(projection, value));

                case ExpressionType.GreaterThan:
                    return Restrictions.Gt(projection, value);

                case ExpressionType.GreaterThanOrEqual:
                    return Restrictions.Ge(projection, value);

                case ExpressionType.LessThan:
                    return Restrictions.Lt(projection, value);

                case ExpressionType.LessThanOrEqual:
                    return Restrictions.Le(projection, value);

                default:
                    throw new NotSupportedException
                    (
                        "the expression contains unsupported features, please revise your code"
                    );
            }
        }

        /// <summary>
        ///     Determines whether the given <see cref="Expression" /> is projected.
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
        ///     True if the <see cref="Expression" /> is considered to be projected; false otherwise.
        /// </returns>
        public static bool IsProjected(Expression expression, string root, QueryHelperData data)
        {
            string expressionRoot = ExpressionHelper.GetRoot(expression);

            return expression is BinaryExpression
                || (expressionRoot != null && (expressionRoot == root || data.Aliases.ContainsValue(expressionRoot)));
        }
    }
}