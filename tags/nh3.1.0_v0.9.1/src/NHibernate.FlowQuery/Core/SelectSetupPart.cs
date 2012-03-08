using System;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Helpers;

namespace NHibernate.FlowQuery.Core
{
    public class SelectSetupPart<TSource, TReturn> : ISelectSetupPart<TSource, TReturn>
        where TSource : class
    {
        #region Constructors (1)

        public SelectSetupPart(string forProperty, ISelectSetup<TSource, TReturn> setup)
        {
            if (string.IsNullOrEmpty(forProperty))
            {
                throw new ArgumentException("forProperty");
            }

            if (setup == null)
            {
                throw new ArgumentNullException("setup");
            }

            ForProperty = forProperty;
            Setup = setup;
        }

        #endregion Constructors

        #region Properties (2)

        private string ForProperty { get; set; }

        private ISelectSetup<TSource, TReturn> Setup { get; set; }

        #endregion Properties

        #region Methods (3)

        protected virtual ISelectSetup<TSource, TReturn> Use(string property)
        {
            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentException("property");
            }

            return Use(Projections.Property(property));
        }

        protected virtual ISelectSetup<TSource, TReturn> Use(IProjection projection)
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

        protected virtual ISelectSetup<TSource, TReturn> Use(Expression<Func<TSource, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            IProjection projection = ProjectionHelper.GetProjection(expression.Body, expression.Parameters[0].Name);

            return Use(projection);
        }

        #endregion Methods



        #region IDistinctSetupPart<TSource,TReturn> Members

        ISelectSetup<TSource, TReturn> ISelectSetupPart<TSource, TReturn>.Use(string property)
        {
            return Use(property);
        }

        ISelectSetup<TSource, TReturn> ISelectSetupPart<TSource, TReturn>.Use(IProjection projection)
        {
            return Use(projection);
        }

        ISelectSetup<TSource, TReturn> ISelectSetupPart<TSource, TReturn>.Use(Expression<Func<TSource, object>> expression)
        {
            return Use(expression);
        }

        #endregion
    }
}