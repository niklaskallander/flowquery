namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Helpers;

    /// <summary>
    ///     A helper class used to combine multiple partial selections.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the source entity.
    /// </typeparam>
    /// <typeparam name="TDestination">
    ///     The <see cref="System.Type" /> of the selection.
    /// </typeparam>
    public class PartialSelection<TSource, TDestination> : IPartialSelection<TSource, TDestination>
        where TSource : class
    {
        /// <summary>
        ///     The partial selection builder.
        /// </summary>
        private readonly PartialSelectionBuilder<TSource, TDestination> _builder;

        /// <summary>
        ///     The partial selections.
        /// </summary>
        private readonly List<Expression<Func<TSource, TDestination>>> _expressions;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PartialSelection{TSource,TDestination}"/> class.
        /// </summary>
        /// <param name="builder">
        ///     The partial selection builder.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="builder" /> is null.
        /// </exception>
        public PartialSelection(PartialSelectionBuilder<TSource, TDestination> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            _builder = builder;

            _expressions = new List<Expression<Func<TSource, TDestination>>>();
        }

        /// <inheritdoc />
        public virtual int Count
        {
            get
            {
                return _expressions.Count;
            }
        }

        /// <inheritdoc />
        public virtual PartialSelection<TSource, TDestination> Add(Expression<Func<TSource, TDestination>> expression)
        {
            if (expression != null)
            {
                if (expression.Body.NodeType == ExpressionType.MemberInit
                    || (_expressions.Count == 0 && expression.Body.NodeType == ExpressionType.New))
                {
                    _expressions.Add(expression);
                }
            }

            return this;
        }

        /// <inheritdoc />
        public virtual Expression<Func<TSource, TDestination>> Compile()
        {
            if (Count > 0)
            {
                return ExpressionHelper.Combine(_expressions.ToArray());
            }

            return null;
        }

        /// <inheritdoc />
        public virtual FlowQuerySelection<TDestination> Select()
        {
            return _builder(this);
        }
    }
}