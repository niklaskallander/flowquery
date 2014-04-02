using System;
using System.Collections.Generic;
using NHibernate.FlowQuery.Core.Joins;
using NHibernate.Metadata;

namespace NHibernate.FlowQuery.Helpers
{
    public class QueryHelperData
    {
        public QueryHelperData(Dictionary<string, string> aliases, List<Join> joins, Func<System.Type, IClassMetadata> metaDataFactory)
        {
            Aliases = aliases;
            Joins = joins;
            MetaDataFactory = metaDataFactory;
        }

        public List<Join> Joins { get; private set; }

        public Dictionary<string, string> Aliases { get; private set; }

        public Func<System.Type, IClassMetadata> MetaDataFactory { get; private set; }
    }
}