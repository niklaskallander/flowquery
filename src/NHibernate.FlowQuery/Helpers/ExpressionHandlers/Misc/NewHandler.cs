namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.Misc
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles projection and construction of new expressions.
    /// </summary>
    public class NewHandler : AbstractHandler
    {
        /// <inheritdoc />
        public override bool CanHandleConstructionOf(Expression expression)
        {
            return expression.NodeType == ExpressionType.New;
        }

        /// <inheritdoc />
        public override bool CanHandleProjectionOf
            (
            Expression expression,
            HelperContext context
            )
        {
            return false;
        }

        /// <inheritdoc />
        public override int Construct
            (
            Expression expression,
            object[] arguments,
            out object value,
            out bool wasHandled
            )
        {
            var newExpression = (NewExpression)expression;

            int i = 0;

            var list = new List<object>();

            foreach (Expression argument in newExpression.Arguments)
            {
                object temp;

                i += ConstructionHelper.Invoke(argument, arguments.Skip(i).ToArray(), out temp);

                list.Add(temp);
            }

            value = newExpression.Constructor.Invoke(list.ToArray());

            wasHandled = true;

            return i;
        }

        /// <inheritdoc />
        public override IProjection Project
            (
            Expression expression,
            HelperContext context
            )
        {
            return null;
        }
    }
}