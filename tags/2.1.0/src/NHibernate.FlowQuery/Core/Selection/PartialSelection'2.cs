using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.FlowQuery.Helpers;

namespace NHibernate.FlowQuery.Core.Selection
{
    public class PartialSelection<TSource, TDestination>
        where TSource : class
    {
        private readonly List<Expression<Func<TSource, TDestination>>> _expressions;

        private readonly PartialSelectionBuilder<TSource, TDestination> _builder;

        public PartialSelection(PartialSelectionBuilder<TSource, TDestination> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            _builder = builder;

            _expressions = new List<Expression<Func<TSource, TDestination>>>();
        }

        public virtual PartialSelection<TSource, TDestination> Add(Expression<Func<TSource, TDestination>> expression)
        {
            if (expression != null)
            {
                if (expression.Body.NodeType == ExpressionType.MemberInit || (_expressions.Count == 0 && expression.Body.NodeType == ExpressionType.New))
                {
                    _expressions.Add(expression);
                }
            }

            return this;
        }

        public virtual int Count
        {
            get { return _expressions.Count; }
        }

        public virtual Expression<Func<TSource, TDestination>> Compile()
        {
            if (Count > 0)
            {
                return ExpressionHelper.Combine(_expressions.ToArray());
            }

            return null;
        }

        public virtual FlowQuerySelection<TDestination> Select()
        {
            return _builder(this);
        }
    }
}