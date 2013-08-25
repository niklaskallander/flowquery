using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Helpers;

namespace NHibernate.FlowQuery
{
    public static class ProjectionListExtensions
    {
        public static ProjectionList AddProperties(this ProjectionList list, params string[] properties)
        {
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

        public static ProjectionList AddProperties<TSource>(this ProjectionList list, Dictionary<string, string> aliases, params Expression<Func<TSource, object>>[] properties)
        {
            foreach (var property in properties)
            {
                if (property == null)
                {
                    throw new ArgumentNullException("property");
                }

                list.AddProperty(property, aliases);
            }

            return list;
        }

        public static ProjectionList AddProperty(this ProjectionList list, string property)
        {
            return list.AddProperty(property, property);
        }

        public static ProjectionList AddProperty(this ProjectionList list, string property, string alias)
        {
            return list.Add(Projections.Property(property), alias);
        }

        public static ProjectionList AddProperty<TSource, TProperty>(this ProjectionList list, Expression<Func<TSource, TProperty>> property, Dictionary<string, string> aliases)
        {
            IProjection projection = ProjectionHelper.GetProjection(property.Body, property.Parameters[0].Name, aliases);

            string alias = null;

            IPropertyProjection propertyProjection = projection as IPropertyProjection;

            if (propertyProjection != null)
            {
                alias = propertyProjection.PropertyName;
            }

            list.Add(projection, alias);

            return list;
        }
    }
}