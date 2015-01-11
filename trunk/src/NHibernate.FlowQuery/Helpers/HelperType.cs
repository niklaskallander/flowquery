namespace NHibernate.FlowQuery.Helpers
{
    /// <summary>
    ///     The <see cref="HelperType" /> is used to indicate the scope of a projection that is resolved using
    ///     the static helpers.
    /// </summary>
    public enum HelperType
    {
        /// <summary>
        ///     Projection/Select list.
        /// </summary>
        Select,

        /// <summary>
        ///     Filter (and join).
        /// </summary>
        Filter,

        /// <summary>
        ///     Order by (asc/desc).
        /// </summary>
        Order,

        /// <summary>
        ///     Group by.
        /// </summary>
        GroupBy,

        /// <summary>
        ///     Some other use-case.
        /// </summary>
        Other
    }
}