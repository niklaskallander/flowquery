using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Expressions;
using NHibernate.FlowQuery.Helpers;
using NHibernate.Transform;

namespace NHibernate.FlowQuery.Core
{
    public partial class FlowQueryImpl<TSource> : IFlowQuery<TSource>
        where TSource : class
    {
        #region Constructors (4)

        protected internal FlowQueryImpl(ISession session, FlowQueryOptions options)
            : this(session, null, options)
        { }

        protected internal FlowQueryImpl(ISession session, Expression<Func<TSource>> alias, FlowQueryOptions options)
            : this(session, alias)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            options.Use(Criteria);
        }

        protected internal FlowQueryImpl(ISession session)
            : this(session, (Expression<Func<TSource>>)null)
        { }

        protected internal FlowQueryImpl(ISession session, Expression<Func<TSource>> alias)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            Session = session;

            string aliasName = null;

            if (alias != null)
            {
                aliasName = ExpressionHelper.GetPropertyName(alias);

                Criteria = Session.CreateCriteria<TSource>(aliasName);
            }
            else
            {
                Criteria = Session.CreateCriteria<TSource>();
            }

            PropertyAliases = new Dictionary<string, string>();

            if (aliasName != null)
            {
                PropertyAliases.Add("entity.root.alias", aliasName);
            }

            OrderByStatements = new List<OrderByStatement>();
        }

        #endregion Constructors

        #region Properties (3)

        protected virtual ICriteria Criteria { get; private set; }

        protected virtual Dictionary<string, string> PropertyAliases { get; private set; }

        protected virtual ISession Session { get; private set; }

        protected virtual List<OrderByStatement> OrderByStatements { get; private set; }

        #endregion Properties



        #region Methods

        #region Select Helpers

        protected virtual void AddOrders<TReturn>(Dictionary<string, IProjection> mappings)
        {
            if (OrderByStatements.Count > 0)
            {
                foreach (var statement in OrderByStatements)
                {
                    if (statement.IsBasedOnSource)
                    {
                        Criteria.AddOrder(statement.Order);
                    }
                    else if (mappings != null)
                    {
                        if (statement.ProjectionSourceType != typeof(TReturn))
                        {
                            throw new InvalidOperationException("unable to order by a projection property on a projection of a type other than the returned one.");
                        }

                        if (!mappings.ContainsKey(statement.Property))
                        {
                            throw new InvalidOperationException("unable to order by a projection property that is not projected into by the query");
                        }

                        Criteria.AddOrder(new Order(mappings[statement.Property], statement.OrderAscending));
                    }
                }
            }
        }

        protected virtual FlowQuerySelection<TReturn> SelectWithConstruction<TReturn>(Expression<Func<TSource, TReturn>> expression, bool distinct)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            Dictionary<string, IProjection> mappings = new Dictionary<string, IProjection>();

            var list = ProjectionHelper.GetProjectionListForExpression(expression.Body, expression.Parameters[0].Name, PropertyAliases, ref mappings);
            if (list == null || list.Length == 0)
            {
                throw new NotSupportedException("The provided expression contains unsupported features please revise your code.");
            }

            AddOrders<TReturn>(mappings);

            IProjection projection = distinct ? Projections.Distinct(list) : list;

            var selection = ConstructionHelper.GetListByExpression<TReturn>(expression.Body, projection, Criteria);

            return CreateSelection(selection);
        }

        protected virtual FlowQuerySelection<TReturn> CreateSelection<TReturn>(IEnumerable<TReturn> selection)
        {
            return new FlowQuerySelection<TReturn>(selection);
        }

        protected virtual FlowQuerySelection<TReturn> SelectBase<TReturn>(Dictionary<string, IProjection> mappings)
        {
            AddOrders<TReturn>(mappings);

            return CreateSelection(Criteria.List<TReturn>());
        }

        protected virtual FlowQuerySelection<TReturn> SelectBase<TReturn>()
        {
            return SelectBase<TReturn>(null);
        }

        #endregion

        #region Select

        protected virtual FlowQuerySelection<TSource> Select()
        {
            return SelectBase<TSource>();
        }

        protected virtual ISelectSetup<TSource, TReturn> Select<TReturn>()
        {
            return new SelectSetup<TSource, TReturn>(this, PropertyAliases);
        }

        protected virtual FlowQuerySelection<TReturn> Select<TReturn>(ISelectSetup<TSource, TReturn> setup)
        {
            Criteria
                .SetProjection(setup.ProjectionList)
                .SetResultTransformer(Transformers.AliasToBean<TReturn>());

            return SelectBase<TReturn>(setup.Mappings);
        }

        protected virtual FlowQuerySelection<TSource> Select(params string[] properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }

            return Select
            (
                Projections
                    .ProjectionList()
                        .AddProperties(properties)
            );
        }

        protected virtual FlowQuerySelection<TSource> Select(IProjection projection)
        {
            return Select<TSource>(projection);
        }

        protected virtual FlowQuerySelection<TReturn> Select<TReturn>(IProjection projection)
        {
            if (projection == null)
            {
                throw new ArgumentNullException("projecton");
            }

            Criteria
                 .SetProjection(projection)
                 .SetResultTransformer(Transformers.AliasToBean<TReturn>());

            return SelectBase<TReturn>();
        }

        protected virtual FlowQuerySelection<TReturn> Select<TReturn>(PropertyProjection projection)
        {
            if (projection == null)
            {
                throw new ArgumentNullException("projection");
            }

            Criteria
                .SetProjection(projection);

            return SelectBase<TReturn>();
        }

        protected virtual FlowQuerySelection<object> Select(PropertyProjection projection)
        {
            return Select<object>(projection);
        }

        protected virtual FlowQuerySelection<TSource> Select(params Expression<Func<TSource, object>>[] properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }

            return Select
            (
                Projections
                    .ProjectionList()
                        .AddProperties(PropertyAliases, properties)
            );
        }

        protected virtual FlowQuerySelection<TReturn> Select<TReturn>(Expression<Func<TSource, TReturn>> expression)
        {
            return SelectWithConstruction(expression, false);
        }

        #endregion

        #region SelectDistinct

        protected virtual ISelectSetup<TSource, TReturn> SelectDistinct<TReturn>()
        {
            return new SelectDistinctSetup<TSource, TReturn>(this, PropertyAliases);
        }

        protected virtual ISelectSetup<TSource, TSource> SelectDistinct()
        {
            return SelectDistinct<TSource>();
        }

        protected virtual FlowQuerySelection<TReturn> SelectDistinct<TReturn>(ISelectSetup<TSource, TReturn> setup)
        {
            Criteria
                .SetProjection(Projections.Distinct(setup.ProjectionList))
                .SetResultTransformer(Transformers.AliasToBean<TReturn>());

            return SelectBase<TReturn>(setup.Mappings);
        }

        protected virtual FlowQuerySelection<TSource> SelectDistinct(IProjection projection)
        {
            if (projection == null)
            {
                throw new ArgumentNullException("projection");
            }
            return Select(Projections.Distinct(projection));
        }

        protected virtual FlowQuerySelection<TSource> SelectDistinct(params string[] properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }

            return SelectDistinct
            (
                Projections
                    .ProjectionList()
                        .AddProperties(properties)
            );
        }

        protected virtual FlowQuerySelection<TReturn> SelectDistinct<TReturn>(IProjection projection)
        {
            if (projection == null)
            {
                throw new ArgumentNullException("projection");
            }

            return Select<TReturn>(Projections.Distinct(projection));
        }

        protected virtual FlowQuerySelection<object> SelectDistinct(PropertyProjection projection)
        {
            return SelectDistinct<object>(projection);
        }

        protected virtual FlowQuerySelection<TReturn> SelectDistinct<TReturn>(PropertyProjection projection)
        {
            if (projection == null)
            {
                throw new ArgumentNullException("projection");
            }

            var selection = Criteria
                .SetProjection(Projections.Distinct(projection));

            return SelectBase<TReturn>();
        }

        protected virtual FlowQuerySelection<TSource> SelectDistinct(params Expression<Func<TSource, object>>[] expressions)
        {
            if (expressions == null)
            {
                throw new ArgumentNullException("expressions");
            }

            return SelectDistinct
            (
                Projections
                    .ProjectionList()
                        .AddProperties(PropertyAliases, expressions)
            );
        }

        protected virtual FlowQuerySelection<TReturn> SelectDistinct<TReturn>(Expression<Func<TSource, TReturn>> expression)
        {
            return SelectWithConstruction(expression, true);
        }

        #endregion

        #region SelectDictionary

        protected virtual Dictionary<TKey, TValue> SelectDictionary<TKey, TValue>(Expression<Func<TSource, TKey>> key, Expression<Func<TSource, TValue>> value)
        {
            Pair<TKey, TValue>[] pairs = Select<Pair<TKey, TValue>>()
                .For(x => x.Key).Use(key)
                .For(x => x.Value).Use(value)
                .Select();

            Dictionary<TKey, TValue> temp = new Dictionary<TKey, TValue>();

            foreach (Pair<TKey, TValue> pair in pairs)
            {
                temp.Add(pair.Key, pair.Value);
            }

            return temp;
        }

        #endregion

        #region Count

        protected virtual int Count()
        {
            return Count<int>(Projections.RowCount(), false);
        }

        protected virtual int Count(string property)
        {
            return Count<int>(property);
        }

        protected virtual int Count(IProjection projection)
        {
            return Count<int>(projection);
        }

        protected virtual TReturn Count<TReturn>(string property)
        {
            return Count<TReturn>(Projections.Property(property));
        }

        protected virtual TReturn Count<TReturn>(IProjection projection)
        {
            return Count<TReturn>(projection, true);
        }

        protected virtual TReturn Count<TReturn>(IProjection projection, bool wrap)
        {
            if (wrap)
            {
                projection = Projections.Count(projection);
            }

            return Criteria
                .SetProjection(projection)
                .UniqueResult<TReturn>();
        }

        protected virtual int Count(Expression<Func<TSource, object>> property)
        {
            return Count<int>(property);
        }

        protected virtual TReturn Count<TReturn>(Expression<Func<TSource, object>> property)
        {
            return Count<TReturn>(ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name));
        }

        protected virtual int CountDistinct(string property)
        {
            return CountDistinct<int>(property);
        }

        protected virtual TReturn CountDistinct<TReturn>(string property)
        {
            return Criteria
                .SetProjection(Projections.CountDistinct(property))
                .UniqueResult<TReturn>();
        }

        protected virtual int CountDistinct(Expression<Func<TSource, object>> property)
        {
            return CountDistinct<int>(property);
        }

        protected virtual TReturn CountDistinct<TReturn>(Expression<Func<TSource, object>> property)
        {
            return CountDistinct<TReturn>(ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name));
        }

        protected virtual long CountLong()
        {
            return Count<long>(Projections.RowCountInt64(), false);
        }

        #endregion

        #region Where, And

        protected virtual IFlowQuery<TSource> Where(params ICriterion[] criterions)
        {
            if (criterions == null)
            {
                throw new ArgumentNullException();
            }

            foreach (var criterion in criterions)
            {
                if (criterion == null)
                {
                    throw new ArgumentNullException("the list of criterions contains null values");
                }
                Criteria.Add(criterion);
            }
            return this;
        }

        protected virtual IFlowQuery<TSource> Where(Expression<Func<TSource, bool>> expression)
        {
            return Where(RestrictionHelper.GetCriterion(expression, expression.Parameters[0].Name, PropertyAliases));
        }

        protected virtual IFlowQuery<TSource> Where(string property, IsExpression expression)
        {
            ICriterion criterion = expression.Compile(property);
            if (expression.Negate)
            {
                criterion = Restrictions.Not(criterion);
            }
            return Where(criterion);
        }

        protected virtual IFlowQuery<TSource> Where(Expression<Func<TSource, object>> property, IsExpression expression)
        {
            return Where(ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name), expression);
        }

        protected virtual IFlowQuery<TSource> Where(Expression<Func<TSource, WhereDelegate, bool>> expression)
        {
            return Where(RestrictionHelper.GetCriterion(expression, expression.Parameters[0].Name, PropertyAliases));
        }

        protected virtual IFlowQuery<TSource> And(params ICriterion[] criterions)
        {
            return Where(criterions);
        }

        protected virtual IFlowQuery<TSource> And(Expression<Func<TSource, bool>> expression)
        {
            return Where(expression);
        }

        protected virtual IFlowQuery<TSource> And(string property, IsExpression expression)
        {
            return Where(property, expression);
        }

        protected virtual IFlowQuery<TSource> And(Expression<Func<TSource, object>> property, IsExpression expression)
        {
            return Where(property, expression);
        }

        protected virtual IFlowQuery<TSource> And(Expression<Func<TSource, WhereDelegate, bool>> expression)
        {
            return Where(expression);
        }

        protected virtual IFlowQuery<TSource> RestrictWithExample(TSource exampleInstance, Func<IExampleWrapper<TSource>, IExampleWrapper<TSource>> exampleConfigurer)
        {
            var wrapper = exampleConfigurer(new ExampleWrapper<TSource>(Example.Create(exampleInstance)));

            return Where(wrapper.Example);
        }

        #endregion

        #region Skip, Take, Limit

        protected virtual IFlowQuery<TSource> Skip(int skip)
        {
            Criteria.SetFirstResult(skip);

            return this;
        }

        protected virtual IFlowQuery<TSource> Take(int take)
        {
            return Limit(take);
        }

        protected virtual IFlowQuery<TSource> Limit(int limit)
        {
            Criteria.SetMaxResults(limit);

            return this;
        }

        protected virtual IFlowQuery<TSource> Limit(int limit, int offset)
        {
            Criteria.SetFirstResult(offset);

            return Limit(limit);
        }

        #endregion

        #region OrderBy

        protected virtual IFlowQuery<TSource> OrderBy(string property)
        {
            OrderByStatements.Add(new OrderByStatement()
            {
                IsBasedOnSource = true,
                Order = Order.Asc(property)
            });

            return this;
        }

        protected virtual IFlowQuery<TSource> OrderBy(IProjection projection)
        {
            OrderByStatements.Add(new OrderByStatement()
            {
                IsBasedOnSource = true,
                Order = Order.Asc(projection)
            });

            return this;
        }

        protected virtual IFlowQuery<TSource> OrderBy(Expression<Func<TSource, object>> property)
        {
            return OrderBy(ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name));
        }

        protected virtual IFlowQuery<TSource> OrderBy<TProjection>(string property)
        {
            OrderByStatements.Add(new OrderByStatement()
            {
                IsBasedOnSource = false,
                OrderAscending = true,
                ProjectionSourceType = typeof(TProjection),
                Property = property
            });

            return this;
        }

        protected virtual IFlowQuery<TSource> OrderBy<TProjection>(Expression<Func<TProjection, object>> projectionProperty)
        {
            return OrderBy<TProjection>(ExpressionHelper.GetPropertyName(projectionProperty.Body, projectionProperty.Parameters[0].Name));
        }

        protected virtual IFlowQuery<TSource> OrderByDescending(string property)
        {
            OrderByStatements.Add(new OrderByStatement()
            {
                IsBasedOnSource = true,
                Order = Order.Desc(property)
            });

            return this;
        }

        protected virtual IFlowQuery<TSource> OrderByDescending(IProjection projection)
        {
            OrderByStatements.Add(new OrderByStatement()
            {
                IsBasedOnSource = true,
                Order = Order.Desc(projection)
            });

            return this;
        }

        protected virtual IFlowQuery<TSource> OrderByDescending(Expression<Func<TSource, object>> property)
        {
            return OrderByDescending(ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name));
        }

        protected virtual IFlowQuery<TSource> OrderByDescending<TProjection>(string property)
        {
            OrderByStatements.Add(new OrderByStatement()
            {
                IsBasedOnSource = false,
                OrderAscending = false,
                ProjectionSourceType = typeof(TProjection),
                Property = property
            });

            return this;
        }

        protected virtual IFlowQuery<TSource> OrderByDescending<TProjection>(Expression<Func<TProjection, object>> projectionProperty)
        {
            return OrderByDescending<TProjection>(ExpressionHelper.GetPropertyName(projectionProperty.Body, projectionProperty.Parameters[0].Name));
        }

        #endregion

        #endregion

        #region IFlowQuery<TSource> Members

        ICriteria IFlowQuery<TSource>.Criteria
        {
            get { return Criteria; }
        }

        FlowQuerySelection<TSource> IFlowQuery<TSource>.Select()
        {
            return Select();
        }

        ISelectSetup<TSource, TReturn> IFlowQuery<TSource>.Select<TReturn>()
        {
            return Select<TReturn>();
        }

        FlowQuerySelection<TSource> IFlowQuery<TSource>.Select(params string[] properties)
        {
            return Select(properties);
        }

        FlowQuerySelection<TSource> IFlowQuery<TSource>.Select(IProjection projection)
        {
            return Select(projection);
        }

        FlowQuerySelection<object> IFlowQuery<TSource>.Select(PropertyProjection projection)
        {
            return Select(projection);
        }

        FlowQuerySelection<TReturn> IFlowQuery<TSource>.Select<TReturn>(PropertyProjection projection)
        {
            return Select<TReturn>(projection);
        }

        FlowQuerySelection<TReturn> IFlowQuery<TSource>.Select<TReturn>(IProjection projection)
        {
            return Select<TReturn>(projection);
        }

        FlowQuerySelection<TSource> IFlowQuery<TSource>.Select(params Expression<Func<TSource, object>>[] expressions)
        {
            return Select(expressions);
        }

        FlowQuerySelection<TReturn> IFlowQuery<TSource>.Select<TReturn>(Expression<Func<TSource, TReturn>> expression)
        {
            return Select<TReturn>(expression);
        }

        Dictionary<TKey, TValue> IFlowQuery<TSource>.SelectDictionary<TKey, TValue>(Expression<Func<TSource, TKey>> key, Expression<Func<TSource, TValue>> value)
        {
            return SelectDictionary(key, value);
        }

        FlowQuerySelection<TSource> IFlowQuery<TSource>.SelectDistinct(IProjection projection)
        {
            return SelectDistinct(projection);
        }

        FlowQuerySelection<TSource> IFlowQuery<TSource>.SelectDistinct(params string[] properties)
        {
            return SelectDistinct(properties);
        }

        FlowQuerySelection<TReturn> IFlowQuery<TSource>.SelectDistinct<TReturn>(IProjection projection)
        {
            return SelectDistinct<TReturn>(projection);
        }

        FlowQuerySelection<TSource> IFlowQuery<TSource>.SelectDistinct(params Expression<Func<TSource, object>>[] expressions)
        {
            return SelectDistinct(expressions);
        }

        FlowQuerySelection<TReturn> IFlowQuery<TSource>.SelectDistinct<TReturn>(Expression<Func<TSource, TReturn>> expression)
        {
            return SelectDistinct<TReturn>(expression);
        }

        ISelectSetup<TSource, TReturn> IFlowQuery<TSource>.SelectDistinct<TReturn>()
        {
            return SelectDistinct<TReturn>();
        }

        ISelectSetup<TSource, TSource> IFlowQuery<TSource>.SelectDistinct()
        {
            return SelectDistinct();
        }

        int IFlowQuery<TSource>.Count()
        {
            return Count();
        }

        long IFlowQuery<TSource>.CountLong()
        {
            return CountLong();
        }

        int IFlowQuery<TSource>.Count(string property)
        {
            return Count(property);
        }

        int IFlowQuery<TSource>.Count(Expression<Func<TSource, object>> property)
        {
            return Count(property);
        }

        int IFlowQuery<TSource>.Count(IProjection projection)
        {
            return Count(projection);
        }

        int IFlowQuery<TSource>.CountDistinct(string property)
        {
            return CountDistinct(property);
        }

        int IFlowQuery<TSource>.CountDistinct(Expression<Func<TSource, object>> property)
        {
            return CountDistinct(property);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.OrderBy(string property)
        {
            return OrderBy(property);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.OrderBy(Expression<Func<TSource, object>> property)
        {
            return OrderBy(property);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.OrderBy(IProjection projection)
        {
            return OrderBy(projection);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.OrderBy<TProjection>(Expression<Func<TProjection, object>> projectionProperty)
        {
            return OrderBy(projectionProperty);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.OrderBy<TProjection>(string property)
        {
            return OrderBy<TProjection>(property);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.OrderByDescending(string property)
        {
            return OrderByDescending(property);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.OrderByDescending(Expression<Func<TSource, object>> property)
        {
            return OrderByDescending(property);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.OrderByDescending(IProjection projection)
        {
            return OrderByDescending(projection);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.OrderByDescending<TProjection>(Expression<Func<TProjection, object>> projectionProperty)
        {
            return OrderByDescending(projectionProperty);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.OrderByDescending<TProjection>(string property)
        {
            return OrderByDescending<TProjection>(property);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Limit(int limit)
        {
            return Limit(limit);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Limit(int limit, int offset)
        {
            return Limit(limit, offset);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Skip(int skip)
        {
            return Skip(skip);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Take(int take)
        {
            return Take(take);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.RestrictWithExample(TSource exampleInstance, Func<IExampleWrapper<TSource>, IExampleWrapper<TSource>> exampleConfigurer)
        {
            return RestrictWithExample(exampleInstance, exampleConfigurer);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Where(Expression<Func<TSource, object>> property, IsExpression expression)
        {
            return Where(property, expression);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Where(string property, IsExpression expression)
        {
            return Where(property, expression);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Where(Expression<Func<TSource, bool>> expression)
        {
            return Where(expression);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Where(params ICriterion[] criterions)
        {
            return Where(criterions);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.Where(Expression<Func<TSource, WhereDelegate, bool>> expression)
        {
            return Where(expression);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.And(Expression<Func<TSource, object>> property, IsExpression expression)
        {
            return And(property, expression);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.And(string property, IsExpression expression)
        {
            return And(property, expression);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.And(Expression<Func<TSource, bool>> expression)
        {
            return And(expression);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.And(params ICriterion[] criterions)
        {
            return And(criterions);
        }

        IFlowQuery<TSource> IFlowQuery<TSource>.And(Expression<Func<TSource, WhereDelegate, bool>> expression)
        {
            return And(expression);
        }

        FlowQuerySelection<object> IFlowQuery<TSource>.SelectDistinct(PropertyProjection projection)
        {
            return SelectDistinct(projection);
        }

        FlowQuerySelection<TReturn> IFlowQuery<TSource>.SelectDistinct<TReturn>(PropertyProjection projection)
        {
            return SelectDistinct<TReturn>(projection);
        }

        FlowQuerySelection<TReturn> IFlowQuery<TSource>.Select<TReturn>(ISelectSetup<TSource, TReturn> setup)
        {
            return Select(setup);
        }

        FlowQuerySelection<TReturn> IFlowQuery<TSource>.SelectDistinct<TReturn>(ISelectSetup<TSource, TReturn> setup)
        {
            return SelectDistinct(setup);
        }

        #endregion
    }
}