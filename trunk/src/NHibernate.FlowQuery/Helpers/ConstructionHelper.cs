using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Expression = System.Linq.Expressions.Expression;

namespace NHibernate.FlowQuery.Helpers
{
    public static class ConstructionHelper
    {
        private static IEnumerable<TDestination> ForMemberInitExpression<TDestination>(MemberInitExpression expression, IEnumerable list)
        {
            List<TDestination> temp = new List<TDestination>();

            foreach (object o in list)
            {
                object instance;

                Invoke(expression, o as object[] ?? new object[] { o }, out instance);

                temp.Add((TDestination)instance);
            }

            return temp;
        }

        private static IEnumerable<TDestination> ForNewExpression<TDestination>(NewExpression expression, IEnumerable list)
        {
            List<TDestination> temp = new List<TDestination>();

            foreach (object o in list)
            {
                object instance;

                Invoke(expression, o as object[] ?? new object[] { o }, out instance);

                temp.Add((TDestination)instance);
            }

            return temp;
        }

        public static bool CanHandle(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (expression.NodeType == ExpressionType.Lambda)
            {
                return CanHandle((expression as LambdaExpression).Body);
            }

            bool isNew = expression.NodeType == ExpressionType.New,
                 isMemberInit = expression.NodeType == ExpressionType.MemberInit;

            return isNew || isMemberInit;
        }

        public static IEnumerable<TDestination> GetListByExpression<TDestination>(Expression expression, IEnumerable list)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (expression.NodeType == ExpressionType.Lambda)
            {
                return GetListByExpression<TDestination>((expression as LambdaExpression).Body, list);
            }

            if (list == null)
            {
                return null;
            }

            bool canHandle = CanHandle(expression);

            if (canHandle)
            {
                if (expression.NodeType == ExpressionType.New)
                {
                    return ForNewExpression<TDestination>(expression as NewExpression, list);
                }

                return ForMemberInitExpression<TDestination>(expression as MemberInitExpression, list);
            }

            return null;
        }

        private static int Invoke(NewExpression expression, object[] arguments, out object instance)
        {
            int i = 0;

            List<object> list = new List<object>();

            foreach (Expression argument in expression.Arguments)
            {
                object value;

                switch (argument.NodeType)
                {
                    case ExpressionType.New:
                        i += Invoke(argument as NewExpression, arguments.Skip(i).ToArray(), out value);
                        break;

                    case ExpressionType.MemberInit:
                        i += Invoke(argument as MemberInitExpression, arguments.Skip(i).ToArray(), out value);
                        break;

                    default:
                        value = arguments[i];
                        i++;
                        break;
                }

                list.Add(value);
            }

            instance = expression.Constructor.Invoke(list.ToArray());

            return i;
        }

        private static int Invoke(MemberInitExpression expression, object[] arguments, out object instance)
        {
            int i = Invoke(expression.NewExpression, arguments, out instance);

            foreach (MemberAssignment binding in expression.Bindings)
            {
                object value;

                switch (binding.Expression.NodeType)
                {
                    case ExpressionType.New:
                        i += Invoke(binding.Expression as NewExpression, arguments.Skip(i).ToArray(), out value);
                        break;

                    case ExpressionType.MemberInit:
                        i += Invoke(binding.Expression as MemberInitExpression, arguments.Skip(i).ToArray(), out value);
                        break;

                    default:
                        value = arguments[i];
                        i++;
                        break;
                }

                SetValue(binding.Member, instance, value);
            }

            return i;
        }

        /// <remarks>
        /// Borrowed from Linq to NHibernate
        /// </remarks>
        private static void SetValue(MemberInfo memberInfo, object instance, object value)
        {
            var field = memberInfo as FieldInfo;

            if (field != null)
            {
                field.SetValue(instance, value);
            }
            else
            {
                var prop = memberInfo as PropertyInfo;

                if (prop != null)
                {
                    prop.SetValue(instance, value, null);
                }
            }
        }
    }
}