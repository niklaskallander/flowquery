using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core.Joins;
using NHibernate.FlowQuery.Core.Orders;
using NHibernate.FlowQuery.Expressions;
using NHibernate.FlowQuery.Helpers;
using NHibernate.SqlCommand;

namespace NHibernate.FlowQuery.Core.Implementors
{
    public abstract class FlowQueryImplementor<TSource, TFlowQuery> : IFlowQuery<TSource, TFlowQuery>, IFlowQuery
        where TSource : class
        where TFlowQuery : class, IFlowQuery<TSource, TFlowQuery>
    {
        protected internal FlowQueryImplementor(Func<System.Type, string, ICriteria> criteriaFactory, string alias = null, FlowQueryOptions options = null, IFlowQuery query = null)
        {
            Query = this as TFlowQuery;

            if (Query == null)
            {
                throw new ArgumentException("The provided TFlowQuery must the type of this instance", "TFlowQuery");
            }

            if (criteriaFactory == null)
            {
                throw new ArgumentNullException("criteriaFactory");
            }

            if (query != null)
            {
                Aliases = query.Aliases.ToDictionary(x => x.Key, x => x.Value);
                Criterions = query.Criterions.ToList();
                Joins = query.Joins.ToList();
                Orders = query.Orders.ToList();

                ResultsToSkip = query.ResultsToSkip;
                ResultsToTake = query.ResultsToTake;
            }
            else
            {
                Aliases = new Dictionary<string, string>();
                Criterions = new List<ICriterion>();
                Joins = new List<Join>();
                Orders = new List<OrderByStatement>();

                if (alias != null)
                {
                    Aliases.Add("entity.root.alias", alias);
                }
            }

            Alias = alias;

            CriteriaFactory = criteriaFactory;

            Options = options;

            Inner = new JoinBuilder<TSource, TFlowQuery>(this, Query, JoinType.InnerJoin);
            LeftOuter = new JoinBuilder<TSource, TFlowQuery>(this, Query, JoinType.LeftOuterJoin);
            RightOuter = new JoinBuilder<TSource, TFlowQuery>(this, Query, JoinType.RightOuterJoin);
            Full = new JoinBuilder<TSource, TFlowQuery>(this, Query, JoinType.FullJoin);

            Order = new OrderBuilder<TSource, TFlowQuery>(this, Query);
            Then = new OrderBuilder<TSource, TFlowQuery>(this, Query);
        }

        public virtual string Alias { get; private set; }

        public virtual FlowQueryOptions Options { get; private set; }

        public virtual Func<System.Type, string, ICriteria> CriteriaFactory { get; private set; }

        protected internal virtual TFlowQuery Query { get; private set; }

        public virtual TFlowQuery Where(params ICriterion[] criterions)
        {
            if (criterions == null)
            {
                throw new ArgumentNullException("criterions");
            }

            foreach (var criterion in criterions)
            {
                if (criterion != null)
                {
                    Criterions.Add(criterion);
                }
            }

            return Query;
        }

        public virtual TFlowQuery Where(string property, IsExpression expression)
        {
            ICriterion criterion = expression.Compile(property);

            if (expression.Negate)
            {
                criterion = Restrictions.Not(criterion);
            }

            return Where(criterion);
        }

        public virtual TFlowQuery Where(Expression<Func<TSource, bool>> expression)
        {
            return Where(RestrictionHelper.GetCriterion(expression, expression.Parameters[0].Name, Aliases));
        }

        public virtual TFlowQuery Where(Expression<Func<TSource, object>> property, IsExpression expression)
        {
            return Where(ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name), expression);
        }

        public virtual TFlowQuery Where(Expression<Func<TSource, WhereDelegate, bool>> expression)
        {
            return Where(RestrictionHelper.GetCriterion(expression, expression.Parameters[0].Name, Aliases));
        }

        public virtual TFlowQuery And(params ICriterion[] criterions)
        {
            return Where(criterions);
        }

        public virtual TFlowQuery And(string property, IsExpression expression)
        {
            return Where(property, expression);
        }

        public virtual TFlowQuery And(Expression<Func<TSource, bool>> expression)
        {
            return Where(expression);
        }

        public virtual TFlowQuery And(Expression<Func<TSource, object>> property, IsExpression expression)
        {
            return Where(property, expression);
        }

        public virtual TFlowQuery And(Expression<Func<TSource, WhereDelegate, bool>> expression)
        {
            return Where(expression);
        }

        public virtual TFlowQuery RestrictByExample(TSource exampleInstance, Action<IExampleWrapper<TSource>> example)
        {
            if (example == null)
            {
                throw new ArgumentNullException("example");
            }

            if (exampleInstance == null)
            {
                throw new ArgumentNullException("exampleInstance");
            }

            IExampleWrapper<TSource> wrapper = new ExampleWrapper<TSource>(Example.Create(exampleInstance));

            example(wrapper);

            return Where(wrapper.Example);
        }

        public virtual List<ICriterion> Criterions { get; private set; }

        public virtual IJoinBuilder<TSource, TFlowQuery> Inner { get; private set; }

        public virtual IJoinBuilder<TSource, TFlowQuery> LeftOuter { get; private set; }

        public virtual IJoinBuilder<TSource, TFlowQuery> RightOuter { get; private set; }

        public virtual IJoinBuilder<TSource, TFlowQuery> Full { get; private set; }

        public virtual Dictionary<string, string> Aliases { get; private set; }

        public virtual List<Join> Joins { get; private set; }

        public virtual IOrderBuilder<TSource, TFlowQuery> Order { get; private set; }

        public virtual IOrderBuilder<TSource, TFlowQuery> Then { get; private set; }

        public virtual List<OrderByStatement> Orders { get; private set; }

        public virtual TFlowQuery Limit(int limit)
        {
            return Take(limit);
        }

        public virtual TFlowQuery Limit(int limit, int offset)
        {
            return Take(limit).Skip(offset);
        }

        public virtual TFlowQuery Skip(int skip)
        {
            ResultsToSkip = skip;

            return Query;
        }

        public virtual TFlowQuery Take(int take)
        {
            ResultsToTake = take;

            return Query;
        }

        public virtual int? ResultsToSkip { get; private set; }

        public virtual int? ResultsToTake { get; private set; }
    }
}