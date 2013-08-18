using System;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core.Implementors;
using NHibernate.FlowQuery.Helpers;

namespace NHibernate.FlowQuery.Core.Orders
{
    public class OrderBuilder<TSource, TFlowQuery> : IOrderBuilder<TSource, TFlowQuery>
        where TSource : class
        where TFlowQuery : class, IFlowQuery<TSource, TFlowQuery>
    {
        protected internal virtual FlowQueryImplementor<TSource, TFlowQuery> Implementor { get; set; }

        protected internal virtual TFlowQuery Query { get; set; }

        protected internal OrderBuilder(FlowQueryImplementor<TSource, TFlowQuery> implementor, TFlowQuery query)
        {
            if (implementor == null)
            {
                throw new ArgumentNullException("implementor");
            }

            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            if (!object.ReferenceEquals(implementor, query))
            {
                throw new ArgumentException("implementor and query must be the same refence", "query");
            }

            Implementor = implementor;

            Query = query;
        }

        public virtual TFlowQuery By(string property, bool ascending = true)
        {
            return ascending
                ? ByAscending(property)
                : ByDescending(property);
        }

        public virtual TFlowQuery By(IProjection projection, bool ascending = true)
        {
            return ascending
                ? ByAscending(projection)
                : ByDescending(projection);
        }

        public virtual TFlowQuery By(Expression<Func<TSource, object>> property, bool ascending = true)
        {
            return ascending
                ? ByAscending(property)
                : ByDescending(property);
        }

        public virtual TFlowQuery By<TProjection>(Expression<Func<TProjection, object>> projection, bool ascending = true)
        {
            return ascending
                ? ByAscending<TProjection>(projection)
                : ByDescending<TProjection>(projection);
        }

        public virtual TFlowQuery By<TProjection>(string property, bool ascending = true)
        {
            return ascending
                ? ByAscending<TProjection>(property)
                : ByDescending<TProjection>(property);
        }

        public virtual TFlowQuery ByAscending(string property)
        {
            Implementor.Orders.Add(new OrderByStatement()
            {
                IsBasedOnSource = true,
                Order = Order.Asc(property)
            });

            return Query;
        }

        public virtual TFlowQuery ByAscending(IProjection projection)
        {
            Implementor.Orders.Add(new OrderByStatement()
            {
                IsBasedOnSource = true,
                Order = Order.Asc(projection)
            });

            return Query;
        }

        public virtual TFlowQuery ByAscending(Expression<Func<TSource, object>> property)
        {
            return ByAscending(ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name));
        }

        public virtual TFlowQuery ByAscending<TProjection>(string property)
        {
            Implementor.Orders.Add(new OrderByStatement()
            {
                IsBasedOnSource = false,
                OrderAscending = true,
                ProjectionSourceType = typeof(TProjection),
                Property = property
            });

            return Query;
        }

        public virtual TFlowQuery ByAscending<TProjection>(Expression<Func<TProjection, object>> projection)
        {
            return ByAscending<TProjection>(ExpressionHelper.GetPropertyName(projection.Body, projection.Parameters[0].Name));
        }

        public virtual TFlowQuery ByDescending(string property)
        {
            Implementor.Orders.Add(new OrderByStatement()
            {
                IsBasedOnSource = true,
                Order = Order.Desc(property)
            });

            return Query;
        }

        public virtual TFlowQuery ByDescending(IProjection projection)
        {
            Implementor.Orders.Add(new OrderByStatement()
            {
                IsBasedOnSource = true,
                Order = Order.Desc(projection)
            });

            return Query;
        }

        public virtual TFlowQuery ByDescending(Expression<Func<TSource, object>> property)
        {
            return ByDescending(ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name));
        }

        public virtual TFlowQuery ByDescending<TProjection>(string property)
        {
            Implementor.Orders.Add(new OrderByStatement()
            {
                IsBasedOnSource = false,
                OrderAscending = false,
                ProjectionSourceType = typeof(TProjection),
                Property = property
            });

            return Query;
        }

        public virtual TFlowQuery ByDescending<TProjection>(Expression<Func<TProjection, object>> projection)
        {
            return ByDescending<TProjection>(ExpressionHelper.GetPropertyName(projection.Body, projection.Parameters[0].Name));
        }
    }
}