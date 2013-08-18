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
        public SelectSetup(SelectionBuilder<TSource, TDestination> selectionBuilder, Dictionary<string, string> aliases)
        {
            if (selectionBuilder == null)
            {
                throw new ArgumentNullException("selectionBuilder");
            }

            Aliases = aliases;

            Mappings = new Dictionary<string, IProjection>();

            ProjectionList = Projections.ProjectionList();

            SelectionBuilder = selectionBuilder;
        }

        protected virtual Dictionary<string, string> Aliases { get; set; }

        protected virtual Dictionary<string, IProjection> Mappings { get; set; }

        protected virtual ProjectionList ProjectionList { get; private set; }

        protected virtual SelectionBuilder<TSource, TDestination> SelectionBuilder { get; set; }

        protected virtual ISelectSetupPart<TSource, TDestination> For(string property)
        {
            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentException("property");
            }

            return new SelectSetupPart<TSource, TDestination>(property, this, Aliases);
        }

        protected virtual ISelectSetupPart<TSource, TDestination> For(Expression<Func<TDestination, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            string property = ExpressionHelper.GetPropertyName(expression.Body, expression.Parameters[0].Name);

            return For(property);
        }

        protected virtual FlowQuerySelection<TDestination> Select()
        {
            if (ProjectionList.Length == 0)
            {
                throw new InvalidOperationException("No setup has been made");
            }

            return SelectionBuilder(this);
        }



        #region ISelectSetup<TSource, TDestination> Members

        Dictionary<string, IProjection> ISelectSetup<TSource, TDestination>.Mappings
        {
            get { return Mappings; }
        }

        ProjectionList ISelectSetup<TSource, TDestination>.ProjectionList
        {
            get { return ProjectionList; }
        }

        ISelectSetupPart<TSource, TDestination> ISelectSetup<TSource, TDestination>.For(string property)
        {
            return For(property);
        }

        ISelectSetupPart<TSource, TDestination> ISelectSetup<TSource, TDestination>.For(Expression<Func<TDestination, object>> expression)
        {
            return For(expression);
        }

        FlowQuerySelection<TDestination> ISelectSetup<TSource, TDestination>.Select()
        {
            return Select();
        }

        #endregion
    }
}