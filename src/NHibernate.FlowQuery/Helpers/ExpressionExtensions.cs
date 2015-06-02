namespace NHibernate.FlowQuery.Helpers
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    ///     Various extensions for <see cref="Expression" /> objects.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        ///     Flattens the given <see cref="BinaryExpression" /> and returns a list of all sub-expressions.
        /// </summary>
        /// <param name="expression">
        ///     The <see cref="BinaryExpression" /> to flatten out.
        /// </param>
        /// <returns>
        ///     A list of all sub-expressions.
        /// </returns>
        public static IEnumerable<Expression> Flatten(this BinaryExpression expression)
        {
            var expressions = new List<Expression>();

            if (expression.Left is BinaryExpression)
            {
                expressions.AddRange((expression.Left as BinaryExpression).Flatten());
            }
            else
            {
                expressions.Add(expression.Left);
            }

            if (expression.Right is BinaryExpression)
            {
                expressions.AddRange((expression.Right as BinaryExpression).Flatten());
            }
            else
            {
                expressions.Add(expression.Right);
            }

            return expressions;
        }
    }
}