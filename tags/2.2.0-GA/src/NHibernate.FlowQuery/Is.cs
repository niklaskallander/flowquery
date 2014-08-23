namespace NHibernate.FlowQuery
{
    using System.Collections;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Expressions;

    using IsEmptyExpression = NHibernate.FlowQuery.Expressions.IsEmptyExpression;

    /// <summary>
    ///     A static helper class defining a set of utilities to create filters used in 
    ///     <see cref="NHibernate.FlowQuery" /> queries.
    /// </summary>
    public static class Is
    {
        /// <summary>
        ///     The negative <see cref="IsHelper" /> instance.
        /// </summary>
        private static readonly IsHelper Negative;

        /// <summary>
        ///     The positive <see cref="IsHelper" /> instance.
        /// </summary>
        private static readonly IsHelper Positive;

        /// <summary>
        ///     Initializes static members of the <see cref="Is" /> class.
        /// </summary>
        static Is()
        {
            Positive = new IsHelper(false);
            Negative = new IsHelper(true);
        }

        /// <summary>
        ///     Gets a <see cref="IsHelper" /> instance that can be used to specify negated query filters.
        /// </summary>
        /// <value>
        ///     A <see cref="IsHelper" /> instance that can be used to specify negated query filters.
        /// </value>
        public static IsHelper Not
        {
            get
            {
                return Negative;
            }
        }

        /// <summary>
        ///     Creates a "is between" filter.
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
        public static IsExpression Between(object lowValue, object highValue)
        {
            return Positive.Between(lowValue, highValue);
        }

        /// <summary>
        ///     Creates an "is empty" filter. Useful to filter on a sub-query that shouldn't yield any results, or to 
        ///     filter on an association path pointing to a collection that should be empty.
        /// </summary>
        /// <returns>
        ///     The <see cref="IsEmptyExpression"/> filter.
        /// </returns>
        public static IsEmptyExpression Empty()
        {
            return Positive.Empty();
        }

        /// <summary>
        ///     Creates an "is equal to" filter.
        /// </summary>
        /// <param name="value">
        ///     The value to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression EqualTo(object value)
        {
            return Positive.EqualTo(value);
        }

        /// <summary>
        ///     Creates an "is equal to" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression EqualTo(IDetachedImmutableFlowQuery query)
        {
            return Positive.EqualTo(query);
        }

        /// <summary>
        ///     Creates an "is equal to all" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression EqualToAll(IDetachedImmutableFlowQuery query)
        {
            return Positive.EqualToAll(query);
        }

        /// <summary>
        ///     Creates a "is greater than" filter.
        /// </summary>
        /// <param name="value">
        ///     The value to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression GreaterThan(object value)
        {
            return Positive.GreaterThan(value);
        }

        /// <summary>
        ///     Creates a "is greater than" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression GreaterThan(IDetachedImmutableFlowQuery query)
        {
            return Positive.GreaterThan(query);
        }

        /// <summary>
        ///     Creates a "is greater than all" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression GreaterThanAll(IDetachedImmutableFlowQuery query)
        {
            return Positive.GreaterThanAll(query);
        }

        /// <summary>
        ///     Creates a "is greater than or equal to" filter.
        /// </summary>
        /// <param name="value">
        ///     The value to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression GreaterThanOrEqualTo(object value)
        {
            return Positive.GreaterThanOrEqualTo(value);
        }

        /// <summary>
        ///     Creates a "is greater than or equal to" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression GreaterThanOrEqualTo(IDetachedImmutableFlowQuery query)
        {
            return Positive.GreaterThanOrEqualTo(query);
        }

        /// <summary>
        ///     Creates a "is greater than or equal to all" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression GreaterThanOrEqualToAll(IDetachedImmutableFlowQuery query)
        {
            return Positive.GreaterThanOrEqualToAll(query);
        }

        /// <summary>
        ///     Creates a "is greater than or equal to some" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression GreaterThanOrEqualToSome(IDetachedImmutableFlowQuery query)
        {
            return Positive.GreaterThanOrEqualToSome(query);
        }

        /// <summary>
        ///     Creates a "is greater than some" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression GreaterThanSome(IDetachedImmutableFlowQuery query)
        {
            return Positive.GreaterThanSome(query);
        }

        /// <summary>
        ///     Creates a "is in" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the results to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression In(DetachedCriteria query)
        {
            return In(query.DetachedFlowQuery());
        }

        /// <summary>
        ///     Creates a "is in" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the results to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression In(IDetachedImmutableFlowQuery query)
        {
            return Positive.In(query);
        }

        /// <summary>
        ///     Creates a "is in" filter.
        /// </summary>
        /// <param name="values">
        ///     The values to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression In(params object[] values)
        {
            return Positive.In(values);
        }

        /// <summary>
        ///     Creates a "is in" filter.
        /// </summary>
        /// <param name="values">
        ///     The values to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression In(IEnumerable values)
        {
            return Positive.In(values);
        }

        /// <summary>
        ///     Creates a "less than" filter.
        /// </summary>
        /// <param name="value">
        ///     The value to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression LessThan(object value)
        {
            return Positive.LessThan(value);
        }

        /// <summary>
        ///     Creates a "is less than" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression LessThan(IDetachedImmutableFlowQuery query)
        {
            return Positive.LessThan(query);
        }

        /// <summary>
        ///     Creates a "is less than all" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression LessThanAll(IDetachedImmutableFlowQuery query)
        {
            return Positive.LessThanAll(query);
        }

        /// <summary>
        ///     Creates a "is less than or equal to" filter.
        /// </summary>
        /// <param name="value">
        ///     The value to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression LessThanOrEqualTo(object value)
        {
            return Positive.LessThanOrEqualTo(value);
        }

        /// <summary>
        ///     Creates a "is less than or equal to" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression LessThanOrEqualTo(IDetachedImmutableFlowQuery query)
        {
            return Positive.LessThanOrEqualTo(query);
        }

        /// <summary>
        ///     Creates a "is less than or equal to all" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression LessThanOrEqualToAll(IDetachedImmutableFlowQuery query)
        {
            return Positive.LessThanOrEqualToAll(query);
        }

        /// <summary>
        ///     Creates a "is less than or equal to some" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression LessThanOrEqualToSome(IDetachedImmutableFlowQuery query)
        {
            return Positive.LessThanOrEqualToSome(query);
        }

        /// <summary>
        ///     Creates a "is less than some" filter.
        /// </summary>
        /// <param name="query">
        ///     The sub-query yielding the result to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression LessThanSome(IDetachedImmutableFlowQuery query)
        {
            return Positive.LessThanSome(query);
        }

        /// <summary>
        ///     Creates a "is like" filter.
        /// </summary>
        /// <param name="value">
        ///     The value to match.
        /// </param>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression Like(object value)
        {
            return Positive.Like(value);
        }

        /// <summary>
        ///     Creates a "is null" filter.
        /// </summary>
        /// <returns>
        ///     The <see cref="IsExpression"/> filter.
        /// </returns>
        public static IsExpression Null()
        {
            return Positive.Null();
        }
    }
}