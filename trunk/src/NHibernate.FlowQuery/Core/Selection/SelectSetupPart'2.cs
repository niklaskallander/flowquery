using System;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Helpers;

namespace NHibernate.FlowQuery.Core.Selection
{
    public class SelectSetupPart<TSource, TDestination> : ISelectSetupPart<TSource, TDestination>
        where TSource : class
    {
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

        private string ForProperty { get; set; }

        private QueryHelperData Data { get; set; }

        private ISelectSetup<TSource, TDestination> Setup { get; set; }

        public virtual ISelectSetup<TSource, TDestination> Use(string property)
        {
            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentException("property");
            }

            return Use(Projections.Property(property));
        }

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

        public virtual ISelectSetup<TSource, TDestination> Use<TProjection>(Expression<Func<TSource, TProjection>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            IProjection projection = ProjectionHelper.GetProjection(expression.Body, expression.Parameters[0].Name, Data);

            return Use(projection);
        }
    }
}