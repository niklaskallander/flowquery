namespace NHibernate.FlowQuery
{
    using System;

    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.Implementations;

    /// <summary>
    ///     A utility class used for specifying additional filters to run on <see cref="ICriteria" /> instances created
    ///     by <see cref="NHibernate.FlowQuery" />. In other words: a means to do things on the <see cref="ICriteria" />
    ///     instance that <see cref="NHibernate.FlowQuery" /> might not support yet, only have partial support for, or 
    ///     have an issue with.
    /// </summary>
    public class FlowQueryOptions
    {
        /// <summary>
        ///     The default criteria builder.
        /// </summary>
        private static readonly ICriteriaBuilder DefaultBuilder;

        /// <summary>
        ///     The criteria builder.
        /// </summary>
        private static ICriteriaBuilder _builder;

        /// <summary>
        ///     The criteria filters.
        /// </summary>
        private Action<ICriteria> _options;

        /// <summary>
        /// Initializes static members of the <see cref="FlowQueryOptions"/> class.
        /// </summary>
        static FlowQueryOptions()
        {
            DefaultBuilder = new CriteriaBuilder();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FlowQueryOptions"/> class.
        /// </summary>
        public FlowQueryOptions()
        {
            _options = delegate { };
        }

        /// <summary>
        ///     Gets or sets the global criteria builder.
        /// </summary>
        /// <value>
        ///     The global criteria builder.
        /// </value>
        public static ICriteriaBuilder GlobalCriteriaBuilder
        {
            get
            {
                return _builder ?? DefaultBuilder;
            }

            set
            {
                _builder = value;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether errors found for an order by statement set using any of 
        ///     <see cref="IFlowQuery{TSource,TQuery}.OrderBy{TProjection}(string,bool)"/> and 
        ///     <see cref="IFlowQuery{TSource,TQuery}.OrderByDescending{TProjection}(string)"/> should be suppressed 
        ///     (Globally).
        /// </summary>
        /// <value>
        ///     A value indicating whether errors found for an order by statement set using any of 
        ///     <see cref="IFlowQuery{TSource,TQuery}.OrderBy{TProjection}(string,bool)"/> and 
        ///     <see cref="IFlowQuery{TSource,TQuery}.OrderByDescending{TProjection}(string)"/> should be suppressed 
        ///     (Globally).
        /// </value>
        /// <remarks>
        ///     The potential errors you can suppress are: a) the order by projection is not used
        ///     when executing the query. and b) the order by projection was made on a different
        ///     <see cref="Type"/> than what was used when executing the query.
        /// </remarks>
        public static bool GloballySuppressOrderByProjectionErrors { get; set; }

        /// <summary>
        ///     Gets or sets the criteria builder.
        /// </summary>
        /// <value>
        ///     The criteria builder.
        /// </value>
        public ICriteriaBuilder CriteriaBuilder { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether errors found for an order by statement set using any of 
        ///     <see cref="IFlowQuery{TSource,TQuery}.OrderBy{TProjection}(string,bool)" /> and 
        ///     <see cref="IFlowQuery{TSource,TQuery}.OrderByDescending{TProjection}(string)" /> should be suppressed.
        /// </summary>
        /// <value>
        ///     A value indicating whether errors found for an order by statement set using any of 
        ///     <see cref="IFlowQuery{TSource,TQuery}.OrderBy{TProjection}(string,bool)" /> and 
        ///     <see cref="IFlowQuery{TSource,TQuery}.OrderByDescending{TProjection}(string)" /> should be suppressed.
        /// </value>
        /// <remarks>
        ///     The potential errors you can suppress are: a) the order by projection is not used when executing the 
        ///     query. and b) the order by projection was made on a different <see cref="Type" /> than what was used 
        ///     when executing the query.
        /// </remarks>
        public virtual bool ShouldSuppressOrderByProjectionErrors { get; set; }

        /// <summary>
        ///     Add a <see cref="Action{ICriteria}" /> filter to this <see cref="FlowQueryOptions" /> instance.
        /// </summary>
        /// <param name="option">
        ///     The <see cref="Action{ICriteria}" /> filter to add.
        /// </param>
        /// <returns>
        ///     This <see cref="FlowQueryOptions" /> instance.
        /// </returns>
        public virtual FlowQueryOptions Add(Action<ICriteria> option)
        {
            if (option != null)
            {
                _options += option;
            }

            return this;
        }

        /// <summary>
        ///     Specify whether errors found for order by statements set using any of 
        ///     <see cref="IFlowQuery{TSource,TQuery}.OrderBy{TProjection}(string,bool)" /> and 
        ///     <see cref="IFlowQuery{TSource,TQuery}.OrderByDescending{TProjection}(string)" /> should be suppressed.
        /// </summary>
        /// <remarks>
        ///     The potential errors you can suppress are: a) the order by projection is not used when executing the 
        ///     query. and b) the order by projection was made on a different <see cref="Type" /> than what was used 
        ///     when executing the query.
        /// </remarks>
        /// <param name="suppress">
        ///     An optional value. If set to true (default) errors will be suppressed, otherwise specify false.
        /// </param>
        /// <returns>
        ///     The <see cref="FlowQueryOptions" /> instance. For chaining possibilities.
        /// </returns>
        public virtual FlowQueryOptions SuppressOrderByProjectionErrors(bool suppress = true)
        {
            ShouldSuppressOrderByProjectionErrors = suppress;

            return this;
        }

        /// <summary>
        ///     All added options will be run against the provided criteria object.
        /// </summary>
        /// <param name="criteria">
        ///     The <see cref="ICriteria" /> instance to filter using this <see cref="FlowQueryOptions" /> instance.
        /// </param>
        protected internal virtual void Use(ICriteria criteria)
        {
            if (criteria != null)
            {
                _options(criteria);
            }
        }
    }
}