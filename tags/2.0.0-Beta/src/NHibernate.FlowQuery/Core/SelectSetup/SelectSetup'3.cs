using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Helpers;

namespace NHibernate.FlowQuery.Core.SelectSetup
{
    public delegate FlowQuerySelection<TReturn> SelectionBuilder<TSource, TReturn>(ISelectSetup<TSource, TReturn> selectSetup)
        where TSource : class;

    public class SelectSetup<TSource, TReturn> : ISelectSetup<TSource, TReturn>
        where TSource : class
    {
        #region Constructors (1)

        public SelectSetup(SelectionBuilder<TSource, TReturn> selectionBuilder, Dictionary<string, string> aliases)
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

        #endregion Constructors

        #region Properties (3)


        protected virtual Dictionary<string, string> Aliases { get; set; }

        protected virtual Dictionary<string, IProjection> Mappings { get; set; }

        protected virtual ProjectionList ProjectionList { get; private set; }

        protected virtual SelectionBuilder<TSource, TReturn> SelectionBuilder { get; set; }

        #endregion Properties

        #region Methods (3)

        protected virtual ISelectSetupPart<TSource, TReturn> For(string property)
        {
            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentException("property");
            }

            return new SelectSetupPart<TSource, TReturn>(property, this, Aliases);
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

            return SelectionBuilder(this);
        }

        #endregion Methods



        #region ISelectSetup<TSource, TReturn> Members

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