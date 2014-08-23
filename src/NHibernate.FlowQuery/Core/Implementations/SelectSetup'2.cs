namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers;

    /// <summary>
    ///     An interface defining the functionality and structure of a class handling per-property-mapping of a
    ///     <see cref="NHibernate.FlowQuery" /> query selection.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the source entity.
    /// </typeparam>
    /// <typeparam name="TDestination">
    ///     The <see cref="System.Type" /> of the selection.
    /// </typeparam>
    public class SelectSetup<TSource, TDestination> : ISelectSetup<TSource, TDestination>
        where TSource : class
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SelectSetup{TSource,TDestination}" /> class.
        /// </summary>
        /// <param name="selectionBuilder">
        ///     The selection builder.
        /// </param>
        /// <param name="data">
        ///     The <see cref="QueryHelperData" /> instance.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="selectionBuilder" /> is null.
        /// </exception>
        public SelectSetup(SelectionBuilder<TSource, TDestination> selectionBuilder, QueryHelperData data)
        {
            if (selectionBuilder == null)
            {
                throw new ArgumentNullException("selectionBuilder");
            }

            Data = data;

            Mappings = new Dictionary<string, IProjection>();

            ProjectionList = Projections.ProjectionList();

            SelectionBuilder = selectionBuilder;
        }

        /// <inheritdoc />
        public Dictionary<string, IProjection> Mappings { get; private set; }

        /// <inheritdoc />
        public ProjectionList ProjectionList { get; private set; }

        /// <summary>
        ///     Gets the <see cref="QueryHelperData" /> instance.
        /// </summary>
        /// <value>
        ///     The <see cref="QueryHelperData" /> instance.
        /// </value>
        protected QueryHelperData Data { get; private set; }

        /// <summary>
        ///     Gets the selection builder.
        /// </summary>
        /// <value>
        ///     The selection builder.
        /// </value>
        protected SelectionBuilder<TSource, TDestination> SelectionBuilder { get; private set; }

        /// <inheritdoc />
        public virtual ISelectSetupPart<TSource, TDestination> For(string property)
        {
            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentException("property");
            }

            return new SelectSetupPart<TSource, TDestination>(property, this, Data);
        }

        /// <inheritdoc />
        public virtual ISelectSetupPart<TSource, TDestination> For(Expression<Func<TDestination, object>> property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            string propertyName = ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name);

            return For(propertyName);
        }

        /// <inheritdoc />
        public virtual FlowQuerySelection<TDestination> Select()
        {
            if (ProjectionList.Length == 0)
            {
                throw new InvalidOperationException("No setup has been made");
            }

            return SelectionBuilder(this);
        }
    }
}