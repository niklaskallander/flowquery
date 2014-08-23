namespace NHibernate.FlowQuery.Core.Structures
{
    using NHibernate.Criterion;
    using NHibernate.SqlCommand;

    /// <summary>
    ///     A class representing a join statement.
    /// </summary>
    public class Join
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Join" /> class.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <param name="alias">
        ///     The alias.
        /// </param>
        /// <param name="joinType">
        ///     The join type.
        /// </param>
        /// <param name="withClause">
        ///     An extra join filter (may be null).
        /// </param>
        /// <param name="isCollection">
        ///     A flag indicating whether the given property is a collection.
        /// </param>
        public Join(string property, string alias, JoinType joinType, ICriterion withClause, bool isCollection)
        {
            Property = property;
            Alias = alias;
            JoinType = joinType;
            WithClause = withClause;
            IsCollection = isCollection;
        }

        /// <summary>
        ///     Gets the alias.
        /// </summary>
        /// <value>
        ///     The alias.
        /// </value>
        public virtual string Alias { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the joined association path is a collection.
        /// </summary>
        /// <value>
        ///     A value indicating whether the joined association path is a collection.
        /// </value>
        public virtual bool IsCollection { get; private set; }

        /// <summary>
        ///     Gets the join type.
        /// </summary>
        /// <value>
        ///     The join type.
        /// </value>
        public virtual JoinType JoinType { get; private set; }

        /// <summary>
        ///     Gets the property.
        /// </summary>
        /// <value>
        ///     The property.
        /// </value>
        public virtual string Property { get; private set; }

        /// <summary>
        ///     Gets the extra join filter (or null if not specified).
        /// </summary>
        /// <value>
        ///     An extra join filter.
        /// </value>
        public virtual ICriterion WithClause { get; private set; }
    }
}