namespace NHibernate.FlowQuery.Core
{
    using NHibernate.FlowQuery.Expressions;

    /// <summary>
    ///     A delegate used in query filters to provide users with the possibility to specify disjunctions (OR) of 
    ///     complex filters.
    /// </summary>
    /// <param name="property">
    ///     The property to filter.
    /// </param>
    /// <param name="expression">
    ///     The filter.
    /// </param>
    /// <returns>
    ///     A <see cref="bool" /> value indicating whether the filter condition passed.
    /// </returns>
    public delegate bool WhereDelegate(object property, IsExpression expression);
}