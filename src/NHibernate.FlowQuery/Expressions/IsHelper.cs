using System.Collections;
using NHibernate.FlowQuery.Core;
using NHibernate.FlowQuery.Expressions.SubqueryExpressions;

namespace NHibernate.FlowQuery.Expressions
{
    public class IsHelper
    {
        #region Constructors (1)

        public IsHelper(bool isNegated)
        {
            IsNegated = isNegated;
        }

        #endregion Constructors

        #region Properties (1)

        protected bool IsNegated { get; private set; }

        #endregion Properties

        #region Methods (26)

        public IsExpression Between(object lowValue, object highValue)
        {
            return Fix(new IsBetweenExpression(lowValue, highValue));
        }

        public IsExpression EqualTo(object value)
        {
            return Fix(new IsEqualExpression(value));
        }

        public IsExpression EqualTo<TSource>(ISubFlowQuery<TSource> query)
        {
            return Fix(new IsEqualToSubqueryExpression<TSource>(query));
        }

        public IsExpression EqualToAll<TSource>(ISubFlowQuery<TSource> query)
        {
            return Fix(new IsEqualToAllSubqueryExpression<TSource>(query));
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

        public IsExpression GreaterThan<TSource>(ISubFlowQuery<TSource> query)
        {
            return Fix(new IsGreaterThanSubqueryExpression<TSource>(query));
        }

        public IsExpression GreaterThanAll<TSource>(ISubFlowQuery<TSource> query)
        {
            return Fix(new IsGreaterThanAllSubqueryExpression<TSource>(query));
        }

        public IsExpression GreaterThanOrEqualTo(object value)
        {
            return Fix(new IsGreaterThanOrEqualExpression(value));
        }

        public IsExpression GreaterThanOrEqualTo<TSource>(ISubFlowQuery<TSource> query)
        {
            return Fix(new IsGreaterThanOrEqualToSubqueryExpression<TSource>(query));
        }

        public IsExpression GreaterThanOrEqualToAll<TSource>(ISubFlowQuery<TSource> query)
        {
            return Fix(new IsGreaterThanOrEqualToAllSubqueryExpression<TSource>(query));
        }

        public IsExpression GreaterThanOrEqualToSome<TSource>(ISubFlowQuery<TSource> query)
        {
            return Fix(new IsGreaterThanOrEqualToSomeSubqueryExpression<TSource>(query));
        }

        public IsExpression GreaterThanSome<TSource>(ISubFlowQuery<TSource> query)
        {
            return Fix(new IsGreaterThanSomeSubqueryExpression<TSource>(query));
        }

        public IsExpression In<TSource>(ISubFlowQuery<TSource> query)
        {
            return Fix(new IsInSubqueryExpression<TSource>(query));
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

        public IsExpression LessThan<TSource>(ISubFlowQuery<TSource> query)
        {
            return Fix(new IsLessThanSubqueryExpression<TSource>(query));
        }

        public IsExpression LessThan(object value)
        {
            return Fix(new IsLessThanExpression(value));
        }

        public IsExpression LessThanAll<TSource>(ISubFlowQuery<TSource> query)
        {
            return Fix(new IsLessThanAllSubqueryExpression<TSource>(query));
        }

        public IsExpression LessThanOrEqualTo<TSource>(ISubFlowQuery<TSource> query)
        {
            return Fix(new IsLessThanOrEqualToSubqueryExpression<TSource>(query));
        }

        public IsExpression LessThanOrEqualTo(object value)
        {
            return Fix(new IsLessThanOrEqualExpression(value));
        }

        public IsExpression LessThanOrEqualToAll<TSource>(ISubFlowQuery<TSource> query)
        {
            return Fix(new IsLessThanOrEqualToAllSubqueryExpression<TSource>(query));
        }

        public IsExpression LessThanOrEqualToSome<TSource>(ISubFlowQuery<TSource> query)
        {
            return Fix(new IsLessThanOrEqualToSomeSubqueryExpression<TSource>(query));
        }

        public IsExpression LessThanSome<TSource>(ISubFlowQuery<TSource> query)
        {
            return Fix(new IsLessThanSomeSubqueryExpression<TSource>(query));
        }

        public IsExpression Like(object value)
        {
            return Fix(new IsLikeExpression(value));
        }

        public IsExpression Null()
        {
            return Fix(new IsNullExpression());
        }

        #endregion Methods
    }
}