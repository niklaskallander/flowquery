using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace NHibernate.FlowQuery.Core
{
    public interface IMorphableFlowQuery : IFlowQuery
    {
        string CommentValue { get; }

        LambdaExpression Constructor { get; }

        int FetchSizeValue { get; }

        bool IsDistinct { get; }

        bool? IsReadOnly { get; }

        Dictionary<string, IProjection> Mappings { get; }

        IProjection Projection { get; }

        IResultTransformer ResultTransformer { get; }

        int? TimeoutValue { get; }
    }
}