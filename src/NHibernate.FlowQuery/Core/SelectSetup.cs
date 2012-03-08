using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Helpers;
namespace NHibernate.FlowQuery.Core
{
    public class SelectSetup<TSource, TReturn> : ISelectSetup<TSource, TReturn>
        where TSource : class
    {
        #region Constructors (1)

        public SelectSetup(IFlowQuery<TSource> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            Query = query;

            ProjectionList = Projections.ProjectionList();

            Mappings = new Dictionary<string, IProjection>();
        }

        #endregion Constructors

        #region Properties (3)

        protected virtual Dictionary<string, IProjection> Mappings { get; private set; }

        protected virtual ProjectionList ProjectionList { get; private set; }

        protected virtual IFlowQuery<TSource> Query { get; private set; }

        #endregion Properties

        #region Methods (3)

        protected virtual ISelectSetupPart<TSource, TReturn> For(string property)
        {
            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentException("property");
            }

            return new SelectSetupPart<TSource, TReturn>(property, this);
        }

        protected virtual ISelectSetupPart<TSource, TReturn> For(Expression<Func<TReturn, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            string property = ExpressionHelper.GetPropertyName(expression.Body, expression.Parameters[0].Name);

            return For(property);
        }

        protected virtual FlowQuerySelection<TReturn> Select()
        {
            if (ProjectionList.Length == 0)
            {
                throw new InvalidOperationException("No setup has been made");
            }

            return Query.Select<TReturn>(this);
        }

        #endregion Methods



        #region IDistinctSetup<TSource,TReturn> Members

        Dictionary<string, IProjection> ISelectSetup<TSource, TReturn>.Mappings
        {
            get { return Mappings; }
        }

        ProjectionList ISelectSetup<TSource, TReturn>.ProjectionList
        {
            get { return ProjectionList; }
        }

        ISelectSetupPart<TSource, TReturn> ISelectSetup<TSource, TReturn>.For(string property)
        {
            return For(property);
        }

        ISelectSetupPart<TSource, TReturn> ISelectSetup<TSource, TReturn>.For(Expression<Func<TReturn, object>> expression)
        {
            return For(expression);
        }

        FlowQuerySelection<TReturn> ISelectSetup<TSource, TReturn>.Select()
        {
            return Select();
        }

        #endregion
    }
}