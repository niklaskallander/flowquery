namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers;

    /// <summary>
    ///     An interface defining the functionality and structure of a class handling the projection part of
    ///     per-property-mapping of a <see cref="NHibernate.FlowQuery" /> query selection.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the source entity.
    /// </typeparam>
    /// <typeparam name="TDestination">
    ///     The <see cref="System.Type" /> of the selection.
    /// </typeparam>
    public class SelectSetupPart<TSource, TDestination> : ISelectSetupPart<TSource, TDestination>
        where TSource : class
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SelectSetupPart{TSource,TDestination}" /> class.
        /// </summary>
        /// <param name="forProperty">
        ///     The property name.
        /// </param>
        /// <param name="setup">
        ///     The <see cref="ISelectSetup{TSource, TDestination}" /> instance.
        /// </param>
        /// <param name="data">
        ///     The <see cref="QueryHelperData" /> instance.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="forProperty" /> is null or <see cref="string.Empty" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="setup" /> is null.
        /// </exception>
        public SelectSetupPart(string forProperty, ISelectSetup<TSource, TDestination> setup, QueryHelperData data)
        {
            if (string.IsNullOrEmpty(forProperty))
            {
                throw new ArgumentException("forProperty");
            }

            if (setup == null)
            {
                throw new ArgumentNullException("setup");
            }

            Data = data;

            ForProperty = forProperty;

            Setup = setup;
        }

        /// <summary>
        ///     Gets or sets the <see cref="QueryHelperData" /> instance.
        /// </summary>
        /// <value>
        ///     The <see cref="QueryHelperData" /> instance.
        /// </value>
        private QueryHelperData Data { get; set; }

        /// <summary>
        ///     Gets or sets the property name.
        /// </summary>
        /// <value>
        ///     The property name.
        /// </value>
        private string ForProperty { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="ISelectSetup{TSource, TDestination}" /> instance.
        /// </summary>
        /// <value>
        ///     The <see cref="ISelectSetup{TSource, TDestination}" /> instance.
        /// </value>
        private ISelectSetup<TSource, TDestination> Setup { get; set; }

        /// <inheritdoc />
        public virtual ISelectSetup<TSource, TDestination> Use(string property)
        {
            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentException("property");
            }

            return Use(Projections.Property(property));
        }

        /// <inheritdoc />
        public virtual ISelectSetup<TSource, TDestination> Use(IProjection projection)
        {
            if (projection == null)
            {
                throw new ArgumentNullException("projection");
            }

            Setup.ProjectionList.Add(projection, ForProperty);

            if (!Setup.Mappings.ContainsKey(ForProperty))
            {
                Setup.Mappings.Add(ForProperty, projection);
            }

            return Setup;
        }

        /// <inheritdoc />
        public virtual ISelectSetup<TSource, TDestination> Use<TProjection>
            (
            Expression<Func<TSource, TProjection>> expression
            )
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            IProjection projection = ProjectionHelper
                .GetProjection(expression, Data);

            return Use(projection);
        }
    }
}