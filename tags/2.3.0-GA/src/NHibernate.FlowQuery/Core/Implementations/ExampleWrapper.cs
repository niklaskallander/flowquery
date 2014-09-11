namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers;

    /// <summary>
    ///     An example wrapper used by <see cref="IFlowQuery{TSource,TQuery}.RestrictByExample" /> to filter a query
    ///     based on properties specified on an underlying entity.
    /// </summary>
    /// <typeparam name="TEntity">
    ///     The type of the underlying entity on which the filter should apply.
    /// </typeparam>
    public class ExampleWrapper<TEntity> : IExampleWrapper<TEntity>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ExampleWrapper{TEntity}" /> class.
        /// </summary>
        /// <param name="example">
        ///     The wrapped <see cref="Example" /> object.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="example" /> is null.
        /// </exception>
        public ExampleWrapper(Example example)
        {
            if (example == null)
            {
                throw new ArgumentNullException("example");
            }

            Example = example;
        }

        /// <inheritdoc />
        public virtual Example Example { get; private set; }

        /// <inheritdoc />
        public virtual IExampleWrapper<TEntity> EnableLike()
        {
            Example.EnableLike();

            return this;
        }

        /// <inheritdoc />
        public virtual IExampleWrapper<TEntity> EnableLike(MatchMode matchMode)
        {
            Example.EnableLike(matchMode);

            return this;
        }

        /// <inheritdoc />
        public virtual IExampleWrapper<TEntity> ExcludeNulls()
        {
            Example.ExcludeNulls();

            return this;
        }

        /// <inheritdoc />
        public virtual IExampleWrapper<TEntity> ExcludeProperty(string property)
        {
            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentException("property");
            }

            Example.ExcludeProperty(property);

            return this;
        }

        /// <inheritdoc />
        public virtual IExampleWrapper<TEntity> ExcludeProperty(Expression<Func<TEntity, object>> property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            string propertyName = ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name);

            return ExcludeProperty(propertyName);
        }

        /// <inheritdoc />
        public virtual IExampleWrapper<TEntity> ExcludeZeroes()
        {
            Example.ExcludeZeroes();

            return this;
        }
    }
}