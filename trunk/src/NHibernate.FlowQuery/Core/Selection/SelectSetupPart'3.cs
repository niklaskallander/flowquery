using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Helpers;

namespace NHibernate.FlowQuery.Core.Selection
{
    public class SelectSetupPart<TSource, TDestination> : ISelectSetupPart<TSource, TDestination>
        where TSource : class
    {
        #region Constructors (1)

        public SelectSetupPart(string forProperty, ISelectSetup<TSource, TDestination> setup, Dictionary<string, string> aliases)
        {
            if (string.IsNullOrEmpty(forProperty))
            {
                throw new ArgumentException("forProperty");
            }

            if (setup == null)
            {
                throw new ArgumentNullException("setup");
            }

            Aliases = aliases;

            ForProperty = forProperty;

            Setup = setup;
        }

        #endregion Constructors

        #region Properties (3)

        private string ForProperty { get; set; }

        private Dictionary<string, string> Aliases { get; set; }

        private ISelectSetup<TSource, TDestination> Setup { get; set; }

        #endregion Properties

        #region Methods (4)

        protected virtual ISelectSetup<TSource, TDestination> Use(string property)
        {
            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentException("property");
            }

            return Use(Projections.Property(property));
        }

        protected virtual ISelectSetup<TSource, TDestination> Use(IProjection projection)
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

        protected virtual ISelectSetup<TSource, TDestination> Use<TProjection>(Expression<Func<TSource, TProjection>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            IProjection projection = ProjectionHelper.GetProjection(expression.Body, expression.Parameters[0].Name, Aliases);

            return Use(projection);
        }

        #endregion Methods



        #region IDistinctSetupPart<TSource, TDestination> Members

        ISelectSetup<TSource, TDestination> ISelectSetupPart<TSource, TDestination>.Use(string property)
        {
            return Use(property);
        }

        ISelectSetup<TSource, TDestination> ISelectSetupPart<TSource, TDestination>.Use(IProjection projection)
        {
            return Use(projection);
        }

        ISelectSetup<TSource, TDestination> ISelectSetupPart<TSource, TDestination>.Use<TProjection>(Expression<Func<TSource, TProjection>> expression)
        {
            return Use(expression);
        }

        #endregion
    }
}