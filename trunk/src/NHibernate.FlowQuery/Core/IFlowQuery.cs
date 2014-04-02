using System;
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core.CustomProjections;
using NHibernate.FlowQuery.Core.Fetches;
using NHibernate.FlowQuery.Core.Joins;
using NHibernate.FlowQuery.Core.Locks;
using NHibernate.Metadata;

namespace NHibernate.FlowQuery.Core
{
    public interface IFlowQuery
    {
        FlowQueryOptions Options { get; }

        Func<System.Type, string, ICriteria> CriteriaFactory { get; }

        Func<System.Type, IClassMetadata> MetaDataFactory { get; }

        string Alias { get; }

        bool IsCacheable { get; }

        CacheMode? CacheMode { get; }

        string CacheRegion { get; }

        Dictionary<string, string> Aliases { get; }

        List<ICriterion> Criterions { get; }

        List<Fetch> Fetches { get; }

        List<FqGroupByProjection> GroupBys { get; }

        List<Join> Joins { get; }

        List<Lock> Locks { get; }

        List<OrderByStatement> Orders { get; }

        int? ResultsToSkip { get; }

        int? ResultsToTake { get; }
    }
}