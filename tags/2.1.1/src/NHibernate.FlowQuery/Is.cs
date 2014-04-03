using System.Collections;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Expressions;

namespace NHibernate.FlowQuery
{
    public static class Is
    {
        private static readonly IsHelper Negative;
        private static readonly IsHelper Positive;

        static Is()
        {
            Positive = new IsHelper(false);
            Negative = new IsHelper(true);
        }

        public static IsHelper Not
        {
            get { return Negative; }
        }

        public static IsExpression Between(object lowValue, object highValue)
        {
            return Positive.Between(lowValue, highValue);
        }

        public static Expressions.IsEmptyExpression Empty()
        {
            return Positive.Empty();
        }

        public static IsExpression EqualTo(object value)
        {
            return Positive.EqualTo(value);
        }

        public static IsExpression EqualTo(IDetachedImmutableFlowQuery query)
        {
            return Positive.EqualTo(query);
        }

        public static IsExpression EqualToAll(IDetachedImmutableFlowQuery query)
        {
            return Positive.EqualToAll(query);
        }

        public static IsExpression GreaterThan(object value)
        {
            return Positive.GreaterThan(value);
        }

        public static IsExpression GreaterThan(IDetachedImmutableFlowQuery query)
        {
            return Positive.GreaterThan(query);
        }

        public static IsExpression GreaterThanAll(IDetachedImmutableFlowQuery query)
        {
            return Positive.GreaterThanAll(query);
        }

        public static IsExpression GreaterThanOrEqualTo(object value)
        {
            return Positive.GreaterThanOrEqualTo(value);
        }

        public static IsExpression GreaterThanOrEqualTo(IDetachedImmutableFlowQuery query)
        {
            return Positive.GreaterThanOrEqualTo(query);
        }

        public static IsExpression GreaterThanOrEqualToAll(IDetachedImmutableFlowQuery query)
        {
            return Positive.GreaterThanOrEqualToAll(query);
        }

        public static IsExpression GreaterThanOrEqualToSome(IDetachedImmutableFlowQuery query)
        {
            return Positive.GreaterThanOrEqualToSome(query);
        }

        public static IsExpression GreaterThanSome(IDetachedImmutableFlowQuery query)
        {
            return Positive.GreaterThanSome(query);
        }

        public static IsExpression In(DetachedCriteria query)
        {
            return In(query.DetachedFlowQuery());
        }

        public static IsExpression In(IDetachedImmutableFlowQuery query)
        {
            return Positive.In(query);
        }

        public static IsExpression In(params object[] values)
        {
            return Positive.In(values);
        }

        public static IsExpression In(IEnumerable value)
        {
            return Positive.In(value);
        }

        public static IsExpression LessThan(object value)
        {
            return Positive.LessThan(value);
        }

        public static IsExpression LessThan(IDetachedImmutableFlowQuery query)
        {
            return Positive.LessThan(query);
        }

        public static IsExpression LessThanAll(IDetachedImmutableFlowQuery query)
        {
            return Positive.LessThanAll(query);
        }

        public static IsExpression LessThanOrEqualTo(object value)
        {
            return Positive.LessThanOrEqualTo(value);
        }

        public static IsExpression LessThanOrEqualTo(IDetachedImmutableFlowQuery query)
        {
            return Positive.LessThanOrEqualTo(query);
        }

        public static IsExpression LessThanOrEqualToAll(IDetachedImmutableFlowQuery query)
        {
            return Positive.LessThanOrEqualToAll(query);
        }

        public static IsExpression LessThanOrEqualToSome(IDetachedImmutableFlowQuery query)
        {
            return Positive.LessThanOrEqualToSome(query);
        }

        public static IsExpression LessThanSome(IDetachedImmutableFlowQuery query)
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
    }
}