namespace NHibernate.FlowQuery
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers;

    /// <summary>
    ///     A helper class defining a set of extension methods useful for modifying <see cref="ProjectionList" />
    ///     instances.
    /// </summary>
    public static class ProjectionListExtensions
    {
        /// <summary>
        ///     Adds the specified properties to the projection list.
        /// </summary>
        /// <param name="list">
        ///     The projection list.
        /// </param>
        /// <param name="properties">
        ///     The properties.
        /// </param>
        /// <returns>
        ///     The <see cref="ProjectionList" /> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="list" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="properties" /> contain null-entries or <see cref="String.Empty" />-entries.
        /// </exception>
        public static ProjectionList AddProperties(this ProjectionList list, params string[] properties)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            foreach (string property in properties)
            {
                if (string.IsNullOrEmpty(property))
                {
                    throw new ArgumentException("property cannot be null and it cannot be empty");
                }

                list.AddProperty(property);
            }

            return list;
        }

        /// <summary>
        ///     Adds the specified properties to the projection list.
        /// </summary>
        /// <param name="list">
        ///     The projection list.
        /// </param>
        /// <param name="data">
        ///     A <see cref="QueryHelperData" /> instance.
        /// </param>
        /// <param name="properties">
        ///     The property expressions.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="ProjectionList" /> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="list" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="properties" /> contain null-entries.
        /// </exception>
        public static ProjectionList AddProperties<TSource>
            (
            this ProjectionList list,
            QueryHelperData data,
            params Expression<Func<TSource, object>>[] properties
            )
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            foreach (Expression<Func<TSource, object>> property in properties)
            {
                if (property == null)
                {
                    throw new ArgumentException("Properties contains null value", "properties");
                }

                list.AddProperty(property, data);
            }

            return list;
        }

        /// <summary>
        ///     Adds the specified property to the projection list.
        /// </summary>
        /// <param name="list">
        ///     The projection list.
        /// </param>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <returns>
        ///     The <see cref="ProjectionList" /> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="list" /> is null.
        /// </exception>
        public static ProjectionList AddProperty(this ProjectionList list, string property)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            return list.AddProperty(property, property);
        }

        /// <summary>
        ///     Adds the specified property to the projection list.
        /// </summary>
        /// <param name="list">
        ///     The projection list.
        /// </param>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <returns>
        ///     The <see cref="ProjectionList" /> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="list" /> is null.
        /// </exception>
        public static ProjectionList AddProperty(this ProjectionList list, string property, string alias)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            return list.Add(Projections.Property(property), alias);
        }

        /// <summary>
        ///     Adds the specified property to the projection list.
        /// </summary>
        /// <param name="list">
        ///     The projection list.
        /// </param>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <param name="data">
        ///     A <see cref="QueryHelperData" /> instance.
        /// </param>
        /// <typeparam name="TSource">
        ///     The <see cref="System.Type" /> of the source entity.
        /// </typeparam>
        /// <typeparam name="TProperty">
        ///     The <see cref="System.Type" /> of the property.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="ProjectionList" /> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="list" /> is null.
        /// </exception>
        public static ProjectionList AddProperty<TSource, TProperty>
            (
            this ProjectionList list,
            Expression<Func<TSource, TProperty>> property,
            QueryHelperData data
            )
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            IProjection projection = ProjectionHelper.GetProjection(property, data);

            string alias = null;

            var propertyProjection = projection as IPropertyProjection;

            if (propertyProjection != null)
            {
                alias = propertyProjection.PropertyName;
            }

            list.Add(projection, alias);

            return list;
        }
    }
}