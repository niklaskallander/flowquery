namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Helpers;

    /// <summary>
    ///     A class representing a detached query (a.k.a. subquery).
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the underlying data for this query.
    /// </typeparam>
    public class DetachedFlowQuery<TSource>
        : MorphableFlowQueryBase<TSource, IDetachedFlowQuery<TSource>>, IDetachedFlowQuery<TSource>
        where TSource : class
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DetachedFlowQuery{TSource}" /> class.
        /// </summary>
        /// <param name="criteriaFactory">
        ///     The criteria factory.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <param name="query">
        ///     The query.
        /// </param>
        protected internal DetachedFlowQuery
            (
            Func<Type, string, ICriteria> criteriaFactory,
            string alias = null,
            FlowQueryOptions options = null,
            IMorphableFlowQuery query = null
            )
            : base(criteriaFactory, alias, options, query)
        {
        }

        /// <inheritdoc />
        public virtual DetachedCriteria Criteria
        {
            get
            {
                ICriteriaBuilder criteriaBuilder;

                if (Options != null && Options.CriteriaBuilder != null)
                {
                    criteriaBuilder = Options.CriteriaBuilder;
                }
                else
                {
                    criteriaBuilder = FlowQueryOptions.GlobalCriteriaBuilder;
                }

                return criteriaBuilder.Build<TSource>(this);
            }
        }

        /// <inheritdoc />
        public virtual IDetachedFlowQuery<TSource> Copy()
        {
            return new DetachedFlowQuery<TSource>(CriteriaFactory, Alias, Options, this);
        }

        /// <inheritdoc />
        public virtual IDetachedFlowQuery<TSource> Count()
        {
            Projection = Projections.RowCount();

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return this;
        }

        /// <inheritdoc />
        public virtual IDetachedFlowQuery<TSource> Count(string property)
        {
            Projection = IsDistinct
                ? Projections.CountDistinct(property)
                : Projections.Count(property);

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return this;
        }

        /// <inheritdoc />
        public virtual IDetachedFlowQuery<TSource> Count(IProjection projection)
        {
            Projection = Projections.Count(projection);

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return this;
        }

        /// <inheritdoc />
        public virtual IDetachedFlowQuery<TSource> Count(Expression<Func<TSource, object>> property)
        {
            string propertyName = ExpressionHelper.GetPropertyName(property.Body, property.Parameters[0].Name);

            return Count(propertyName);
        }

        /// <inheritdoc />
        public virtual IDetachedFlowQuery<TSource> CountLong()
        {
            Project(Projections.RowCountInt64());

            IsDistinct = false; // Selection Helper would wrap the projection in a distinct projection otherwise.

            return this;
        }

        /// <inheritdoc />
        public virtual IDetachedFlowQuery<TSource> Select(string property)
        {
            return Project(property);
        }

        /// <inheritdoc />
        public virtual IDetachedFlowQuery<TSource> Select(IProjection projection)
        {
            return Project(projection);
        }

        /// <inheritdoc />
        public virtual IDetachedFlowQuery<TSource> Select(Expression<Func<TSource, object>> expression)
        {
            return Project(expression);
        }

        /// <inheritdoc />
        public virtual IDetachedFlowQuery<TSource> SetRootAlias<TAlias>(Expression<Func<TAlias>> alias)
            where TAlias : class
        {
            if (alias == null)
            {
                throw new ArgumentNullException("alias");
            }

            string rootAliasName = ExpressionHelper.GetPropertyName(alias);

            Aliases.Add(rootAliasName, rootAliasName);

            return this;
        }

        /// <inheritdoc />
        public override IDelayedFlowQuery<TSource> Delayed()
        {
            if (CriteriaFactory == null)
            {
                throw new InvalidOperationException
                (
                    "This DetachedFlowQuery instance was not created from a ISession instance and cannot be " +
                        "transformed to a DelayedFlowQuery instance without a ISession being specified. Use the " +
                        "overload taking an ISession instance instead."
                );
            }

            return base.Delayed();
        }

        /// <inheritdoc />
        public IDelayedFlowQuery<TSource> Delayed(ISession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            return new DelayedFlowQuery<TSource>(session.CreateCriteria, Alias, Options, this);
        }

        /// <inheritdoc />
        public virtual IDelayedFlowQuery<TSource> Delayed(IStatelessSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            return new DelayedFlowQuery<TSource>(session.CreateCriteria, Alias, Options, this);
        }

        /// <inheritdoc />
        IDetachedFlowQuery<TSource> IDetachedFlowQuery<TSource>.Distinct()
        {
            return Distinct();
        }

        /// <inheritdoc />
        public override IImmediateFlowQuery<TSource> Immediate()
        {
            if (CriteriaFactory == null)
            {
                throw new InvalidOperationException
                (
                    "This DetachedFlowQuery instance was not created from a ISession instance and cannot be " +
                        "transformed to a ImmediateFlowQuery instance without a ISession being specified. Use the " +
                        "overload taking an ISession instance instead."
                );
            }

            return base.Immediate();
        }

        /// <inheritdoc />
        public virtual IImmediateFlowQuery<TSource> Immediate(ISession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            return new ImmediateFlowQuery<TSource>(session.CreateCriteria, Alias, Options, this);
        }

        /// <inheritdoc />
        public IImmediateFlowQuery<TSource> Immediate(IStatelessSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            return new ImmediateFlowQuery<TSource>(session.CreateCriteria, Alias, Options, this);
        }

        /// <inheritdoc />
        IDetachedFlowQuery<TSource> IDetachedFlowQuery<TSource>.Indistinct()
        {
            return Indistinct();
        }
    }
}