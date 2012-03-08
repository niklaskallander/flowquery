using System.Collections;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Expressions;

namespace NHibernate.FlowQuery
{
    public static class Is
    {
        #region Constructors (1)

        static Is()
        {
            Positive = new IsHelper(false);
            Negative = new IsHelper(true);
        }

        #endregion Constructors

        #region Properties (3)

        private static IsHelper Negative { get; set; }

        public static IsHelper Not
        {
            get { return Negative; }
        }

        private static IsHelper Positive { get; set; }

        #endregion Properties

        #region Methods (23)

        public static IsExpression Between(object lowValue, object highValue)
        {
            return Positive.Between(lowValue, highValue);
        }

        public static IsExpression EqualTo(object value)
        {
            return Positive.EqualTo(value);
        }

        public static IsExpression EqualTo<TSource>(ISubFlowQuery<TSource> query)
        {
            return Positive.EqualTo(query);
        }

        public static IsExpression EqualToAll<TSource>(ISubFlowQuery<TSource> query)
        {
            return Positive.EqualToAll(query);
        }

        public static IsExpression GreaterThan(object value)
        {
            return Positive.GreaterThan(value);
        }

        public static IsExpression GreaterThan<TSource>(ISubFlowQuery<TSource> query)
        {
            return Positive.GreaterThan(query);
        }

        public static IsExpression GreaterThanAll<TSource>(ISubFlowQuery<TSource> query)
        {
            return Positive.GreaterThanAll(query);
        }

        public static IsExpression GreaterThanOrEqualTo(object value)
        {
            return Positive.GreaterThanOrEqualTo(value);
        }

        public static IsExpression GreaterThanOrEqualTo<TSource>(ISubFlowQuery<TSource> query)
        {
            return Positive.GreaterThanOrEqualTo(query);
        }

        public static IsExpression GreaterThanOrEqualToAll<TSource>(ISubFlowQuery<TSource> query)
        {
            return Positive.GreaterThanOrEqualToAll(query);
        }

        public static IsExpression GreaterThanOrEqualToSome<TSource>(ISubFlowQuery<TSource> query)
        {
            return Positive.GreaterThanOrEqualToSome(query);
        }

        public static IsExpression GreaterThanSome<TSource>(ISubFlowQuery<TSource> query)
        {
            return Positive.GreaterThanSome(query);
        }

        public static IsExpression In<TSource>(ISubFlowQuery<TSource> query)
        {
            return Positive.In(query);
        }

        public static IsExpression In(params object[] values)
        {
            return Positive.In(values);
        }

        public static IsExpression In<TEnumerable>(TEnumerable value)
            where TEnumerable : IEnumerable
        {
            return Positive.In(value);
        }

        public static IsExpression LessThan(object value)
        {
            return Positive.LessThan(value);
        }

        public static IsExpression LessThan<TSource>(ISubFlowQuery<TSource> query)
        {
            return Positive.LessThan(query);
        }

        public static IsExpression LessThanAll<TSource>(ISubFlowQuery<TSource> query)
        {
            return Positive.LessThanAll(query);
        }

        public static IsExpression LessThanOrEqualTo(object value)
        {
            return Positive.LessThanOrEqualTo(value);
        }

        public static IsExpression LessThanOrEqualTo<TSource>(ISubFlowQuery<TSource> query)
        {
            return Positive.LessThanOrEqualTo(query);
        }

        public static IsExpression LessThanOrEqualToAll<TSource>(ISubFlowQuery<TSource> query)
        {
            return Positive.LessThanOrEqualToAll(query);
        }

        public static IsExpression LessThanOrEqualToSome<TSource>(ISubFlowQuery<TSource> query)
        {
            return Positive.LessThanOrEqualToSome(query);
        }

        public static IsExpression LessThanSome<TSource>(ISubFlowQuery<TSource> query)
        {
            return Positive.LessThanSome(query);
        }

        public static IsExpression Like(object value)
        {
            return Positive.Like(value);
        }

        public static IsExpression Null()
        {
            return Positive.Null();
        }

        #endregion Methods
    }
}