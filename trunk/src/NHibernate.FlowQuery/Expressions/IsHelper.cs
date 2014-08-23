namespace NHibernate.FlowQuery.Expressions
{
    using System.Collections;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Expressions.SubqueryExpressions;

    /// <summary>
    ///     A helper class defining a set of utilities to create filters used in <see cref="NHibernate.FlowQuery" /> 
    ///     queries.
    /// </summary>
    public class IsHelper
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IsHelper"/> class.
        /// </summary>
        /// <param name="isNegated">
        ///     A value indicating whether filters created by this <see cref="IsHelper" /> instance should be negated.
        /// </param>
        public IsHelper(bool isNegated)
        {
            IsNegated = isNegated;
        }

        /// <summary>
        ///     Gets a value indicating whether filters created by this <see cref="IsHelper" /> instance will be 
        ///     negated.
        /// </summary>
        /// <value>
        ///     A value indicating whether filters created by this <see cref="IsHelper" /> instance will be negated.
        /// </value>
        protected bool IsNegated { get; private set; }

        /// <summary>
        ///     Creates a "is (not) between" filter.
        /// </summary>
        /// <param name="lowValue">
        ///     The low value to match.
        /// </param>
        /// <param name="highValue">
        ///     The high value to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression Between(object lowValue, object highValue)
        {
            return Fix(new IsBetweenExpression(lowValue, highValue));
        }

        /// <summary>
        ///     Creates an "is (not) empty" filter. Useful to filter on a sub-query that shouldn't yield any results, or
        ///     to filter on an association path pointing to a collection that should (not) be empty.
        /// </summary>
        /// <returns>
        ///     The <see cref="IsEmptyExpression"/> filter.
        /// </returns>
        public virtual IsEmptyExpression Empty()
        {
            return Fix(new IsEmptyExpression());
        }

        /// <summary>
        ///     Creates an "is (not) equal to" filter.
        /// </summary>
        /// <param name="value">
        ///     The value to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression EqualTo(object value)
        {
            return Fix(new IsEqualExpression(value));
        }

        /// <summary>
        ///     Creates an "is (not) equal to" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression EqualTo(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsEqualToSubqueryExpression(query));
        }

        /// <summary>
        ///     Creates an "is (not) equal to all" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression EqualToAll(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsEqualToAllSubqueryExpression(query));
        }

        /// <summary>
        ///     Creates a "is (not) greater than" filter.
        /// </summary>
        /// <param name="value">
        ///     The value to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression GreaterThan(object value)
        {
            return Fix(new IsGreaterThanExpression(value));
        }

        /// <summary>
        ///     Creates a "is (not) greater than" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression GreaterThan(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsGreaterThanSubqueryExpression(query));
        }

        /// <summary>
        ///     Creates a "is (not) greater than all" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression GreaterThanAll(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsGreaterThanAllSubqueryExpression(query));
        }

        /// <summary>
        ///     Creates a "is (not) greater than or equal to" filter.
        /// </summary>
        /// <param name="value">
        ///     The value to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression GreaterThanOrEqualTo(object value)
        {
            return Fix(new IsGreaterThanOrEqualExpression(value));
        }

        /// <summary>
        ///     Creates a "is (not) greater than or equal to" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression GreaterThanOrEqualTo(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsGreaterThanOrEqualToSubqueryExpression(query));
        }

        /// <summary>
        ///     Creates a "is (not) greater than or equal to all" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression GreaterThanOrEqualToAll(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsGreaterThanOrEqualToAllSubqueryExpression(query));
        }

        /// <summary>
        ///     Creates a "is (not) greater than or equal to some" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression GreaterThanOrEqualToSome(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsGreaterThanOrEqualToSomeSubqueryExpression(query));
        }

        /// <summary>
        ///     Creates a "is (not) greater than some" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression GreaterThanSome(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsGreaterThanSomeSubqueryExpression(query));
        }

        /// <summary>
        ///     Creates a "is (not) in" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the results to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression In(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsInSubqueryExpression(query));
        }

        /// <summary>
        ///     Creates a "is (not) in" filter.
        /// </summary>
        /// <param name="values">
        ///     The values to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression In(params object[] values)
        {
            return Fix(new IsInValuesExpression(values));
        }

        /// <summary>
        ///     Creates a "is (not) in" filter.
        /// </summary>
        /// <param name="values">
        ///     The values to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression In(IEnumerable values)
        {
            return Fix(new IsInValuesExpression(values));
        }

        /// <summary>
        ///     Creates a "less (not) than" filter.
        /// </summary>
        /// <param name="value">
        ///     The value to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression LessThan(object value)
        {
            return Fix(new IsLessThanExpression(value));
        }

        /// <summary>
        ///     Creates a "is (not) less than" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression LessThan(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsLessThanSubqueryExpression(query));
        }

        /// <summary>
        ///     Creates a "is (not) less than all" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression LessThanAll(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsLessThanAllSubqueryExpression(query));
        }

        /// <summary>
        ///     Creates a "is (not) less than or equal to" filter.
        /// </summary>
        /// <param name="value">
        ///     The value to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression LessThanOrEqualTo(object value)
        {
            return Fix(new IsLessThanOrEqualExpression(value));
        }

        /// <summary>
        ///     Creates a "is (not) less than or equal to" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression LessThanOrEqualTo(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsLessThanOrEqualToSubqueryExpression(query));
        }

        /// <summary>
        ///     Creates a "is (not) less than or equal to all" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression LessThanOrEqualToAll(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsLessThanOrEqualToAllSubqueryExpression(query));
        }

        /// <summary>
        ///     Creates a "is (not) less than or equal to some" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression LessThanOrEqualToSome(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsLessThanOrEqualToSomeSubqueryExpression(query));
        }

        /// <summary>
        ///     Creates a "is (not) less than some" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression LessThanSome(IDetachedImmutableFlowQuery query)
        {
            return Fix(new IsLessThanSomeSubqueryExpression(query));
        }

        /// <summary>
        ///     Creates a "is (not) like" filter.
        /// </summary>
        /// <param name="value">
        ///     The value to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression Like(object value)
        {
            return Fix(new IsLikeExpression(value));
        }

        /// <summary>
        ///     Creates a "is (not) null" filter.
        /// </summary>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public virtual IsExpression Null()
        {
            return Fix(new IsNullExpression());
        }

        /// <summary>
        ///     Sets the <see cref="IsExpression.Negated" /> flag on the provided <see cref="IsExpression" /> to the 
        ///     value of <see cref="IsNegated" />.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="IsExpression" /> filter to fix.
        /// </param>
        /// <typeparam name="T">
        ///     The <see cref="System.Type" /> of the <see cref="IsExpression" /> filter.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="T:T"/> filter.
        /// </returns>
        protected virtual T Fix<T>(T expression)
            where T : IsExpression
        {
            expression.Negated = IsNegated;

            return expression;
        }
    }
}