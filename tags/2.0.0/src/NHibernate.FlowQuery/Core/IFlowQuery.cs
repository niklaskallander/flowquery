using System;
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.FlowQuery.Core.Joins;

namespace NHibernate.FlowQuery.Core
{
    public interface IFlowQuery
    {
        FlowQueryOptions Options { get; }

        Func<System.Type, string, ICriteria> CriteriaFactory { get; }

        string Alias { get; }

        Dictionary<string, string> Aliases { get; }

        List<ICriterion> Criterions { get; }

        List<Join> Joins { get; }

        List<OrderByStatement> Orders { get; }

        int? ResultsToSkip { get; }

        int? ResultsToTake { get; }
    }
}