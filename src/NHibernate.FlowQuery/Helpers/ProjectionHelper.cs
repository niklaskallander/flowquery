using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using Expression = System.Linq.Expressions.Expression;

namespace NHibernate.FlowQuery.Helpers
{
    public static class ProjectionHelper
    {
        #region Methods (11)

        public static IEnumerable<Expression> FlattenBinaryExpression(BinaryExpression expression)
        {
            List<Expression> expressions = new List<Expression>();
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

        public static IProjection ForExpression(Expression expression, string rootName)
        {
            return GetProjection(expression, rootName);
        }

        public static void ForMemberInitExpression(MemberInitExpression expression, string rootName, ref ProjectionList list, ref Dictionary<string, IProjection> mappings)
        {
            ForNewExpression(expression.NewExpression, rootName, ref list, ref mappings);

            foreach (MemberAssignment memberAssigment in expression.Bindings)
            {
                switch (memberAssigment.Expression.NodeType)
                {
                    case ExpressionType.MemberInit:
                        ForMemberInitExpression(memberAssigment.Expression as MemberInitExpression, rootName, ref list, ref mappings);
                        break;

                    case ExpressionType.New:
                        ForNewExpression(memberAssigment.Expression as NewExpression, rootName, ref list, ref mappings);
                        break;

                    default:
                        IProjection projection = ForExpression(memberAssigment.Expression, rootName);

                        list.Add(projection);

                        if (!mappings.ContainsKey(memberAssigment.Member.Name))
                        {
                            mappings.Add(memberAssigment.Member.Name, projection);
                        }
                        break;
                }
            }
        }

        public static void ForNewExpression(NewExpression expression, string rootName, ref ProjectionList list, ref Dictionary<string, IProjection> mappings)
        {
            foreach (Expression argument in expression.Arguments)
            {
                switch (argument.NodeType)
                {
                    case ExpressionType.MemberInit:
                        ForMemberInitExpression(argument as MemberInitExpression, rootName, ref list, ref mappings);
                        break;

                    case ExpressionType.New:
                        ForNewExpression(argument as NewExpression, rootName, ref list, ref mappings);
                        break;

                    default:
                        IProjection projection = ForExpression(argument, rootName);

                        list.Add(projection);
                        break;
                }
            }
        }

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
                case ExpressionType.Add:
                default:
                    return "+";
            }
        }

        private static IProjection GetArithmeticProjection(BinaryExpression expression, string root)
        {
            if (expression.NodeType == ExpressionType.Add && expression.Type == typeof(string))
            {
                return GetConcatenationProjection(expression, root);
            }

            string operation = GetArithmeticOperation(expression.NodeType);

            return new SqlFunctionProjection
            (
                new VarArgsSQLFunction("(", operation, ")"),
                NHibernateUtil.GuessType(expression.Left.Type),
                GetProjection(expression.Left, root),
                GetProjection(expression.Right, root)
            );
        }

        private static IProjection GetConcatenationProjection(BinaryExpression expression, string root)
        {
            var projections = new List<IProjection>();
            foreach (var expressionPart in FlattenBinaryExpression(expression))
            {
                var projection = GetProjection(expressionPart, root);

                projections.Add(projection);
            }

            return new SqlFunctionProjection("concat", NHibernateUtil.String, projections.ToArray());
        }

        private static IProjection GetConditionalProjection(ConditionalExpression expression, string rootName)
        {
            return Projections.Conditional
            (
                RestrictionHelper.GetCriterion(expression.Test, rootName, new Dictionary<string, string>()),
                GetProjection(expression.IfTrue, rootName),
                GetProjection(expression.IfFalse, rootName)
            );
        }

        private static IProjection GetMethodCallProjection(MethodCallExpression expression, string rootName)
        {
            Expression subExpression = expression.Object ?? expression.Arguments[0];
            IProjection projection = GetProjection(subExpression, rootName);
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
                    return Projections.Cast(NHibernateUtil.Decimal, Projections.Avg(projection));
                case "Count":
                    return Projections.Count(projection);
                case "CountDistinct":
                    return Projections.CountDistinct(ExpressionHelper.GetPropertyName(subExpression, rootName));
                case "As":
                    return Projections.Property(ExpressionHelper.GetPropertyName(subExpression, rootName));
                case "Substring":
                    return new SqlFunctionProjection
                    (
                        "substring",
                        NHibernateUtil.String,
                        projection,
                        Projections.Constant(ExpressionHelper.GetValue(expression.Arguments[0])),
                        Projections.Constant(ExpressionHelper.GetValue(expression.Arguments[1]))
                    );
                default:
                    throw new NotSupportedException("the expression contains unsupported features, please revise your code");
            }
        }

        public static IProjection GetProjection(Expression expression, string root)
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
                    return GetConditionalProjection(Expression.Condition(expression, Expression.Constant(true), Expression.Constant(false)), root);
                case ExpressionType.Add:
                case ExpressionType.Subtract:
                case ExpressionType.Divide:
                case ExpressionType.Multiply:
                    return GetArithmeticProjection(expression as BinaryExpression, root);
                case ExpressionType.Conditional:
                    return GetConditionalProjection(expression as ConditionalExpression, root);
                case ExpressionType.Call:
                    return GetMethodCallProjection(expression as MethodCallExpression, root);
                case ExpressionType.MemberAccess:
                    return Projections.Property(ExpressionHelper.GetPropertyName(expression, root));
                case ExpressionType.Convert:
                    return GetProjection((expression as UnaryExpression).Operand, root);
                case ExpressionType.Constant:
                default:
                    return Projections.Constant(ExpressionHelper.GetValue(expression));
            }
        }

        public static ProjectionList GetProjectionListForExpression(Expression expression, string root, ref Dictionary<string, IProjection> mappings)
        {
            ProjectionList list = Projections.ProjectionList();
            switch (expression.NodeType)
            {
                case ExpressionType.New:
                    ForNewExpression(expression as NewExpression, root, ref list, ref mappings);
                    break;

                case ExpressionType.MemberInit:
                    ForMemberInitExpression(expression as MemberInitExpression, root, ref list, ref mappings);
                    break;

                default:
                    IProjection projection = ForExpression(expression, root);

                    list.Add(projection);
                    break;
            }
            return list;
        }

        #endregion Methods
    }
}