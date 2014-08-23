namespace NHibernate.FlowQuery.Core.CustomProjections
{
    using System.Collections.Generic;

    using NHibernate.Criterion;
    using NHibernate.SqlCommand;
    using NHibernate.Type;

    /// <summary>
    ///     A helper projection which makes it possible to have a <see cref="IProjection" /> object to be used in the
    ///     group by-clause of a sql query without also having the same projection in the select list.
    ///     <seealso cref="FqProjectionList" />.
    /// </summary>
    public class FqGroupByProjection : GroupedProjection
    {
        /// <summary>
        ///     Indicates whether the underlying <see cref="IProjection" /> object should be included in the select
        ///     list.
        /// </summary>
        private readonly bool _includeInSelectList;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FqGroupByProjection" /> class.
        /// </summary>
        /// <param name="projection">
        ///     The underlying <see cref="IProjection" /> object.
        /// </param>
        /// <param name="includeInSelectList">
        ///     Indicates whether the provided <see cref="IProjection" /> object <paramref name="projection" />
        ///     should be included in the select list.
        /// </param>
        public FqGroupByProjection(IProjection projection, bool includeInSelectList = true)
            : base(projection)
        {
            _includeInSelectList = includeInSelectList;
        }

        /// <summary>
        ///     Gets the aliases.
        /// </summary>
        /// <value>
        ///     The aliases.
        /// </value>
        public override string[] Aliases
        {
            get
            {
                if (_includeInSelectList)
                {
                    return base.Aliases;
                }

                return new string[0];
            }
        }

        /// <summary>
        ///     Get the SQL select clause column aliases for a particular user-visible alias.
        /// </summary>
        /// <param name="loc">
        ///     The column index.
        /// </param>
        /// <returns>
        ///     The column aliases.
        /// </returns>
        public override string[] GetColumnAliases(int loc)
        {
            if (_includeInSelectList)
            {
                return base.GetColumnAliases(loc);
            }

            return new string[0];
        }

        /// <summary>
        ///     Get the SQL select clause column aliases for a particular user-visible alias.
        /// </summary>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="loc">
        ///     The index.
        /// </param>
        /// <returns>
        ///     The column aliases.
        /// </returns>
        public override string[] GetColumnAliases(string alias, int loc)
        {
            if (_includeInSelectList)
            {
                return base.GetColumnAliases(alias, loc);
            }

            return new string[0];
        }

        /// <summary>
        ///     Return types for a particular user-visible alias.
        /// </summary>
        /// <param name="criteria">
        ///     The criteria.
        /// </param>
        /// <param name="criteriaQuery">
        ///     The criteria query.
        /// </param>
        /// <returns>
        ///     The types.
        /// </returns>
        public override IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            if (_includeInSelectList)
            {
                return base.GetTypes(criteria, criteriaQuery);
            }

            return new IType[0];
        }

        /// <summary>
        ///     Return types for a particular user-visible alias.
        /// </summary>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="criteria">
        ///     The criteria.
        /// </param>
        /// <param name="criteriaQuery">
        ///     The criteria query.
        /// </param>
        /// <returns>
        ///     The types.
        /// </returns>
        public override IType[] GetTypes(string alias, ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            if (_includeInSelectList)
            {
                return base.GetTypes(alias, criteria, criteriaQuery);
            }

            return new IType[0];
        }

        /// <summary>
        ///     Render the SQL Fragment.
        /// </summary>
        /// <param name="criteria">
        ///     The criteria.
        /// </param>
        /// <param name="position">
        ///     The position.
        /// </param>
        /// <param name="criteriaQuery">
        ///     The criteria query.
        /// </param>
        /// <param name="enabledFilters">
        ///     The enabled filters.
        /// </param>
        /// <returns>
        ///     The rendered SQL Fragment.
        /// </returns>
        public override SqlString ToSqlString
            (
            ICriteria criteria, 
            int position, 
            ICriteriaQuery criteriaQuery, 
            IDictionary<string, IFilter> enabledFilters
            )
        {
            if (_includeInSelectList)
            {
                return base.ToSqlString(criteria, position, criteriaQuery, enabledFilters);
            }

            return new SqlString(string.Empty);
        }
    }
}