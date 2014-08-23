namespace NHibernate.FlowQuery.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core;
    using NHibernate.FlowQuery.Core.CustomProjections;
    using NHibernate.FlowQuery.Core.Structures;
    using NHibernate.Transform;

    /// <summary>
    ///     A helper class containing information used to create <see cref="ICriteria" /> and
    ///     <see cref="DetachedCriteria" /> instances used to build the final <see cref="FlowQuerySelection{T}" />
    ///     instances.
    /// </summary>
    public class QuerySelection : IQueryableFlowQuery
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QuerySelection" /> class.
        /// </summary>
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="query" /> is null.
        /// </exception>
        protected QuerySelection(IQueryableFlowQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            Alias = query.Alias;
            Aliases = query.Aliases.ToDictionary(x => x.Key, x => x.Value);
            CacheMode = query.CacheMode;
            CacheRegion = query.CacheRegion;
            CommentValue = query.CommentValue;
            Constructor = query.Constructor;
            CriteriaFactory = query.CriteriaFactory;
            Criterions = query.Criterions.ToList();
            Data = query.Data;
            Fetches = query.Fetches.ToList();
            FetchSizeValue = query.FetchSizeValue;
            GroupBys = query.GroupBys.ToList();
            IsCacheable = query.IsCacheable;
            IsDelayed = query.IsDelayed;
            IsDistinct = query.IsDistinct;
            IsReadOnly = query.IsReadOnly;
            Joins = query.Joins.ToList();
            Locks = query.Locks.ToList();
            Mappings = query.Mappings == null
                ? null
                : query.Mappings.ToDictionary(x => x.Key, x => x.Value);
            Options = query.Options;
            Orders = query.Orders.ToList();
            Projection = query.Projection;
            ResultsToSkip = query.ResultsToSkip;
            ResultsToTake = query.ResultsToTake;
            ResultTransformer = query.ResultTransformer;
            TimeoutValue = query.TimeoutValue;
        }

        /// <summary>
        ///     Gets the root entity alias for the query.
        /// </summary>
        /// <value>
        ///     The root entity alias for the query.
        /// </value>
        public virtual string Alias { get; private set; }

        /// <summary>
        ///     Gets all the aliases for the query.
        /// </summary>
        /// <value>
        ///     The all the aliases for the query.
        /// </value>
        public virtual Dictionary<string, string> Aliases { get; private set; }

        /// <summary>
        ///     Gets the cache mode for the query (if any).
        /// </summary>
        /// <value>
        ///     The cache mode for the query (if any).
        /// </value>
        public CacheMode? CacheMode { get; private set; }

        /// <summary>
        ///     Gets the cache region for the query (if any).
        /// </summary>
        /// <value>
        ///     The cache region for the query (if any).
        /// </value>
        public string CacheRegion { get; private set; }

        /// <summary>
        ///     Gets the comment for the query (if any).
        /// </summary>
        /// <value>
        ///     The comment for the query (if any).
        /// </value>
        public virtual string CommentValue { get; private set; }

        /// <summary>
        ///     Gets the constructor for the query (if any).
        /// </summary>
        /// <value>
        ///     The constructor for the query (if any).
        /// </value>
        public virtual LambdaExpression Constructor { get; private set; }

        /// <summary>
        ///     Gets the criteria factory for the query (if any).
        /// </summary>
        /// <value>
        ///     The criteria factory for the query (if any).
        /// </value>
        public virtual Func<Type, string, ICriteria> CriteriaFactory { get; private set; }

        /// <summary>
        ///     Gets the criterions for the query (if any).
        /// </summary>
        /// <value>
        ///     The criterions for the query (if any).
        /// </value>
        public virtual List<ICriterion> Criterions { get; private set; }

        /// <summary>
        ///     Gets the <see cref="QueryHelperData" /> info.
        /// </summary>
        /// <value>
        ///     The <see cref="QueryHelperData" /> info.
        /// </value>
        public QueryHelperData Data { get; private set; }

        /// <summary>
        ///     Gets the fetch size value for the query.
        /// </summary>
        /// <value>
        ///     The fetch size value for the query.
        /// </value>
        public int FetchSizeValue { get; private set; }

        /// <summary>
        ///     Gets the fetches for the query (if any).
        /// </summary>
        /// <value>
        ///     The fetches for the query (if any).
        /// </value>
        public virtual List<Fetch> Fetches { get; private set; }

        /// <summary>
        ///     Gets the group by statements for the query (if any).
        /// </summary>
        /// <value>
        ///     The group by statements for the query (if any).
        /// </value>
        public virtual List<FqGroupByProjection> GroupBys { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the query is cache-able.
        /// </summary>
        /// <value>
        ///     A value indicating whether the query is cache-able.
        /// </value>
        public bool IsCacheable { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the query is delayed.
        /// </summary>
        /// <value>
        ///     A value indicating whether the query is delayed.
        /// </value>
        public virtual bool IsDelayed { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the query is distinct.
        /// </summary>
        /// <value>
        ///     A value indicating whether the query is distinct.
        /// </value>
        public virtual bool IsDistinct { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the query is read only.
        /// </summary>
        /// <value>
        ///     A value indicating whether the query is read only.
        /// </value>
        public virtual bool? IsReadOnly { get; private set; }

        /// <summary>
        ///     Gets the joins for the query (if any).
        /// </summary>
        /// <value>
        ///     The joins for the query (if any).
        /// </value>
        public virtual List<Join> Joins { get; private set; }

        /// <summary>
        ///     Gets the locks for the query (if any).
        /// </summary>
        /// <value>
        ///     The locks for the query (if any).
        /// </value>
        public virtual List<Lock> Locks { get; private set; }

        /// <summary>
        ///     Gets the mappings for the query (if any).
        /// </summary>
        /// <value>
        ///     The mappings for the query (if any).
        /// </value>
        public virtual Dictionary<string, IProjection> Mappings { get; private set; }

        /// <summary>
        ///     Gets the options for the query (if any).
        /// </summary>
        /// <value>
        ///     The options for the query (if any).
        /// </value>
        public virtual FlowQueryOptions Options { get; private set; }

        /// <summary>
        ///     Gets the orders for the query (if any).
        /// </summary>
        /// <value>
        ///     The orders for the query (if any).
        /// </value>
        public virtual List<OrderByStatement> Orders { get; private set; }

        /// <summary>
        ///     Gets the projection for the query (if any).
        /// </summary>
        /// <value>
        ///     The projection for the query (if any).
        /// </value>
        public virtual IProjection Projection { get; private set; }

        /// <summary>
        ///     Gets the result transformer for the query (if any).
        /// </summary>
        /// <value>
        ///     The result transformer for the query (if any).
        /// </value>
        public virtual IResultTransformer ResultTransformer { get; private set; }

        /// <summary>
        ///     Gets the results to skip for the query (if set).
        /// </summary>
        /// <value>
        ///     The results to skip for the query (if set).
        /// </value>
        public virtual int? ResultsToSkip { get; private set; }

        /// <summary>
        ///     Gets the results to take for the query (if set).
        /// </summary>
        /// <value>
        ///     The results to take for the query (if set).
        /// </value>
        public virtual int? ResultsToTake { get; private set; }

        /// <summary>
        ///     Gets the timeout value for the query (if set).
        /// </summary>
        /// <value>
        ///     The timeout value for the query (if set).
        /// </value>
        public virtual int? TimeoutValue { get; private set; }

        /// <summary>
        ///     Creates an instance of this class.
        /// </summary>
        /// <param name="query">
        ///     The query to create an instance from.
        /// </param>
        /// <returns>
        ///     The created <see cref="QuerySelection" /> instance.
        /// </returns>
        public static QuerySelection Create(IQueryableFlowQuery query)
        {
            return new QuerySelection(query);
        }
    }
}