using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Helpers;

namespace NHibernate.FlowQuery.Core.Selection
{
    public class SelectSetup<TSource, TDestination> : ISelectSetup<TSource, TDestination>
        where TSource : class
    {
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

        protected QueryHelperData Data { get; private set; }

        public Dictionary<string, IProjection> Mappings { get; private set; }

        public ProjectionList ProjectionList { get; private set; }

        protected SelectionBuilder<TSource, TDestination> SelectionBuilder { get; private set; }

        public virtual ISelectSetupPart<TSource, TDestination> For(string property)
        {
            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentException("property");
            }

            return new SelectSetupPart<TSource, TDestination>(property, this, Data);
        }

        public virtual ISelectSetupPart<TSource, TDestination> For(Expression<Func<TDestination, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            string property = ExpressionHelper.GetPropertyName(expression.Body, expression.Parameters[0].Name);

            return For(property);
        }

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