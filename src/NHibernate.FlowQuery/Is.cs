using System.Collections;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Expressions;

namespace NHibernate.FlowQuery
{
    public static class Is
    {
        private static readonly IsHelper m_Negative;
        private static readonly IsHelper m_Positive;

        static Is()
        {
            m_Positive = new IsHelper(false);
            m_Negative = new IsHelper(true);
        }

        public static IsHelper Not
        {
            get { return m_Negative; }
        }

        public static IsExpression Between(object lowValue, object highValue)
        {
            return m_Positive.Between(lowValue, highValue);
        }

        public static IsExpression EqualTo(object value)
        {
            return m_Positive.EqualTo(value);
        }

        public static IsExpression EqualTo(IDetachedImmutableFlowQuery query)
        {
            return m_Positive.EqualTo(query);
        }

        public static IsExpression EqualToAll(IDetachedImmutableFlowQuery query)
        {
            return m_Positive.EqualToAll(query);
        }

        public static IsExpression GreaterThan(object value)
        {
            return m_Positive.GreaterThan(value);
        }

        public static IsExpression GreaterThan(IDetachedImmutableFlowQuery query)
        {
            return m_Positive.GreaterThan(query);
        }

        public static IsExpression GreaterThanAll(IDetachedImmutableFlowQuery query)
        {
            return m_Positive.GreaterThanAll(query);
        }

        public static IsExpression GreaterThanOrEqualTo(object value)
        {
            return m_Positive.GreaterThanOrEqualTo(value);
        }

        public static IsExpression GreaterThanOrEqualTo(IDetachedImmutableFlowQuery query)
        {
            return m_Positive.GreaterThanOrEqualTo(query);
        }

        public static IsExpression GreaterThanOrEqualToAll(IDetachedImmutableFlowQuery query)
        {
            return m_Positive.GreaterThanOrEqualToAll(query);
        }

        public static IsExpression GreaterThanOrEqualToSome(IDetachedImmutableFlowQuery query)
        {
            return m_Positive.GreaterThanOrEqualToSome(query);
        }

        public static IsExpression GreaterThanSome(IDetachedImmutableFlowQuery query)
        {
            return m_Positive.GreaterThanSome(query);
        }

        public static IsExpression In(IDetachedImmutableFlowQuery query)
        {
            return m_Positive.In(query);
        }

        public static IsExpression In(params object[] values)
        {
            return m_Positive.In(values);
        }

        public static IsExpression In(IEnumerable value)
        {
            return m_Positive.In(value);
        }

        public static IsExpression LessThan(object value)
        {
            return m_Positive.LessThan(value);
        }

        public static IsExpression LessThan(IDetachedImmutableFlowQuery query)
        {
            return m_Positive.LessThan(query);
        }

        public static IsExpression LessThanAll(IDetachedImmutableFlowQuery query)
        {
            return m_Positive.LessThanAll(query);
        }

        public static IsExpression LessThanOrEqualTo(object value)
        {
            return m_Positive.LessThanOrEqualTo(value);
        }

        public static IsExpression LessThanOrEqualTo(IDetachedImmutableFlowQuery query)
        {
            return m_Positive.LessThanOrEqualTo(query);
        }

        public static IsExpression LessThanOrEqualToAll(IDetachedImmutableFlowQuery query)
        {
            return m_Positive.LessThanOrEqualToAll(query);
        }

        public static IsExpression LessThanOrEqualToSome(IDetachedImmutableFlowQuery query)
        {
            return m_Positive.LessThanOrEqualToSome(query);
        }

        public static IsExpression LessThanSome(IDetachedImmutableFlowQuery query)
        {
            return m_Positive.LessThanSome(query);
        }

        public static IsExpression Like(object value)
        {
            return m_Positive.Like(value);
        }

        public static IsExpression Null()
        {
            return m_Positive.Null();
        }
    }
}