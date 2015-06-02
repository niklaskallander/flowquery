namespace NHibernate.FlowQuery.Helpers
{
    using System.Linq.Expressions;

    /// <summary>
    ///     A helper class used when resolving projections with <see cref="ProjectionHelper" />.
    /// </summary>
    public class HelperContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HelperContext" /> class.
        /// </summary>
        /// <param name="data">
        ///     The <see cref="QueryHelperData" /> data.
        /// </param>
        /// <param name="expression">
        ///     A lambda expression.
        /// </param>
        /// <param name="type">
        ///     The <see cref="HelperType" />.
        /// </param>
        public HelperContext
            (
            QueryHelperData data,
            LambdaExpression expression,
            HelperType type
            )
            : this(data, (string)null, type)
        {
            RootAlias = expression != null && expression.Parameters.Count > 0
                ? expression.Parameters[0].Name
                : null;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HelperContext" /> class.
        /// </summary>
        /// <param name="data">
        ///     The <see cref="QueryHelperData" /> data.
        /// </param>
        /// <param name="rootAlias">
        ///     The root alias.
        /// </param>
        /// <param name="type">
        ///     The <see cref="HelperType" />.
        /// </param>
        public HelperContext
            (
            QueryHelperData data,
            string rootAlias,
            HelperType type
            )
        {
            Data = data;
            RootAlias = rootAlias;
            Type = type;
        }

        /// <summary>
        ///     Gets or sets the <see cref="QueryHelperData" /> data.
        /// </summary>
        /// <value>
        ///     The <see cref="QueryHelperData" /> data.
        /// </value>
        public virtual QueryHelperData Data { get; set; }

        /// <summary>
        ///     Gets or sets the root alias.
        /// </summary>
        /// <value>
        ///     The root alias.
        /// </value>
        public virtual string RootAlias { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="HelperType" />.
        /// </summary>
        /// <value>
        ///     The <see cref="HelperType" />.
        /// </value>
        public virtual HelperType Type { get; set; }
    }
}