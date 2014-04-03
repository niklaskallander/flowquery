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

        public virtual IsExpression Between(object lowValue, object highValue)
        {
            return Fix(new IsBetweenExpression(lowValue, highValue));
        }

        public virtual IsEmptyExpression Empty()
        {
            return Fix(new IsEmptyExpression());
        }

        public virtual IsExpression EqualTo(object value)
        {
            return Fix(new IsEqualExpression(value));
        }

        public virtual IsExpression EqualTo(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsEqualToSubqueryExpression(query));
        }

        public virtual IsExpression EqualToAll(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsEqualToAllSubqueryExpression(query));
        }

        protected virtual T Fix<T>(T expression)
            where T : IsExpression
        {
            expression.Negated = IsNegated;

            return expression;
        }

        public virtual IsExpression GreaterThan(object value)
        {
            return Fix(new IsGreaterThanExpression(value));
        }

        public virtual IsExpression GreaterThan(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsGreaterThanSubqueryExpression(query));
        }

        public virtual IsExpression GreaterThanAll(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsGreaterThanAllSubqueryExpression(query));
        }

        public virtual IsExpression GreaterThanOrEqualTo(object value)
        {
            return Fix(new IsGreaterThanOrEqualExpression(value));
        }

        public virtual IsExpression GreaterThanOrEqualTo(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsGreaterThanOrEqualToSubqueryExpression(query));
        }

        public virtual IsExpression GreaterThanOrEqualToAll(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsGreaterThanOrEqualToAllSubqueryExpression(query));
        }

        public virtual IsExpression GreaterThanOrEqualToSome(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsGreaterThanOrEqualToSomeSubqueryExpression(query));
        }

        public virtual IsExpression GreaterThanSome(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsGreaterThanSomeSubqueryExpression(query));
        }

        public virtual IsExpression In(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsInSubqueryExpression(query));
        }

        public virtual IsExpression In(params object[] values)
        {
            return Fix(new IsInValuesExpression(values));
        }

        public virtual IsExpression In(IEnumerable value)
        {
            return Fix(new IsInValuesExpression(value));
        }

        public virtual IsExpression LessThan(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsLessThanSubqueryExpression(query));
        }

        public virtual IsExpression LessThan(object value)
        {
            return Fix(new IsLessThanExpression(value));
        }

        public virtual IsExpression LessThanAll(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsLessThanAllSubqueryExpression(query));
        }

        public virtual IsExpression LessThanOrEqualTo(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsLessThanOrEqualToSubqueryExpression(query));
        }

        public virtual IsExpression LessThanOrEqualTo(object value)
        {
            return Fix(new IsLessThanOrEqualExpression(value));
        }

        public virtual IsExpression LessThanOrEqualToAll(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsLessThanOrEqualToAllSubqueryExpression(query));
        }

        public virtual IsExpression LessThanOrEqualToSome(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsLessThanOrEqualToSomeSubqueryExpression(query));
        }

        public virtual IsExpression LessThanSome(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsLessThanSomeSubqueryExpression(query));
        }

        public virtual IsExpression Like(object value)
        {
            return Fix(new IsLikeExpression(value));
        }

        public virtual IsExpression Null()
        {
            return Fix(new IsNullExpression());
        }
    }
}