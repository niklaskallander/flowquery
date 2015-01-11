namespace NHibernate.FlowQuery.Core.Structures
{
    /// <summary>
    ///     Represents a fetching strategy component of an entity association.
    /// </summary>
    public class Fetch
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Fetch" /> class.
        /// </summary>
        /// <param name="path">
        ///     The association path.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="fetchMode">
        ///     The fetch mode.
        /// </param>
        public Fetch(string path, string alias, FetchMode fetchMode)
        {
            Alias = alias;
            Path = path;
            FetchMode = fetchMode;
        }

        /// <summary>
        ///     Gets the alias used for the fetching strategy component.
        /// </summary>
        /// <value>
        ///     The alias.
        /// </value>
        public virtual string Alias { get; private set; }

        /// <summary>
        ///     Gets the <see cref="FetchMode" /> mode for this fetching strategy component.
        /// </summary>
        /// <value>
        ///     The fetch mode.
        /// </value>
        public virtual FetchMode FetchMode { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether this fetching strategy component has an alias.
        /// </summary>
        /// <value>
        ///     A value indicating whether this fetching strategy component has an alias.
        /// </value>
        public virtual bool HasAlias
        {
            get
            {
                return Alias != Path;
            }
        }

        /// <summary>
        ///     Gets the entity association path for this fetching strategy component.
        /// </summary>
        /// <value>
        ///     The association path.
        /// </value>
        public virtual string Path { get; private set; }
    }
}