using System;
using System.Linq;
using NHibernate.FlowQuery.Core.Implementors;

namespace NHibernate.FlowQuery.Core.Fetches
{
    public class FetchBuilder<TSource, TQuery> : IFetchBuilder<TSource, TQuery>
        where TSource : class
        where TQuery : class, IFlowQuery<TSource, TQuery>
    {
        private readonly FlowQueryImplementor<TSource, TQuery> _implementor; 
        private readonly TQuery _query;
        private readonly string _path;
        private readonly string _alias;

        public FetchBuilder(FlowQueryImplementor<TSource, TQuery> implementor, TQuery query, string path, string alias)
        {
            _implementor = implementor;
            _query = query;
            _path = path;
            _alias = alias;
        }

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

        public virtual TQuery WithJoin()
        {
            return With(FetchMode.Join);
        }

        public virtual TQuery WithSelect()
        {
            return With(FetchMode.Select);
        }

        public virtual TQuery Eagerly()
        {
            return WithJoin();
        }

        public virtual TQuery Lazily()
        {
            return WithSelect();
        }
    }
}