namespace NHibernate.FlowQuery.Helpers.ExpressionHandlers
{
    using NHibernate.Criterion;

    using Expression = System.Linq.Expressions.Expression;

    /// <summary>
    ///     A simple base class for expression handlers that removes the need to duplicate construction parts in a lot
    ///     of sub-classes.
    /// </summary>
    public abstract class AbstractHandler : IExpressionHandler
    {
        /// <inheritdoc />
        public virtual bool CanHandleConstructionOf(Expression expression)
        {
            return false;
        }

        /// <inheritdoc />
        public abstract bool CanHandleProjectionOf
            (
            Expression expression,
            HelperContext context
            );

        /// <inheritdoc />
        public virtual int Construct
            (
            Expression expression,
            object[] arguments,
            out object value,
            out bool wasHandled
            )
        {
            value = null;
            wasHandled = false;

            return 0;
        }

        /// <inheritdoc />
        public abstract IProjection Project
            (
            Expression expression,
            HelperContext context
            );
    }
}