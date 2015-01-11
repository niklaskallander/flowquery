namespace NHibernate.FlowQuery.Helpers
{
    using System.Collections.Generic;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core.Structures;

    /// <summary>
    ///     The class containing information used to build projections and filters.
    /// </summary>
    public class QueryHelperData
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QueryHelperData" /> class.
        /// </summary>
        /// <param name="aliases">
        ///     The aliases for the query (if any).
        /// </param>
        /// <param name="joins">
        ///     The joins for the query (if any).
        /// </param>
        public QueryHelperData
            (
            Dictionary<string, string> aliases,
            List<Join> joins
            )
        {
            Aliases = aliases;
            Joins = joins;
            Mappings = new Dictionary<string, IProjection>();
        }

        /// <summary>
        ///     Gets the aliases for the query (if any).
        /// </summary>
        /// <value>
        ///     The aliases for the query (if any).
        /// </value>
        public Dictionary<string, string> Aliases { get; private set; }

        /// <summary>
        ///     Gets the joins for the query (if any).
        /// </summary>
        /// <value>
        ///     The joins for the query (if any).
        /// </value>
        public List<Join> Joins { get; private set; }

        /// <summary>
        ///     Gets the mappings for the query (if any).
        /// </summary>
        /// <value>
        ///     The mappings for the query (if any).
        /// </value>
        public Dictionary<string, IProjection> Mappings { get; private set; }
    }
}