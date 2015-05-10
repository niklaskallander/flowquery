namespace NHibernate.FlowQuery.Core
{
    using System.Collections.Generic;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core.Structures;
    using NHibernate.FlowQuery.Helpers;

    /// <summary>
    ///     Specifies a <see cref="IFilterableQuery" /> used to filter a query by an alias as temporary query root.
    /// </summary>
    public interface IFilterableQuery
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
        ///     Gets the <see cref="QueryHelperData" /> info.
        /// </summary>
        /// <value>
        ///     The <see cref="QueryHelperData" /> info.
        /// </value>
        QueryHelperData Data { get; }

        /// <summary>
        ///     Gets the criterions.
        /// </summary>
        /// <value>
        ///     Contains all filters (a.k.a. restrictions) made for this query.
        /// </value>
        List<ICriterion> Criterions { get; }

        /// <summary>
        ///     Gets the join list.
        /// </summary>
        /// <value>
        ///     Contains info about all the joins made for this query.
        /// </value>
        List<Join> Joins { get; }
    }
}