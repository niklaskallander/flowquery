namespace NHibernate.FlowQuery.Core.Fetches
{
    public class Fetch
    {
        private readonly string _alias;
        private readonly string _path;
        private readonly FetchMode _fetchMode;

        public Fetch(string path, string alias, FetchMode fetchMode)
        {
            _alias = alias;
            _path = path;
            _fetchMode = fetchMode;
        }

        public bool HasAlias
        {
            get { return Alias != Path; }
        }

        public string Alias
        {
            get { return _alias; }
        }

        public string Path
        {
            get { return _path; }
        }

        public FetchMode FetchMode
        {
            get { return _fetchMode; }
        }
    }
}