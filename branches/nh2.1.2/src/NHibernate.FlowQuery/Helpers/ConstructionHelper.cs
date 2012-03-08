using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Criterion;
using Expression = System.Linq.Expressions.Expression;

namespace NHibernate.FlowQuery.Helpers
{
    public static class ConstructionHelper
    {
        #region Methods (7)

        public static IEnumerable<TReturn> ForMemberInitExpression<TReturn>(MemberInitExpression expression, IList list)
        {
            List<TReturn> returnList = new List<TReturn>();
            foreach (object o in list)
            {
                object instance;
                Invoke(expression, o as object[] ?? new object[] { o }, out instance);
                returnList.Add((TReturn)instance);
            }
            return returnList;
        }

        public static IEnumerable<TReturn> ForNewExpression<TReturn>(NewExpression expression, IList list)
        {
            List<TReturn> returnList = new List<TReturn>();
            foreach (object o in list)
            {
                object instance;
                Invoke(expression, o as object[] ?? new object[] { o }, out instance);
                returnList.Add((TReturn)instance);
            }
            return returnList;
        }

        public static IEnumerable<TReturn> GetListByExpression<TReturn>(Expression expression, IProjection projection, ICriteria criteria)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (projection == null)
            {
                throw new ArgumentNullException("projection");
            }

            if (criteria == null)
            {
                throw new ArgumentNullException("criteria");
            }

            switch (expression.NodeType)
            {
                case ExpressionType.New:
                    return ForNewExpression<TReturn>
                    (
                        expression as NewExpression,
                        criteria
                            .SetProjection(projection)
                            .List()
                    );

                case ExpressionType.MemberInit:
                    return ForMemberInitExpression<TReturn>
                    (
                        expression as MemberInitExpression,
                        criteria
                            .SetProjection(projection)
                            .List()
                    );

                case ExpressionType.Lambda:
                    return GetListByExpression<TReturn>((expression as LambdaExpression).Body, projection, criteria);

                default:
                    return criteria
                            .SetProjection(projection)
                            .List<TReturn>();
            }
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

        #endregion Methods
    }
}