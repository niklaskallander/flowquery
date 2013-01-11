﻿using NHibernate.Criterion;
using NHibernate.FlowQuery.Core;

namespace NHibernate.FlowQuery.Expressions.SubqueryExpressions
{
    public class IsLessThanSubqueryExpression<TSource> : SimpleIsExpression
    {
        #region Constructors (1)

        public IsLessThanSubqueryExpression(ISubFlowQuery<TSource> value)
            : base(value)
        { }

        #endregion Constructors

        #region Methods (1)

        public override ICriterion Compile(string property)
        {
            return Subqueries.PropertyLt(property, (Value as SubFlowQueryImpl<TSource>).Criteria);
        }

        #endregion Methods
    }
}