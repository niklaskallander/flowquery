using System;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Helpers;

namespace NHibernate.FlowQuery.Core
{
    public class ExampleWrapper<TEntity> : IExampleWrapper<TEntity>
    {
		#region Constructors (1) 

        public ExampleWrapper(Example example)
        {
            if (example == null)
            {
                throw new ArgumentNullException("example");
            }

            Example = example;
        }

		#endregion Constructors 

		#region Properties (1) 

        public virtual Example Example { get; private set; }

		#endregion Properties 

		#region Methods (6) 

        protected virtual IExampleWrapper<TEntity> EnableLike()
        {
            Example.EnableLike();

            return this;
        }

        protected virtual IExampleWrapper<TEntity> EnableLike(MatchMode matchMode)
        {
            Example.EnableLike(matchMode);

            return this;
        }

        protected virtual IExampleWrapper<TEntity> ExcludeNulls()
        {
            Example.ExcludeNulls();

            return this;
        }

        protected virtual IExampleWrapper<TEntity> ExcludeProperty(string property)
        {
            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentException("property");
            }

            Example.ExcludeProperty(property);

            return this;
        }

        protected virtual IExampleWrapper<TEntity> ExcludeProperty(Expression<Func<TEntity, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            string property = ExpressionHelper.GetPropertyName(expression.Body, expression.Parameters[0].Name);

            return ExcludeProperty(property);
        }

        protected virtual IExampleWrapper<TEntity> ExcludeZeroes()
        {
            Example.ExcludeZeroes();

            return this;
        }

		#endregion Methods 



        #region IExampleWrapper<TEntity> Members

        Example IExampleWrapper<TEntity>.Example
        {
            get { return Example; }
        }

        IExampleWrapper<TEntity> IExampleWrapper<TEntity>.EnableLike()
        {
            return EnableLike();
        }

        IExampleWrapper<TEntity> IExampleWrapper<TEntity>.EnableLike(MatchMode matchMode)
        {
            return EnableLike(matchMode);
        }

        IExampleWrapper<TEntity> IExampleWrapper<TEntity>.ExcludeNulls()
        {
            return ExcludeNulls();
        }

        IExampleWrapper<TEntity> IExampleWrapper<TEntity>.ExcludeProperty(string property)
        {
            return ExcludeProperty(property);
        }

        IExampleWrapper<TEntity> IExampleWrapper<TEntity>.ExcludeProperty(Expression<Func<TEntity, object>> expression)
        {
            return ExcludeProperty(expression);
        }

        IExampleWrapper<TEntity> IExampleWrapper<TEntity>.ExcludeZeroes()
        {
            return ExcludeZeroes();
        }

        #endregion
    }
}