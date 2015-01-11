namespace NHibernate.FlowQuery.Core.Implementations
{
    using System;
    using System.Linq;

    using NHibernate.FlowQuery.Core.Structures;

    /// <summary>
    ///     A helper class used to build fetching strategies with a nice syntax.
    /// </summary>
    /// <typeparam name="TSource">
    ///     The <see cref="System.Type" /> of the source for the <see cref="IFlowQuery{TSource, TQuery}" /> query
    ///     used to create this <see cref="FetchBuilder{TSource, TQuery}" /> instance.
    /// </typeparam>
    /// <typeparam name="TQuery">
    ///     The <see cref="System.Type" /> of the <see cref="IFlowQuery{TSource, TQuery}" /> query this
    ///     <see cref="FetchBuilder{TSource, TQuery}" /> instance is created from.
    /// </typeparam>
    public class FetchBuilder<TSource, TQuery> : IFetchBuilder<TSource, TQuery>
        where TSource : class
        where TQuery : class, IFlowQuery<TSource, TQuery>
    {
        /// <summary>
        ///     The alias used for the fetching strategy this <see cref="FetchBuilder{TSource, TQuery}" /> instance
        ///     represents.
        /// </summary>
        private readonly string _alias;

        /// <summary>
        ///     The <see cref="FlowQueryBase{TSource, TQuery}" /> query that created this
        ///     <see cref="FetchBuilder{TSource, TQuery}" /> instance.
        /// </summary>
        /// <remarks>
        ///     Should be the same object reference as <see cref="_query" />.
        /// </remarks>
        private readonly IFlowQuery _implementor;

        /// <summary>
        ///     The association path on <typeparamref name="TSource" /> used for the fetching strategy this
        ///     <see cref="FetchBuilder{TSource, TQuery}" /> instance represents.
        /// </summary>
        private readonly string _path;

        /// <summary>
        ///     The <see cref="IFlowQuery{TSource, TQuery}" /> query that created this
        ///     <see cref="FetchBuilder{TSource, TQuery}" /> instance.
        /// </summary>
        /// <remarks>
        ///     Should be the same object reference as <see cref="_implementor" />.
        /// </remarks>
        private readonly TQuery _query;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FetchBuilder{TSource, TQuery}" /> class.
        /// </summary>
        /// <param name="implementor">
        ///     The <see cref="FlowQueryBase{TSource, TQuery}" /> query that creates this
        ///     <see cref="FetchBuilder{TSource, TQuery}" /> instance.
        /// </param>
        /// <param name="query">
        ///     The <see cref="IFlowQuery{TSource, TQuery}" /> query that creates this
        ///     <see cref="FetchBuilder{TSource, TQuery}" /> instance.
        /// </param>
        /// <param name="path">
        ///     The association path on <typeparamref name="TSource" /> used for the fetching strategy this
        ///     <see cref="FetchBuilder{TSource, TQuery}" /> instance should represent.
        /// </param>
        /// <param name="alias">
        ///     The alias used for the fetching strategy this <see cref="FetchBuilder{TSource, TQuery}" /> instance
        ///     should represent.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="implementor" /> and <paramref name="query" /> are not the same object reference.
        /// </exception>
        public FetchBuilder(IFlowQuery implementor, TQuery query, string path, string alias)
        {
            if (!ReferenceEquals(implementor, query))
            {
                throw new ArgumentException("|implementor| and |query| must be the same object reference.");
            }

            _implementor = implementor;
            _query = query;
            _path = path;
            _alias = alias;
        }

        /// <inheritdoc />
        public virtual TQuery Eagerly()
        {
            return WithJoin();
        }

        /// <inheritdoc />
        public virtual TQuery Lazily()
        {
            return WithSelect();
        }

        /// <inheritdoc />
        public virtual TQuery WithJoin()
        {
            return With(FetchMode.Join);
        }

        /// <inheritdoc />
        public virtual TQuery WithSelect()
        {
            return With(FetchMode.Select);
        }

        /// <summary>
        ///     Creates the final fetching strategy with the provided <see cref="FetchMode" />.
        /// </summary>
        /// <param name="mode">
        ///     The <see cref="FetchMode" /> value to use for the built fetching strategy.
        /// </param>
        /// <returns>
        ///     The <see cref="T:TQuery"/> query this <see cref="FetchBuilder{TSource, TQuery}" /> was created with.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     The alias (<see cref="_alias" />) is already used for another fetching strategy.
        /// </exception>
        protected virtual TQuery With(FetchMode mode)
        {
            bool exists = _implementor.Fetches.Any(x => x.Path == _path && x.Alias == _alias);

            if (exists)
            {
                return _query;
            }

            bool aliasUsed = _implementor.Fetches.Any(x => x.HasAlias && x.Alias == _alias);

            if (aliasUsed)
            {
                throw new InvalidOperationException("The alias provided for the fetching strategy is already used.");
            }

            string[] steps = _path.Split('.');

            string path = string.Empty;

            for (int i = 0; i < steps.Length - 1; i++)
            {
                if (i > 0)
                {
                    path += ".";
                }

                path += steps[i];

                bool pathExists = _implementor.Fetches
                    .Any(x => x.Path == path);

                if (!pathExists)
                {
                    _implementor.Fetches.Add(new Fetch(path, path, mode));
                }
            }

            _implementor.Fetches.Add(new Fetch(_path, _alias, mode));

            return _query;
        }
    }
}