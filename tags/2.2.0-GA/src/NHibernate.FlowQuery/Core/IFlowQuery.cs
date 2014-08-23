﻿namespace NHibernate.FlowQuery.Core
{
    using System;
    using System.Collections.Generic;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core.CustomProjections;
    using NHibernate.FlowQuery.Core.Structures;
    using NHibernate.FlowQuery.Helpers;
    using NHibernate.Metadata;

    /// <summary>
    ///     An interface defining the base structure of a <see cref="NHibernate.FlowQuery" /> query.
    /// </summary>
    /// <remarks>
    ///     This interface is not intended to exposed publicly as to let users manipulate the properties directly. It
    ///     is intended purely for internal use when building <see cref="ICriteria" />s and transforming queries between
    ///     the different alterations (immediate, delayed, detached).
    /// </remarks>
    public interface IFlowQuery
    {
        /// <summary>
        ///     Gets the alias.
        /// </summary>
        /// <value>
        ///     The root entity alias.
        /// </value>
        string Alias { get; }

        /// <summary>
        ///     Gets the aliases.
        /// </summary>
        /// <value>
        ///     Contains the root entity alias + all aliases used for joined association paths.
        /// </value>
        Dictionary<string, string> Aliases { get; }

        /// <summary>
        ///     Gets the cache mode.
        /// </summary>
        /// <value>
        ///     The (<see cref="CacheMode" />) cache mode.
        /// </value>
        CacheMode? CacheMode { get; }

        /// <summary>
        ///     Gets the cache region.
        /// </summary>
        /// <value>
        ///     The cache region.
        /// </value>
        string CacheRegion { get; }

        /// <summary>
        ///     Gets the criteria factory.
        /// </summary>
        /// <value>
        ///     The criteria factory is used to create <seealso cref="ICriteria" /> instances.
        /// </value>
        Func<Type, string, ICriteria> CriteriaFactory { get; }

        /// <summary>
        ///     Gets the criterions.
        /// </summary>
        /// <value>
        ///     Contains all filters (a.k.a. restrictions) made for this query.
        /// </value>
        List<ICriterion> Criterions { get; }

        /// <summary>
        ///     Gets the <see cref="QueryHelperData" /> info.
        /// </summary>
        /// <value>
        ///     The <see cref="QueryHelperData" /> info.
        /// </value>
        QueryHelperData Data { get; }

        /// <summary>
        ///     Gets the fetching strategy list.
        /// </summary>
        /// <value>
        ///     Contains info about all fetching strategies specified for the query.
        /// </value>
        List<Fetch> Fetches { get; }

        /// <summary>
        ///     Gets the <code>GROUP BY</code> list.
        /// </summary>
        /// <value>
        ///     Contains info about <code>GROUP BY</code>s specified for the query.
        /// </value>
        List<FqGroupByProjection> GroupBys { get; }

        /// <summary>
        ///     Gets a value indicating whether the query is cache-able.
        /// </summary>
        /// <value>
        ///     Indicates whether the query is cache-able (true) or not (false).
        /// </value>
        bool IsCacheable { get; }

        /// <summary>
        ///     Gets the join list.
        /// </summary>
        /// <value>
        ///     Contains info about all the joins made for this query.
        /// </value>
        List<Join> Joins { get; }

        /// <summary>
        ///     Gets the lock list.
        /// </summary>
        /// <value>
        ///     The lock list.
        /// </value>
        List<Lock> Locks { get; }

        /// <summary>
        ///     Gets the options.
        /// </summary>
        /// <value>
        ///     The options.
        /// </value>
        FlowQueryOptions Options { get; }

        /// <summary>
        ///     Gets the orders.
        /// </summary>
        /// <value>
        ///     Contains info about all orders specified for the query.
        /// </value>
        List<OrderByStatement> Orders { get; }

        /// <summary>
        ///     Gets the results to skip.
        /// </summary>
        /// <value>
        ///     Indicates the number of results to skip.
        /// </value>
        int? ResultsToSkip { get; }

        /// <summary>
        ///     Gets the results to take.
        /// </summary>
        /// <value>
        ///     Indicates the number of results to take.
        /// </value>
        int? ResultsToTake { get; }
    }
}