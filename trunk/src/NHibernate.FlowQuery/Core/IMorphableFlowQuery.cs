namespace NHibernate.FlowQuery.Core
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using NHibernate.Criterion;
    using NHibernate.Transform;

    /// <summary>
    ///     An interface defining the basic structure of a transformable <see cref="NHibernate.FlowQuery" /> query.
    /// </summary>
    /// <remarks>
    ///     This interface is not intended to exposed publicly as to let users manipulate the properties directly. It 
    ///     is intended purely for internal use when building <see cref="ICriteria" />s and transforming queries between
    ///     the different alterations (immediate, delayed, detached).
    /// </remarks>
    public interface IMorphableFlowQuery : IFlowQuery
    {
        /// <summary>
        ///     Gets the comment specified to included in the query (or null if no comment has been specified).
        /// </summary>
        /// <value>
        ///     The comment specified to be included in the query (or null if no comment has been specified).
        /// </value>
        string CommentValue { get; }

        /// <summary>
        ///     Gets the <see cref="LambdaExpression" /> used as projection (or null if no such projection has been 
        ///     made).
        /// </summary>
        /// <value>
        ///     The <see cref="LambdaExpression" /> used for projection (or null if no such projection has been made).
        /// </value>
        LambdaExpression Constructor { get; }

        /// <summary>
        ///     Gets the fetch size value (not to be confused with <see cref="IFlowQuery.ResultsToTake" />).
        /// </summary>
        /// <value>
        ///     The fetch size value (not to be confused with <see cref="IFlowQuery.ResultsToTake" />).
        /// </value>
        int FetchSizeValue { get; }

        /// <summary>
        ///     Gets a value indicating whether the query is distinct.
        /// </summary>
        /// <value>
        ///     A value indicating whether the query is distinct.
        /// </value>
        bool IsDistinct { get; }

        /// <summary>
        ///     Gets a value indicating whether the query is read only.
        /// </summary>
        /// <value>
        ///     A value indicating whether the query is read only.
        /// </value>
        bool? IsReadOnly { get; }

        /// <summary>
        ///     Gets the projection mappings created when ordering by members of a projection instead of the members on
        ///     the queried entity.
        /// </summary>
        /// <value>
        ///     The projection mappings created when ordering by members of a projection instead of the members on the 
        ///     queried entity.
        /// </value>
        Dictionary<string, IProjection> Mappings { get; }

        /// <summary>
        ///     Gets the projection.
        /// </summary>
        /// <value>
        ///     The projection.
        /// </value>
        IProjection Projection { get; }

        /// <summary>
        ///     Gets the result transformer.
        /// </summary>
        /// <value>
        ///     The result transformer.
        /// </value>
        IResultTransformer ResultTransformer { get; }

        /// <summary>
        ///     Gets the timeout value (or null if no timeout has been specified).
        /// </summary>
        /// <value>
        ///     The timeout value (or null if no timeout has been specified).
        /// </value>
        int? TimeoutValue { get; }
    }
}