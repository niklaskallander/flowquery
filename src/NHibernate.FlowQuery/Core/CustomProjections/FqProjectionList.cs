namespace NHibernate.FlowQuery.Core.CustomProjections
{
    using System.Collections.Generic;

    using NHibernate.Criterion;
    using NHibernate.Engine;
    using NHibernate.SqlCommand;
    using NHibernate.Type;

    /// <summary>
    ///     A helper projection which makes it possible to have a <see cref="IProjection" /> object to be used in the
    ///     group by-clause of a sql query without also having the same projection in the select list.
    ///     See also <seealso cref="FqGroupByProjection" />.
    /// </summary>
    /// <remarks>
    ///     This implementation simply wraps a <see cref="ProjectionList" /> object and tweaks a few of its methods.
    /// </remarks>
    public class FqProjectionList : IProjection
    {
        /// <summary>
        ///     The <see cref="IProjection" /> objects added to this <see cref="FqProjectionList" /> object.
        /// </summary>
        private readonly IList<IProjection> _elements = new List<IProjection>();

        /// <summary>
        ///     The wrapped <see cref="ProjectionList" /> object.
        /// </summary>
        private readonly ProjectionList _list;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FqProjectionList" /> class.
        /// </summary>
        public FqProjectionList()
        {
            _list = Projections.ProjectionList();
        }

        /// <summary>
        ///     Gets the aliases.
        /// </summary>
        /// <value>
        ///     The aliases.
        /// </value>
        public string[] Aliases
        {
            get
            {
                return _list.Aliases;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the projection is aggregate.
        /// </summary>
        /// <value>
        ///     The projection is aggregate.
        /// </value>
        public bool IsAggregate
        {
            get
            {
                return _list.IsAggregate;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the projection is grouped.
        /// </summary>
        /// <value>
        ///     The projection is grouped.
        /// </value>
        public bool IsGrouped
        {
            get
            {
                return _list.IsGrouped;
            }
        }

        /// <summary>
        ///     Gets the length.
        /// </summary>
        /// <value>
        ///     The length.
        /// </value>
        public int Length
        {
            get
            {
                return _elements.Count;
            }
        }

        /// <summary>
        ///     Gets the <see cref="IProjection" /> at the given index.
        /// </summary>
        /// <param name="index">
        ///     The index.
        /// </param>
        /// <returns>
        ///     The <see cref="IProjection" />.
        /// </returns>
        public IProjection this[int index]
        {
            get
            {
                return _elements[index];
            }
        }

        /// <summary>
        ///     Adds the provided <see cref="IProjection" /> object to this <see cref="FqProjectionList" />.
        /// </summary>
        /// <param name="projection">
        ///     The <see cref="IProjection" /> object to add.
        /// </param>
        /// <returns>
        ///     This <see cref="FqProjectionList" /> object. Useful for chaining.
        /// </returns>
        public FqProjectionList Add(IProjection projection)
        {
            _elements.Add(projection);

            _list.Add(projection);

            return this;
        }

        /// <summary>
        ///     Get the SQL column aliases used by this projection for the columns it writes for inclusion into the
        ///     <code>SELECT</code> clause (<see cref="M:IProjection.ToSqlString" />) for a particular criteria-level
        ///     alias.
        /// </summary>
        /// <param name="alias">
        ///     The criteria-level alias.
        /// </param>
        /// <param name="position">
        ///     Just as in <see cref="M:IProjection.ToSqlString" />, represents the number of columns rendered prior to
        ///     this projection.
        /// </param>
        /// <param name="criteria">
        ///     The local criteria to which this project is attached (for resolution).
        /// </param>
        /// <param name="criteriaQuery">
        ///     The overall criteria query instance.
        /// </param>
        /// <returns>
        ///     The columns aliases.
        /// </returns>
        public string[] GetColumnAliases(string alias, int position, ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            return _list.GetColumnAliases(alias, position, criteria, criteriaQuery);
        }

        /// <summary>
        ///     Get the SQL column aliases used by this projection for the columns it writes for inclusion into the
        ///     <code>SELECT</code> clause (<see cref="M:IProjection.ToSqlString" />) for a particular criteria-level
        ///     alias.
        /// </summary>
        /// <param name="position">
        ///     Just as in <see cref="M:IProjection.ToSqlString" />, represents the number of columns rendered prior to
        ///     this projection.
        /// </param>
        /// <param name="criteria">
        ///     The local criteria to which this project is attached (for resolution).
        /// </param>
        /// <param name="criteriaQuery">
        ///     The overall criteria query instance.
        /// </param>
        /// <returns>
        ///     The columns aliases.
        /// </returns>
        public string[] GetColumnAliases(int position, ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            return _list.GetColumnAliases(position, criteria, criteriaQuery);
        }

        /// <summary>
        ///     Gets the typed values for parameters in this projection.
        /// </summary>
        /// <param name="criteria">
        ///     The criteria.
        /// </param>
        /// <param name="criteriaQuery">
        ///     The criteria query.
        /// </param>
        /// <returns>
        ///     The typed values.
        /// </returns>
        public TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            return _list.GetTypedValues(criteria, criteriaQuery);
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
        public IType[] GetTypes(string alias, ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            return _list.GetTypes(alias, criteria, criteriaQuery);
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
        public IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            return _list.GetTypes(criteria, criteriaQuery);
        }

        /// <summary>
        ///     Render the SQL Fragment to be used in the Group By Clause.
        /// </summary>
        /// <param name="criteria">
        ///     The criteria.
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
        public SqlString ToGroupSqlString
            (
            ICriteria criteria,
            ICriteriaQuery criteriaQuery,
            IDictionary<string, IFilter> enabledFilters
            )
        {
            return _list.ToGroupSqlString(criteria, criteriaQuery, enabledFilters);
        }

        /// <summary>
        ///     Generates the <see cref="SqlString" /> object (SQL fragment for the select list) the same way as
        ///     <see cref="ProjectionList.ToSqlString" /> does, but with the slight difference of omitting group by-only
        ///     <see cref="FqGroupByProjection" /> projections.
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
        ///     The generated <see cref="SqlString" /> object.
        /// </returns>
        public SqlString ToSqlString
            (
            ICriteria criteria,
            int position,
            ICriteriaQuery criteriaQuery,
            IDictionary<string, IFilter> enabledFilters
            )
        {
            var buffer = new SqlStringBuilder();

            bool lastHadValue = false;

            for (int i = 0; i < _list.Length; i++)
            {
                IProjection projection = this[i];

                SqlString value = projection.ToSqlString(criteria, position, criteriaQuery, enabledFilters);

                bool hasValue = value.Length > 0;

                if (hasValue)
                {
                    if (lastHadValue)
                    {
                        buffer.Add(", ");
                    }

                    string[] aliases = projection
                        .GetColumnAliases(position, criteria, criteriaQuery);

                    position += aliases.Length;

                    buffer.Add(value);
                }

                lastHadValue = hasValue;
            }

            return buffer.ToSqlString();
        }
    }
}