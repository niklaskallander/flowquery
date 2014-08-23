namespace NHibernate.FlowQuery.Helpers
{
    using System.Linq.Expressions;

    using NHibernate.FlowQuery.Core.Implementations;

    /// <summary>
    ///     A utility class to replace and merge <see cref="ParameterExpression" />s in
    ///     <see cref="LambdaExpression" />s. Used by <see cref="ExpressionHelper.Combine{TSource, TDestination}" /> to
    ///     combine multiple <see cref="LambdaExpression" />s into one for
    ///     <see cref="PartialSelection{TSource,TDestination}" />s.
    /// </summary>
    public class ParameterReplaceVisitor : ExpressionVisitor
    {
        /// <summary>
        ///     The from parameter.
        /// </summary>
        private readonly ParameterExpression _from;

        /// <summary>
        ///     The to parameter.
        /// </summary>
        private readonly ParameterExpression _to;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParameterReplaceVisitor" /> class.
        /// </summary>
        /// <param name="from">
        ///     The from parameter.
        /// </param>
        /// <param name="to">
        ///     The to parameter.
        /// </param>
        public ParameterReplaceVisitor(ParameterExpression from, ParameterExpression to)
        {
            _from = from;
            _to = to;
        }

        /// <summary>
        ///     Visits the <see cref="ParameterExpression" />.
        /// </summary>
        /// <param name="node">
        ///     The expression to visit.
        /// </param>
        /// <returns>
        ///     The modified expression, if it or any sub-expression was modified; otherwise, returns the original
        ///     expression.
        /// </returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _from
                ? _to
                : base.VisitParameter(node);
        }
    }
}