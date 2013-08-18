﻿using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsGreaterThanSomeSubqueryExpression : SubqueryIsExpressionBase
    {
        public IsGreaterThanSomeSubqueryExpression(IDetachedImmutableFlowQuery value)
            : base(value)
        { }

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyGtSome(property, Query.Criteria);
        }
    }
}