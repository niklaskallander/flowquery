namespace NHibernate.FlowQuery.Core
{
    /// <summary>
    ///     An interface defining the basic structure of a projectable <see cref="NHibernate.FlowQuery" /> query.
    /// </summary>
    /// <remarks>
    ///     This interface is not intended to exposed publicly as to let users manipulate the properties directly. It 
    ///     is intended purely for internal use when building <see cref="ICriteria" />s and transforming queries between
    ///     the different alterations (immediate, delayed, detached).
    /// </remarks>
    public interface IQueryableFlowQuery : IMorphableFlowQuery
    {
        /// <summary>
        ///     Gets a value indicating whether the query is delayed.
        /// </summary>
        /// <value>
        ///     Indicates whether the query is delayed (true) or not (false).
        /// </value>
        /// <remarks>
        ///     When set to true, query execution will be deferred until the results are required by user code. This
        ///     makes it possible for <see cref="NHibernate" /> to batch multiple queries in one round-trip to the
        ///     database instead of making one round-trip per query.
        /// </remarks>
        bool IsDelayed { get; }
    }
}