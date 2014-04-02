using System;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Helpers;

namespace NHibernate.FlowQuery.Core
{
    public class ExampleWrapper<TEntity> : IExampleWrapper<TEntity>
    {
        public ExampleWrapper(Example example)
        {
            if (example == null)
            {
                throw new ArgumentNullException("example");
            }

            Example = example;
        }

        public virtual Example Example { get; private set; }

        public virtual IExampleWrapper<TEntity> EnableLike()
        {
            Example.EnableLike();

            return this;
        }

        public virtual IExampleWrapper<TEntity> EnableLike(MatchMode matchMode)
        {
            Example.EnableLike(matchMode);

            return this;
        }

        public virtual IExampleWrapper<TEntity> ExcludeNulls()
        {
            Example.ExcludeNulls();

            return this;
        }

        public virtual IExampleWrapper<TEntity> ExcludeProperty(string property)
        {
            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentException("property");
            }

            Example.ExcludeProperty(property);

            return this;
        }

        public virtual IExampleWrapper<TEntity> ExcludeProperty(Expression<Func<TEntity, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            string property = ExpressionHelper.GetPropertyName(expression.Body, expression.Parameters[0].Name);

            return ExcludeProperty(property);
        }

        public virtual IExampleWrapper<TEntity> ExcludeZeroes()
        {
            Example.ExcludeZeroes();

            return this;
        }
    }
}