namespace NHibernate.FlowQuery.Helpers
{
    /// <summary>
    ///     The <see cref="HelperType" /> is used to indicate the scope of a projection that is resolved using the 
    ///     static helpers.
    /// </summary>
    public enum HelperType
    {
        /// <summary>
        ///     Projection/Select list helper.
        /// </summary>
        Select,

        /// <summary>
        ///     Filter (and join) helper.
        /// </summary>
        Filter,

        /// <summary>
        ///     Order by (asc/desc) helper.
        /// </summary>
        Order,

        /// <summary>
        ///     Group by helper.
        /// </summary>
        GroupBy,

        /// <summary>
        ///     Some other use-case helper.
        /// </summary>
        Other
    }
}