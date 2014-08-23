namespace NHibernate.FlowQuery.Core.CustomProjections
{
    using NHibernate.Criterion;

    /// <summary>
    ///     A helper projection which exposes the aliased <see cref="IProjection" /> object.
    /// </summary>
    public class FqAliasProjection : AliasedProjection
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FqAliasProjection" /> class.
        /// </summary>
        /// <param name="projection">
        ///     The <see cref="IProjection" /> to be aliased.
        /// </param>
        /// <param name="alias">
        ///     The alias for this <see cref="FqAliasProjection" />.
        /// </param>
        public FqAliasProjection(IProjection projection, string alias)
            : base(projection, alias)
        {
            Projection = projection;
        }

        /// <summary>
        ///     Gets the aliased <see cref="IProjection" /> object.
        /// </summary>
        /// <value>
        ///     The aliased <see cref="IProjection" /> object.
        /// </value>
        public IProjection Projection { get; private set; }
    }
}