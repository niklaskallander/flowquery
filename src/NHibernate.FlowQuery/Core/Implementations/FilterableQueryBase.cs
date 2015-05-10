namespace NHibernate.FlowQuery.Core.Implementations
{
    using System.Collections.Generic;
    using System.Linq;

    using NHibernate.Criterion;
    using NHibernate.FlowQuery.Core.Structures;
    using NHibernate.FlowQuery.Helpers;

    /// <summary>
    ///     The filterable query base.
    /// </summary>
    public abstract class FilterableQueryBase : IFilterableQuery
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FilterableQueryBase" /> class.
        /// </summary>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="query">
        ///     The query.
        /// </param>
        protected FilterableQueryBase
            (
            string alias = null,
            IFilterableQuery query = null
            )
        {
            if (query != null)
            {
                Aliases = query.Aliases.ToDictionary(x => x.Key, x => x.Value);
                Criterions = query.Criterions.ToList();
                Joins = query.Joins.ToList();
            }
            else
            {
                Aliases = new Dictionary<string, string>();
                Criterions = new List<ICriterion>();
                Joins = new List<Join>();

                if (alias != null)
                {
                    Aliases.Add("entity.root.alias", alias);
                }
            }

            Alias = alias;

            Data = new QueryHelperData(Aliases, Joins);
        }

        /// <inheritdoc />
        public string Alias { get; private set; }

        /// <inheritdoc />
        public Dictionary<string, string> Aliases { get; private set; }

        /// <inheritdoc />
        public List<ICriterion> Criterions { get; private set; }

        /// <inheritdoc />
        public QueryHelperData Data { get; private set; }

        /// <inheritdoc />
        public List<Join> Joins { get; private set; }
    }
}