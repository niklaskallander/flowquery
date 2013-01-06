using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Expressions;
using Expression = System.Linq.Expressions.Expression;

namespace NHibernate.FlowQuery.Helpers
{
    public static class RestrictionHelper
    {
        #region Methods (8)

        public static ICriterion GetCriterionForInvoke(InvocationExpression expression, string root, Dictionary<string, string> aliases)
        {
            if (expression.Expression.Type == typeof(WhereDelegate))
            {
                string property = ExpressionHelper.GetPropertyName(expression.Arguments[0], root);

                IsExpression isExpression = ExpressionHelper.GetValue<IsExpression>(expression.Arguments[1]);

                ICriterion criterion = isExpression.Compile(property);
                if (isExpression.Negate)
                {
                    criterion = Restrictions.Not(criterion);
                }

                return criterion;
            }
            else
            {
                return Restrictions.Eq(Projections.Constant(ExpressionHelper.GetValue<bool>(expression)), true);
            }
        }

        public static ICriterion GetCriterion(Expression expression, string root, Dictionary<string, string> aliases)
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
                    return GetCriterionForBinary(expression as BinaryExpression, root, aliases);
                case ExpressionType.Call:
                    return GetCriterionForMethodCall(expression as MethodCallExpression, root, aliases);
                case ExpressionType.Lambda:
                    return GetCriterion((expression as LambdaExpression).Body, root, aliases);
                case ExpressionType.Not:
                    return Restrictions.Not(GetCriterion((expression as UnaryExpression).Operand, root, aliases));
                case ExpressionType.MemberAccess:
                    return Restrictions.Eq(ExpressionHelper.GetPropertyName(expression, root), true);
                case ExpressionType.Invoke:
                    return GetCriterionForInvoke(expression as InvocationExpression, root, aliases);
                case ExpressionType.Constant:
                default:
                    return Restrictions.Eq(Projections.Constant(ExpressionHelper.GetValue<bool>(expression)), true);
            }
        }

        public static ICriterion GetCriterionForBinary(BinaryExpression expression, string root, Dictionary<string, string> aliases)
        {
            return GetCriterionForBinary(expression.Left, expression.Right, expression.NodeType, root, aliases);
        }

        public static ICriterion GetCriterionForBinary(Expression a, Expression b, ExpressionType type, string root, Dictionary<string, string> aliases)
        {
            if (a.NodeType == ExpressionType.Convert)
            {
                return GetCriterionForBinary((a as UnaryExpression).Operand, b, type, root, aliases);
            }

            if (b.NodeType == ExpressionType.Convert)
            {
                return GetCriterionForBinary(a, (b as UnaryExpression).Operand, type, root, aliases);
            }

            switch (type)
            {
                case ExpressionType.AndAlso:
                    return Restrictions.And(GetCriterion(a, root, aliases), GetCriterion(b, root, aliases));
                case ExpressionType.OrElse:
                    return Restrictions.Or(GetCriterion(a, root, aliases), GetCriterion(b, root, aliases));
                case ExpressionType.ExclusiveOr:
                    ICriterion criterionA = GetCriterion(a, root, aliases);
                    ICriterion criterionB = GetCriterion(b, root, aliases);
                    return Restrictions.Or
                    (
                        Restrictions.And(criterionA, Restrictions.Not(criterionB)),
                        Restrictions.And(criterionB, Restrictions.Not(criterionA))
                    );
                default:
                    break;
            }

            bool aIsProjected = IsProjected(a, root, aliases);
            bool bIsProjected = IsProjected(b, root, aliases);

            if (aIsProjected && bIsProjected) // Projection Projection
            {
                return GetProjectionProjectionCriterion(ProjectionHelper.GetProjection(a, root, aliases), ProjectionHelper.GetProjection(b, root, aliases), type);
            }
            else if ((aIsProjected && !bIsProjected) || (!aIsProjected && !bIsProjected))
            {
                return GetProjectionValueCriterion(a, ExpressionHelper.GetValue(b), type, root, aliases, false);
            }
            return GetProjectionValueCriterion(b, ExpressionHelper.GetValue(a), type, root, aliases, true);
        }

        public static ICriterion GetCriterionForMethodCall(MethodCallExpression expression, string root, Dictionary<string, string> aliases)
        {
            if (IsProjected(expression, root, aliases)) // Not a value expression
            {
                IProjection projection = ProjectionHelper.GetProjection(expression.Object ?? expression.Arguments[0], root, aliases);

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
                                List<object> objs = new List<object>();
                                foreach (var obj in value as IEnumerable)
                                {
                                    objs.Add(obj);
                                }

                                return Restrictions.In
                                (
                                    projection,
                                    objs.ToArray()
                                );
                            }

                            if (value is SubFlowQuery)
                            {
                                return Subqueries.PropertyIn
                                (
                                    ExpressionHelper.GetPropertyName(expression, root),
                                    (value as SubFlowQuery).Criteria
                                );
                            }
                        }
                        return Restrictions.In
                        (
                            projection,
                            value as ICollection
                        );
                    case "Between":
                    case "IsBetween":
                        return Restrictions.Between
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

                throw new NotSupportedException("The expression contains unsupported features, please revise your code");
            }
            else
            {
                return Restrictions.Eq(Projections.Constant(ExpressionHelper.GetValue<bool>(expression)), true);
            }
        }

        public static ICriterion GetLikeCriterion(IProjection projection, Expression value, MatchMode matchMode)
        {
            return Restrictions.Like(projection, ExpressionHelper.GetValue<string>(value), matchMode);
        }

        public static ICriterion GetProjectionProjectionCriterion(IProjection projectionA, IProjection projectionB, ExpressionType type)
        {
            PropertyProjection a = projectionA as PropertyProjection;
            PropertyProjection b = projectionB as PropertyProjection;
            if (type == ExpressionType.Equal)
            {
                if (a != null && b != null)
                {
                    return Restrictions.EqProperty(a.PropertyName, b.PropertyName);
                }
                else if (a != null)
                {
                    return Restrictions.EqProperty(a.PropertyName, projectionB);
                }
                else if (b != null)
                {
                    return Restrictions.EqProperty(projectionA, b.PropertyName);
                }
                return Restrictions.EqProperty(projectionA, projectionB);
            }
            else if (type == ExpressionType.GreaterThan)
            {
                if (a != null && b != null)
                {
                    return Restrictions.GtProperty(a.PropertyName, b.PropertyName);
                }
                else if (a != null)
                {
                    return Restrictions.GtProperty(a.PropertyName, projectionB);
                }
                else if (b != null)
                {
                    return Restrictions.GtProperty(projectionA, b.PropertyName);
                }
                return Restrictions.GtProperty(projectionA, projectionB);
            }
            else if (type == ExpressionType.GreaterThanOrEqual)
            {
                if (a != null && b != null)
                {
                    return Restrictions.GeProperty(a.PropertyName, b.PropertyName);
                }
                else if (a != null)
                {
                    return Restrictions.GeProperty(a.PropertyName, projectionB);
                }
                else if (b != null)
                {
                    return Restrictions.GeProperty(projectionA, b.PropertyName);
                }
                return Restrictions.GeProperty(projectionA, projectionB);
            }
            else if (type == ExpressionType.LessThan)
            {
                if (a != null && b != null)
                {
                    return Restrictions.LtProperty(a.PropertyName, b.PropertyName);
                }
                else if (a != null)
                {
                    return Restrictions.LtProperty(a.PropertyName, projectionB);
                }
                else if (b != null)
                {
                    return Restrictions.LtProperty(projectionA, b.PropertyName);
                }
                return Restrictions.LtProperty(projectionA, projectionB);
            }
            else if (type == ExpressionType.LessThanOrEqual)
            {
                if (a != null && b != null)
                {
                    return Restrictions.LeProperty(a.PropertyName, b.PropertyName);
                }
                else if (a != null)
                {
                    return Restrictions.LeProperty(a.PropertyName, projectionB);
                }
                else if (b != null)
                {
                    return Restrictions.LeProperty(projectionA, b.PropertyName);
                }
                return Restrictions.LeProperty(projectionA, projectionB);
            }
            else
            {
                ICriterion criterion = null;
                if (a != null && b != null)
                {
                    criterion = Restrictions.EqProperty(a.PropertyName, b.PropertyName);
                }
                else if (a != null)
                {
                    criterion = Restrictions.EqProperty(a.PropertyName, projectionB);
                }
                else if (b != null)
                {
                    criterion = Restrictions.EqProperty(projectionA, b.PropertyName);
                }
                else
                {
                    criterion = Restrictions.EqProperty(projectionA, projectionB);
                }
                return Restrictions.Not(criterion);
            }
        }

        public static ICriterion GetProjectionValueCriterion(Expression expression, object value, ExpressionType type, string root, Dictionary<string, string> aliases, bool overTurned)
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

            IProjection projection = ProjectionHelper.GetProjection(expression, root, aliases);
            switch (type)
            {
                case ExpressionType.Equal:
                    if (value == null)
                    {
                        return Restrictions.IsNull(projection);
                    }
                    else if (value is bool)
                    {
                        return GetCriterion((bool)value ? expression : Expression.Not(expression), root, aliases);
                    }
                    return Restrictions.Eq(projection, value);
                case ExpressionType.NotEqual:
                    if (value == null)
                    {
                        return Restrictions.IsNotNull(projection);
                    }
                    else if (value is bool)
                    {
                        return GetCriterion(!(bool)value ? expression : Expression.Not(expression), root, aliases);
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
                    throw new NotSupportedException("the expression contains unsupported features, please revise your code");
            }
        }

        public static bool IsProjected(Expression expression, string root, Dictionary<string, string> aliases)
        {
            string expressionRoot = ExpressionHelper.GetRoot(expression);
            return expression is BinaryExpression || (expressionRoot != null && (expressionRoot == root || aliases.ContainsValue(expressionRoot)));
        }

        #endregion Methods
    }
}