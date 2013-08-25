using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Helpers;

namespace NHibernate.FlowQuery.Core.Selection
{
    public class PartialSelection<TSource, TDestination>
        where TSource : class
    {
        private List<Expression<Func<TSource, TDestination>>> m_Expressions;

        private PartialSelectionBuilder<TSource, TDestination> m_Builder;

        public PartialSelection(PartialSelectionBuilder<TSource, TDestination> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            m_Builder = builder;

            m_Expressions = new List<Expression<Func<TSource, TDestination>>>();
        }

        public virtual PartialSelection<TSource, TDestination> Add(Expression<Func<TSource, TDestination>> expression)
        {
            if (expression != null)
            {
                if (expression.Body.NodeType == ExpressionType.MemberInit || (m_Expressions.Count == 0 && expression.Body.NodeType == ExpressionType.New))
                {
                    m_Expressions.Add(expression);
                }
            }

            return this;
        }

        public virtual int Count
        {
            get { return m_Expressions.Count; }
        }

        public virtual Expression<Func<TSource, TDestination>> Compile()
        {
            if (Count > 0)
            {
                return ExpressionHelper.Combine(m_Expressions.ToArray());
            }

            return null;
        }

        public virtual FlowQuerySelection<TDestination> Select()
        {
            return m_Builder(this);
        }
    }
}