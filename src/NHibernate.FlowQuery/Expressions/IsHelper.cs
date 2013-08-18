using System.Collections;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Expressions.SubqueryExpressions;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsHelper
    {
        public IsHelper(bool isNegated)
        {
            IsNegated = isNegated;
        }

        protected bool IsNegated { get; private set; }

        public IsExpression Between(object lowValue, object highValue)
        {
            return Fix(new IsBetweenExpression(lowValue, highValue));
        }

        public IsExpression EqualTo(object value)
        {
            return Fix(new IsEqualExpression(value));
        }

        public IsExpression EqualTo(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsEqualToSubqueryExpression(query));
        }

        public IsExpression EqualToAll(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsEqualToAllSubqueryExpression(query));
        }

        protected virtual IsExpression Fix(IsExpression expression)
        {
            expression.Negate = IsNegated;

            return expression;
        }

        public IsExpression GreaterThan(object value)
        {
            return Fix(new IsGreaterThanExpression(value));
        }

        public IsExpression GreaterThan(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsGreaterThanSubqueryExpression(query));
        }

        public IsExpression GreaterThanAll(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsGreaterThanAllSubqueryExpression(query));
        }

        public IsExpression GreaterThanOrEqualTo(object value)
        {
            return Fix(new IsGreaterThanOrEqualExpression(value));
        }

        public IsExpression GreaterThanOrEqualTo(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsGreaterThanOrEqualToSubqueryExpression(query));
        }

        public IsExpression GreaterThanOrEqualToAll(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsGreaterThanOrEqualToAllSubqueryExpression(query));
        }

        public IsExpression GreaterThanOrEqualToSome(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsGreaterThanOrEqualToSomeSubqueryExpression(query));
        }

        public IsExpression GreaterThanSome(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsGreaterThanSomeSubqueryExpression(query));
        }

        public IsExpression In(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsInSubqueryExpression(query));
        }

        public IsExpression In(params object[] values)
        {
            return Fix(new IsInValuesExpression(values));
        }

        public IsExpression In<TEnumerable>(TEnumerable value)
            where TEnumerable : IEnumerable
        {
            return Fix(new IsInValuesExpression(value));
        }

        public IsExpression LessThan(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsLessThanSubqueryExpression(query));
        }

        public IsExpression LessThan(object value)
        {
            return Fix(new IsLessThanExpression(value));
        }

        public IsExpression LessThanAll(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsLessThanAllSubqueryExpression(query));
        }

        public IsExpression LessThanOrEqualTo(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsLessThanOrEqualToSubqueryExpression(query));
        }

        public IsExpression LessThanOrEqualTo(object value)
        {
            return Fix(new IsLessThanOrEqualExpression(value));
        }

        public IsExpression LessThanOrEqualToAll(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsLessThanOrEqualToAllSubqueryExpression(query));
        }

        public IsExpression LessThanOrEqualToSome(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsLessThanOrEqualToSomeSubqueryExpression(query));
        }

        public IsExpression LessThanSome(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsLessThanSomeSubqueryExpression(query));
        }

        public IsExpression Like(object value)
        {
            return Fix(new IsLikeExpression(value));
        }

        public IsExpression Null()
        {
            return Fix(new IsNullExpression());
        }
    }
}