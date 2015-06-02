namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers.MethodCalls
{
    using System.Linq.Expressions;

    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     Handles <see cref="MethodCallExpression" /> expressions representing calls to
    ///     <see cref="FlowQueryHelper.Project{TIn,TOut}" />.
    /// </summary>
    public class ProjectHandler : AbstractMethodCallHandler
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProjectHandler" /> class.
        /// </summary>
        public ProjectHandler()
            : base(supportedMethodNames: "Project")
        {
        }

        /// <inheritdoc />
        protected override bool CanHandleConstruction
        {
            get
            {
                return true;
            }
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
            var methodCall = expression as MethodCallExpression;

            if (methodCall != null)
            {
                var lambda = ExpressionHelper.GetValue<LambdaExpression>(methodCall.Arguments[1]);

                if (lambda != null)
                {
                    wasHandled = true;

                    return ConstructionHelper.Invoke(lambda, arguments, out value);
                }
            }

            return base.Construct(expression, arguments, out value, out wasHandled);
        }

        /// <inheritdoc />
        protected override IProjection ProjectCore
            (
            MethodCallExpression expression,
            Expression subExpression,
            IProjection projection,
            HelperContext context
            )
        {
            var fromExpression = ExpressionHelper.GetValue<LambdaExpression>(expression.Arguments[1]);

            if (fromExpression == null)
            {
                return null;
            }

            string tempRoot = context.RootAlias;

            context.RootAlias = fromExpression.Parameters[0].Name;

            if (context.Data.Aliases.ContainsValue(context.RootAlias))
            {
                // if the parameter to the mapping expression is the same as a joined alias we do not want it to be 
                // treated as the "expected root" as it would cause properties in the map to be resolved from the query
                // root entity, when the map might actually be for a joined alias.
                context.RootAlias = "null";
            }

            //// TODO: Might want to check if (original) rootName != alias (e.g. expression.Arguments[0])
            //// if that is the case we might have to change the parameter name in the mapping expression so that the
            //// projections can be properly resolved.
            IProjection result = ProjectionHelper
                .GetProjection(fromExpression, context);

            context.RootAlias = tempRoot;

            return result;
        }
    }
}